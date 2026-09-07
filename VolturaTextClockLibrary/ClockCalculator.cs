using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;

namespace VolturaTextClock.Library
{
    public sealed class ClockReading
    {
        public string[] Rows { get; }
        public bool[][] Active { get; }
        public string Text { get; }
        public ClockReading(string[] rows, bool[][] active, string text) { Rows = rows; Active = active; Text = text; }
    }

    public static class ClockCalculator
    {
        private static readonly ResourceManager Words = new ResourceManager("VolturaTextClock.Library.ClockWords", typeof(ClockCalculator).Assembly);
        public static TextClockTheme.LANGUAGE ResolveLanguage(TextClockTheme.LANGUAGE language) => language == TextClockTheme.LANGUAGE.System
            ? (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "sv" ? TextClockTheme.LANGUAGE.Swedish : TextClockTheme.LANGUAGE.English) : language;

        public static ClockReading GetReading(DateTime time, TextClockTheme.LANGUAGE language)
        {
            bool swedish = ResolveLanguage(language) == TextClockTheme.LANGUAGE.Swedish;
            var culture = CultureInfo.GetCultureInfo(swedish ? "sv" : "en");
            string Get(string key) => Words.GetString(key, culture);
            string[] rows = Get("Rows").Split('|');
            bool[][] active = rows.Select(row => new bool[row.Length]).ToArray();
            var tokens = new List<string>(Get("Intro").Split('|'));
            int minute = (int)Math.Round(time.Minute / 5.0, MidpointRounding.AwayFromZero) * 5;
            int hour = time.Hour;
            if (minute == 60) { hour++; minute = 0; }
            if (swedish)
            {
                if (minute > 20) hour++;
                if (minute == 25) tokens.AddRange(new[] { Get("Five"), Get("To"), Get("Half") });
                else if (minute == 30) tokens.Add(Get("Half"));
                else if (minute == 35) tokens.AddRange(new[] { Get("Five"), Get("Past"), Get("Half") });
                else if (minute != 0) tokens.AddRange(new[] { Get(MinuteKey(minute > 30 ? 60 - minute : minute)), Get(minute > 30 ? "To" : "Past") });
            }
            else
            {
                if (minute > 30) hour++;
                int amount = minute > 30 ? 60 - minute : minute;
                if (amount == 25) tokens.AddRange(new[] { Get("Twenty"), Get("Five") });
                else if (amount != 0) tokens.Add(Get(MinuteKey(amount)));
                if (minute != 0) tokens.Add(Get(minute > 30 ? "To" : "Past"));
            }
            foreach (string token in tokens) Mark(token, 0, swedish ? 4 : 4, last: swedish);
            string hourWord = Get("Hours").Split('|')[hour % 12];
            Mark(hourWord, swedish ? 5 : 4, rows.Length - 1, last: false);
            tokens.Add(hourWord);
            if (!swedish && minute == 0) { string clock = Get("OClock"); Mark(clock, 9, 9, false); tokens.Add(clock); }
            return new ClockReading(rows, active, string.Join(" ", tokens));

            void Mark(string word, int firstRow, int lastRow, bool last)
            {
                for (int row = firstRow; row <= lastRow; row++)
                {
                    int column = last ? rows[row].LastIndexOf(word, StringComparison.Ordinal) : rows[row].IndexOf(word, StringComparison.Ordinal);
                    if (column < 0) continue;
                    for (int i = 0; i < word.Length; i++) active[row][column + i] = true;
                    return;
                }
                throw new InvalidOperationException("Clock resource does not contain word: " + word);
            }
        }
        private static string MinuteKey(int minute) => minute switch { 5 => "Five", 10 => "Ten", 15 => "Quarter", 20 => "Twenty", 30 => "Half", _ => throw new ArgumentOutOfRangeException(nameof(minute)) };
    }
}
