#region Using statements

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using static VolturaTextClock.Program;

#endregion

namespace VolturaTextClock
{
    /// <summary>
    ///     Application log
    /// </summary>
    internal static class Log
    {
        #region Internal methods

        /// <summary>
        ///     Init log
        /// </summary>
        internal static void Init()
        {
            FileStream fs = null;
            try
            {
                Trace.Listeners.Clear();
                string logFile = Path.GetFileNameWithoutExtension(Application.ExecutablePath) + ".log";
                string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                fs = new FileStream(Path.Combine(appDataFolder, logFile), FileMode.Append, FileAccess.Write, FileShare.ReadWrite | FileShare.Delete, 1024, FileOptions.WriteThrough);
                TextWriterTraceListener traceListener = new TextWriterTraceListener(fs);
                Trace.Listeners.Add(traceListener);
                Trace.AutoFlush = true;
                Trace.UseGlobalLock = false;
            }
            catch (Exception ex)
            {
                fs?.Dispose();
                Debug.WriteLine(ex);
            }
        }

        internal static void Close(string info)
        {
            Info = info;
            Close();
        }

        internal static void Close()
        {
            Truncate();
            Trace.Flush();
            Trace.Close();
        }

        /// <summary>
        ///     Truncate log
        /// </summary>
        internal static void Truncate()
        {
            Trace.Flush();
            Trace.Close();
            Trace.Listeners.Clear();
            try
            {
                string logFile = Path.GetFileNameWithoutExtension(Application.ExecutablePath) + ".log";
                string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                FileInfo fi = new FileInfo(Path.Combine(appDataFolder, logFile));
                if (fi.Exists)
                {
                    int trimSize = AppConfig.GetValue<int>("logFileSizeMB", 10) * 1024 * 1024;
                    if (fi.Length > trimSize)
                    {
                        using MemoryStream ms = new MemoryStream(trimSize);
                        using FileStream s = new FileStream(logFile, FileMode.Open, FileAccess.ReadWrite);
                        s.Seek(-trimSize, SeekOrigin.End);
                        byte[] bytes = new byte[trimSize];
                        s.Read(bytes, 0, trimSize);
                        ms.Write(bytes, 0, trimSize);
                        ms.Position = 0;
                        s.SetLength(trimSize);
                        s.Position = 0;
                        ms.CopyTo(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            Init();
        }

        internal static void Show(string info)
        {
            Info = info;
            Show();
        }

        internal static void Show()
        {
            string logFile = Path.GetFileNameWithoutExtension(Application.ExecutablePath) + ".log";
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            logFile = Path.Combine(appDataFolder, logFile);
            FileInfo fi = new FileInfo(logFile);
            if (!fi.Exists)
            {
                return;
            }

            Process process = null;
            try
            {
#pragma warning disable IDE0017 // Simplify object initialization
                process = new Process();
#pragma warning restore IDE0017 // Simplify object initialization
                process.StartInfo = new ProcessStartInfo(logFile) { UseShellExecute = true };
                process.Start();
            }
            catch (InvalidOperationException ex)
            {
                Error = ex;
            }
            finally
            {
                process?.Dispose();
            }
        }

        #endregion

        #region Public static log properties

        /// <summary>
        ///     Log info
        /// </summary>
        internal static string Info
        {
            private get => string.Empty;
            set
            {
                try
                {
                    Trace.TraceInformation("{0} {1}",
                        DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff", CultureInfo.InvariantCulture),
                        value);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        /// <summary>
        ///     Log error
        /// </summary>
        internal static Exception Error
        {
            private get => new ArgumentNullException("VolturaTextClock");
            set
            {
                try
                {
                    Trace.TraceError("{0} {1}",
                        DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff", CultureInfo.InvariantCulture),
                        value);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        /// <summary>
        ///     Log an error string
        /// </summary>
        internal static string ErrorString
        {
            // ReSharper disable once UnusedMember.Local
            private get => "VolturaTextClock";
            set => Trace.TraceError("{0} {1}",
                    DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff", CultureInfo.InvariantCulture),
                    value);
        }

        #endregion
    }
}