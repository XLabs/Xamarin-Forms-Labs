using System;
using System.IO;
using System.Text;

namespace XLabs.Serialization
{
    /// <summary>
    /// Serializer extensions.
    /// </summary>
    public static class StringSerializerExtensions
    {
        /// <summary>
        /// Serializes to stream.
        /// </summary>
		/// <param name="serializer">The serializer.</param>
        /// <param name="obj">Object to serialize.</param>
        /// <param name="stream">Stream to serialize to.</param>
        public static void SerializeToStream(this IStringSerializer serializer, object obj, Stream stream)
        {
            var streamWriter = new StreamWriter(stream)
            {
                AutoFlush = true
            };

            var str = serializer.Serialize(obj);
            streamWriter.Write(str);
        }

        /// <summary>
        /// Deserialize from stream.
        /// </summary>
        /// <returns>The deserialized object.</returns>
        /// <param name="serializer">The string serializer.</param>
        /// <param name="stream">Stream to deserialize from.</param>
        /// <typeparam name="T">The type of object to deserialize.</typeparam>
        public static T DeserializeFromStream<T>(this IStringSerializer serializer, Stream stream)
        {
            var text = new StreamReader(stream).ReadToEnd();
            return serializer.Deserialize<T>(text);
        }

        /// <summary>
        /// Deserialize from stream.
        /// </summary>
        /// <returns>The deserialized object.</returns>
        /// <param name="serializer">The string serializer.</param>
        /// <param name="stream">Stream to deserialize from.</param>
        /// <param name="type">The type of object to deserialize.</param>
        public static object DeserializeFromStream(this IStringSerializer serializer, Stream stream, Type type)
        {
            var text = new StreamReader(stream).ReadToEnd();
            return serializer.Deserialize(text, type);
        }

        /// <summary>
        /// Serializes to writer.
        /// </summary>
		/// <param name="serializer">The serializer.</param>
        /// <param name="obj">Object to serialize.</param>
        /// <param name="writer">Writer.</param>
        public static void SerializeToWriter(this IStringSerializer serializer, object obj, TextWriter writer)
        {
            writer.Write(serializer.Serialize(obj));
        }

        /// <summary>
		/// Deserialize from reader.
        /// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="serializer">The serializer.</param>
        /// <param name="reader">Reader to deserialize from.</param>
		/// <returns>The serialized object from reader.</returns>
        public static T DeserializeFromReader<T>(this IStringSerializer serializer, TextReader reader)
        {
            return serializer.Deserialize<T>(reader.ReadToEnd());
        }

		/// <summary>
		/// Deserializes from bytes.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="serializer">The serializer.</param>
		/// <param name="data">The data.</param>
		/// <param name="encoding">The encoding.</param>
		/// <returns>T.</returns>
        public static T DeserializeFromBytes<T>(this IStringSerializer serializer, byte[] data, Encoding encoding = null)
        {
            var encoder = encoding ?? Encoding.UTF8;
            var str = encoder.GetString(data, 0, data.Length);
            return serializer.Deserialize<T>(str);
        }

		/// <summary>
		/// Deserializes from bytes.
		/// </summary>
		/// <param name="serializer">The serializer.</param>
		/// <param name="data">The data.</param>
		/// <param name="type">The type.</param>
		/// <param name="encoding">The encoding.</param>
		/// <returns>System.Object.</returns>
        public static object DeserializeFromBytes(this IStringSerializer serializer, byte[] data, Type type, Encoding encoding = null)
        {
            var encoder = encoding ?? Encoding.UTF8;
            var str = encoder.GetString(data, 0, data.Length);
            return serializer.Deserialize(str, type);
        }

		/// <summary>
		/// Gets the serialized bytes.
		/// </summary>
		/// <param name="serializer">The serializer.</param>
		/// <param name="obj">The object.</param>
		/// <param name="encoding">The encoding.</param>
		/// <returns>System.Byte[].</returns>
        public static byte[] GetSerializedBytes(this IStringSerializer serializer, object obj, Encoding encoding = null)
        {
            var encoder = encoding ?? Encoding.UTF8;
            var str = serializer.Serialize(obj);
            return encoder.GetBytes(str);
        }
    }
}
