using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace VolturaTextClock.Library
{
    public static class TextClock
    {
        public static float GetIntensity(TextClockTheme theme, DateTime time) =>
            theme.Flicker ? .88f + .12f * (float)Math.Sin(time.TimeOfDay.TotalSeconds * 4) : 1;

        public static Bitmap Render(TextClockTheme theme, Size size, DateTime time, Image background = null, float intensity = 1)
        {
            if (size.Width <= 0 || size.Height <= 0) throw new ArgumentOutOfRangeException(nameof(size));
            var image = new Bitmap(size.Width, size.Height);
            using var graphics = Graphics.FromImage(image);
            graphics.Clear(Color.FromArgb(20, 24, 29));
            if (background != null) graphics.DrawImage(background, new Rectangle(Point.Empty, size));
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            var reading = ClockCalculator.GetReading(time, theme.Language);
            float cellWidth = size.Width / 13f, cellHeight = size.Height / 12f;
            using var activeFont = CreateFont(theme.ActiveFont, theme.ActiveFontSize, theme.ActiveBold, theme.ActiveItalic, size);
            using var inactiveFont = CreateFont(theme.InactiveFont, theme.InactiveFontSize, theme.InactiveBold, theme.InactiveItalic, size);
            using var activeBrush = new SolidBrush(Color.FromArgb((int)(255 * Math.Clamp(intensity, .65f, 1)), ColorTranslator.FromHtml(theme.ActiveColor)));
            using var inactiveBrush = new SolidBrush(ColorTranslator.FromHtml(theme.InactiveColor));
            using var glowBrush = new SolidBrush(Color.FromArgb(40, ColorTranslator.FromHtml(theme.GlowColor)));
            using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            for (int row = 0; row < reading.Rows.Length; row++)
            {
                float left = (size.Width - reading.Rows[row].Length * cellWidth) / 2;
                for (int col = 0; col < reading.Rows[row].Length; col++)
                {
                    var bounds = new RectangleF(left + col * cellWidth, (row + 1) * cellHeight, cellWidth, cellHeight);
                    string letter = reading.Rows[row][col].ToString();
                    bool active = reading.Active[row][col];
                    var font = active ? activeFont : inactiveFont;
                    if (active && theme.Glow)
                    {
                        float radius = Math.Max(1, size.Width / 240f);
                        foreach (var offset in new[] { new PointF(-radius, 0), new PointF(radius, 0), new PointF(0, -radius), new PointF(0, radius) })
                        { var halo = bounds; halo.Offset(offset); graphics.DrawString(letter, font, glowBrush, halo, format); }
                    }
                    graphics.DrawString(letter, font, active ? activeBrush : inactiveBrush, bounds, format);
                }
            }
            return image;
        }

        private static Font CreateFont(string family, float points, bool bold, bool italic, Size size)
        {
            float pixels = Math.Clamp(points, 8, 36) * Math.Min(size.Width, size.Height) / 480f;
            FontStyle style = (bold ? FontStyle.Bold : FontStyle.Regular) | (italic ? FontStyle.Italic : FontStyle.Regular);
            try { return new Font(family, pixels, style, GraphicsUnit.Pixel); }
            catch (ArgumentException) { return new Font(FontFamily.GenericSansSerif, pixels, style, GraphicsUnit.Pixel); }
        }
    }
}
