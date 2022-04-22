using System;

    public static class DateTimeHelper
    {
        public static DateTime ToMexicoTime(this DateTime day)
        {
        return TimeZoneInfo.ConvertTime(day, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time (Mexico)"));
        }
    } //PACIFIC STANDAET TIME (MEXICO)

