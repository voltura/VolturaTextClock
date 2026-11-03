using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace VolturaTextClock;

internal static class Program
{
    internal static IConfiguration AppConfig;
    internal static string ConfigurationFile;

    [STAThread]
    private static int Main(string[] args)
    {
        if (args.Length == 1 && args[0] == "--installer-health-check")
        {
            try
            {
                using var image = Library.TextClock.Render(new Library.TextClockTheme(), new System.Drawing.Size(128, 128), DateTime.Now);
                using var reader = new System.IO.StreamReader(typeof(Program).Assembly.GetManifestResourceStream("VolturaTextClock.Updates.update-signing-public.pem"));
                using var rsa = System.Security.Cryptography.RSA.Create();
                rsa.ImportFromPem(reader.ReadToEnd());
                return 0;
            }
            catch { return 1; }
        }
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        try
        {
            ConfigurationFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VolturaTextClockAppsettings.json");
            SettingsStore.Initialize(ConfigurationFile);
            AppConfig = new ConfigurationBuilder().AddJsonFile(ConfigurationFile, false, false).Build();
            UiText.Language = AppConfig.GetValue("interfaceLanguage", "System");
            StartWithWindows.Active = AppConfig.GetValue("autoStart", false);
            Application.Run(new VolturaTextClockForm());
            return 0;
        }
        catch (Exception error)
        {
            Log.Error = error;
            MessageBox.Show(UiText.Get("StartupFailed"), "Voltura TextClock", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return 1;
        }
        finally { (AppConfig as IDisposable)?.Dispose(); Log.Close("Application closed"); }
    }
}

internal static class ConfigExtensions
{
    internal static T GetValue<T>(this IConfiguration configuration, string keyName, T valueIfMissing)
    {
        if (configuration == null) return valueIfMissing;
        try
        {
            ((IConfigurationRoot)configuration).Reload();
            string value = configuration[$"appSettings:{keyName}"];
            return value == null ? valueIfMissing : (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }
        catch (Exception error) when (error is FormatException or InvalidCastException or OverflowException or InvalidDataException)
        { return valueIfMissing; }
    }

    internal static void AddOrUpdateAppSetting<T>(this IConfiguration configuration, string keyName, T value)
    {
        SettingsStore.Save(Program.ConfigurationFile, new Dictionary<string, object> { [keyName] = value });
        (configuration as IConfigurationRoot)?.Reload();
    }
}
