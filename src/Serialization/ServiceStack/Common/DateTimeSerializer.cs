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
using System.IO;
using System.Text;
using System.Xml;
using ServiceStack.Text.Json;

namespace ServiceStack.Text.Common
{
    public static class DateTimeSerializer
    {
        public const string ShortDateTimeFormat = "yyyy-MM-dd";                               //11
        public const string DefaultDateTimeFormat = "dd/MM/yyyy HH:mm:ss";                    //20
        public const string DefaultDateTimeFormatWithFraction = "dd/MM/yyyy HH:mm:ss.fff";    //24
        public const string XsdDateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffffffZ";               //29
        public const string XsdDateTimeFormat3F = "yyyy-MM-ddTHH:mm:ss.fffZ";                 //25
        public const string XsdDateTimeFormatSeconds = "yyyy-MM-ddTHH:mm:ssZ";                //21
        public const string DateTimeFormatSecondsUtcOffset = "yyyy-MM-ddTHH:mm:sszzz";        //22
        public const string DateTimeFormatTicksUtcOffset = "yyyy-MM-ddTHH:mm:ss.fffffffzzz";  //30

        public const string EscapedWcfJsonPrefix = "\\/Date(";
        public const string EscapedWcfJsonSuffix = ")\\/";
        public const string WcfJsonPrefix = "/Date(";
        public const char WcfJsonSuffix = ')';
        public const string UnspecifiedOffset = "-0000";
        public const string UtcOffset = "+0000";

        /// <summary>
        /// If AlwaysUseUtc is set to true then convert all DateTime to UTC.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private static DateTime Prepare(this DateTime dateTime, bool parsedAsUtc=false)
        {
            if (JsConfig.AlwaysUseUtc)
            {
                return dateTime.Kind != DateTimeKind.Utc ? dateTime.ToStableUniversalTime() : dateTime;
            }
            return parsedAsUtc ? dateTime.ToLocalTime() : dateTime;
        }

        public static DateTime? ParseShortestNullableXsdDateTime(string dateTimeStr)
        {
            if (dateTimeStr == null)
                return null;

            return ParseShortestXsdDateTime(dateTimeStr);
        }

        public static DateTime ParseShortestXsdDateTime(string dateTimeStr)
        {
            if (string.IsNullOrEmpty(dateTimeStr))
                return DateTime.MinValue;

            if (dateTimeStr.StartsWith(EscapedWcfJsonPrefix, StringComparison.Ordinal) || dateTimeStr.StartsWith(WcfJsonPrefix, StringComparison.Ordinal))
                return ParseWcfJsonDate(dateTimeStr).Prepare();

            if (dateTimeStr.Length == DefaultDateTimeFormat.Length
                || dateTimeStr.Length == DefaultDateTimeFormatWithFraction.Length)
            {
                return DateTime.Parse(dateTimeStr, CultureInfo.InvariantCulture).Prepare();
            }

            if (dateTimeStr.Length == XsdDateTimeFormatSeconds.Length)
                return DateTime.ParseExact(dateTimeStr, XsdDateTimeFormatSeconds, null, DateTimeStyles.AdjustToUniversal).Prepare(parsedAsUtc:true); 

            if (dateTimeStr.Length >= XsdDateTimeFormat3F.Length
                && dateTimeStr.Length <= XsdDateTimeFormat.Length
                && dateTimeStr.EndsWith("Z"))
            {
#if NETFX_CORE
                var dateTimeType = JsConfig.DateHandler != JsonDateHandler.ISO8601
                    ? "yyyy-MM-ddTHH:mm:sszzzzzzz"
                        : "yyyy-MM-ddTHH:mm:sszzzzzzz";

                return XmlConvert.ToDateTimeOffset(dateTimeStr, dateTimeType).DateTime.Prepare();
#else
                var dateTime = Env.IsMono ? ParseManual(dateTimeStr) : null;
                if (dateTime != null)
                    return dateTime.Value;

                return XmlConvert.ToDateTime(dateTimeStr, XmlDateTimeSerializationMode.Utc).Prepare();
#endif
            }

            try
            {
                return DateTime.Parse(dateTimeStr, null, DateTimeStyles.AssumeLocal).Prepare();
            }
            catch (FormatException)
            {
                var manualDate = ParseManual(dateTimeStr);
                if (manualDate != null)
                    return manualDate.Value;

                throw;
            }
        }

