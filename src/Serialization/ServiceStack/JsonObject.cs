using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using ServiceStack.Text.Common;
using ServiceStack.Text.Json;

namespace ServiceStack.Text
{
	public static class JsonExtensions
	{
		public static T JsonTo<T>(this Dictionary<string, string> map, string key)
		{
			return Get<T>(map, key);
		}

        /// <summary>
        /// Get JSON string value converted to T
        /// </summary>
        public static T Get<T>(this Dictionary<string, string> map, string key)
		{
			string strVal;
			return map.TryGetValue(key, out strVal) ? JsonSerializer.DeserializeFromString<T>(strVal) : default(T);
		}

        /// <summary>
        /// Get JSON string value
        /// </summary>
        public static string Get(this Dictionary<string, string> map, string key)
		{
			string strVal;
            return map.TryGetValue(key, out strVal) ? JsonTypeSerializer.Instance.UnescapeString(strVal) : null;
		}

		public static JsonArrayObjects ArrayObjects(this string json)
		{
			return Text.JsonArrayObjects.Parse(json);
		}

		public static List<T> ConvertAll<T>(this JsonArrayObjects jsonArrayObjects, Func<JsonObject, T> converter)
		{
			var results = new List<T>();

			foreach (var jsonObject in jsonArrayObjects)
			{
				results.Add(converter(jsonObject));
			}

			return results;
		}

		public static T ConvertTo<T>(this JsonObject jsonObject, Func<JsonObject, T> converFn)
		{
			return jsonObject == null 
				? default(T) 
				: converFn(jsonObject);
		}

		public static Dictionary<string, string> ToDictionary(this JsonObject jsonObject)
		{
			return jsonObject == null 
				? new Dictionary<string, string>() 
				: new Dictionary<string, string>(jsonObject);
		}
	}

	public class JsonObject : Dictionary<string, string>
	{
        /// <summary>
        /// Get JSON string value
        /// </summary>
        public new string this[string key]
        {
            get { return this.Get(key); }
            set { base[key] = value; }
        }

        public static JsonObject Parse(string json)
        {
            return JsonSerializer.DeserializeFromString<JsonObject>(json);
        }

        public static JsonArrayObjects ParseArray(string json)
        {
            return JsonArrayObjects.Parse(json);
        }

		public JsonArrayObjects ArrayObjects(string propertyName)
		{
			string strValue;
			return this.TryGetValue(propertyName, out strValue)
				? JsonArrayObjects.Parse(strValue)
				: null;
		}

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
        public string GetUnescaped(string key)
        {
            return base[key];
        }

        /// <summary>
        /// Get unescaped string value
        /// </summary>
        public string Child(string key)
        {
            return base[key];
        }
#if !SILVERLIGHT && !MONOTOUCH
        static readonly Regex NumberRegEx = new Regex(@"^[0-9]*(?:\.[0-9]*)?$", RegexOptions.Compiled);
#else
        static readonly Regex NumberRegEx = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
#endif
        /// <summary>
        /// Write JSON Array, Object, bool or number values as raw string
        /// </summary>
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

	public class JsonArrayObjects : List<JsonObject>
	{
		public static JsonArrayObjects Parse(string json)
		{
			return JsonSerializer.DeserializeFromString<JsonArrayObjects>(json);
		}
	}

    public struct JsonValue
    {
        private readonly string json;

        public JsonValue(string json)
        {
            this.json = json;
        }

        public T As<T>()
        {
            return JsonSerializer.DeserializeFromString<T>(json);
        }
        
        public override string ToString()
        {
            return json;
        }
    }

}