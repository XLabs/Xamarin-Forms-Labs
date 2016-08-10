using System;

namespace XLabs.Serialization
{
    /// <summary>
    /// Common interface for byte buffer serializer.
    /// </summary>
    public interface IByteSerializer
    {
        /// <summary>
        /// Serializes object to a byte array.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize to.</typeparam>
        /// <param name="obj">Object to serialize.</param>
        /// <returns>Serialized byte[] of the object.</returns>
        byte[] SerializeToBytes<T>(T obj);

        /// <summary>
        /// Deserializes byte array into an object.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize to.</typeparam>
        /// <param name="data">Serialized object as byte buffer.</param>
        /// <returns>Object of type T.</returns>
        T Deserialize<T>(byte[] data);

        /// <summary>
        /// Deserializes byte array into an object.
        /// </summary>
        /// <param name="data">Serialized object as byte buffer.</param>
        /// <param name="type">Type of object to deserialize.</param>
        /// <returns>Deserialized object.</returns>
        object Deserialize(byte[] data, Type type);
    }
}
