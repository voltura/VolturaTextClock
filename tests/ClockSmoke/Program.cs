using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using VolturaTextClock;
using VolturaTextClock.Library;

internal static class Smoke
{
    private static readonly BindingFlags Hidden = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
    private static readonly string Output = Path.Combine(AppContext.BaseDirectory, "smoke-output");

    [STAThread]
    private static void Main()
    {
        Directory.CreateDirectory(Output);
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        Application.EnableVisualStyles();
        foreach (var language in new[] { TextClockTheme.LANGUAGE.Swedish, TextClockTheme.LANGUAGE.English })
        {
            for (int minute = 0; minute < 24 * 60; minute++)
            {
                var reading = ClockCalculator.GetReading(new DateTime(2026, 1, 1).AddMinutes(minute), language);
                Check(reading.Active.Sum(row => row.Count(value => value)) > 5, "clock has highlighted words");
            }
            Console.WriteLine($"PASS {language}: all 1,440 minutes have valid word mappings");
        }
        Expect(10, 25, TextClockTheme.LANGUAGE.Swedish, "KLOCKAN ÄR FEM I HALV ELVA");
        Expect(10, 35, TextClockTheme.LANGUAGE.Swedish, "KLOCKAN ÄR FEM ÖVER HALV ELVA");
        Expect(10, 25, TextClockTheme.LANGUAGE.English, "IT IS TWENTY FIVE PAST TEN");
        Expect(10, 45, TextClockTheme.LANGUAGE.English, "IT IS QUARTER TO ELEVEN");
        Expect(23, 58, TextClockTheme.LANGUAGE.Swedish, "KLOCKAN ÄR TOLV");
        Expect(23, 58, TextClockTheme.LANGUAGE.English, "IT IS TWELVE OCLOCK");
        Expect(12, 0, TextClockTheme.LANGUAGE.Swedish, "KLOCKAN ÄR TOLV");
        var savedCulture = CultureInfo.CurrentUICulture;
        CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("sv-SE");
        Check(ClockCalculator.ResolveLanguage(TextClockTheme.LANGUAGE.System) == TextClockTheme.LANGUAGE.Swedish, "Swedish system language");
        CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("de-DE");
        Check(ClockCalculator.ResolveLanguage(TextClockTheme.LANGUAGE.System) == TextClockTheme.LANGUAGE.English, "English fallback language");
        CultureInfo.CurrentUICulture = savedCulture;

        foreach (int size in new[] { 480, 600, 720, 960 })
        {
            using var image = TextClock.Render(new TextClockTheme { Language = TextClockTheme.LANGUAGE.English }, new Size(size, size), new DateTime(2026, 1, 1, 10, 25, 0));
            Check(image.Size == new Size(size, size), "render dimensions match requested DPI size");
            image.Save(Path.Combine(Output, $"english-{size}.png"));
        }
        using (var image = TextClock.Render(new TextClockTheme { Language = TextClockTheme.LANGUAGE.Swedish }, new Size(480, 480), new DateTime(2026, 1, 1, 10, 25, 0)))
            image.Save(Path.Combine(Output, "swedish-480.png"));
        Console.WriteLine("PASS known time phrases, system language and four render sizes");

        var assembly = typeof(VolturaTextClockForm).Assembly;
        var program = assembly.GetType("VolturaTextClock.Program")!;
        string settingsFile = Path.Combine(Output, "settings.json");
        Configure(false);
        using (var form = new VolturaTextClockForm())
        {
            form.Show(); Application.DoEvents();
            Check(form.ShowInTaskbar && form.MinimizeBox, "taskbar minimizing is enabled");
            var picture = (PictureBox)typeof(VolturaTextClockForm).GetField("clockPicBox", Hidden)!.GetValue(form)!;
            Check(picture.Image != null && picture.Image.Size == picture.ClientSize, "visible clock renders at control size");
            foreach (int size in new[] { 600, 720, 480 })
            {
                form.ClientSize = new Size(size, size); Application.DoEvents();
                Check(picture.ClientSize == form.ClientSize && picture.Image!.Size == picture.ClientSize, "resized clock fills form without clipping");
            }
            int originalDpi = form.DeviceDpi;
            foreach (int dpi in new[] { 96, 144, 192, originalDpi })
            {
                var suggested = new NativeRect { Left = form.Left, Top = form.Top, Right = form.Left + 480 * dpi / 96, Bottom = form.Top + 480 * dpi / 96 };
                nint memory = Marshal.AllocHGlobal(Marshal.SizeOf<NativeRect>());
                try
                {
                    Marshal.StructureToPtr(suggested, memory, false);
                    SendMessage(form.Handle, 0x02E0, (nint)((dpi << 16) | dpi), memory);
                    Application.DoEvents();
                    Check(picture.ClientSize == form.ClientSize && picture.Image!.Size == picture.ClientSize, "DPI change redraws at the actual control size");
                }
                finally { Marshal.FreeHGlobal(memory); }
            }
            form.WindowState = FormWindowState.Minimized; Application.DoEvents();
            form.WindowState = FormWindowState.Normal; Application.DoEvents();
            Check(picture.Image != null, "restored clock has an image");
            form.Close();
        }
        Configure(true);
        using (var form = new VolturaTextClockForm())
        {
            form.Show(); Application.DoEvents();
            Check(form.WindowState == FormWindowState.Minimized, "start-minimized setting takes effect");
            form.Close();
        }
        Configure(false);
        using (var settings = new SettingsForm())
        {
            settings.Show(); Application.DoEvents();
            Console.WriteLine($"SETTINGS DPI={settings.DeviceDpi} Font={settings.Font.SizeInPoints} Height={settings.Font.Height} Size={settings.Size}");
            var language = (ComboBox)typeof(SettingsForm).GetField("language", Hidden)!.GetValue(settings)!;
            language.SelectedItem = "English";
            var appearance = (TextClockTheme)typeof(SettingsForm).GetField("theme", Hidden)!.GetValue(settings)!;
            appearance.ActiveColor = "#AADDFF";
            appearance.ActiveItalic = true;
            appearance.Flicker = true;
            var backgrounds = (ComboBox)typeof(SettingsForm).GetField("background", Hidden)!.GetValue(settings)!;
            foreach (string template in new[] { "Original", "Brushed", "Rust", "None" })
            {
                backgrounds.SelectedItem = template;
                var preview = (PictureBox)typeof(SettingsForm).GetField("preview", Hidden)!.GetValue(settings)!;
                Check(preview.Image != null, "background template renders");
            }
            appearance.BackgroundImagePath = Path.Combine(Output, "swedish-480.png");
            backgrounds.SelectedItem = "Custom";
            using (var unlocked = File.Open(appearance.BackgroundImagePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                Check(unlocked.Length > 0, "custom background is not left locked");
            backgrounds.SelectedItem = "Original";
            using var screenshot = new Bitmap(settings.Width, settings.Height);
            settings.DrawToBitmap(screenshot, new Rectangle(Point.Empty, screenshot.Size));
            screenshot.Save(Path.Combine(Output, "settings.png"));
            // Only the test settings file is injected; startup remains disabled and the test has its own product name.
            typeof(SettingsForm).GetMethod("SaveSettings", Hidden)!.Invoke(settings, null);
        }
        using (var reopened = new SettingsForm())
        {
            var theme = (TextClockTheme)typeof(SettingsForm).GetField("theme", Hidden)!.GetValue(reopened)!;
            Check(theme.Language == TextClockTheme.LANGUAGE.English, "language persists after reopening settings");
            Check(theme.ActiveColor == "#AADDFF" && theme.ActiveItalic && theme.Flicker, "appearance options persist");
        }
        var checker = assembly.GetType("VolturaTextClock.UpdateChecker")!;
        var compare = checker.GetMethod("TryGetNewerVersion", Hidden)!;
        Check(compare.Invoke(null, new object[] { "v1.0.9", new Version(1, 0, 8, 0) }) is Version, "newer release detected");
        Check(compare.Invoke(null, new object[] { "v1.0.8", new Version(1, 0, 8, 0) }) is null, "equivalent three-part release is not an update");
        Check(compare.Invoke(null, new object[] { "v1.0.7", new Version(1, 0, 8, 0) }) is null, "older release rejected");
        Check(compare.Invoke(null, new object[] { "not-a-version", new Version(1, 0, 8, 0) }) is null, "invalid release tag rejected");
        Console.WriteLine("PASS taskbar/restore, startup, settings persistence and update version handling");
        Console.WriteLine("Rendered previews: " + Output);

        void Configure(bool minimized)
        {
            File.WriteAllText(settingsFile, JsonConvert.SerializeObject(new { appSettings = new { autoStart = false, startMinimized = minimized, alwaysOnTop = false, automaticUpdateCheck = false, mainFormLocation = "40,40", clockTheme = "{}" } }));
            program.GetField("ConfigurationFile", Hidden)!.SetValue(null, settingsFile);
            if (program.GetField("AppConfig", Hidden)!.GetValue(null) is IDisposable previous) previous.Dispose();
            program.GetField("AppConfig", Hidden)!.SetValue(null, new ConfigurationBuilder().AddJsonFile(settingsFile).Build());
        }
    }

    private static void Expect(int hour, int minute, TextClockTheme.LANGUAGE language, string text) =>
        Check(ClockCalculator.GetReading(new DateTime(2026, 1, 1, hour, minute, 0), language).Text == text, "expected time: " + text);
    private static void Check(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    [StructLayout(LayoutKind.Sequential)]
    private struct NativeRect { public int Left, Top, Right, Bottom; }
    [DllImport("user32.dll")]
    private static extern nint SendMessage(nint window, int message, nint wParam, nint lParam);
}