        public static DateTime? ParseManual(string dateTimeStr)
        {
            if (dateTimeStr == null || dateTimeStr.Length < "YYYY-MM-DD".Length)
                return null;

            var dateKind = DateTimeKind.Utc;
            if (dateTimeStr.EndsWith("Z"))
            {
                dateTimeStr = dateTimeStr.Substring(0, dateTimeStr.Length - 1);
            }

            var parts = dateTimeStr.Split('T');
            if (parts.Length == 1)
                parts = dateTimeStr.SplitOnFirst(' ');

            var dateParts = parts[0].Split('-');
            int hh = 0, min = 0, ss = 0, ms = 0;
            double subMs = 0;
            int offsetMultiplier = 0;

            if (parts.Length == 2)
            {
                var timeStringParts = parts[1].Split('+');
                if (timeStringParts.Length == 2)
                {
                    offsetMultiplier = -1;
                }
                else
                {
                    timeStringParts = parts[1].Split('-');
                    if (timeStringParts.Length == 2)
                    {
                        offsetMultiplier = 1;
                    }
                }

                var timeOffset = timeStringParts.Length == 2 ? timeStringParts[1] : null;
                var timeParts = timeStringParts[0].Split(':');

                if (timeParts.Length == 3)
                {
                    int.TryParse(timeParts[0], out hh);
                    int.TryParse(timeParts[1], out min);

                    var secParts = timeParts[2].Split('.');
                    int.TryParse(secParts[0], out ss);
                    if (secParts.Length == 2)
                    {
                        var msStr = secParts[1].PadRight(3, '0');
                        ms = int.Parse(msStr.Substring(0, 3));

                        if (msStr.Length > 3)
                        {
                            var subMsStr = msStr.Substring(3);
                            subMs = double.Parse(subMsStr) / Math.Pow(10, subMsStr.Length);
                        }
                    }
                }

                var dateTime = new DateTime(int.Parse(dateParts[0]), int.Parse(dateParts[1]), int.Parse(dateParts[2]), hh, min, ss, ms, dateKind);
                if (subMs != 0)
                {
                    dateTime.AddMilliseconds(subMs);
                }

                if (offsetMultiplier != 0 && timeOffset != null)
                {
                    timeParts = timeOffset.Split(':');
                    if (timeParts.Length == 2)
                    {
                        hh = int.Parse(timeParts[0]);
                        min = int.Parse(timeParts[1]);
                    }
                    else
                    {
                        hh = int.Parse(timeOffset.Substring(0, 2));
                        min = int.Parse(timeOffset.Substring(2));
                    }

                    dateTime = dateTime.AddHours(offsetMultiplier * hh);
                    dateTime = dateTime.AddMinutes(offsetMultiplier * min);
                }

                return dateTime.ToLocalTime().Prepare();
            }

            return null;
        }

        public static string ToDateTimeString(DateTime dateTime)
        {
            return dateTime.ToStableUniversalTime().ToString(XsdDateTimeFormat);
        }

        public static DateTime ParseDateTime(string dateTimeStr)
        {
            return DateTime.ParseExact(dateTimeStr, XsdDateTimeFormat, null);
        }

        public static DateTimeOffset ParseDateTimeOffset(string dateTimeOffsetStr)
        {
            if (string.IsNullOrEmpty(dateTimeOffsetStr)) return default(DateTimeOffset);

            // for interop, do not assume format based on config
            // format: prefer TimestampOffset, DCJSCompatible
            if (dateTimeOffsetStr.StartsWith(EscapedWcfJsonPrefix, StringComparison.Ordinal) ||
                dateTimeOffsetStr.StartsWith(WcfJsonPrefix, StringComparison.Ordinal))
            {
                return ParseWcfJsonDateOffset(dateTimeOffsetStr);
            }

            // format: next preference ISO8601
            // assume utc when no offset specified
            if (dateTimeOffsetStr.LastIndexOfAny(TimeZoneChars) < 10)
            {
                if (!dateTimeOffsetStr.EndsWith("Z")) dateTimeOffsetStr += "Z";
#if __MonoCS__
                // Without that Mono uses a Local timezone))
                dateTimeOffsetStr = dateTimeOffsetStr.Substring(0, dateTimeOffsetStr.Length - 1) + "+00:00"; 
#endif
            }

            return DateTimeOffset.Parse(dateTimeOffsetStr, CultureInfo.InvariantCulture);
        }

        public static string ToXsdDateTimeString(DateTime dateTime)
        {
#if NETFX_CORE
            return XmlConvert.ToString(dateTime.ToStableUniversalTime(), XsdDateTimeFormat);
#else
            return XmlConvert.ToString(dateTime.ToStableUniversalTime(), XmlDateTimeSerializationMode.Utc);
#endif
        }

