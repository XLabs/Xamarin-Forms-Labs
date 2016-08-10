using System.IO;
using System.Runtime.Serialization.Json;

namespace XLabs.Serialization
{
    /// <summary>
    /// The system JSON serializer.
    /// </summary>
    public class SystemJsonSerializer : StreamSerializer, IJsonSerializer
    {
        /// <summary>
        /// Gets the format.
        /// </summary>
        public override SerializationFormat Format => SerializationFormat.Json;

        /// <summary>
        /// Cleans memory.
        /// </summary>
        public override void Flush()
        {
        }

        /// <summary>
        /// Serializes object to a stream.
        /// </summary>
        /// <param name="obj">Object to serialize.</param>
        /// <param name="stream">Stream to serialize to.</param>
        public override void Serialize<T>(T obj, Stream stream)
        {
            var serializer = new DataContractJsonSerializer(obj.GetType());
            serializer.WriteObject(stream, obj);
        }

        /// <summary>
        /// Deserializes stream into an object.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize to.</typeparam>
        /// <param name="stream">Stream to deserialize from.</param>
        /// <returns>Object of type T.</returns>
        public override T Deserialize<T>(Stream stream)
        {
            return (T)Deserialize(stream, typeof (T));
        }

        /// <summary>
        /// Deserializes stream into an object.
        /// </summary>
        /// <param name="stream">Stream to deserialize from.</param>
        /// <param name="type">Type of object to deserialize.</param>
        /// <returns>Deserialized object.</returns>
        public override object Deserialize(Stream stream, System.Type type)
        {
            var serializer = new DataContractJsonSerializer(type);
            return serializer.ReadObject(stream);
        }

        /// <summary>
        /// Deserializes string into an object.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize to.</typeparam>
        /// <param name="data">Serialized object.</param>
        /// <returns>Object of type T.</returns>
        public override T Deserialize<T>(string data)
        {
            return this.DeserializeFromString<T>(data, System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// Serializes object to a string.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize.</typeparam>
        /// <param name="obj">Object to serialize.</param>
        /// <returns>Serialized string of the object.</returns>
        public override string Serialize<T>(T obj)
        {
            return this.SerializeToString(obj, System.Text.Encoding.UTF8);
        }
    }
}
