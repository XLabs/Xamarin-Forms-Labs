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
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using ServiceStack.Text.Common;

namespace ServiceStack.Text.Json
{
	/// <summary>
	/// Class JsonReader.
	/// </summary>
	internal static class JsonReader
	{
		/// <summary>
		/// The instance
		/// </summary>
		public static readonly JsReader<JsonTypeSerializer> Instance = new JsReader<JsonTypeSerializer>();

		/// <summary>
		/// The parse function cache
		/// </summary>
		private static Dictionary<Type, ParseFactoryDelegate> ParseFnCache = new Dictionary<Type, ParseFactoryDelegate>();

		/// <summary>
		/// Gets the parse function.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>ParseStringDelegate.</returns>
		public static ParseStringDelegate GetParseFn(Type type)
		{
			ParseFactoryDelegate parseFactoryFn;
            ParseFnCache.TryGetValue(type, out parseFactoryFn);

            if (parseFactoryFn != null) return parseFactoryFn();

            var genericType = typeof(JsonReader<>).MakeGenericType(type);
            var mi = genericType.GetPublicStaticMethod("GetParseFn");
            parseFactoryFn = (ParseFactoryDelegate)mi.MakeDelegate(typeof(ParseFactoryDelegate));

            Dictionary<Type, ParseFactoryDelegate> snapshot, newCache;
            do
            {
                snapshot = ParseFnCache;
                newCache = new Dictionary<Type, ParseFactoryDelegate>(ParseFnCache);
                newCache[type] = parseFactoryFn;

            } while (!ReferenceEquals(
                Interlocked.CompareExchange(ref ParseFnCache, newCache, snapshot), snapshot));
            
            return parseFactoryFn();
		}
	}

	/// <summary>
	/// Class JsonReader.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public static class JsonReader<T>
	{
		/// <summary>
		/// The read function
		/// </summary>
		private static readonly ParseStringDelegate ReadFn;

		/// <summary>
		/// Initializes static members of the <see cref="JsonReader{T}"/> class.
		/// </summary>
		static JsonReader()
		{
			ReadFn = JsonReader.Instance.GetParseFn<T>();
		}

		/// <summary>
		/// Gets the parse function.
		/// </summary>
		/// <returns>ParseStringDelegate.</returns>
		public static ParseStringDelegate GetParseFn()
		{
			return ReadFn ?? Parse;
		}

		/// <summary>
		/// Parses the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>System.Object.</returns>
		/// <exception cref="System.NotSupportedException">Can not deserialize interface type: 
		/// 						+ typeof(T).Name</exception>
		public static object Parse(string value)
		{
			if (ReadFn == null)
			{
                if (typeof(T).IsAbstract() || typeof(T).IsInterface())
                {
					if (string.IsNullOrEmpty(value)) return null;
					var concreteType = DeserializeType<JsonTypeSerializer>.ExtractType(value);
					if (concreteType != null)
					{
						return JsonReader.GetParseFn(concreteType)(value);
					}
					throw new NotSupportedException("Can not deserialize interface type: "
						+ typeof(T).Name);
				}
			}
			return value == null 
			       	? null 
			       	: ReadFn(value);
		}
	}
}