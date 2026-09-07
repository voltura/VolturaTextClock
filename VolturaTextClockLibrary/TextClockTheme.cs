namespace VolturaTextClock.Library
{
    public class TextClockTheme
    {
        public enum LANGUAGE { Swedish, English, System }
        public LANGUAGE Language { get; set; } = LANGUAGE.System;
        public string BackgroundImagePath { get; set; } = "";
        public string BackgroundTemplate { get; set; } = "Original";
        public string ClockImageFullPath { get; set; } = "";
        public string ActiveFont { get; set; } = "Verdana";
        public float ActiveFontSize { get; set; } = 24;
        public bool ActiveBold { get; set; } = true;
        public bool ActiveItalic { get; set; }
        public string InactiveFont { get; set; } = "Verdana";
        public float InactiveFontSize { get; set; } = 24;
        public bool InactiveBold { get; set; }
        public bool InactiveItalic { get; set; }
        public string ActiveColor { get; set; } = "#FFFFF9";
        public string InactiveColor { get; set; } = "#61666C";
        public string GlowColor { get; set; } = "#FFFFFF";
        public bool Glow { get; set; } = true;
        public bool Flicker { get; set; }
    }
}
