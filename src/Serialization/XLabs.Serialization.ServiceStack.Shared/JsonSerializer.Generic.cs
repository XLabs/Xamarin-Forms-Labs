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
using System.Reflection;
using ServiceStack.Text.Common;
using ServiceStack.Text.Json;

namespace ServiceStack.Text
{
	/// <summary>
	/// Class JsonSerializer.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class JsonSerializer<T> : ITypeSerializer<T>
	{
		/// <summary>
		/// Determines whether this serializer can create the specified type from a string.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns><c>true</c> if this instance [can create from string] the specified type; otherwise, <c>false</c>.</returns>
		public bool CanCreateFromString(Type type)
		{
			return JsonReader.GetParseFn(type) != null;
		}

		/// <summary>
		/// Parses the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>T.</returns>
		public T DeserializeFromString(string value)
		{
			if (string.IsNullOrEmpty(value)) return default(T);
			return (T)JsonReader<T>.Parse(value);
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
            if (typeof(T) == typeof(object) || typeof(T).IsAbstract() || typeof(T).IsInterface())
            {
                if (typeof(T).IsAbstract() || typeof(T).IsInterface()) JsState.IsWritingDynamic = true;
                var result = JsonSerializer.SerializeToString(value, value.GetType());
                if (typeof(T).IsAbstract() || typeof(T).IsInterface()) JsState.IsWritingDynamic = false;
                return result;
            }

			var sb = new StringBuilder();
			using (var writer = new StringWriter(sb))
			{
				JsonWriter<T>.WriteObject(writer, value);
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
            if (typeof(T) == typeof(object) || typeof(T).IsAbstract() || typeof(T).IsInterface())
            {
                if (typeof(T).IsAbstract() || typeof(T).IsInterface()) JsState.IsWritingDynamic = true;
                JsonSerializer.SerializeToWriter(value, value.GetType(), writer);
                if (typeof(T).IsAbstract() || typeof(T).IsInterface()) JsState.IsWritingDynamic = false;
                return;
            }
           
            JsonWriter<T>.WriteObject(writer, value);
		}
	}
}