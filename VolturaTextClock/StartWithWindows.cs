using Microsoft.Win32;
using System.Windows.Forms;

namespace VolturaTextClock
{
    public static class StartWithWindows
    {
        #region Internal static property that updates registry for application to start when Windows start

        internal static bool Active
        {
            get => Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run\\", Application.ProductName, null) != null;
            set
            {
                using RegistryKey registryKey = Registry.CurrentUser;
                using RegistryKey regRun = registryKey?.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\", true);
                if (value)
                {
                    regRun?.SetValue(Application.ProductName, Application.ExecutablePath);
                }
                else
                {
                    if (regRun?.GetValue(Application.ProductName) != null)
                    {
                        regRun?.DeleteValue(Application.ProductName);
                    }
                }
                registryKey?.Flush();
            }
        }

        #endregion Internal static property that updates registry for application to start when Windows start
    }
}
