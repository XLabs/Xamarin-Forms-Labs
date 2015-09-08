namespace SerializationTests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using XLabs.Serialization;

    public static class SerializerTestExtensions
    {
        public static bool CanSerializeString<T>(this IStringSerializer serializer, T item)
        {
            var text = serializer.Serialize(item);

            var obj = serializer.Deserialize<T>(text);

            return obj.Equals(item);
        }

        public static bool CanSerializeBytes<T>(this IByteSerializer serializer, T item)
        {
            var text = serializer.SerializeToBytes(item);

            var obj = serializer.Deserialize<T>(text);

            return obj.Equals(item);
        }

        public static bool CanSerializeStream<T>(this IStreamSerializer serializer, T item)
        {
            var encoder = Encoding.UTF8;
            using (var stream = new StreamReader(new MemoryStream(), encoder))
            {
                serializer.Serialize<T>(item, stream.BaseStream);
                stream.BaseStream.Position = 0;
                var obj = serializer.Deserialize<T>(stream.BaseStream);
                return obj.Equals(item);
            }
        }

        public static bool CanSerializeEnumerable<T>(this ISerializer serializer, IEnumerable<T> list)
        {
            var text = serializer.Serialize(list);

            var obj = serializer.Deserialize<IEnumerable<T>>(text);

            return obj.SequenceEqual(list);
        }
    }
}
