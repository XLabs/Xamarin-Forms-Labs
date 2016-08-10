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
using System.Collections.Generic;
using System.Text;
using ServiceStack.Text.Common;

namespace ServiceStack.Text
{
	/// <summary>
	/// Class TextExtensions.
	/// </summary>
	public static class TextExtensions
	{
		/// <summary>
		/// To the CSV field.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns>System.String.</returns>
		public static string ToCsvField(this string text)
        {
            return string.IsNullOrEmpty(text) || !CsvWriter.HasAnyEscapeChars(text)
                       ? text
                       : string.Concat
                             (
                                 CsvConfig.ItemDelimiterString,
                                 text.Replace(CsvConfig.ItemDelimiterString, CsvConfig.EscapedItemDelimiterString),
                                 CsvConfig.ItemDelimiterString
                             );
        }

		/// <summary>
		/// To the CSV field.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns>System.Object.</returns>
		public static object ToCsvField(this object text)
        {
            return text == null || !JsWriter.HasAnyEscapeChars(text.ToString())
                       ? text
                       : string.Concat
                             (
                                 JsWriter.QuoteString,
                                 text.ToString().Replace(JsWriter.QuoteString, TypeSerializer.DoubleQuoteString),
                                 JsWriter.QuoteString
                             );
        }

		/// <summary>
		/// Froms the CSV field.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns>System.String.</returns>
		public static string FromCsvField(this string text)
		{
            return string.IsNullOrEmpty(text) || !text.StartsWith(CsvConfig.ItemDelimiterString, StringComparison.Ordinal)
			       	? text
					: text.Substring(CsvConfig.ItemDelimiterString.Length, text.Length - CsvConfig.EscapedItemDelimiterString.Length)
						.Replace(CsvConfig.EscapedItemDelimiterString, CsvConfig.ItemDelimiterString);
		}

		/// <summary>
		/// Froms the CSV fields.
		/// </summary>
		/// <param name="texts">The texts.</param>
		/// <returns>List&lt;System.String&gt;.</returns>
		public static List<string> FromCsvFields(this IEnumerable<string> texts)
		{
			var safeTexts = new List<string>();
			foreach (var text in texts)
			{
				safeTexts.Add(FromCsvField(text));
			}
			return safeTexts;
		}

		/// <summary>
		/// Froms the CSV fields.
		/// </summary>
		/// <param name="texts">The texts.</param>
		/// <returns>System.String[].</returns>
		public static string[] FromCsvFields(params string[] texts)
		{
			var textsLen = texts.Length;
			var safeTexts = new string[textsLen];
			for (var i = 0; i < textsLen; i++)
			{
				safeTexts[i] = FromCsvField(texts[i]);
			}
			return safeTexts;
		}

		/// <summary>
		/// Serializes to string.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <returns>System.String.</returns>
		public static string SerializeToString<T>(this T value)
		{
			return JsonSerializer.SerializeToString(value);
		}
	}
}