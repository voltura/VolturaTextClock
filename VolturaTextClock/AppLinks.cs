using System.Diagnostics;

namespace VolturaTextClock;

internal static class AppLinks
{
    internal const string Website = "https://textclock.voltura.se/";
    internal const string Source = "https://github.com/voltura/VolturaTextClock";
    internal static void Open(string url)
    {
        try { using var process = Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }); }
        catch (System.ComponentModel.Win32Exception error) { Log.Error = error; }
    }
}
