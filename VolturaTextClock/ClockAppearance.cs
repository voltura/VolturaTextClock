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
            try
            {
                var theme = JsonConvert.DeserializeObject<TextClockTheme>(AppConfig.GetValue("clockTheme", "{}")) ?? new TextClockTheme();
                var defaults = new TextClockTheme();
                if (!Enum.IsDefined(theme.Language)) theme.Language = TextClockTheme.LANGUAGE.System;
                if (!float.IsFinite(theme.ActiveFontSize)) theme.ActiveFontSize = defaults.ActiveFontSize;
                if (!float.IsFinite(theme.InactiveFontSize)) theme.InactiveFontSize = defaults.InactiveFontSize;
                theme.ActiveFontSize = Math.Clamp(theme.ActiveFontSize, 8, 36);
                theme.InactiveFontSize = Math.Clamp(theme.InactiveFontSize, 8, 36);
                theme.ActiveFont ??= defaults.ActiveFont;
                theme.InactiveFont ??= defaults.InactiveFont;
                theme.ActiveColor = ValidColor(theme.ActiveColor, defaults.ActiveColor);
                theme.InactiveColor = ValidColor(theme.InactiveColor, defaults.InactiveColor);
                theme.GlowColor = ValidColor(theme.GlowColor, defaults.GlowColor);
                if (theme.BackgroundTemplate is not ("Original" or "Brushed" or "Rust" or "None" or "Custom")) theme.BackgroundTemplate = "Original";
                return theme;
            }
            catch (JsonException ex) { Log.Error = ex; return new TextClockTheme(); }
        }

        private static string ValidColor(string color, string fallback)
        {
            if (string.IsNullOrWhiteSpace(color)) return fallback;
            try { return ColorTranslator.FromHtml(color).IsEmpty ? fallback : color; }
            catch (ArgumentException) { return fallback; }
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
