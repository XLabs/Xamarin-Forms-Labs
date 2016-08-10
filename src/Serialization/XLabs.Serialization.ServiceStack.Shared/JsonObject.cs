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
using System.Text.RegularExpressions;
using ServiceStack.Text.Common;
using ServiceStack.Text.Json;

namespace ServiceStack.Text
{
	/// <summary>
	/// Class JsonExtensions.
	/// </summary>
	public static class JsonExtensions
	{
		/// <summary>
		/// Jsons to.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="map">The map.</param>
		/// <param name="key">The key.</param>
		/// <returns>T.</returns>
		public static T JsonTo<T>(this Dictionary<string, string> map, string key)
		{
			return Get<T>(map, key);
		}

		/// <summary>
		/// Get JSON string value converted to T
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="map">The map.</param>
		/// <param name="key">The key.</param>
		/// <returns>T.</returns>
		public static T Get<T>(this Dictionary<string, string> map, string key)
		{
			string strVal;
			return map.TryGetValue(key, out strVal) ? JsonSerializer.DeserializeFromString<T>(strVal) : default(T);
		}

		/// <summary>
		/// Get JSON string value
		/// </summary>
		/// <param name="map">The map.</param>
		/// <param name="key">The key.</param>
		/// <returns>System.String.</returns>
		public static string Get(this Dictionary<string, string> map, string key)
		{
			string strVal;
            return map.TryGetValue(key, out strVal) ? JsonTypeSerializer.Instance.UnescapeString(strVal) : null;
		}

		/// <summary>
		/// Arrays the objects.
		/// </summary>
		/// <param name="json">The json.</param>
		/// <returns>JsonArrayObjects.</returns>
		public static JsonArrayObjects ArrayObjects(this string json)
		{
			return Text.JsonArrayObjects.Parse(json);
		}

		/// <summary>
		/// Converts all.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="jsonArrayObjects">The json array objects.</param>
		/// <param name="converter">The converter.</param>
		/// <returns>List&lt;T&gt;.</returns>
		public static List<T> ConvertAll<T>(this JsonArrayObjects jsonArrayObjects, Func<JsonObject, T> converter)
		{
			var results = new List<T>();

			foreach (var jsonObject in jsonArrayObjects)
			{
				results.Add(converter(jsonObject));
			}

			return results;
		}

		/// <summary>
		/// Converts to.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="jsonObject">The json object.</param>
		/// <param name="converFn">The conver function.</param>
		/// <returns>T.</returns>
		public static T ConvertTo<T>(this JsonObject jsonObject, Func<JsonObject, T> converFn)
		{
			return jsonObject == null 
				? default(T) 
				: converFn(jsonObject);
		}

		/// <summary>
		/// To the dictionary.
		/// </summary>
		/// <param name="jsonObject">The json object.</param>
		/// <returns>Dictionary&lt;System.String, System.String&gt;.</returns>
		public static Dictionary<string, string> ToDictionary(this JsonObject jsonObject)
		{
			return jsonObject == null 
				? new Dictionary<string, string>() 
				: new Dictionary<string, string>(jsonObject);
		}
	}

	/// <summary>
	/// Class JsonObject.
	/// </summary>
	public class JsonObject : Dictionary<string, string>
	{
		/// <summary>
		/// Get JSON string value
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>System.String.</returns>
		public new string this[string key]
        {
            get { return this.Get(key); }
            set { base[key] = value; }
        }

		/// <summary>
		/// Parses the specified json.
		/// </summary>
		/// <param name="json">The json.</param>
		/// <returns>JsonObject.</returns>
		public static JsonObject Parse(string json)
        {
            return JsonSerializer.DeserializeFromString<JsonObject>(json);
        }

		/// <summary>
		/// Parses the array.
		/// </summary>
		/// <param name="json">The json.</param>
		/// <returns>JsonArrayObjects.</returns>
		public static JsonArrayObjects ParseArray(string json)
        {
            return JsonArrayObjects.Parse(json);
        }

		/// <summary>
		/// Arrays the objects.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns>JsonArrayObjects.</returns>
		public JsonArrayObjects ArrayObjects(string propertyName)
		{
			string strValue;
			return this.TryGetValue(propertyName, out strValue)
				? JsonArrayObjects.Parse(strValue)
				: null;
		}

		/// <summary>
		/// Objects the specified property name.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns>JsonObject.</returns>
		public JsonObject Object(string propertyName)
		{
			string strValue;
			return this.TryGetValue(propertyName, out strValue)
				? Parse(strValue)
				: null;
		}

		/// <summary>
		/// Get unescaped string value
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>System.String.</returns>
		public string GetUnescaped(string key)
        {
            return base[key];
        }

		/// <summary>
		/// Get unescaped string value
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>System.String.</returns>
		public string Child(string key)
        {
            return base[key];
        }
#if !SILVERLIGHT && !MONOTOUCH
        static readonly Regex NumberRegEx = new Regex(@"^[0-9]*(?:\.[0-9]*)?$", RegexOptions.Compiled);
#else
		/// <summary>
		/// The number reg ex
		/// </summary>
		static readonly Regex NumberRegEx = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
#endif
		/// <summary>
		/// Write JSON Array, Object, bool or number values as raw string
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="value">The value.</param>
		public static void WriteValue(TextWriter writer, object value)
        {
            var strValue = value as string;
            if (!string.IsNullOrEmpty(strValue))
            {
                var firstChar = strValue[0];
                var lastChar = strValue[strValue.Length - 1];
                if ((firstChar == JsWriter.MapStartChar && lastChar == JsWriter.MapEndChar)
                    || (firstChar == JsWriter.ListStartChar && lastChar == JsWriter.ListEndChar) 
                    || JsonUtils.True == strValue
                    || JsonUtils.False == strValue
                    || NumberRegEx.IsMatch(strValue))
                {
                    writer.Write(strValue);
                    return;
                }
            }
            JsonUtils.WriteString(writer, strValue);
        }
    }

	/// <summary>
	/// Class JsonArrayObjects.
	/// </summary>
	public class JsonArrayObjects : List<JsonObject>
	{
		/// <summary>
		/// Parses the specified json.
		/// </summary>
		/// <param name="json">The json.</param>
		/// <returns>JsonArrayObjects.</returns>
		public static JsonArrayObjects Parse(string json)
		{
			return JsonSerializer.DeserializeFromString<JsonArrayObjects>(json);
		}
	}

	/// <summary>
	/// Struct JsonValue
	/// </summary>
	public struct JsonValue
    {
		/// <summary>
		/// The json
		/// </summary>
		private readonly string json;

		/// <summary>
		/// Initializes a new instance of the <see cref="JsonValue"/> struct.
		/// </summary>
		/// <param name="json">The json.</param>
		public JsonValue(string json)
        {
            this.json = json;
        }

		/// <summary>
		/// Ases this instance.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns>T.</returns>
		public T As<T>()
        {
            return JsonSerializer.DeserializeFromString<T>(json);
        }

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public override string ToString()
        {
            return json;
        }
    }

}