using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices.ComTypes;
using EngraveMemory.Engine;

namespace EngraveMemory.Common
{
    public static class Extensions
    {
        public static IEnumerable<T> AsArray<T>(this T item) => new[] {item};
        
        public static string ToGoodDateString(this DateTime date)
        {
            if (date.Date == DateTime.Now.Date) return "сегодня";

            if (date.Date == DateTime.Now.Date.AddDays(-1)) return "вчера";

            if (date.Date.Year == DateTime.Now.Date.Year) return date.ToString("dd MMMM", new CultureInfo("ru-RU")); 
            
            return date.ToString("dd MMMM yyyy", new CultureInfo("ru-RU"));
        }

        private static TimeSpan[] Intervals = {TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(20)};
        
        public static IEnumerable<DateTime> GetRepeatDateTimes(this DateTime dateTime)
        {
            var currentDate = dateTime;
            var result = new List<DateTime>();
            foreach (var interval in Intervals)
            {
                var toAdd = currentDate.Add(interval);
                result.Add(toAdd);
                currentDate = toAdd;
            }
            return result;
        }
    }
}