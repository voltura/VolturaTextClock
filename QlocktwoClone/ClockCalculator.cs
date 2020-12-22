using System;
using System.Collections.Generic;

namespace QlocktwoClone
{
    internal static class ClockCalculator
    {
        public static string[] GetEvenFiveMinuteTimeNoHour()
        {
            DateTime exactTime = DateTime.Now;
            int hour = exactTime.Hour;
            int min = 5 * (int)Math.Round(exactTime.Minute / 5.0);
            List<string> timeList = new List<string>();

            if (min == 0 || min == 60) // no minutes
            {
                return timeList.ToArray();
            }

            if (min <= 20) // min över hour
            {
                timeList.Add(GetMin(min));
                timeList.Add("ÖVER");
            }
            else if (min == 25) // fem i halv (hour + 1)
            {
                timeList.Add(GetMin(5));
                timeList.Add("I");
                timeList.Add("HALV");
            }
            else if (min == 30) // halv (hour + 1)
            {
                timeList.Add("HALV");
            }
            else if (min == 35) // fem över halv (hour + 1)
            {
                timeList.Add(GetMin(5));
                timeList.Add("ÖVER");
                timeList.Add("HALV");
            }
            else if (min > 35) // min i (hour + 1)
            {
                timeList.Add(GetMin(min));
                timeList.Add("I");
            }

            return timeList.ToArray();
        }

        public static string GetEvenFiveMinuteTimeHour()
        {
            DateTime exactTime = DateTime.Now;
            int hour = exactTime.Hour;
            int min = 5 * (int)Math.Round(exactTime.Minute / 5.0);

            if (min <= 20)
            {
                return GetHour(hour);
            }
            if (min == 60) // hour increase
            {
                hour = (hour + 1 <= 24) ? hour + 1 : 1;
                return GetHour(hour);
            }
            return GetHour(hour + 1);
        }

        private static string GetMin(int min)
        {
            string result;
            switch (min)
            {
                case 5:
                case 25:
                case 35:
                case 55:
                    result = "FEM";
                    break;
                case 10:
                case 50:
                    result = "TIO";
                    break;
                case 15:
                case 45:
                    result = "KVART";
                    break;
                case 30:
                    result = "HALV";
                    break;
                default:
                    result = "TJUGO";
                    break;
            }
            return result;
        }

        private static string GetHour(int hour)
        {
            string result;
            switch (hour)
            {
                case 1:
                case 13:
                    result = "ETT";
                    break;
                case 2:
                case 14:
                    result = "TVÅ";
                    break;
                case 3:
                case 15:
                    result = "TRE";
                    break;
                case 4:
                case 16:
                    result = "FYRA";
                    break;
                case 5:
                case 17:
                    result = "FEM";
                    break;
                case 6:
                case 18:
                    result = "SEX";
                    break;
                case 7:
                case 19:
                    result = "SJU";
                    break;
                case 8:
                case 20:
                    result = "ÅTTA";
                    break;
                case 9:
                case 21:
                    result = "NIO";
                    break;
                case 10:
                case 22:
                    result = "TIO";
                    break;
                case 11:
                case 23:
                    result = "EVLA";
                    break;
                case 12:
                case 24:
                case 0:
                default:
                    result = "TOLV";
                    break;
            }
            return result;
        }

        #region Unused code

        /*
        public static string[] GetEvenFiveMinuteTime()
        {
            DateTime exactTime = DateTime.Now;
            int hour = exactTime.Hour;
            int min = 5 * (int)Math.Round(exactTime.Minute / 5.0);
            List<string> timeList = new List<string>();

            if (min == 0)
            {
                timeList.Add(GetHour(hour));
                return timeList.ToArray();
            }
            else if (min == 60) // hour increase
            {
                hour = (hour + 1 <= 24) ? hour + 1 : 1;
                timeList.Add(GetHour(hour));
                return timeList.ToArray();
            }

            if (min <= 20) // min över hour
            {
                timeList.Add(GetMin(min));
                timeList.Add("ÖVER");
                timeList.Add(GetHour(hour));
            }
            else if (min == 25) // fem i halv (hour + 1)
            {
                timeList.Add(GetMin(5));
                timeList.Add("I");
                timeList.Add("HALV");
                timeList.Add(GetHour(hour + 1));
            }
            else if (min == 30) // halv (hour + 1)
            {
                timeList.Add("HALV");
                timeList.Add(GetHour(hour + 1));
            }
            else if (min == 35) // fem över halv (hour + 1)
            {
                timeList.Add(GetMin(5));
                timeList.Add("ÖVER");
                timeList.Add("HALV");
                timeList.Add(GetHour(hour + 1));
            }
            else if (min > 35) // min i (hour + 1)
            {
                timeList.Add(GetMin(min));
                timeList.Add("I");
                timeList.Add(GetHour(hour + 1));
            }

            return timeList.ToArray();
        }
        public static string GetAMorPM(DateTime exactTime)
        {
            return exactTime.ToString("tt", CultureInfo.InvariantCulture);
        }
        */

        #endregion Unused code
    }
}
