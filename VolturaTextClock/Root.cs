using Newtonsoft.Json;

namespace VolturaTextClock
{
    public class Root
    {
        public AppSettings appSettings { get; set; }
    }

    public class AppSettings
    {
        [JsonProperty("alwaysOnTop")]
        public bool alwaysOnTop { get; set; }
        [JsonProperty("autoStart")]
        public bool autoStart { get; set; }
        [JsonProperty("startMinimized")]
        public bool startMinimized { get; set; }
        [JsonProperty("mainFormLocation")]
        public string mainFormLocation { get; set; }
        [JsonProperty("logFileSizeMB")]
        public int logFileSizeMB { get; set; }
    }
}
