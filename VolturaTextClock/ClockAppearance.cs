using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using VolturaTextClock.Library;
using static VolturaTextClock.Program;

namespace VolturaTextClock
{
    internal static class ClockAppearance
    {
        internal static TextClockTheme LoadTheme()
        {
            try { return JsonConvert.DeserializeObject<TextClockTheme>(AppConfig.GetValue("clockTheme", "{}")) ?? new TextClockTheme(); }
            catch (JsonException ex) { Log.Error = ex; return new TextClockTheme(); }
        }

        internal static Image LoadBackground(TextClockTheme theme)
        {
            if (theme.BackgroundTemplate == "None") return null;
            if (theme.BackgroundTemplate == "Custom")
            {
                try { using var file = Image.FromFile(theme.BackgroundImagePath); return new Bitmap(file); }
                catch (Exception ex) when (ex is IOException || ex is ArgumentException || ex is UnauthorizedAccessException || ex is OutOfMemoryException) { Log.Error = ex; }
            }
            if (theme.BackgroundTemplate == "Brushed" || theme.BackgroundTemplate == "Rust")
            {
                using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("VolturaTextClock." + theme.BackgroundTemplate + ".jpg");
                if (stream != null) { using var image = Image.FromStream(stream); return new Bitmap(image); }
            }
            return new Bitmap(Properties.Resources.background);
        }
    }
}
