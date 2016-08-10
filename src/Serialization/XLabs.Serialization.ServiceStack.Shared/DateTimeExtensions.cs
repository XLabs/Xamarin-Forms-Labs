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
		/// <summary>
		/// The unix epoch
		/// </summary>
		public const long UnixEpoch = 621355968000000000L;
		/// <summary>
		/// The unix epoch date time UTC
		/// </summary>
		private static readonly DateTime UnixEpochDateTimeUtc = new DateTime(UnixEpoch, DateTimeKind.Utc);
		/// <summary>
		/// The unix epoch date time unspecified
		/// </summary>
		private static readonly DateTime UnixEpochDateTimeUnspecified = new DateTime(UnixEpoch, DateTimeKind.Unspecified);
		/// <summary>
		/// The minimum date time UTC
		/// </summary>
		private static readonly DateTime MinDateTimeUtc = new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		/// <summary>
		/// To the unix time.
		/// </summary>
		/// <param name="dateTime">The date time.</param>
		/// <returns>System.Int64.</returns>
		public static long ToUnixTime(this DateTime dateTime)
        {
            return (dateTime.ToStableUniversalTime().Ticks - UnixEpoch) / TimeSpan.TicksPerSecond;
        }

		/// <summary>
		/// Froms the unix time.
		/// </summary>
		/// <param name="unixTime">The unix time.</param>
		/// <returns>DateTime.</returns>
		public static DateTime FromUnixTime(this double unixTime)
        {
            return UnixEpochDateTimeUtc + TimeSpan.FromSeconds(unixTime);
        }

		/// <summary>
		/// To the unix time ms alt.
		/// </summary>
		/// <param name="dateTime">The date time.</param>
		/// <returns>System.Int64.</returns>
		public static long ToUnixTimeMsAlt(this DateTime dateTime)
        {
            return (dateTime.ToStableUniversalTime().Ticks - UnixEpoch) / TimeSpan.TicksPerMillisecond;
        }

		/// <summary>
		/// The local time zone
		/// </summary>
		private static TimeZoneInfo LocalTimeZone = TimeZoneInfo.Local;
		/// <summary>
		/// To the unix time ms.
		/// </summary>
		/// <param name="dateTime">The date time.</param>
		/// <returns>System.Int64.</returns>
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

		/// <summary>
		/// To the unix time ms.
		/// </summary>
		/// <param name="ticks">The ticks.</param>
		/// <returns>System.Int64.</returns>
		public static long ToUnixTimeMs(this long ticks)
        {
            return (ticks - UnixEpoch) / TimeSpan.TicksPerMillisecond;
        }

		/// <summary>
		/// Froms the unix time ms.
		/// </summary>
		/// <param name="msSince1970">The ms since1970.</param>
		/// <returns>DateTime.</returns>
		public static DateTime FromUnixTimeMs(this double msSince1970)
        {
            return UnixEpochDateTimeUtc + TimeSpan.FromMilliseconds(msSince1970);
        }

		/// <summary>
		/// Froms the unix time ms.
		/// </summary>
		/// <param name="msSince1970">The ms since1970.</param>
		/// <returns>DateTime.</returns>
		public static DateTime FromUnixTimeMs(this long msSince1970)
        {
            return UnixEpochDateTimeUtc + TimeSpan.FromMilliseconds(msSince1970);
        }

		/// <summary>
		/// Froms the unix time ms.
		/// </summary>
		/// <param name="msSince1970">The ms since1970.</param>
		/// <param name="offset">The offset.</param>
		/// <returns>DateTime.</returns>
		public static DateTime FromUnixTimeMs(this long msSince1970, TimeSpan offset)
        {
            return UnixEpochDateTimeUnspecified + TimeSpan.FromMilliseconds(msSince1970) + offset;
        }

		/// <summary>
		/// Froms the unix time ms.
		/// </summary>
		/// <param name="msSince1970">The ms since1970.</param>
		/// <param name="offset">The offset.</param>
		/// <returns>DateTime.</returns>
		public static DateTime FromUnixTimeMs(this double msSince1970, TimeSpan offset)
        {
            return UnixEpochDateTimeUnspecified + TimeSpan.FromMilliseconds(msSince1970) + offset;
        }

		/// <summary>
		/// Froms the unix time ms.
		/// </summary>
		/// <param name="msSince1970">The ms since1970.</param>
		/// <returns>DateTime.</returns>
		public static DateTime FromUnixTimeMs(string msSince1970)
        {
            long ms;
            if (long.TryParse(msSince1970, out ms)) return ms.FromUnixTimeMs();

            // Do we really need to support fractional unix time ms time strings??
            return double.Parse(msSince1970).FromUnixTimeMs();
        }

		/// <summary>
		/// Froms the unix time ms.
		/// </summary>
		/// <param name="msSince1970">The ms since1970.</param>
		/// <param name="offset">The offset.</param>
		/// <returns>DateTime.</returns>
		public static DateTime FromUnixTimeMs(string msSince1970, TimeSpan offset)
        {
            long ms;
            if (long.TryParse(msSince1970, out ms)) return ms.FromUnixTimeMs(offset);

            // Do we really need to support fractional unix time ms time strings??
            return double.Parse(msSince1970).FromUnixTimeMs(offset);
        }

		/// <summary>
		/// Rounds to ms.
		/// </summary>
		/// <param name="dateTime">The date time.</param>
		/// <returns>DateTime.</returns>
		public static DateTime RoundToMs(this DateTime dateTime)
        {
            return new DateTime((dateTime.Ticks / TimeSpan.TicksPerMillisecond) * TimeSpan.TicksPerMillisecond);
        }

		/// <summary>
		/// Rounds to second.
		/// </summary>
		/// <param name="dateTime">The date time.</param>
		/// <returns>DateTime.</returns>
		public static DateTime RoundToSecond(this DateTime dateTime)
        {
            return new DateTime((dateTime.Ticks / TimeSpan.TicksPerSecond) * TimeSpan.TicksPerSecond);
        }

		/// <summary>
		/// To the shortest XSD date time string.
		/// </summary>
		/// <param name="dateTime">The date time.</param>
		/// <returns>System.String.</returns>
		public static string ToShortestXsdDateTimeString(this DateTime dateTime)
        {
            return DateTimeSerializer.ToShortestXsdDateTimeString(dateTime);
        }

		/// <summary>
		/// Froms the shortest XSD date time string.
		/// </summary>
		/// <param name="xsdDateTime">The XSD date time.</param>
		/// <returns>DateTime.</returns>
		public static DateTime FromShortestXsdDateTimeString(this string xsdDateTime)
        {
            return DateTimeSerializer.ParseShortestXsdDateTime(xsdDateTime);
        }

		/// <summary>
		/// Determines whether [is equal to the second] [the specified other date time].
		/// </summary>
		/// <param name="dateTime">The date time.</param>
		/// <param name="otherDateTime">The other date time.</param>
		/// <returns><c>true</c> if [is equal to the second] [the specified other date time]; otherwise, <c>false</c>.</returns>
		public static bool IsEqualToTheSecond(this DateTime dateTime, DateTime otherDateTime)
        {
            return dateTime.ToStableUniversalTime().RoundToSecond().Equals(otherDateTime.ToStableUniversalTime().RoundToSecond());
        }

		/// <summary>
		/// To the time offset string.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="seperator">The seperator.</param>
		/// <returns>System.String.</returns>
		public static string ToTimeOffsetString(this TimeSpan offset, string seperator = "")
        {
            var hours = Math.Abs(offset.Hours).ToString(CultureInfo.InvariantCulture);
            var minutes = Math.Abs(offset.Minutes).ToString(CultureInfo.InvariantCulture);
            return (offset < TimeSpan.Zero ? "-" : "+")
                + (hours.Length == 1 ? "0" + hours : hours)
                + seperator
                + (minutes.Length == 1 ? "0" + minutes : minutes);
        }

		/// <summary>
		/// Froms the time offset string.
		/// </summary>
		/// <param name="offsetString">The offset string.</param>
		/// <returns>TimeSpan.</returns>
		public static TimeSpan FromTimeOffsetString(this string offsetString)
        {
            if (!offsetString.Contains(":"))
                offsetString = offsetString.Insert(offsetString.Length - 2, ":");

            offsetString = offsetString.TrimStart('+');

            return TimeSpan.Parse(offsetString);
        }

		/// <summary>
		/// To the stable universal time.
		/// </summary>
		/// <param name="dateTime">The date time.</param>
		/// <returns>DateTime.</returns>
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

		/// <summary>
		/// FMTs the sortable date.
		/// </summary>
		/// <param name="from">From.</param>
		/// <returns>System.String.</returns>
		public static string FmtSortableDate(this DateTime from)
        {
            return from.ToString("yyyy-MM-dd");
        }

		/// <summary>
		/// FMTs the sortable date time.
		/// </summary>
		/// <param name="from">From.</param>
		/// <returns>System.String.</returns>
		public static string FmtSortableDateTime(this DateTime from)
        {
            return from.ToString("u");
        }

		/// <summary>
		/// Lasts the monday.
		/// </summary>
		/// <param name="from">From.</param>
		/// <returns>DateTime.</returns>
		public static DateTime LastMonday(this DateTime from)
        {
            var mondayOfWeek = from.Date.AddDays(-(int)from.DayOfWeek + 1);
            return mondayOfWeek;
        }

		/// <summary>
		/// Starts the of last month.
		/// </summary>
		/// <param name="from">From.</param>
		/// <returns>DateTime.</returns>
		public static DateTime StartOfLastMonth(this DateTime from)
        {
            return new DateTime(from.Date.Year, from.Date.Month, 1).AddMonths(-1);
        }

		/// <summary>
		/// Ends the of last month.
		/// </summary>
		/// <param name="from">From.</param>
		/// <returns>DateTime.</returns>
		public static DateTime EndOfLastMonth(this DateTime from)
        {
            return new DateTime(from.Date.Year, from.Date.Month, 1).AddDays(-1);
        }
    }

}
