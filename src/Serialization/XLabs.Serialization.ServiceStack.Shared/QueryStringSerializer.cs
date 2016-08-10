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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Linq;
using ServiceStack.Text.Common;
using ServiceStack.Text.Jsv;

namespace ServiceStack.Text
{
	/// <summary>
	/// Class QueryStringSerializer.
	/// </summary>
	public static class QueryStringSerializer
	{
		/// <summary>
		/// The instance
		/// </summary>
		internal static readonly JsWriter<JsvTypeSerializer> Instance = new JsWriter<JsvTypeSerializer>();

		/// <summary>
		/// The write function cache
		/// </summary>
		private static Dictionary<Type, WriteObjectDelegate> WriteFnCache = new Dictionary<Type, WriteObjectDelegate>();

		/// <summary>
		/// Gets the write function.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>WriteObjectDelegate.</returns>
		internal static WriteObjectDelegate GetWriteFn(Type type)
		{
			try
			{
				WriteObjectDelegate writeFn;
                if (WriteFnCache.TryGetValue(type, out writeFn)) return writeFn;

                var genericType = typeof(QueryStringWriter<>).MakeGenericType(type);
                var mi = genericType.GetPublicStaticMethod("WriteFn");
                var writeFactoryFn = (Func<WriteObjectDelegate>)mi.MakeDelegate(
                    typeof(Func<WriteObjectDelegate>));

                writeFn = writeFactoryFn();

                Dictionary<Type, WriteObjectDelegate> snapshot, newCache;
                do
                {
                    snapshot = WriteFnCache;
                    newCache = new Dictionary<Type, WriteObjectDelegate>(WriteFnCache);
                    newCache[type] = writeFn;

                } while (!ReferenceEquals(
                    Interlocked.CompareExchange(ref WriteFnCache, newCache, snapshot), snapshot));
                
                return writeFn;
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Writes the late bound object.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="value">The value.</param>
		public static void WriteLateBoundObject(TextWriter writer, object value)
		{
			if (value == null) return;
			var writeFn = GetWriteFn(value.GetType());
			writeFn(writer, value);
		}

		/// <summary>
		/// Gets the value type to string method.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>WriteObjectDelegate.</returns>
		internal static WriteObjectDelegate GetValueTypeToStringMethod(Type type)
		{
			return Instance.GetValueTypeToStringMethod(type);
		}

		/// <summary>
		/// Serializes to string.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <returns>System.String.</returns>
		public static string SerializeToString<T>(T value)
		{
			var sb = new StringBuilder();
			using (var writer = new StringWriter(sb, CultureInfo.InvariantCulture))
			{
				GetWriteFn(value.GetType())(writer, value);
			}
			return sb.ToString();
		}
	}

	/// <summary>
	/// Implement the serializer using a more static approach
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public static class QueryStringWriter<T>
	{
		/// <summary>
		/// The cache function
		/// </summary>
		private static readonly WriteObjectDelegate CacheFn;

		/// <summary>
		/// Writes the function.
		/// </summary>
		/// <returns>WriteObjectDelegate.</returns>
		public static WriteObjectDelegate WriteFn()
		{
			return CacheFn;
		}

		/// <summary>
		/// Initializes static members of the <see cref="QueryStringWriter{T}"/> class.
		/// </summary>
		static QueryStringWriter()
		{
			if (typeof(T) == typeof(object))
			{
				CacheFn = QueryStringSerializer.WriteLateBoundObject;
			}
            else if (typeof (T).AssignableFrom(typeof (IDictionary))
                || typeof (T).HasInterface(typeof (IDictionary)))
            {
                CacheFn = WriteIDictionary;
            }
			else
			{
                var isEnumerable = typeof(T).AssignableFrom(typeof(IEnumerable))
                    || typeof(T).HasInterface(typeof(IEnumerable));

                if ((typeof(T).IsClass() || typeof(T).IsInterface()) 
                    && !isEnumerable)
                {
					var canWriteType = WriteType<T, JsvTypeSerializer>.Write;
					if (canWriteType != null)
					{
						CacheFn = WriteType<T, JsvTypeSerializer>.WriteQueryString;
						return;
					}
				}

				CacheFn = QueryStringSerializer.Instance.GetWriteFn<T>();
			}
		}

		/// <summary>
		/// Writes the object.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="value">The value.</param>
		public static void WriteObject(TextWriter writer, object value)
		{
			if (writer == null) return;
			CacheFn(writer, value);
		}

		/// <summary>
		/// The serializer
		/// </summary>
		private static readonly ITypeSerializer Serializer = JsvTypeSerializer.Instance;
		/// <summary>
		/// Writes the i dictionary.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="oMap">The o map.</param>
		public static void WriteIDictionary(TextWriter writer, object oMap)
        {
            WriteObjectDelegate writeKeyFn = null;
            WriteObjectDelegate writeValueFn = null;

            try
            {
                JsState.QueryStringMode = true;

                var map = (IDictionary)oMap;
                var ranOnce = false;
                foreach (var key in map.Keys)
                {
                    var dictionaryValue = map[key];
                    if (dictionaryValue == null) continue;

                    if (writeKeyFn == null)
                    {
                        var keyType = key.GetType();
                        writeKeyFn = Serializer.GetWriteFn(keyType);
                    }

                    if (writeValueFn == null)
                        writeValueFn = Serializer.GetWriteFn(dictionaryValue.GetType());

                    if (ranOnce)
                        writer.Write("&");
                    else
                        ranOnce = true;

                    JsState.WritingKeyCount++;
                    JsState.IsWritingValue = false;

                    writeKeyFn(writer, key);

                    JsState.WritingKeyCount--;

                    writer.Write("=");

                    JsState.IsWritingValue = true;
                    writeValueFn(writer, dictionaryValue);
                    JsState.IsWritingValue = false;
                }
            }
            finally 
            {
                JsState.QueryStringMode = false;
            }
        }
    }
	
}