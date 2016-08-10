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
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Linq;
using ServiceStack.Text.Json;

namespace ServiceStack.Text.Common
{
	/// <summary>
	/// Class DeserializeKeyValuePair.
	/// </summary>
	/// <typeparam name="TSerializer">The type of the t serializer.</typeparam>
	internal static class DeserializeKeyValuePair<TSerializer>
        where TSerializer : ITypeSerializer
    {
		/// <summary>
		/// The serializer
		/// </summary>
		private static readonly ITypeSerializer Serializer = JsWriter.GetTypeSerializer<TSerializer>();

		/// <summary>
		/// The key index
		/// </summary>
		const int KeyIndex = 0;
		/// <summary>
		/// The value index
		/// </summary>
		const int ValueIndex = 1;

		/// <summary>
		/// Gets the parse method.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>ParseStringDelegate.</returns>
		public static ParseStringDelegate GetParseMethod(Type type)
        {
            var mapInterface = type.GetTypeWithGenericInterfaceOf(typeof(KeyValuePair<,>));

            var keyValuePairArgs = mapInterface.GenericTypeArguments();
            var keyTypeParseMethod = Serializer.GetParseFn(keyValuePairArgs[KeyIndex]);
            if (keyTypeParseMethod == null) return null;

            var valueTypeParseMethod = Serializer.GetParseFn(keyValuePairArgs[ValueIndex]);
            if (valueTypeParseMethod == null) return null;

            var createMapType = type.HasAnyTypeDefinitionsOf(typeof(KeyValuePair<,>))
                ? null : type;

            return value => ParseKeyValuePairType(value, createMapType, keyValuePairArgs, keyTypeParseMethod, valueTypeParseMethod);
        }

		/// <summary>
		/// Parses the key value pair.
		/// </summary>
		/// <typeparam name="TKey">The type of the t key.</typeparam>
		/// <typeparam name="TValue">The type of the t value.</typeparam>
		/// <param name="value">The value.</param>
		/// <param name="createMapType">Type of the create map.</param>
		/// <param name="parseKeyFn">The parse key function.</param>
		/// <param name="parseValueFn">The parse value function.</param>
		/// <returns>System.Object.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">Incorrect KeyValuePair property:  + key</exception>
		public static object ParseKeyValuePair<TKey, TValue>(
            string value, Type createMapType,
            ParseStringDelegate parseKeyFn, ParseStringDelegate parseValueFn)
        {
            if (value == null) return default(KeyValuePair<TKey, TValue>);

            var index = VerifyAndGetStartIndex(value, createMapType);

            if (JsonTypeSerializer.IsEmptyMap(value, index)) return new KeyValuePair<TKey, TValue>();
            var keyValue = default(TKey);
            var valueValue = default(TValue);

            var valueLength = value.Length;
            while (index < valueLength)
            {
                var key = Serializer.EatMapKey(value, ref index);
                Serializer.EatMapKeySeperator(value, ref index);
                var keyElementValue = Serializer.EatTypeValue(value, ref index);

                if (key.CompareIgnoreCase("key") == 0)
                    keyValue = (TKey)parseKeyFn(keyElementValue);
                else if (key.CompareIgnoreCase("value") == 0)
                    valueValue = (TValue)parseValueFn(keyElementValue);
                else
                    throw new SerializationException("Incorrect KeyValuePair property: " + key);
                Serializer.EatItemSeperatorOrMapEndChar(value, ref index);
            }

            return new KeyValuePair<TKey, TValue>(keyValue, valueValue);
        }

		/// <summary>
		/// Verifies the start index of the and get.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="createMapType">Type of the create map.</param>
		/// <returns>System.Int32.</returns>
		private static int VerifyAndGetStartIndex(string value, Type createMapType)
        {
            var index = 0;
            if (!Serializer.EatMapStartChar(value, ref index))
            {
                //Don't throw ex because some KeyValueDataContractDeserializer don't have '{}'
                Tracer.Instance.WriteDebug("WARN: Map definitions should start with a '{0}', expecting serialized type '{1}', got string starting with: {2}",
                                           JsWriter.MapStartChar, createMapType != null ? createMapType.Name : "Dictionary<,>", value.Substring(0, value.Length < 50 ? value.Length : 50));
            }
            return index;
        }

		/// <summary>
		/// The parse delegate cache
		/// </summary>
		private static Dictionary<string, ParseKeyValuePairDelegate> ParseDelegateCache
            = new Dictionary<string, ParseKeyValuePairDelegate>();

		/// <summary>
		/// Delegate ParseKeyValuePairDelegate
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="createMapType">Type of the create map.</param>
		/// <param name="keyParseFn">The key parse function.</param>
		/// <param name="valueParseFn">The value parse function.</param>
		/// <returns>System.Object.</returns>
		private delegate object ParseKeyValuePairDelegate(string value, Type createMapType,
            ParseStringDelegate keyParseFn, ParseStringDelegate valueParseFn);

		/// <summary>
		/// Parses the type of the key value pair.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="createMapType">Type of the create map.</param>
		/// <param name="argTypes">The argument types.</param>
		/// <param name="keyParseFn">The key parse function.</param>
		/// <param name="valueParseFn">The value parse function.</param>
		/// <returns>System.Object.</returns>
		public static object ParseKeyValuePairType(string value, Type createMapType, Type[] argTypes,
            ParseStringDelegate keyParseFn, ParseStringDelegate valueParseFn)
        {

            ParseKeyValuePairDelegate parseDelegate;
            var key = GetTypesKey(argTypes);
            if (ParseDelegateCache.TryGetValue(key, out parseDelegate))
                return parseDelegate(value, createMapType, keyParseFn, valueParseFn);

            var mi = typeof(DeserializeKeyValuePair<TSerializer>).GetPublicStaticMethod("ParseKeyValuePair");
            var genericMi = mi.MakeGenericMethod(argTypes);
            parseDelegate = (ParseKeyValuePairDelegate)genericMi.MakeDelegate(typeof(ParseKeyValuePairDelegate));

            Dictionary<string, ParseKeyValuePairDelegate> snapshot, newCache;
            do
            {
                snapshot = ParseDelegateCache;
                newCache = new Dictionary<string, ParseKeyValuePairDelegate>(ParseDelegateCache);
                newCache[key] = parseDelegate;

            } while (!ReferenceEquals(
                Interlocked.CompareExchange(ref ParseDelegateCache, newCache, snapshot), snapshot));

            return parseDelegate(value, createMapType, keyParseFn, valueParseFn);
        }

		/// <summary>
		/// Gets the types key.
		/// </summary>
		/// <param name="types">The types.</param>
		/// <returns>System.String.</returns>
		private static string GetTypesKey(params Type[] types)
        {
            var sb = new StringBuilder(256);
            foreach (var type in types)
            {
                if (sb.Length > 0)
                    sb.Append(">");

                sb.Append(type.FullName);
            }
            return sb.ToString();
        }
    }
}