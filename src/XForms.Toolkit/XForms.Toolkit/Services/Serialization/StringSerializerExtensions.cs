using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.Toolkit.Services.Serialization
{
    /// <summary>
    /// Serializer extensions.
    /// </summary>
    public static class StringSerializerExtensions
    {
        /// <summary>
        /// Serializes to stream.
        /// </summary>
        /// <param name="obj">Object to serialize.</param>
        /// <param name="stream">Stream.</param>
        public static void SerializeToStream(this IStringSerializer serializer, object obj, Stream stream)
        {
            using (var streamWriter = new StreamWriter(stream))
            {
                streamWriter.Write(serializer.Serialize(obj));
            }
        }

        /// <summary>
        /// Deserializes from stream.
        /// </summary>
        /// <returns>The from stream.</returns>
        /// <param name="stream">Stream.</param>
        /// <typeparam name="T">The type of object to deserialize.</typeparam>
        public static T DeserializeFromStream<T>(this IStringSerializer serializer, Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            {
                var text = streamReader.ReadToEnd();
                return serializer.Deserialize<T>(text);
            }
        }

        /// <summary>
        /// Serializes to writer.
        /// </summary>
        /// <param name="obj">Object to serialize.</param>
        /// <param name="writer">Writer.</param>
        public static void SerializeToWriter(this IStringSerializer serializer, object obj, TextWriter writer)
        {
            writer.Write(serializer.Serialize(obj));
        }

        /// <summary>
        /// Deserializes from reader.
        /// </summary>
        /// <returns>The serialized object from reader.</returns>
        /// <param name="reader">Reader to deserialize from.</param>
        public static T DeserializeFromReader<T>(this IStringSerializer serializer, TextReader reader)
        {
            return serializer.Deserialize<T>(reader.ReadToEnd());
        }

        public static T DeserializeFromBytes<T>(this IStringSerializer serializer, byte[] data, Encoding encoding = null)
        {
            var encoder = encoding ?? Encoding.UTF8;
            var str = encoder.GetString(data, 0, data.Length);
            return serializer.Deserialize<T>(str);
        }

        public static byte[] SerializeToBytes(this IStringSerializer serializer, object obj, Encoding encoding = null)
        {
            var encoder = encoding ?? Encoding.UTF8;
            var str = serializer.Serialize(obj);
            return encoder.GetBytes(str);
        }
    }
}
