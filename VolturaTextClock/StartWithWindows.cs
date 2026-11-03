using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace VolturaTextClock;

public static class StartWithWindows
{
    private const string RunKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
    private const string ValueName = "VolturaTextClock";
    internal static bool Active
    {
        get => Registry.GetValue(@"HKEY_CURRENT_USER\" + RunKey, ValueName, null) != null;
        set
        {
            if (System.Reflection.Assembly.GetEntryAssembly() != typeof(StartWithWindows).Assembly) return;
            using var key = Registry.CurrentUser.CreateSubKey(RunKey);
            if (value) key.SetValue(ValueName, "\"" + Application.ExecutablePath + "\"");
            else key.DeleteValue(ValueName, false);
            var legacy = key.GetValue("Voltura TextClock") as string;
            if (legacy?.Contains("VolturaTextClock.exe", StringComparison.OrdinalIgnoreCase) == true)
                key.DeleteValue("Voltura TextClock", false);
        }
    }
}
