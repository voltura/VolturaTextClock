namespace VolturaTextClock.Library
{
    public class TextClockTheme
    {
        public enum LANGUAGE
        {
            Swedish,
            English
        }

        public LANGUAGE Language { get; set; }
        public string BackgroundImagePath { get; set; }
        public string ClockImageFullPath { get; set; }
    }
}
