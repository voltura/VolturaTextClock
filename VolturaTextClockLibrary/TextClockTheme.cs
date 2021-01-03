namespace VolturaTextClock.Library
{
    /// <summary>
    /// Theme for clock
    /// </summary>
    public class TextClockTheme
    {
        /// <summary>
        /// Clock language
        /// </summary>
        public enum LANGUAGE
        {
            Swedish,
            English
        }

        /// <summary>
        /// Clock language
        /// </summary>
        public LANGUAGE Language { get; set; }

        /// <summary>
        /// File path to clock background image
        /// </summary>
        public string BackgroundImagePath { get; set; }

        /// <summary>
        /// File path to use for clock image
        /// </summary>
        public string ClockImageFullPath { get; set; }
    }
}
