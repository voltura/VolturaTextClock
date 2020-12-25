using System;
using System.IO;
using System.Runtime.Versioning;
using System.Windows.Forms;
using static QlocktwoClone.Program;

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
        private SettingsForm m_SettingsForm;

        public QlocktwoCloneForm()
        {
            SetBrowserEmulationToIE11();
            m_ImagePath = SaveImageToDisk();
            InitializeComponent();
            Visible = false;
            UpdateUI();
        }

        private void SetFormLocationFromSettings()
        {
            string formLocation = AppConfig.GetValue<string>("mainFormLocation", "40,40");
            string[] formLeftAndTop = formLocation.Split(new char[] { ',' });
            if (formLeftAndTop.Length == 2)
            {
                int tmpLeft = Convert.ToInt32(formLeftAndTop[0]);
                int tmpTop = Convert.ToInt32(formLeftAndTop[1]);
                if (tmpTop >= 0)
                {
                    Location = new System.Drawing.Point(tmpLeft, tmpTop);
                }
            }
        }

        public SettingsForm ApplicationSettingsForm
        {
            get
            {
                if (m_SettingsForm == null)
                {
                    m_SettingsForm = new SettingsForm();
                }
                return m_SettingsForm;
            }
            set => m_SettingsForm = value;
        }

        [SupportedOSPlatform("windows")]
        private static void SetBrowserEmulationToIE11()
        {
            using Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
    @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true);
            string app = AppDomain.CurrentDomain.FriendlyName;
            key.SetValue(app, 11000, Microsoft.Win32.RegistryValueKind.DWord);
            key.Close();
        }

        private void QlocktwoCloneForm_Load(object sender, EventArgs e)
        {
            UpdateClockText();
            SetFormLocationFromSettings();
            TopMost = AppConfig.GetValue("alwaysOnTop", false);
            pinBtn.Text = TopMost ? "Unpin" : "Pin";
            Visible = true;
        }

        private void UpdateUI()
        {
            Controls.SetChildIndex(panel1, 9);
            panel1.MoveOtherWithMouse(this);
            Controls.SetChildIndex(settingsPicBox, 0);
            Controls.SetChildIndex(optionsPicBox, 0);
            browser.SendToBack();
            ((Control)browser).Enabled = false;
            Controls.SetChildIndex(browser, 10);
            optionsPicBox.Image = optionsPicBox.BackgroundImage = Properties.Resources.options;
            optionsPicBox.BackgroundImageLayout = ImageLayout.Zoom;
            settingsPicBox.Image = optionsPicBox.BackgroundImage = Properties.Resources.gear;
            settingsPicBox.BackgroundImageLayout = ImageLayout.Zoom;
            closeBtn.TextAlign = minimizeBtn.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        }

        private void OptionsPicBox_Click(object sender, EventArgs e)
        {
            ToogleButtons();
        }

        private void SettingsPicBox_Click(object sender, EventArgs e)
        {
            ShowSettingsForm();
        }

        private void CloseSettingsForm()
        {
            if (ApplicationSettingsForm.WindowState != FormWindowState.Normal ||
                !ApplicationSettingsForm.Visible)
            {
                return;
            }

            ApplicationSettingsForm.Visible = false;
            ApplicationSettingsForm.Close();
        }

        private void ShowSettingsForm()
        {
            Log.Info = "Displayed settings form";
            if (ApplicationSettingsForm.Visible)
            {
                ApplicationSettingsForm.BringToFront();
                ApplicationSettingsForm.Focus();
            }
            else
            {
                if (TopMost)
                {
                    Visible = false;
                }
                ApplicationSettingsForm.ShowDialog(this);
                Visible = true;
                TopMost = AppConfig.GetValue("alwaysOnTop", false);
                pinBtn.Text = TopMost ? "Unpin" : "Pin";
            }
        }

        private void ToogleButtons()
        {
            settingsPicBox.Visible = pinBtn.Visible = closeBtn.Visible = minimizeBtn.Visible = !minimizeBtn.Visible;
            browser.SendToBack();
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
            catch (Exception ex)
            {
                Log.Error = ex;
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
  color: #FFFFF9;
  font-weight: bold;
  text-shadow: 0 0 4px #FFFFFF;
}
body {
  background-color: #000000;
}
";
            string htmlClock = @$"<!DOCTYPE html>
<html>
    <head>
        <meta http-equiv=""X-UA-Compatible"" content=""IE=11""/>
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
            if (DialogResult.Yes == MessageBox.Show(this, "Do you want to close the application?", 
                "Close application", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
            {
                Application.Exit();
            }
        }

        private void MinimizeBtn_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            ToogleButtons();
        }

        private void Browser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            browser.Visible = false;
        }

        private void Browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            StartTimerAndShowBrowser();
        }

        private void PinBtn_Click(object sender, EventArgs e)
        {
            TopMost = !TopMost;
            pinBtn.Text = TopMost ? "Unpin" : "Pin";
            AppConfig.AddOrUpdateAppSetting("alwaysOnTop", TopMost);
            ToogleButtons();
        }

        private void QlocktwoCloneForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Location.X >= 0)
            {
                AppConfig.AddOrUpdateAppSetting("mainFormLocation", $"{Location.X},{Location.Y}");
            }
        }
    }
}
