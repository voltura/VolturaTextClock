using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VolturaTextClock;

internal static class SettingsStore
{
    private static readonly object Gate = new();
    internal static readonly string Defaults = """
        {"appSettings":{"alwaysOnTop":true,"autoStart":false,"startMinimized":false,
        "automaticUpdateCheck":false,"interfaceLanguage":"System","clockTheme":"{}",
        "mainFormLocation":"40,40","logFileSizeMB":10}}
        """;

    internal static void Initialize(string path)
    {
        lock (Gate)
        {
            var data = Read(path);
            var defaults = JObject.Parse(Defaults);
            foreach (var property in ((JObject)defaults["appSettings"]).Properties())
                if (data["appSettings"][property.Name] == null)
                    data["appSettings"][property.Name] = property.Value.DeepClone();
            AtomicWrite(path, data.ToString(Formatting.Indented));
        }
    }

    internal static JObject Read(string path)
    {
        if (!File.Exists(path)) return JObject.Parse(Defaults);
        try
        {
            var data = JObject.Parse(File.ReadAllText(path));
            if (data["appSettings"] is not JObject) throw new JsonException("Missing settings object.");
            return data;
        }
        catch (JsonException)
        {
            File.Copy(path, path + ".corrupt-" + DateTime.UtcNow.ToString("yyyyMMddHHmmssfff") + "-" + Guid.NewGuid().ToString("N"));
            return JObject.Parse(Defaults);
        }
    }

    internal static void Save(string path, IReadOnlyDictionary<string, object> values)
    {
        lock (Gate)
        {
            var data = Read(path);
            foreach (var entry in values)
                data["appSettings"][entry.Key] = entry.Value == null ? JValue.CreateNull() : JToken.FromObject(entry.Value);
            AtomicWrite(path, data.ToString(Formatting.Indented));
        }
    }

    private static void AtomicWrite(string path, string content)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path)));
        var temporary = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            using (var stream = new FileStream(temporary, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                using var writer = new StreamWriter(stream, leaveOpen: true);
                writer.Write(content);
                writer.Flush();
                stream.Flush(true);
            }
            File.Move(temporary, path, true);
        }
        finally { if (File.Exists(temporary)) File.Delete(temporary); }
    }
}
