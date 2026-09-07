using Newtonsoft.Json;

namespace VolturaTextClock
{
    public class Root
    {
        [JsonProperty("appSettings")]
        public AppSettings AppSettings { get; set; }
    }

    public class AppSettings
    {
        [JsonProperty("alwaysOnTop")]
        public bool AlwaysOnTop { get; set; }
        [JsonProperty("autoStart")]
        public bool AutoStart { get; set; }
        [JsonProperty("startMinimized")]
        public bool StartMinimized { get; set; }
        [JsonProperty("automaticUpdateCheck")]
        public bool AutomaticUpdateCheck { get; set; }
        [JsonProperty("clockTheme")]
        public string ClockTheme { get; set; } = "{}";
        [JsonProperty("mainFormLocation")]
        public string MainFormLocation { get; set; }
        [JsonProperty("logFileSizeMB")]
        public int LogFileSizeMB { get; set; }
    }
}
