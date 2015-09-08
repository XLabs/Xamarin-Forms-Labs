using System;
using System.IO;

namespace XLabs.Serialization
{
    /// <summary>
    /// Defines a generic interface for stream serializer.
    /// </summary>
    public interface IStreamSerializer
    {
        /// <summary>
        /// Serializes object to a stream.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize.</typeparam>
        /// <param name="obj">Object to serialize.</param>
        /// <param name="stream">Stream to serialize to.</param>
        void Serialize<T>(T obj, Stream stream);

        /// <summary>
        /// Deserializes stream into an object.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize to.</typeparam>
        /// <param name="stream">Stream to deserialize from.</param>
        /// <returns>Object of type T.</returns>
        T Deserialize<T>(Stream stream);

        /// <summary>
        /// Deserializes stream into an object.
        /// </summary>
        /// <param name="stream">Stream to deserialize from.</param>
        /// <param name="type">Type of object to deserialize.</param>
        /// <returns>Deserialized object.</returns>
        object Deserialize(Stream stream, Type type);
    }
}
