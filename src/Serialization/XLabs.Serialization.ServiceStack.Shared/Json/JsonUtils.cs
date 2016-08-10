// ***********************************************************************
// Assembly         : XLabs.Serialization.ServiceStack.WP8
// Author           : rmarinho
// Created          : 09-08-2015
//
// Last Modified By : rmarinho
// Last Modified On : 09-08-2015
// ***********************************************************************
// <copyright file="JsonUtils.cs" company="XLabs">
//     Copyright © ServiceStack 2013 & XLabs
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.IO;

namespace ServiceStack.Text.Json
{
	/// <summary>
	/// Class JsonUtils.
	/// </summary>
	public static class JsonUtils
	{
		/// <summary>
		/// The escape character
		/// </summary>
		public const char EscapeChar = '\\';
		/// <summary>
		/// The quote character
		/// </summary>
		public const char QuoteChar = '"';
		/// <summary>
		/// The null
		/// </summary>
		public const string Null = "null";
		/// <summary>
		/// The true
		/// </summary>
		public const string True = "true";
		/// <summary>
		/// The false
		/// </summary>
		public const string False = "false";

		/// <summary>
		/// The escape chars
		/// </summary>
		static readonly char[] EscapeChars = new[]
			{
				QuoteChar, '\n', '\r', '\t', '"', '\\', '\f', '\b',
			};

		/// <summary>
		/// The length from largest character
		/// </summary>
		private const int LengthFromLargestChar = '\\' + 1;
		/// <summary>
		/// The escape character flags
		/// </summary>
		private static readonly bool[] EscapeCharFlags = new bool[LengthFromLargestChar];

		/// <summary>
		/// Initializes static members of the <see cref="JsonUtils"/> class.
		/// </summary>
		static JsonUtils()
		{
			foreach (var escapeChar in EscapeChars)
			{
				EscapeCharFlags[escapeChar] = true;
			}
		}

		/// <summary>
		/// Writes the string.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="value">The value.</param>
		public static void WriteString(TextWriter writer, string value)
		{
			if (value == null)
			{
				writer.Write(JsonUtils.Null);
				return;
			}
			if (!HasAnyEscapeChars(value))
			{
				writer.Write(QuoteChar);
				writer.Write(value);
				writer.Write(QuoteChar);
				return;
			}

			var hexSeqBuffer = new char[4];
			writer.Write(QuoteChar);

			var len = value.Length;
            for (var i = 0; i < len; i++)
            {
                switch (value[i])
                {
                    case '\n':
                        writer.Write("\\n");
                        continue;

                    case '\r':
                        writer.Write("\\r");
                        continue;

                    case '\t':
                        writer.Write("\\t");
                        continue;

                    case '"':
                    case '\\':
                        writer.Write('\\');
                        writer.Write(value[i]);
                        continue;

                    case '\f':
                        writer.Write("\\f");
                        continue;

                    case '\b':
                        writer.Write("\\b");
                        continue;
                }

                //Is printable char?
                if (value[i] >= 32 && value[i] <= 126)
                {
                    writer.Write(value[i]);
                    continue;
                }

                // http://json.org/ spec requires any control char to be escaped
                if (JsConfig.EscapeUnicode || char.IsControl(value[i]))
                {
                    // Default, turn into a \uXXXX sequence
                    IntToHex(value[i], hexSeqBuffer);
                    writer.Write("\\u");
                    writer.Write(hexSeqBuffer);
                }
                else
                    writer.Write(value[i]);
            }

			writer.Write(QuoteChar);
		}

		/// <summary>
		/// micro optimizations: using flags instead of value.IndexOfAny(EscapeChars)
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if [has any escape chars] [the specified value]; otherwise, <c>false</c>.</returns>
		private static bool HasAnyEscapeChars(string value)
		{
			var len = value.Length;
			for (var i = 0; i < len; i++)
			{
				var c = value[i];

                // non-printable
                if (!(value[i] >= 32 && value[i] <= 126)) return true;

				if (c >= LengthFromLargestChar || !EscapeCharFlags[c]) continue;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Ints to hexadecimal.
		/// </summary>
		/// <param name="intValue">The int value.</param>
		/// <param name="hex">The hexadecimal.</param>
		public static void IntToHex(int intValue, char[] hex)
		{
			for (var i = 0; i < 4; i++)
			{
				var num = intValue % 16;

				if (num < 10)
					hex[3 - i] = (char)('0' + num);
				else
					hex[3 - i] = (char)('A' + (num - 10));

				intValue >>= 4;
			}
		}

		/// <summary>
		/// Determines whether [is js object] [the specified value].
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if [is js object] [the specified value]; otherwise, <c>false</c>.</returns>
		public static bool IsJsObject(string value)
		{
			return !string.IsNullOrEmpty(value)
				&& value[0] == '{'
				&& value[value.Length - 1] == '}';
		}

		/// <summary>
		/// Determines whether [is js array] [the specified value].
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if [is js array] [the specified value]; otherwise, <c>false</c>.</returns>
		public static bool IsJsArray(string value)
		{
			return !string.IsNullOrEmpty(value)
				&& value[0] == '['
				&& value[value.Length - 1] == ']';
		}
	}

}