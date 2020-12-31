using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace VolturaTextClock.Library
{
    public static class TextClockHtml
    {
        private const string CLOCK_TEXT_01 = @"<r></r><r><z>&nbsp;</z><z>K</z><z>L</z><z>O</z><z>C</z><z>K</z><z>A</z><z>N</z><z>T</z><z>Ä</z><z>R</z><z>K</z><z>&nbsp;</z></r><r><z>&nbsp;</z><z>F</z><z>E</z><z>M</z><z>Y</z><z>I</z><z>S</z><z>T</z><z>I</z><z>O</z><z>N</z><z>I</z><z>&nbsp;</z></r><r><z>&nbsp;</z><z>K</z><z>V</z><z>A</z><z>R</z><z>T</z><z>Q</z><z>I</z><z>E</z><z>N</z><z>Z</z><z>O</z><z>&nbsp;</z></r><r><z>&nbsp;</z><z>T</z><z>J</z><z>U</z><z>G</z><z>O</z><z>L</z><z>I</z><z>V</z><z>I</z><z>P</z><z>M</z><z>&nbsp;</z></r><r><z>&nbsp;</z><z>Ö</z><z>V</z><z>E</z><z>R</z><z>K</z><z>A</z><z>M</z><z>H</z><z>A</z><z>L</z><z>V</z><z>&nbsp;</z></r>";
        private const string CLOCK_TEXT_02 = @"<r><z>&nbsp;</z><z>E</z><z>T</z><z>T</z><z>U</z><z>S</z><z>V</z><z>L</z><z>X</z><z>T</z><z>V</z><z>Å</z><z>&nbsp;</z></r><r><z>&nbsp;</z><z>T</z><z>R</z><z>E</z><z>M</z><z>Y</z><z>K</z><z>Y</z><z>F</z><z>Y</z><z>R</z><z>A</z><z>&nbsp;</z></r><r><z>&nbsp;</z><z>F</z><z>E</z><z>M</z><z>S</z><z>F</z><z>L</z><z>O</z><z>R</z><z>S</z><z>E</z><z>X</z><z>&nbsp;</z></r><r><z>&nbsp;</z><z>S</z><z>J</z><z>U</z><z>Å</z><z>T</z><z>T</z><z>A</z><z>I</z><z>N</z><z>I</z><z>O</z><z>&nbsp;</z></r><r><z>&nbsp;</z><z>T</z><z>I</z><z>O</z><z>E</z><z>L</z><z>V</z><z>A</z><z>T</z><z>O</z><z>L</z><z>V</z><z>&nbsp;</z></r><r></r>";

        public static string GetHtml(TextClockTheme theme)
        {
            List<string> m_TimeText;
            string m_HourText;
            int m_Location;
            string m_ImagePath = theme.BackgroundImagePath;

            m_TimeText = ClockCalculator.GetEvenFiveMinuteTimeNoHour();
            m_HourText = ClockCalculator.GetEvenFiveMinuteTimeHour();
            string clockText01 = CLOCK_TEXT_01;
            string clockText02 = CLOCK_TEXT_02;
            StringBuilder sb;
            foreach (string timePart in m_TimeText)
            {
                sb = new StringBuilder();
                foreach (char c in timePart)
                {
                    sb.Append($"<z>{c}</z>");
                }
                string formattedTimepart = sb.ToString();
                m_Location = clockText01.LastIndexOf(formattedTimepart, StringComparison.Ordinal);
                if (m_Location >= 0)
                {
                    formattedTimepart = formattedTimepart.Replace("<z>", "<x>");
                    formattedTimepart = formattedTimepart.Replace("</z>", "</x>");
                    clockText01 = clockText01.Remove(m_Location, formattedTimepart.Length).Insert(m_Location, formattedTimepart);
                }
            }
            sb = new StringBuilder();
            foreach (char c in m_HourText)
            {
                sb.Append($"<z>{c}</z>");
            }
            string formattedHour = sb.ToString();
            m_Location = clockText02.IndexOf(formattedHour, StringComparison.Ordinal);
            if (m_Location >= 0)
            {
                formattedHour = formattedHour.Replace("<z>", "<x>");
                formattedHour = formattedHour.Replace("</z>", "</x>");
                clockText02 = clockText02.Remove(m_Location, formattedHour.Length).Insert(m_Location, formattedHour);
            }
            float scaleX;
            using (Graphics g = Graphics.FromImage(new Bitmap(10, 10)))
            {
                scaleX = g.DpiX / 96.0f;
            }

            float scalePercent = scaleX;
            const string fontSize = "120%";
            string bodyWidth = $"{480.0f * scalePercent:#.}px";
            string bodyHeight = $"{480.0f * scalePercent:#.}px";
            string style = @"
html,
body {
    margin: 0;
    height: " + bodyHeight + @";
    width: " + bodyWidth + @";
    font-size: " + fontSize + @";
    font-family: 'Courier New', Courier, monospace;
    font-weight: normal;
    background-image: url('file:///" + m_ImagePath + @"');
    background-repeat: no-repeat;
    background-size: 100% 100%;
    color: #F8F8F8;
    background-color: #000000;
}
#columns {
    display: flex;
    flex-direction: column;
    justify-content: space-around;
    height: " + bodyHeight + @";
    width: " + bodyWidth + @";
}
x {
    color: #FFFFF9;
    font-weight: bold;
    text-shadow: 0 0 5px #FFFFFF;
}
r {
    display: flex;
    flex-direction: rows;
    justify-content: space-between;
}
";
            string htmlClock = $@"<!DOCTYPE html>
<html>
    <head>
        <meta http-equiv=""X-UA-Compatible"" content=""IE=11""/>
        <style type=""text/css"">
            {style}
        </style>
    </head>
    <body>
        <div id=""columns"">{clockText01}{clockText02}</div>
    </body>
</html>";
            return htmlClock;
        }
    }
}
