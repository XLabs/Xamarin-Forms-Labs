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
using System.Text;
using System.Threading;
using System.Reflection;
using System.Linq;
using ServiceStack.Text.Common;

namespace ServiceStack.Text.Jsv
{
	public class JsvSerializer<T>
	{
		Dictionary<Type, ParseStringDelegate> DeserializerCache = new Dictionary<Type, ParseStringDelegate>();

		public T DeserializeFromString(string value, Type type)
		{
			ParseStringDelegate parseFn;
            if (DeserializerCache.TryGetValue(type, out parseFn)) return (T)parseFn(value);

            var genericType = typeof(T).MakeGenericType(type);
            var mi = genericType.GetMethodInfo("DeserializeFromString", new[] { typeof(string) });
            parseFn = (ParseStringDelegate)mi.MakeDelegate(typeof(ParseStringDelegate));

            Dictionary<Type, ParseStringDelegate> snapshot, newCache;
            do
            {
                snapshot = DeserializerCache;
                newCache = new Dictionary<Type, ParseStringDelegate>(DeserializerCache);
                newCache[type] = parseFn;

            } while (!ReferenceEquals(
                Interlocked.CompareExchange(ref DeserializerCache, newCache, snapshot), snapshot));
            
            return (T)parseFn(value);
		}

		public T DeserializeFromString(string value)
		{
			if (typeof(T) == typeof(string)) return (T)(object)value;

			return (T)JsvReader<T>.Parse(value);
		}

		public void SerializeToWriter(T value, TextWriter writer)
		{
			JsvWriter<T>.WriteObject(writer, value);
		}

		public string SerializeToString(T value)
		{
			if (value == null) return null;
			if (value is string) return value as string;

			var sb = new StringBuilder();
			using (var writer = new StringWriter(sb))
			{
				JsvWriter<T>.WriteObject(writer, value);
			}
			return sb.ToString();
		}
	}
}