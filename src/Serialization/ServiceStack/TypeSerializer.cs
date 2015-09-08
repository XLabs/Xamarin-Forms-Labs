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
using System.Globalization;
using System.IO;
using System.Text;
using System.Reflection;
using ServiceStack.Text.Common;
using ServiceStack.Text.Jsv;

namespace ServiceStack.Text
{
	/// <summary>
	/// Creates an instance of a Type from a string value
	/// </summary>
	public static class TypeSerializer
	{
		private static readonly UTF8Encoding UTF8EncodingWithoutBom = new UTF8Encoding(false);

		public const string DoubleQuoteString = "\"\"";

		/// <summary>
		/// Determines whether the specified type is convertible from string.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>
		/// 	<c>true</c> if the specified type is convertible from string; otherwise, <c>false</c>.
		/// </returns>
		public static bool CanCreateFromString(Type type)
		{
			return JsvReader.GetParseFn(type) != null;
		}

		/// <summary>
		/// Parses the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static T DeserializeFromString<T>(string value)
		{
			if (string.IsNullOrEmpty(value)) return default(T);
			return (T)JsvReader<T>.Parse(value);
		}

		public static T DeserializeFromReader<T>(TextReader reader)
		{
			return DeserializeFromString<T>(reader.ReadToEnd());
		}

		/// <summary>
		/// Parses the specified type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static object DeserializeFromString(string value, Type type)
		{
			return value == null 
			       	? null 
			       	: JsvReader.GetParseFn(type)(value);
		}

		public static object DeserializeFromReader(TextReader reader, Type type)
		{
			return DeserializeFromString(reader.ReadToEnd(), type);
		}

		public static string SerializeToString<T>(T value)
		{
			if (value == null || value is Delegate) return null;
			if (typeof(T) == typeof(string)) return value as string;
            if (typeof(T) == typeof(object) || typeof(T).IsAbstract() || typeof(T).IsInterface())
            {
                if (typeof(T).IsAbstract() || typeof(T).IsInterface()) JsState.IsWritingDynamic = true;
                var result = SerializeToString(value, value.GetType());
                if (typeof(T).IsAbstract() || typeof(T).IsInterface()) JsState.IsWritingDynamic = false;
                return result;
            }

			var sb = new StringBuilder();
			using (var writer = new StringWriter(sb, CultureInfo.InvariantCulture))
			{
                JsvWriter<T>.WriteRootObject(writer, value);
			}
			return sb.ToString();
		}

		public static string SerializeToString(object value, Type type)
		{
			if (value == null) return null;
			if (type == typeof(string)) return value as string;

			var sb = new StringBuilder();
			using (var writer = new StringWriter(sb, CultureInfo.InvariantCulture))
			{
				JsvWriter.GetWriteFn(type)(writer, value);
			}
			return sb.ToString();
		}

		public static void SerializeToWriter<T>(T value, TextWriter writer)
		{
			if (value == null) return;
			if (typeof(T) == typeof(string))
			{
				writer.Write(value);
				return;
			}
			if (typeof(T) == typeof(object))
			{
                if (typeof(T).IsAbstract() || typeof(T).IsInterface()) JsState.IsWritingDynamic = true;
                SerializeToWriter(value, value.GetType(), writer);
                if (typeof(T).IsAbstract() || typeof(T).IsInterface()) JsState.IsWritingDynamic = false;
                return;
			}

            JsvWriter<T>.WriteRootObject(writer, value);
		}

		public static void SerializeToWriter(object value, Type type, TextWriter writer)
		{
			if (value == null) return;
			if (type == typeof(string))
			{
				writer.Write(value);
				return;
			}

			JsvWriter.GetWriteFn(type)(writer, value);
		}

		public static void SerializeToStream<T>(T value, Stream stream)
		{
			if (value == null) return;
			if (typeof(T) == typeof(object))
			{
                if (typeof(T).IsAbstract() || typeof(T).IsInterface()) JsState.IsWritingDynamic = true;
                SerializeToStream(value, value.GetType(), stream);
                if (typeof(T).IsAbstract() || typeof(T).IsInterface()) JsState.IsWritingDynamic = false;
                return;
			}

			var writer = new StreamWriter(stream, UTF8EncodingWithoutBom);
            JsvWriter<T>.WriteRootObject(writer, value);
			writer.Flush();
		}

		public static void SerializeToStream(object value, Type type, Stream stream)
		{
			var writer = new StreamWriter(stream, UTF8EncodingWithoutBom);
			JsvWriter.GetWriteFn(type)(writer, value);
			writer.Flush();
		}

		public static T Clone<T>(T value)
		{
			var serializedValue = SerializeToString(value);
			var cloneObj = DeserializeFromString<T>(serializedValue);
			return cloneObj;
		}

		public static T DeserializeFromStream<T>(Stream stream)
		{
			using (var reader = new StreamReader(stream, UTF8EncodingWithoutBom))
			{
				return DeserializeFromString<T>(reader.ReadToEnd());
			}
		}

		public static object DeserializeFromStream(Type type, Stream stream)
		{
			using (var reader = new StreamReader(stream, UTF8EncodingWithoutBom))
			{
				return DeserializeFromString(reader.ReadToEnd(), type);
			}
		}

		/// <summary>
		/// Useful extension method to get the Dictionary[string,string] representation of any POCO type.
		/// </summary>
		/// <returns></returns>
		public static Dictionary<string, string> ToStringDictionary<T>(this T obj)
			where T : class
		{
			var jsv = SerializeToString(obj);
			var map = DeserializeFromString<Dictionary<string, string>>(jsv);
			return map;
		}

        /// <summary>
        /// Recursively prints the contents of any POCO object in a human-friendly, readable format
        /// </summary>
        /// <returns></returns>
        public static string Dump<T>(this T instance)
        {
            return SerializeAndFormat(instance);
        }

        /// <summary>
        /// Print Dump to Console.WriteLine
        /// </summary>
        public static void PrintDump<T>(this T instance)
        {
#if NETFX_CORE
            System.Diagnostics.Debug.WriteLine(SerializeAndFormat(instance));
#else
            Console.WriteLine(SerializeAndFormat(instance));
#endif
        }

        /// <summary>
        /// Print string.Format to Console.WriteLine
        /// </summary>
        public static void Print(this string text, params object[] args)
        {
#if NETFX_CORE
            if (args.Length > 0)
                System.Diagnostics.Debug.WriteLine(text, args);
            else
                System.Diagnostics.Debug.WriteLine(text);
#else
            if (args.Length > 0)
                Console.WriteLine(text, args);
            else
                Console.WriteLine(text);
#endif
        }

		public static string SerializeAndFormat<T>(this T instance)
		{
		    var fn = instance as Delegate;
		    if (fn != null)
                return Dump(fn);

			var dtoStr = SerializeToString(instance);
			var formatStr = JsvFormatter.Format(dtoStr);
			return formatStr;
		}

        public static string Dump(this Delegate fn)
        {
            var method = fn.GetType().GetMethod("Invoke");
            var sb = new StringBuilder();
            foreach (var param in method.GetParameters())
            {
                if (sb.Length > 0)
                    sb.Append(", ");

                sb.AppendFormat("{0} {1}", param.ParameterType.Name, param.Name);
            }

            var info = "{0} {1}({2})".Fmt(method.ReturnType.Name, fn.Method.Name, sb);
            return info;
        }

	}
}