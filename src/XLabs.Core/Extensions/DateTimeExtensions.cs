using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLabs.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime UnixTime = new DateTime(1970, 1, 1);

        public static TimeSpan SinceUnixTime(this DateTime time)
        {
            return time - UnixTime;
        }

        public static TimeSpan SinceUnixTime(this DateTimeOffset time)
        {
            return time - UnixTime;
        }

        public static TimeSpan? SinceUnixTime(this DateTime? time)
        {
            return time == null ? null : time - UnixTime;
        }

        public static TimeSpan? SinceUnixTime(this DateTimeOffset? time)
        {
            return time == null ? null : time - UnixTime;
        }

        public static long? FullMilliseconds(this TimeSpan? timeSpan)
        {
            return timeSpan == null ? default(long?) : (long)timeSpan.Value.TotalMilliseconds;
        }
    }
}
