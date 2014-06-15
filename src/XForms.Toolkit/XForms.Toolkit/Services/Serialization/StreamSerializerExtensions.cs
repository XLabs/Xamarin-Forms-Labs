using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.Toolkit.Services.Serialization
{
    public static class StreamSerializerExtensions
    {
        /// <summary>
        /// Deserializes from stream.
        /// </summary>
        /// <returns>The from stream.</returns>
        /// <param name="stream">Stream.</param>
        /// <typeparam name="T">The type of object to deserialize.</typeparam>
        public static T DeserializeFromString<T>(this IStreamSerializer serializer, string value, Encoding encoding = null)
        {
            var encoder = encoding ?? Encoding.UTF8;
            using (var stream = new MemoryStream(encoder.GetBytes(value)))
            {
                return serializer.Deserialize<T>(stream);
            }
        }

        public static string SerializeToString<T>(this IStreamSerializer serializer, T obj, Encoding encoding = null)
        {
            var encoder = encoding ?? Encoding.UTF8;
            using (var stream = new StreamReader(new MemoryStream(), encoder))
            {
                serializer.Serialize<T>(obj, stream.BaseStream);
                return stream.ReadToEnd();
            }
        }

        public static T DeserializeFromBytes<T>(this IStreamSerializer serializer, byte[] data, Encoding encoding = null)
        {
            using (var stream = new MemoryStream(data))
            {
                return serializer.Deserialize<T>(stream);
            }
        }

        public static byte[] SerializeToBytes(this IStreamSerializer serializer, object obj, Encoding encoding = null)
        {
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(obj, stream);
                return stream.ToArray();
            }
        }
    }
}
