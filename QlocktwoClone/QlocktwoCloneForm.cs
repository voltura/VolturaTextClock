using System;
using System.IO;
using System.Windows.Forms;

namespace QlocktwoClone
{
    public partial class QlocktwoCloneForm : Form
    {
        private const string CLOCK_TEXT_01 = @"FEMYISTIONI<BR>KVARTQIENZO<BR>TJUGOLIVIPM<BR>ÖVERKAMHALV<BR>";
        private const string CLOCK_TEXT_02 = @"ETTUSVLXTVÅ<BR>TREMYKYFYRA<BR>FEMSFLORSEX<BR>SJUÅTTAINIO<BR>TIOELVATOLV";
        private System.Collections.Generic.List<string> m_TimeText;
        private string m_HourText;
        private int m_Location;
        private readonly string m_ImagePath;
        private string m_OldHtmlClock = string.Empty;

        public QlocktwoCloneForm()
        {
            SetBrowserEmulationToIE11();
            m_ImagePath = SaveImageToDisk();
            InitializeComponent();
            UpdateUI();
        }

        private static void SetBrowserEmulationToIE11()
        {
            using Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
    @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true);
            string app = Path.GetFileName(Application.ExecutablePath);
            key.SetValue(app, 11001, Microsoft.Win32.RegistryValueKind.DWord);
            key.Close();
        }

        private void QlocktwoCloneForm_Load(object sender, EventArgs e)
        {
            UpdateClockText();
        }

        private void UpdateUI()
        {
            moveBtn.MoveOtherWithMouse(this);
            browser.SendToBack();
            picBox.Image = picBox.BackgroundImage = Properties.Resources.gear;
            picBox.BackgroundImageLayout = ImageLayout.Zoom;
            closeBtn.TextAlign = minimizeBtn.TextAlign = moveBtn.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        }

        private void PicBox_Click(object sender, EventArgs e)
        {
            ToogleButtons();
        }

        private void ToogleButtons()
        {
            closeBtn.Visible = !closeBtn.Visible;
            moveBtn.Visible = !moveBtn.Visible;
            minimizeBtn.Visible = !minimizeBtn.Visible;
        }

        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            clockTimer.Stop();
            UpdateClockText();
        }

        private static string SaveImageToDisk()
        {
            string path = Path.Combine(Path.GetTempPath(), "metal.jpg");
            
            try
            {
                if (File.Exists(path))
                {
                    File.SetAttributes(path, FileAttributes.Normal);
                    File.Delete(path);
                }
                Properties.Resources.glossy_black.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch
            {
            }
            return path.Replace('\\', '/');
        }

        private void UpdateClockText()
        {
            m_TimeText = ClockCalculator.GetEvenFiveMinuteTimeNoHour();
            m_HourText = ClockCalculator.GetEvenFiveMinuteTimeHour();
            string qlockText01 = CLOCK_TEXT_01;
            string qlockText02 = CLOCK_TEXT_02;

            foreach (string timePart in m_TimeText)
            {
                m_Location = qlockText01.LastIndexOf(timePart);
                if (m_Location >= 0)
                {
                    qlockText01 = qlockText01.Remove(m_Location, timePart.Length).Insert(m_Location, $"<span class='highlightedText'>{timePart}</span>");
                }
            }
            m_Location = qlockText02.IndexOf(m_HourText);
            if (m_Location >= 0)
            {
                qlockText02 = qlockText02.Remove(m_Location, m_HourText.Length).Insert(m_Location, $"<span class='highlightedText'>{m_HourText}</span>");
            }
            string style = @"
html,
body {
  position: relative;
  margin: 0;
  height: 100%;
  font-size: 35px; 
  font-family: 'Courier New', Courier, monospace;
  background-image: url('file:///" + m_ImagePath + @"');
  background-repeat: no-repeat;
  background-size: auto;
  color: #F8F8F8;
background-color: #000000;
  font-weight: normal;
  letter-spacing: 0.4em;
}
div {
  padding: 34px 44px 34px 44px;
}
.highlightedText {
  color: white;
  font-weight: bold;
  text-shadow: 0 0 3px #FF0000, 0 0 5px #0000FF;
}
";
            string htmlClock = @$"<!DOCTYPE html>
<html>
    <head>
        <style type=""text/css"">
            {style}
        </style>
    </head>
    <body>
        <div><span class='highlightedText'>KLOCKAN</span>T<span class='highlightedText'>ÄR</span>K<BR>{qlockText01}{qlockText02}</div>
    </body>
</html>";
            StartTimerAndShowBrowser();
            if (m_OldHtmlClock != htmlClock)
            {
                m_OldHtmlClock = htmlClock;
                browser.Navigate("about:blank");
                if (browser.Document == null)
                {
                    browser.Document.OpenNew(true);
                }
                browser.Document.Write(htmlClock);
                browser.Refresh();
            }            
        }

        private void StartTimerAndShowBrowser()
        {
            if (browser.Visible == false)
            {
                browser.Visible = true;
            }
            if (!clockTimer.Enabled)
            {
                clockTimer.Interval = 60000 - DateTime.Now.Millisecond;
                clockTimer.Start();
            }
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MinimizeBtn_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void Browser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            browser.Visible = false;
        }

        private void Browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            StartTimerAndShowBrowser();
        }
    }
}
