// ***********************************************************************
// Assembly         : XLabs.Serialization
// Author           : rmarinho
// Created          : 09-08-2015
//
// Last Modified By : rmarinho
// Last Modified On : 09-08-2015
// ***********************************************************************
// <copyright file="StreamSerializerExtensions.cs" company="">
//     Copyright © XLabs 2014
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.IO;
using System.Text;

namespace XLabs.Serialization
{
	/// <summary>
	/// Class StreamSerializerExtensions.
	/// </summary>
	public static class StreamSerializerExtensions
    {
		/// <summary>
		/// Deserializes from string.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="serializer">The serializer.</param>
		/// <param name="value">The value.</param>
		/// <param name="encoding">The encoding.</param>
		/// <returns>T.</returns>
		public static T DeserializeFromString<T>(this IStreamSerializer serializer, string value, Encoding encoding = null)
        {
            var bytes = encoding == null ? Convert.FromBase64String(value) : encoding.GetBytes(value);

            using (var stream = new MemoryStream(bytes))
            {
                return serializer.Deserialize<T>(stream);
            }
        }

		/// <summary>
		/// Deserializes from string.
		/// </summary>
		/// <param name="serializer">The serializer.</param>
		/// <param name="value">The value.</param>
		/// <param name="type">The type.</param>
		/// <param name="encoding">The encoding.</param>
		/// <returns>System.Object.</returns>
		public static object DeserializeFromString(this IStreamSerializer serializer, string value, Type type, Encoding encoding = null)
        {
            var bytes = encoding == null ? Convert.FromBase64String(value) : encoding.GetBytes(value);

            using (var stream = new MemoryStream(bytes))
            {
                return serializer.Deserialize(stream, type);
            }
        }

		/// <summary>
		/// Serializes to string.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="serializer">The serializer.</param>
		/// <param name="obj">The object.</param>
		/// <param name="encoding">The encoding.</param>
		/// <returns>System.String.</returns>
		public static string SerializeToString<T>(this IStreamSerializer serializer, T obj, Encoding encoding = null)
        {
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(obj, stream);
                stream.Position = 0;
                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);

                return encoding == null ? Convert.ToBase64String(bytes) : encoding.GetString(bytes, 0, bytes.Length);
            }
        }

		/// <summary>
		/// Deserializes from bytes.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="serializer">The serializer.</param>
		/// <param name="data">The data.</param>
		/// <returns>T.</returns>
		public static T DeserializeFromBytes<T>(this IStreamSerializer serializer, byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                return serializer.Deserialize<T>(stream);
            }
        }

		/// <summary>
		/// Deserializes from bytes.
		/// </summary>
		/// <param name="serializer">The serializer.</param>
		/// <param name="data">The data.</param>
		/// <param name="type">The type.</param>
		/// <returns>System.Object.</returns>
		public static object DeserializeFromBytes(this IStreamSerializer serializer, byte[] data, Type type)
        {
            using (var stream = new MemoryStream(data))
            {
                return serializer.Deserialize(stream, type);
            }
        }

		/// <summary>
		/// Gets the serialized bytes.
		/// </summary>
		/// <param name="serializer">The serializer.</param>
		/// <param name="obj">The object.</param>
		/// <returns>System.Byte[].</returns>
		public static byte[] GetSerializedBytes(this IStreamSerializer serializer, object obj)
        {
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(obj, stream);
                return stream.ToArray();
            }
        }
    }
}
