//
// https://github.com/ServiceStack/ServiceStack.Text
// ServiceStack.Text: .NET C# POCO JSON, JSV and CSV Text Serializers.
//
// Authors:
//   Demis Bellot (demis.bellot@gmail.com)
//
// Copyright 2012 ServiceStack Ltd.
//
// Licensed under the same terms of ServiceStack: new BSD license.
//

using System;
using System.Globalization;
using ServiceStack.Text.Common;

namespace ServiceStack.Text
{
    /// <summary>
    /// A fast, standards-based, serialization-issue free DateTime serailizer.
    /// </summary>
    public static class DateTimeExtensions
    {
        public const long UnixEpoch = 621355968000000000L;
        private static readonly DateTime UnixEpochDateTimeUtc = new DateTime(UnixEpoch, DateTimeKind.Utc);
        private static readonly DateTime UnixEpochDateTimeUnspecified = new DateTime(UnixEpoch, DateTimeKind.Unspecified);
        private static readonly DateTime MinDateTimeUtc = new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long ToUnixTime(this DateTime dateTime)
        {
            return (dateTime.ToStableUniversalTime().Ticks - UnixEpoch) / TimeSpan.TicksPerSecond;
        }

        public static DateTime FromUnixTime(this double unixTime)
        {
            return UnixEpochDateTimeUtc + TimeSpan.FromSeconds(unixTime);
        }

        public static long ToUnixTimeMsAlt(this DateTime dateTime)
        {
            return (dateTime.ToStableUniversalTime().Ticks - UnixEpoch) / TimeSpan.TicksPerMillisecond;
        }

        private static TimeZoneInfo LocalTimeZone = TimeZoneInfo.Local;
        public static long ToUnixTimeMs(this DateTime dateTime)
        {
            var dtUtc = dateTime;
            if (dateTime.Kind != DateTimeKind.Utc)
            {
                dtUtc = dateTime.Kind == DateTimeKind.Unspecified && dateTime > DateTime.MinValue
                    ? DateTime.SpecifyKind(dateTime.Subtract(LocalTimeZone.GetUtcOffset(dateTime)), DateTimeKind.Utc)
                    : dateTime.ToStableUniversalTime();
            }

            return (long)(dtUtc.Subtract(UnixEpochDateTimeUtc)).TotalMilliseconds;
        }

        public static long ToUnixTimeMs(this long ticks)
        {
            return (ticks - UnixEpoch) / TimeSpan.TicksPerMillisecond;
        }

        public static DateTime FromUnixTimeMs(this double msSince1970)
        {
            return UnixEpochDateTimeUtc + TimeSpan.FromMilliseconds(msSince1970);
        }

        public static DateTime FromUnixTimeMs(this long msSince1970)
        {
            return UnixEpochDateTimeUtc + TimeSpan.FromMilliseconds(msSince1970);
        }

        public static DateTime FromUnixTimeMs(this long msSince1970, TimeSpan offset)
        {
            return UnixEpochDateTimeUnspecified + TimeSpan.FromMilliseconds(msSince1970) + offset;
        }

        public static DateTime FromUnixTimeMs(this double msSince1970, TimeSpan offset)
        {
            return UnixEpochDateTimeUnspecified + TimeSpan.FromMilliseconds(msSince1970) + offset;
        }

        public static DateTime FromUnixTimeMs(string msSince1970)
        {
            long ms;
            if (long.TryParse(msSince1970, out ms)) return ms.FromUnixTimeMs();

            // Do we really need to support fractional unix time ms time strings??
            return double.Parse(msSince1970).FromUnixTimeMs();
        }

        public static DateTime FromUnixTimeMs(string msSince1970, TimeSpan offset)
        {
            long ms;
            if (long.TryParse(msSince1970, out ms)) return ms.FromUnixTimeMs(offset);

            // Do we really need to support fractional unix time ms time strings??
            return double.Parse(msSince1970).FromUnixTimeMs(offset);
        }

        public static DateTime RoundToMs(this DateTime dateTime)
        {
            return new DateTime((dateTime.Ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond);
        }

        public static DateTime RoundToSecond(this DateTime dateTime)
        {
            return new DateTime((dateTime.Ticks / TimeSpan.TicksPerSecond) * TimeSpan.TicksPerSecond);
        }

        public static string ToShortestXsdDateTimeString(this DateTime dateTime)
        {
            return DateTimeSerializer.ToShortestXsdDateTimeString(dateTime);
        }

        public static DateTime FromShortestXsdDateTimeString(this string xsdDateTime)
        {
            return DateTimeSerializer.ParseShortestXsdDateTime(xsdDateTime);
        }

        public static bool IsEqualToTheSecond(this DateTime dateTime, DateTime otherDateTime)
        {
            return dateTime.ToStableUniversalTime().RoundToSecond().Equals(otherDateTime.ToStableUniversalTime().RoundToSecond());
        }

        public static string ToTimeOffsetString(this TimeSpan offset, string seperator = "")
        {
            var hours = Math.Abs(offset.Hours).ToString(CultureInfo.InvariantCulture);
            var minutes = Math.Abs(offset.Minutes).ToString(CultureInfo.InvariantCulture);
            return (offset < TimeSpan.Zero ? "-" : "+")
                + (hours.Length == 1 ? "0" + hours : hours)
                + seperator
                + (minutes.Length == 1 ? "0" + minutes : minutes);
        }

        public static TimeSpan FromTimeOffsetString(this string offsetString)
        {
            if (!offsetString.Contains(":"))
                offsetString = offsetString.Insert(offsetString.Length - 2, ":");

            offsetString = offsetString.TrimStart('+');

            return TimeSpan.Parse(offsetString);
        }

        public static DateTime ToStableUniversalTime(this DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Utc)
                return dateTime;
            if (dateTime == DateTime.MinValue)
                return MinDateTimeUtc;

#if SILVERLIGHT
			// Silverlight 3, 4 and 5 all work ok with DateTime.ToUniversalTime, but have no TimeZoneInfo.ConverTimeToUtc implementation.
			return dateTime.ToUniversalTime();
#else
            // .Net 2.0 - 3.5 has an issue with DateTime.ToUniversalTime, but works ok with TimeZoneInfo.ConvertTimeToUtc.
            // .Net 4.0+ does this under the hood anyway.
            return TimeZoneInfo.ConvertTimeToUtc(dateTime);
#endif
        }

        public static string FmtSortableDate(this DateTime from)
        {
            return from.ToString("yyyy-MM-dd");
        }

        public static string FmtSortableDateTime(this DateTime from)
        {
            return from.ToString("u");
        }

        public static DateTime LastMonday(this DateTime from)
        {
            var mondayOfWeek = from.Date.AddDays(-(int)from.DayOfWeek + 1);
            return mondayOfWeek;
        }

        public static DateTime StartOfLastMonth(this DateTime from)
        {
            return new DateTime(from.Date.Year, from.Date.Month, 1).AddMonths(-1);
        }

        public static DateTime EndOfLastMonth(this DateTime from)
        {
            return new DateTime(from.Date.Year, from.Date.Month, 1).AddDays(-1);
        }
    }

}
