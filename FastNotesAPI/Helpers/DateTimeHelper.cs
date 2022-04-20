using System;

    public static class DateTimeHelper
    {
        public static DateTime ToMexicoTime(this DateTime day)
        {
        return TimeZoneInfo.ConvertTime(day, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time (Mexico)"));
        }
    }

