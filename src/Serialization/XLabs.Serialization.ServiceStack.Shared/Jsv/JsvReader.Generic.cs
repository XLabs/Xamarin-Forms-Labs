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
using System.Reflection;
using System.Threading;
using System.Linq;
using ServiceStack.Text.Common;

namespace ServiceStack.Text.Jsv
{
	/// <summary>
	/// Class JsvReader.
	/// </summary>
	public static class JsvReader
	{
		/// <summary>
		/// The instance
		/// </summary>
		internal static readonly JsReader<JsvTypeSerializer> Instance = new JsReader<JsvTypeSerializer>();

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

            var genericType = typeof(JsvReader<>).MakeGenericType(type);
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
	/// Class JsvReader.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public static class JsvReader<T>
	{
		/// <summary>
		/// The read function
		/// </summary>
		private static readonly ParseStringDelegate ReadFn;

		/// <summary>
		/// Initializes static members of the <see cref="JsvReader{T}"/> class.
		/// </summary>
		static JsvReader()
		{
			ReadFn = JsvReader.Instance.GetParseFn<T>();
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
                if (typeof(T).IsInterface())
                {
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