        public static string ToXsdTimeSpanString(TimeSpan timeSpan)
        {
            var r = XmlConvert.ToString(timeSpan);
#if __MonoCS__
            // Mono returns DT even if time is 00:00:00
            if (r.EndsWith("DT")) return r.Substring(0, r.Length - 1);
#endif
            return r;
        }

        public static string ToXsdTimeSpanString(TimeSpan? timeSpan)
        {
            return (timeSpan != null) ? ToXsdTimeSpanString(timeSpan.Value) : null;
        }

        public static DateTime ParseXsdDateTime(string dateTimeStr)
        {
#if NETFX_CORE
            return XmlConvert.ToDateTimeOffset(dateTimeStr).DateTime;
#else
            return XmlConvert.ToDateTime(dateTimeStr, XmlDateTimeSerializationMode.Utc);
#endif
        }

        public static TimeSpan ParseTimeSpan(string dateTimeStr)
        {
            return dateTimeStr.StartsWith("P", StringComparison.Ordinal) || dateTimeStr.StartsWith("-P", StringComparison.Ordinal)
                ? ParseXsdTimeSpan(dateTimeStr)
                : TimeSpan.Parse(dateTimeStr);
        }

        public static TimeSpan ParseXsdTimeSpan(string dateTimeStr)
        {
            return XmlConvert.ToTimeSpan(dateTimeStr);
        }

        public static TimeSpan? ParseNullableTimeSpan(string dateTimeStr)
        {
            return string.IsNullOrEmpty(dateTimeStr)
                ? (TimeSpan?)null
                : ParseTimeSpan(dateTimeStr);
        }

        public static TimeSpan? ParseXsdNullableTimeSpan(string dateTimeStr)
        {
            return String.IsNullOrEmpty(dateTimeStr) ?
                null :
                new TimeSpan?(XmlConvert.ToTimeSpan(dateTimeStr));
        }

        public static string ToShortestXsdDateTimeString(DateTime dateTime)
        {
            var timeOfDay = dateTime.TimeOfDay;

            if (timeOfDay.Ticks == 0)
                return dateTime.ToString(ShortDateTimeFormat);

            if (timeOfDay.Milliseconds == 0)
                return dateTime.Kind != DateTimeKind.Utc
                    ? dateTime.ToString(DateTimeFormatSecondsUtcOffset)
                    : dateTime.ToStableUniversalTime().ToString(XsdDateTimeFormatSeconds);

            return dateTime.Kind != DateTimeKind.Utc
                ? dateTime.ToString(DateTimeFormatTicksUtcOffset)
                : ToXsdDateTimeString(dateTime);
        }

        static readonly char[] TimeZoneChars = new[] { '+', '-' };

