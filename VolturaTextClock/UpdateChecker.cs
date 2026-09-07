using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace VolturaTextClock
{
    internal static class UpdateChecker
    {
        internal const string ReleasesUrl = "https://github.com/voltura/VolturaTextClock/releases/latest";
        private static readonly HttpClient Client = CreateClient();
        private static HttpClient CreateClient()
        {
            var client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
            client.DefaultRequestHeaders.UserAgent.ParseAdd("VolturaTextClock/1.0");
            return client;
        }
        internal static async Task<Version> GetNewerVersionAsync(Version current)
        {
            string json = await Client.GetStringAsync("https://api.github.com/repos/voltura/VolturaTextClock/releases/latest");
            using var document = JsonDocument.Parse(json);
            string tag = document.RootElement.GetProperty("tag_name").GetString();
            return TryGetNewerVersion(tag, current);
        }
        internal static Version TryGetNewerVersion(string tag, Version current)
        {
            if (!Version.TryParse(tag?.TrimStart('v', 'V'), out var candidate)) return null;
            var normalizedCurrent = new Version(current.Major, current.Minor, Math.Max(0, current.Build), Math.Max(0, current.Revision));
            var normalizedCandidate = new Version(candidate.Major, candidate.Minor, Math.Max(0, candidate.Build), Math.Max(0, candidate.Revision));
            return normalizedCandidate > normalizedCurrent ? candidate : null;
        }
    }
}
