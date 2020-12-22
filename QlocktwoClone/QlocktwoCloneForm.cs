using System;
using System.IO;
using System.Windows.Forms;
using static System.Environment;

namespace QlocktwoClone
{
    public partial class QlocktwoCloneForm : Form
    {
        private const string CLOCK_TEXT_01 = @"FEMYISTIONI<BR>KVARTQIENZO<BR>TJUGOLIVIPM<BR>ÖVERKAMHALV<BR>";
        private const string CLOCK_TEXT_02 = @"ETTUSVLXTVÅ<BR>TREMYKYFYRA<BR>FEMSFLORSEX<BR>SJUÅTTAINIO<BR>TIOELVATOLV";
        private string[] m_TimeText01;
        private string m_HourText;
        private int m_Location;
        private readonly string m_ImagePath;

        public QlocktwoCloneForm()
        {
            m_ImagePath = SaveImageToDisk();
            InitializeComponent();
        }

        private void QlocktwoCloneForm_Load(object sender, EventArgs e)
        {
            UpdateClockText();
        }

        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            UpdateClockText();
        }

        private string SaveImageToDisk()
        {
            var commonpath = GetFolderPath(SpecialFolder.CommonApplicationData);
            var path = Path.Combine(commonpath, "metal.jpg");
            try
            {
                Properties.Resources.metalBackground.Save(path);
            }
            catch
            {
            }
            return path.Replace('\\', '/');
        }

        private void UpdateClockText()
        {
            m_TimeText01 = ClockCalculator.GetEvenFiveMinuteTimeNoHour();
            m_HourText = ClockCalculator.GetEvenFiveMinuteTimeHour();
            string qlockText01 = CLOCK_TEXT_01;
            string qlockText02 = CLOCK_TEXT_02;

            foreach (string timePart in m_TimeText01)
            {
                m_Location = qlockText01.LastIndexOf(timePart);
                if (m_Location >= 0)
                {
                    qlockText01 = qlockText01.Remove(m_Location, timePart.Length).Insert(m_Location, $"<b>{timePart}</b>");
                }
            }
            m_Location = qlockText02.IndexOf(m_HourText);
            if (m_Location >= 0)
            {
                qlockText02 = qlockText02.Remove(m_Location, m_HourText.Length).Insert(m_Location, $"<b>{m_HourText}</b>");
            }
            string htmlClock = @$"<html><body style='font-size: 35px; font-family: ""Courier New"", Courier, monospace;background-image: url(""file:///{m_ImagePath}"");'><DIV style='padding: 34px 44px 34px 44px;'><b>KLOCKAN</b>T<b>ÄR</b>K<BR>{qlockText01}{qlockText02}<DIV></body></html>";
            if (browser.DocumentText != htmlClock)
            {
                browser.Navigate("about:blank");
                browser.Document.OpenNew(false);
                browser.Document.Write(htmlClock);
                browser.Refresh();
            }
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
