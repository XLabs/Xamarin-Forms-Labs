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

using System.Collections.Generic;
using System.IO;

namespace ServiceStack.Text
{
	/// <summary>
	/// Class CsvStreamExtensions.
	/// </summary>
	public static class CsvStreamExtensions
	{
		/// <summary>
		/// Writes the CSV.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="outputStream">The output stream.</param>
		/// <param name="records">The records.</param>
		public static void WriteCsv<T>(this Stream outputStream, IEnumerable<T> records)
		{
			using (var textWriter = new StreamWriter(outputStream))
			{
				textWriter.WriteCsv(records);
			}
		}

		/// <summary>
		/// Writes the CSV.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="writer">The writer.</param>
		/// <param name="records">The records.</param>
		public static void WriteCsv<T>(this TextWriter writer, IEnumerable<T> records)
		{
			CsvWriter<T>.Write(writer, records);
		}

	}
}