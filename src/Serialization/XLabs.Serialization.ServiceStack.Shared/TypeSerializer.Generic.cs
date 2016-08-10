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
using System.IO;
using System.Text;
using ServiceStack.Text.Jsv;

namespace ServiceStack.Text
{
	/// <summary>
	/// Class TypeSerializer.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class TypeSerializer<T> : ITypeSerializer<T>
	{
		/// <summary>
		/// Determines whether this serializer can create the specified type from a string.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns><c>true</c> if this instance [can create from string] the specified type; otherwise, <c>false</c>.</returns>
		public bool CanCreateFromString(Type type)
		{
			return JsvReader.GetParseFn(type) != null;
		}

		/// <summary>
		/// Parses the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>T.</returns>
		public T DeserializeFromString(string value)
		{
			if (string.IsNullOrEmpty(value)) return default(T);
			return (T)JsvReader<T>.Parse(value);
		}

		/// <summary>
		/// Deserializes from reader.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns>T.</returns>
		public T DeserializeFromReader(TextReader reader)
		{
			return DeserializeFromString(reader.ReadToEnd());
		}

		/// <summary>
		/// Serializes to string.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>System.String.</returns>
		public string SerializeToString(T value)
		{
			if (value == null) return null;
			if (typeof(T) == typeof(string)) return value as string;

			var sb = new StringBuilder();
			using (var writer = new StringWriter(sb))
			{
				JsvWriter<T>.WriteObject(writer, value);
			}
			return sb.ToString();
		}

		/// <summary>
		/// Serializes to writer.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="writer">The writer.</param>
		public void SerializeToWriter(T value, TextWriter writer)
		{
			if (value == null) return;
			if (typeof(T) == typeof(string))
			{
				writer.Write(value);
				return;
			}

			JsvWriter<T>.WriteObject(writer, value);
		}
	}
}