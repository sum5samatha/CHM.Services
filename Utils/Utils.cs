using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CHM.Services.Utils
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime Next(this DateTime from, DayOfWeek dayOfWeek)
        {
            int start = (int)from.DayOfWeek;
            int target = (int)dayOfWeek;
            if (target < start)
                target += 7;
            return from.AddDays(target - start);
        }

        public static int GetDateForWeekDay(DayOfWeek DesiredDay, int Occurrence, int Month, int Year)
        {

            DateTime dtSat = new DateTime(Year, Month, 1);
            int j = 0;
            if (Convert.ToInt32(DesiredDay) - Convert.ToInt32(dtSat.DayOfWeek) >= 0)
                j = Convert.ToInt32(DesiredDay) - Convert.ToInt32(dtSat.DayOfWeek) + 1;
            else
                j = (7 - Convert.ToInt32(dtSat.DayOfWeek)) + (Convert.ToInt32(DesiredDay) + 1);

            int Date = j + (Occurrence - 1) * 7;
            if (!isValidDate(Date, Month, Year))
                Date = j + (Occurrence - 2) * 7; ;

            return Date;


        }

        private static bool isValidDate(int date, int month, int year)
        {
            try
            {
                DateTime dt = new DateTime(year, month, date);
                return dt.Month == month && dt.Day == date;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}