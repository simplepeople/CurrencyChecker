using System;

namespace BPSTests
{
    public static class TimeExtensions
    {
        public static DateTime DateFromTicks(this long ticks)
        {
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime date = start.AddMilliseconds(ticks).ToLocalTime();
            return date;
        }
    }
}
