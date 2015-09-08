using System;
using System.Linq;
using System.Collections.Generic;
using ServiceStack.Text.Common;
using System.Reflection;

namespace ServiceStack.Text
{
    public static class CsvConfig
    {
		static CsvConfig()
		{
            Reset();
		}

		[ThreadStatic]
		private static string tsItemSeperatorString;
		private static string sItemSeperatorString;
		public static string ItemSeperatorString
		{
			get
			{
				return tsItemSeperatorString ?? sItemSeperatorString ?? JsWriter.ItemSeperatorString;
			}
			set
			{
				tsItemSeperatorString = value;
				if (sItemSeperatorString == null) sItemSeperatorString = value;
                ResetEscapeStrings();
			}
		}

		[ThreadStatic]
		private static string tsItemDelimiterString;
		private static string sItemDelimiterString;
		public static string ItemDelimiterString
		{
			get
			{
				return tsItemDelimiterString ?? sItemDelimiterString ?? JsWriter.QuoteString;
			}
			set
			{
				tsItemDelimiterString = value;
				if (sItemDelimiterString == null) sItemDelimiterString = value;
			    EscapedItemDelimiterString = value + value;
			    ResetEscapeStrings();
			}
		}

        private const string DefaultEscapedItemDelimiterString = JsWriter.QuoteString + JsWriter.QuoteString;

        [ThreadStatic]
		private static string tsEscapedItemDelimiterString;
		private static string sEscapedItemDelimiterString;
		internal static string EscapedItemDelimiterString
		{
			get
			{
				return tsEscapedItemDelimiterString ?? sEscapedItemDelimiterString ?? DefaultEscapedItemDelimiterString;
			}
			set
			{
				tsEscapedItemDelimiterString = value;
				if (sEscapedItemDelimiterString == null) sEscapedItemDelimiterString = value;
			}
		}

        private static readonly string[] defaultEscapeStrings = GetEscapeStrings();

		[ThreadStatic]
		private static string[] tsEscapeStrings;
		private static string[] sEscapeStrings;
		public static string[] EscapeStrings
		{
			get
			{
				return tsEscapeStrings ?? sEscapeStrings ?? defaultEscapeStrings;
			}
            private set
            {
				tsEscapeStrings = value;
				if (sEscapeStrings == null) sEscapeStrings = value;
            }
		}

        private static string[] GetEscapeStrings()
        {
            return new[] {ItemDelimiterString, ItemSeperatorString, RowSeparatorString, "\r", "\n"};
        }

        private static void ResetEscapeStrings()
        {
            EscapeStrings = GetEscapeStrings();
        }

		[ThreadStatic]
		private static string tsRowSeparatorString;
		private static string sRowSeparatorString;
		public static string RowSeparatorString
		{
			get
			{
			    return tsRowSeparatorString ?? sRowSeparatorString ?? Environment.NewLine;
			}
		    set
			{
				tsRowSeparatorString = value;
				if (sRowSeparatorString == null) sRowSeparatorString = value;
                ResetEscapeStrings();
			}
		}
       
		public static void Reset()
		{
			tsItemSeperatorString = sItemSeperatorString = null;
			tsItemDelimiterString = sItemDelimiterString = null;
			tsEscapedItemDelimiterString = sEscapedItemDelimiterString = null;
			tsRowSeparatorString = sRowSeparatorString = null;
		    tsEscapeStrings = sEscapeStrings = null;
		}

    }

	public static class CsvConfig<T>
	{
		public static bool OmitHeaders { get; set; }

		private static Dictionary<string, string> customHeadersMap;
		public static Dictionary<string, string> CustomHeadersMap
		{
			get
			{
				return customHeadersMap;
			}
			set
			{
				customHeadersMap = value;
				if (value == null) return;
				CsvWriter<T>.ConfigureCustomHeaders(customHeadersMap);
			}
		}

		public static object CustomHeaders
		{
			set
			{
				if (value == null) return;
                if (value.GetType().IsValueType())
                    throw new ArgumentException("CustomHeaders is a ValueType");

                var propertyInfos = value.GetType().GetPropertyInfos();
                if (propertyInfos.Length == 0) return;

				customHeadersMap = new Dictionary<string, string>();
				foreach (var pi in propertyInfos)
				{
                    var getMethod = pi.GetMethodInfo();
                    if (getMethod == null) continue;

					var oValue = getMethod.Invoke(value, new object[0]);
					if (oValue == null) continue;
					customHeadersMap[pi.Name] = oValue.ToString();
				}
				CsvWriter<T>.ConfigureCustomHeaders(customHeadersMap);
			}
		}

		public static void Reset()
		{
			OmitHeaders = false;
			CsvWriter<T>.Reset();
		}
	}
}