using System;
using System.Collections.Generic;

namespace QlocktwoClone
{
    internal static class ClockCalculator
    {
        public static List<string> GetEvenFiveMinuteTimeNoHour()
        {
            DateTime exactTime = DateTime.Now;
            int min = 5 * (int)Math.Round(exactTime.Minute / 5.0);
            List<string> timeList = new List<string>();

            if (min == 0 || min == 60) // no minutes
            {
                return timeList;
            }

            if (min <= 20) // (min) över hour
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
            else if (min > 35) // (min) i (hour + 1)
            {
                timeList.Add(GetMin(min));
                timeList.Add("I");
            }

            return timeList;
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
            switch (min)
            {
                case 5:
                case 25:
                case 35:
                case 55:
                    return "FEM";
                case 10:
                case 50:
                    return "TIO";
                case 15:
                case 45:
                    return "KVART";
                case 30:
                    return "HALV";
                default:
                    return "TJUGO";
            }
        }

        private static string GetHour(int hour)
        {
            switch (hour)
            {
                case 1:
                case 13:
                    return "ETT";
                case 2:
                case 14:
                    return "TVÅ";
                case 3:
                case 15:
                    return "TRE";
                case 4:
                case 16:
                    return "FYRA";
                case 5:
                case 17:
                    return "FEM";
                case 6:
                case 18:
                    return "SEX";
                case 7:
                case 19:
                    return "SJU";
                case 8:
                case 20:
                    return "ÅTTA";
                case 9:
                case 21:
                    return "NIO";
                case 10:
                case 22:
                    return "TIO";
                case 11:
                case 23:
                    return "ELVA";
                case 12:
                case 24:
                case 0:
                default:
                    return "TOLV";
            }
        }
    }
}
