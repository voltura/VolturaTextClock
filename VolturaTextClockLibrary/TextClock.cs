using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VolturaTextClock.Library
{
    public class TextClock
    {

        public static bool GetImage(TextClockTheme theme)
        {
            string FILE_NAME = theme.ClockImageFullPath;
            string TEMP_FILE_NAME = theme.ClockImageFullPath + ".TEMP";
            bool result = false;
            try
            {
                string HTML = TextClockHtml.GetHtml(theme);
                float scalePercent;
                using (Graphics g = Graphics.FromImage(new Bitmap(10, 10)))
                {
                    scalePercent = g.DpiX / 96.0f;
                }

                Size IMAGE_SIZE = new Size((int)(480 * scalePercent), (int)(480 * scalePercent));
                // enable HTML5 etc
                SetFeatureBrowserFeature("FEATURE_BROWSER_EMULATION", 11000);
                // force software rendering
                SetFeatureBrowserFeature("FEATURE_IVIEWOBJECTDRAW_DMLT9_WITH_GDI", 1);
                SetFeatureBrowserFeature("FEATURE_GPU_RENDERING", 0);

                using (MessageLoopApartment apartment = new MessageLoopApartment())
                {
                    // create WebBrowser on a seprate thread with its own message loop
                    WebBrowser webBrowser = apartment?.Invoke(() => new WebBrowser());

                    // navigate and wait for the result 
                    apartment?.Invoke(() =>
                    {
                        TaskCompletionSource<bool> pageLoadedTcs = new TaskCompletionSource<bool>();
                        webBrowser.DocumentCompleted += (s, e) =>
                            pageLoadedTcs.TrySetResult(true);

                        webBrowser.DocumentText = HTML;
                        return pageLoadedTcs.Task;
                    }).Wait();

                    // save the picture
                    apartment?.Invoke(() =>
                    {
                        try
                        {
                            webBrowser.Size = IMAGE_SIZE;
                            // webBrowser.Document.Body.SetAttribute("scroll", "no");
                            // webBrowser.ScrollBarsEnabled = false;

                            Rectangle rectangle = new Rectangle(0, 0, webBrowser.Width, webBrowser.Height);

                            // get reference DC
                            using (Graphics screenGraphics = webBrowser?.CreateGraphics())
                            {
                                IntPtr screenHdc = screenGraphics.GetHdc();
                                // create a metafile
                                using (Metafile metafile = new Metafile(screenHdc, rectangle, MetafileFrameUnit.Pixel))
                                {
                                    using (Graphics graphics = Graphics.FromImage(metafile))
                                    {
                                        IntPtr hdc = graphics.GetHdc();
                                        OleDraw(webBrowser.ActiveXInstance, DVASPECT_CONTENT, hdc, ref rectangle);
                                        graphics.ReleaseHdc(hdc);
                                    }
                                    // save the metafile as bitmap
                                    metafile?.Save(TEMP_FILE_NAME, ImageFormat.Png);
                                    // compare files
                                    FileInfo ORG_fi = new FileInfo(FILE_NAME);
                                    FileInfo TEMP_fi = new FileInfo(TEMP_FILE_NAME);
                                    if (File.Exists(FILE_NAME) == false || FilesAreEqual(ORG_fi, TEMP_fi) == false)
                                    {
                                        // replace original file with temp file
                                        File.Copy(TEMP_FILE_NAME, FILE_NAME, true);
                                        result = true;
                                    }
                                }
                                screenGraphics?.ReleaseHdc(screenHdc);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Could not save clock image: {ex.Message}");
                        }
                    });

                    // dispose of webBrowser
                    apartment?.Invoke(() => webBrowser?.Dispose());
                    webBrowser = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Generic error getting clock image: {ex.Message}");
            }
            return result;
        }

        // interop
        private const uint DVASPECT_CONTENT = 1;

        [DllImport("ole32.dll", PreserveSig = false)]
        private static extern void OleDraw(
            [MarshalAs(UnmanagedType.IUnknown)] object pUnk,
            uint dwAspect,
            IntPtr hdcDraw,
            [In] ref Rectangle lprcBounds);

        // WebBrowser Feature Control
        // http://msdn.microsoft.com/en-us/library/ie/ee330733(v=vs.85).aspx
        private static void SetFeatureBrowserFeature(string feature, uint value)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime)
            {
                return;
            }

            string appName = AppDomain.CurrentDomain.FriendlyName;
            Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\" + feature,
                appName, value, RegistryValueKind.DWord);
        }

        private const int BYTES_TO_READ = sizeof(long);

        private static bool FilesAreEqual(FileInfo first, FileInfo second)
        {
            if (first.Length != second.Length)
            {
                return false;
            }

            if (string.Equals(first.FullName, second.FullName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            int iterations = (int)Math.Ceiling((double)first.Length / BYTES_TO_READ);

            using (FileStream fs1 = first.OpenRead())
            using (FileStream fs2 = second.OpenRead())
            {
                byte[] one = new byte[BYTES_TO_READ];
                byte[] two = new byte[BYTES_TO_READ];

                for (int i = 0; i < iterations; i++)
                {
                    fs1.Read(one, 0, BYTES_TO_READ);
                    fs2.Read(two, 0, BYTES_TO_READ);

                    if (BitConverter.ToInt64(one, 0) != BitConverter.ToInt64(two, 0))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }

    // MessageLoopApartment
    // more info: http://stackoverflow.com/a/21808747/1768303
    public class MessageLoopApartment : IDisposable
    {
        private Thread _thread; // the STA thread

        private TaskScheduler _taskScheduler; // the STA thread's task scheduler

        public TaskScheduler TaskScheduler => _taskScheduler;

        /// <summary>MessageLoopApartment constructor</summary>
        public MessageLoopApartment()
        {
            TaskCompletionSource<TaskScheduler> tcs = new TaskCompletionSource<TaskScheduler>();

            // start an STA thread and gets a task scheduler
            _thread = new Thread(startArg =>
            {
                void idleHandler(object s, EventArgs e)
                {
                    // handle Application.Idle just once
                    Application.Idle -= idleHandler;
                    // return the task scheduler
                    tcs?.SetResult(TaskScheduler.FromCurrentSynchronizationContext());
                }

                // handle Application.Idle just once
                // to make sure we're inside the message loop
                // and SynchronizationContext has been correctly installed
                Application.Idle += idleHandler;
                Application.Run();
            });

            _thread.SetApartmentState(ApartmentState.STA);
            _thread.IsBackground = true;
            _thread.Start();
            _taskScheduler = tcs?.Task.Result;
        }

        /// <summary>shutdown the STA thread</summary>
        public void Dispose()
        {
            if (_taskScheduler != null)
            {
                TaskScheduler taskScheduler = _taskScheduler;
                _taskScheduler = null;

                // execute Application.ExitThread() on the STA thread
                Task.Factory.StartNew(
                    () => Application.ExitThread(),
                    CancellationToken.None,
                    TaskCreationOptions.None,
                    taskScheduler).Wait();

                _thread?.Join();
                _thread = null;
            }
        }

        /// <summary>Task.Factory.StartNew wrappers</summary>
        public void Invoke(Action action)
        {
            Task.Factory.StartNew(action,
                CancellationToken.None, TaskCreationOptions.None, _taskScheduler).Wait();
        }

        public TResult Invoke<TResult>(Func<TResult> action)
        {
            return Task.Factory.StartNew(action,
                CancellationToken.None, TaskCreationOptions.None, _taskScheduler).Result;
        }
    }
}
