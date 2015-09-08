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
using System.Linq;
using System.Threading;

namespace ServiceStack.Text.Common
{
    internal static class DeserializeArrayWithElements<TSerializer>
        where TSerializer : ITypeSerializer
    {
        private static Dictionary<Type, ParseArrayOfElementsDelegate> ParseDelegateCache
            = new Dictionary<Type, ParseArrayOfElementsDelegate>();

        private delegate object ParseArrayOfElementsDelegate(string value, ParseStringDelegate parseFn);

        public static Func<string, ParseStringDelegate, object> GetParseFn(Type type)
        {
            ParseArrayOfElementsDelegate parseFn;
            if (ParseDelegateCache.TryGetValue(type, out parseFn)) return parseFn.Invoke;

            var genericType = typeof(DeserializeArrayWithElements<,>).MakeGenericType(type, typeof(TSerializer));
#if NETFX_CORE
            var mi = genericType.GetRuntimeMethods().First(p => p.Name.Equals("ParseGenericArray"));
            parseFn = (ParseArrayOfElementsDelegate)mi.CreateDelegate(typeof(ParseArrayOfElementsDelegate));
#else
            var mi = genericType.GetMethod("ParseGenericArray", BindingFlags.Public | BindingFlags.Static);
            parseFn = (ParseArrayOfElementsDelegate)Delegate.CreateDelegate(typeof(ParseArrayOfElementsDelegate), mi);
#endif

            Dictionary<Type, ParseArrayOfElementsDelegate> snapshot, newCache;
            do
            {
                snapshot = ParseDelegateCache;
                newCache = new Dictionary<Type, ParseArrayOfElementsDelegate>(ParseDelegateCache);
                newCache[type] = parseFn;

            } while (!ReferenceEquals(
                Interlocked.CompareExchange(ref ParseDelegateCache, newCache, snapshot), snapshot));

            return parseFn.Invoke;
        }
    }

    internal static class DeserializeArrayWithElements<T, TSerializer>
        where TSerializer : ITypeSerializer
    {
        private static readonly ITypeSerializer Serializer = JsWriter.GetTypeSerializer<TSerializer>();

        public static T[] ParseGenericArray(string value, ParseStringDelegate elementParseFn)
        {
            if ((value = DeserializeListWithElements<TSerializer>.StripList(value)) == null) return null;
            if (value == string.Empty) return new T[0];

            if (value[0] == JsWriter.MapStartChar)
            {
                var itemValues = new List<string>();
                var i = 0;
                do
                {
                    itemValues.Add(Serializer.EatTypeValue(value, ref i));
                    Serializer.EatItemSeperatorOrMapEndChar(value, ref i);
                } while (i < value.Length);

                var results = new T[itemValues.Count];
                for (var j = 0; j < itemValues.Count; j++)
                {
                    results[j] = (T)elementParseFn(itemValues[j]);
                }
                return results;
            }
            else
            {
                var to = new List<T>();
                var valueLength = value.Length;

                var i = 0;
                while (i < valueLength)
                {
                    var elementValue = Serializer.EatValue(value, ref i);
                    var listValue = elementValue;
                    to.Add((T)elementParseFn(listValue));
                    if (Serializer.EatItemSeperatorOrMapEndChar(value, ref i)
                        && i == valueLength)
                    {
                        // If we ate a separator and we are at the end of the value, 
                        // it means the last element is empty => add default
                        to.Add(default(T));
                    }
                }

                return to.ToArray();
            }
        }
    }

    internal static class DeserializeArray<TSerializer>
        where TSerializer : ITypeSerializer
    {
        private static Dictionary<Type, ParseStringDelegate> ParseDelegateCache = new Dictionary<Type, ParseStringDelegate>();

        public static ParseStringDelegate GetParseFn(Type type)
        {
            ParseStringDelegate parseFn;
            if (ParseDelegateCache.TryGetValue(type, out parseFn)) return parseFn;

            var genericType = typeof(DeserializeArray<,>).MakeGenericType(type, typeof(TSerializer));

            var mi = genericType.GetPublicStaticMethod("GetParseFn");
            var parseFactoryFn = (Func<ParseStringDelegate>)mi.MakeDelegate(
                typeof(Func<ParseStringDelegate>));
            parseFn = parseFactoryFn();

            Dictionary<Type, ParseStringDelegate> snapshot, newCache;
            do
            {
                snapshot = ParseDelegateCache;
                newCache = new Dictionary<Type, ParseStringDelegate>(ParseDelegateCache);
                newCache[type] = parseFn;

            } while (!ReferenceEquals(
                Interlocked.CompareExchange(ref ParseDelegateCache, newCache, snapshot), snapshot));

            return parseFn;
        }
    }

    internal static class DeserializeArray<T, TSerializer>
        where TSerializer : ITypeSerializer
    {
        private static readonly ITypeSerializer Serializer = JsWriter.GetTypeSerializer<TSerializer>();

        private static readonly ParseStringDelegate CacheFn;

        static DeserializeArray()
        {
            CacheFn = GetParseFn();
        }

        public static ParseStringDelegate Parse
        {
            get { return CacheFn; }
        }

        public static ParseStringDelegate GetParseFn()
        {
            var type = typeof(T);
            if (!type.IsArray)
                throw new ArgumentException(string.Format("Type {0} is not an Array type", type.FullName));

            if (type == typeof(string[]))
                return ParseStringArray;
            if (type == typeof(byte[]))
                return ParseByteArray;

            var elementType = type.GetElementType();
            var elementParseFn = Serializer.GetParseFn(elementType);
            if (elementParseFn != null)
            {
                var parseFn = DeserializeArrayWithElements<TSerializer>.GetParseFn(elementType);
                return value => parseFn(value, elementParseFn);
            }
            return null;
        }

        public static string[] ParseStringArray(string value)
        {
            if ((value = DeserializeListWithElements<TSerializer>.StripList(value)) == null) return null;
            return value == string.Empty
                    ? new string[0]
                    : DeserializeListWithElements<TSerializer>.ParseStringList(value).ToArray();
        }

        public static byte[] ParseByteArray(string value)
        {
            if ((value = DeserializeListWithElements<TSerializer>.StripList(value)) == null) return null;
            if ((value = Serializer.UnescapeSafeString(value)) == null) return null;
            return value == string.Empty
                    ? new byte[0]
                    : Convert.FromBase64String(value);
        }
    }
}