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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using ServiceStack.Text.Common;

namespace ServiceStack.Text.Json
{
	internal static class JsonWriter
	{
		public static readonly JsWriter<JsonTypeSerializer> Instance = new JsWriter<JsonTypeSerializer>();

		private static Dictionary<Type, WriteObjectDelegate> WriteFnCache = new Dictionary<Type, WriteObjectDelegate>();

        internal static void RemoveCacheFn(Type forType)
        {
            Dictionary<Type, WriteObjectDelegate> snapshot, newCache;
            do
            {
                snapshot = WriteFnCache;
                newCache = new Dictionary<Type, WriteObjectDelegate>(WriteFnCache);
                newCache.Remove(forType);
                
            } while (!ReferenceEquals(
                Interlocked.CompareExchange(ref WriteFnCache, newCache, snapshot), snapshot));
        }

		public static WriteObjectDelegate GetWriteFn(Type type)
		{
			try
			{
				WriteObjectDelegate writeFn;
				if (WriteFnCache.TryGetValue(type, out writeFn)) return writeFn;

				var genericType = typeof(JsonWriter<>).MakeGenericType(type);
                var mi = genericType.GetPublicStaticMethod("WriteFn");
                var writeFactoryFn = (Func<WriteObjectDelegate>)mi.MakeDelegate(typeof(Func<WriteObjectDelegate>));
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

		private static Dictionary<Type, TypeInfo> JsonTypeInfoCache = new Dictionary<Type, TypeInfo>();

		public static TypeInfo GetTypeInfo(Type type)
		{
			try
			{
				TypeInfo writeFn;
				if (JsonTypeInfoCache.TryGetValue(type, out writeFn)) return writeFn;

				var genericType = typeof(JsonWriter<>).MakeGenericType(type);
                var mi = genericType.GetPublicStaticMethod("GetTypeInfo");
                var writeFactoryFn = (Func<TypeInfo>)mi.MakeDelegate(typeof(Func<TypeInfo>));
                writeFn = writeFactoryFn();

				Dictionary<Type, TypeInfo> snapshot, newCache;
				do
				{
					snapshot = JsonTypeInfoCache;
					newCache = new Dictionary<Type, TypeInfo>(JsonTypeInfoCache);
					newCache[type] = writeFn;

				} while (!ReferenceEquals(
					Interlocked.CompareExchange(ref JsonTypeInfoCache, newCache, snapshot), snapshot));

				return writeFn;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static void WriteLateBoundObject(TextWriter writer, object value)
		{
			if (value == null)
			{
				writer.Write(JsonUtils.Null);
				return;
			}

			var type = value.GetType();
			var writeFn = type == typeof(object)
				? WriteType<object, JsonTypeSerializer>.WriteObjectType
				: GetWriteFn(type);

			var prevState = JsState.IsWritingDynamic;
			JsState.IsWritingDynamic = true;
			writeFn(writer, value);
			JsState.IsWritingDynamic = prevState;
		}

		public static WriteObjectDelegate GetValueTypeToStringMethod(Type type)
		{
			return Instance.GetValueTypeToStringMethod(type);
		}
	}

	internal class TypeInfo
	{
        internal bool EncodeMapKey;
        internal bool IsNumeric;
    }

	/// <summary>
	/// Implement the serializer using a more static approach
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal static class JsonWriter<T>
	{
		internal static TypeInfo TypeInfo;
		private static WriteObjectDelegate CacheFn;

        public static void Reset()
        {
            JsonWriter.RemoveCacheFn(typeof(T));

            CacheFn = typeof(T) == typeof(object) 
                ? JsonWriter.WriteLateBoundObject 
                : JsonWriter.Instance.GetWriteFn<T>();
        }

		public static WriteObjectDelegate WriteFn()
		{
			return CacheFn ?? WriteObject;
		}

		public static TypeInfo GetTypeInfo()
		{
			return TypeInfo;
		}

		static JsonWriter()
		{
		    var isNumeric = typeof(T).IsNumericType();
			TypeInfo = new TypeInfo {
                EncodeMapKey = typeof(T) == typeof(bool) || isNumeric,
                IsNumeric = isNumeric
			};

            CacheFn = typeof(T) == typeof(object) 
                ? JsonWriter.WriteLateBoundObject 
                : JsonWriter.Instance.GetWriteFn<T>();
		}

        public static void WriteObject(TextWriter writer, object value)
        {
#if MONOTOUCH
			if (writer == null) return;
#endif
            try
            {
                if (++JsState.Depth > JsConfig.MaxDepth)
                    return;

                CacheFn(writer, value);
            }
            finally
            {
                JsState.Depth--;
            }
        }

        public static void WriteRootObject(TextWriter writer, object value)
        {
#if MONOTOUCH
			if (writer == null) return;
#endif
            JsState.Depth = 0;
            CacheFn(writer, value);
        }
    }

}