        /// <summary>
        /// WCF Json format: /Date(unixts+0000)/
        /// </summary>
        /// <param name="wcfJsonDate"></param>
        /// <returns></returns>
        public static DateTimeOffset ParseWcfJsonDateOffset(string wcfJsonDate)
        {
            if (wcfJsonDate[0] == '\\')
            {
                wcfJsonDate = wcfJsonDate.Substring(1);
            }

            var suffixPos = wcfJsonDate.IndexOf(WcfJsonSuffix);
            var timeString = (suffixPos < 0) ? wcfJsonDate : wcfJsonDate.Substring(WcfJsonPrefix.Length, suffixPos - WcfJsonPrefix.Length);

            // for interop, do not assume format based on config
            if (!wcfJsonDate.StartsWith(WcfJsonPrefix, StringComparison.Ordinal))
            {
                return DateTimeOffset.Parse(timeString, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            }

            var timeZonePos = timeString.LastIndexOfAny(TimeZoneChars);
            var timeZone = timeZonePos <= 0 ? string.Empty : timeString.Substring(timeZonePos);
            var unixTimeString = timeString.Substring(0, timeString.Length - timeZone.Length);

            var unixTime = long.Parse(unixTimeString);

            if (timeZone == string.Empty)
            {
                // when no timezone offset is supplied, then treat the time as UTC
                return unixTime.FromUnixTimeMs();
            }

            // DCJS ignores the offset and considers it local time if any offset exists
            // REVIEW: DCJS shoves offset in a separate field 'offsetMinutes', we have the offset in the format, so shouldn't we use it?
            if (JsConfig.DateHandler == JsonDateHandler.DCJSCompatible || timeZone == UnspecifiedOffset)
            {
                return unixTime.FromUnixTimeMs().ToLocalTime();
            }

            var offset = timeZone.FromTimeOffsetString();
            var date = unixTime.FromUnixTimeMs();
            return new DateTimeOffset(date.Ticks, offset);
        }

        /// <summary>
        /// WCF Json format: /Date(unixts+0000)/
        /// </summary>
        /// <param name="wcfJsonDate"></param>
        /// <returns></returns>
        public static DateTime ParseWcfJsonDate(string wcfJsonDate)
        {
            if (wcfJsonDate[0] == JsonUtils.EscapeChar)
            {
                wcfJsonDate = wcfJsonDate.Substring(1);
            }

            var suffixPos = wcfJsonDate.IndexOf(WcfJsonSuffix);
            var timeString = wcfJsonDate.Substring(WcfJsonPrefix.Length, suffixPos - WcfJsonPrefix.Length);

            // for interop, do not assume format based on config
            if (!wcfJsonDate.StartsWith(WcfJsonPrefix, StringComparison.Ordinal))
            {
                return DateTime.Parse(timeString, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            }

            var timeZonePos = timeString.LastIndexOfAny(TimeZoneChars);
            var timeZone = timeZonePos <= 0 ? string.Empty : timeString.Substring(timeZonePos);
            var unixTimeString = timeString.Substring(0, timeString.Length - timeZone.Length);

            var unixTime = long.Parse(unixTimeString);

            if (timeZone == string.Empty)
            {
                // when no timezone offset is supplied, then treat the time as UTC
                return unixTime.FromUnixTimeMs();
            }

            // DCJS ignores the offset and considers it local time if any offset exists
            if (JsConfig.DateHandler == JsonDateHandler.DCJSCompatible || timeZone == UnspecifiedOffset)
            {
                return unixTime.FromUnixTimeMs().ToLocalTime();
            }

            var offset = timeZone.FromTimeOffsetString();
            var date = unixTime.FromUnixTimeMs(offset);
            return new DateTimeOffset(date, offset).DateTime;
        }

        private static TimeZoneInfo LocalTimeZone = TimeZoneInfo.Local;
        public static void WriteWcfJsonDate(TextWriter writer, DateTime dateTime)
        {
            if (JsConfig.AssumeUtc && dateTime.Kind == DateTimeKind.Unspecified)
            {
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            }

            if (JsConfig.DateHandler == JsonDateHandler.ISO8601)
            {
                writer.Write(dateTime.ToString("o", CultureInfo.InvariantCulture));
                return;
            }

            var timestamp = dateTime.ToUnixTimeMs();
            string offset = null;
            if (dateTime.Kind != DateTimeKind.Utc)
            {
                if (JsConfig.DateHandler == JsonDateHandler.TimestampOffset && dateTime.Kind == DateTimeKind.Unspecified)
                    offset = UnspecifiedOffset;
                else
                    offset = LocalTimeZone.GetUtcOffset(dateTime).ToTimeOffsetString();
            }
            else
            {
                // Normally the JsonDateHandler.TimestampOffset doesn't append an offset for Utc dates, but if
                // the JsConfig.AppendUtcOffset is set then we will
                if (JsConfig.DateHandler == JsonDateHandler.TimestampOffset && JsConfig.AppendUtcOffset.HasValue && JsConfig.AppendUtcOffset.Value)
                    offset = UtcOffset;
            }

            writer.Write(EscapedWcfJsonPrefix);
            writer.Write(timestamp);
            if (offset != null)
            {
                writer.Write(offset);
            }
            writer.Write(EscapedWcfJsonSuffix);
        }

        public static string ToWcfJsonDate(DateTime dateTime)
        {
            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                WriteWcfJsonDate(writer, dateTime);
                return sb.ToString();
            }
        }

        public static void WriteWcfJsonDateTimeOffset(TextWriter writer, DateTimeOffset dateTimeOffset)
        {
            if (JsConfig.DateHandler == JsonDateHandler.ISO8601)
            {
                writer.Write(dateTimeOffset.ToString("o", CultureInfo.InvariantCulture));
                return;
            }

            var timestamp = dateTimeOffset.Ticks.ToUnixTimeMs();
            var offset = dateTimeOffset.Offset == TimeSpan.Zero
                ? null
                : dateTimeOffset.Offset.ToTimeOffsetString();

            writer.Write(EscapedWcfJsonPrefix);
            writer.Write(timestamp);
            if (offset != null)
            {
                writer.Write(offset);
            }
            writer.Write(EscapedWcfJsonSuffix);
        }

        public static string ToWcfJsonDateTimeOffset(DateTimeOffset dateTimeOffset)
        {
            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                WriteWcfJsonDateTimeOffset(writer, dateTimeOffset);
                return sb.ToString();
            }
        }
    }
}