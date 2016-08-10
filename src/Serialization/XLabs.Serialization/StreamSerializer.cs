using System;

namespace XLabs.Serialization
{
    /// <summary>
    /// The stream serializer.
    /// </summary>
    public abstract class StreamSerializer : ISerializer
    {
        #region ISerializer Members

        /// <summary>
        /// Gets the serialization format.
        /// </summary>
        /// <value>Serialization format.</value>
        public abstract SerializationFormat Format { get; }

        /// <summary>
        /// Cleans memory.
        /// </summary>
        public abstract void Flush();
        #endregion

        #region IByteSerializer Members

        /// <summary>
        /// Serializes object to a string.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize to.</typeparam>
        /// <param name="obj">Object to serialize.</param>
        /// <returns>Serialized byte[] of the object.</returns>
        public virtual byte[] SerializeToBytes<T>(T obj)
        {
            return (this as IStreamSerializer).GetSerializedBytes(obj);
        }

        /// <summary>
        /// Deserializes string into an object.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize to.</typeparam>
        /// <param name="data">Serialized object as byte buffer.</param>
        /// <returns>Object of type T.</returns>
        public virtual T Deserialize<T>(byte[] data)
        {
            return (this as IStreamSerializer).DeserializeFromBytes<T>(data);
        }

        /// <summary>
        /// Deserializes string into an object.
        /// </summary>
        /// <param name="data">Serialized object as byte buffer.</param>
        /// <param name="type">Type of object to deserialize.</param>
        /// <returns>Deserialized object.</returns>
        public object Deserialize(byte[] data, System.Type type)
        {
            return (this as IStreamSerializer).DeserializeFromBytes(data, type);
        }
        #endregion

        #region IStreamSerializer Members

        /// <summary>
        /// Serializes object to a stream.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize.</typeparam>
        /// <param name="obj">Object to serialize.</param>
        /// <param name="stream">Stream to serialize to.</param>
        public abstract void Serialize<T>(T obj, System.IO.Stream stream);

        /// <summary>
        /// Deserializes stream into an object.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize to.</typeparam>
        /// <param name="stream">Stream to deserialize from.</param>
        /// <returns>Object of type T.</returns>
        public abstract T Deserialize<T>(System.IO.Stream stream);

        /// <summary>
        /// Deserializes stream into an object.
        /// </summary>
        /// <param name="stream">Stream to deserialize from.</param>
        /// <param name="type">Type of object to deserialize.</param>
        /// <returns>Deserialized object.</returns>
        public abstract object Deserialize(System.IO.Stream stream, Type type);
        #endregion

        #region IStringSerializer Members
        /// <summary>
        /// Serializes object to a string.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize.</typeparam>
        /// <param name="obj">Object to serialize.</param>
        /// <returns>Serialized string of the object.</returns>
        public virtual string Serialize<T>(T obj)
        {
            return (this as IStreamSerializer).SerializeToString(obj);
        }

        /// <summary>
        /// Deserializes string into an object.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize to.</typeparam>
        /// <param name="data">Serialized object.</param>
        /// <returns>Object of type T.</returns>
        public virtual T Deserialize<T>(string data)
        {
            return (this as IStreamSerializer).DeserializeFromString<T>(data);
        }

		/// <summary>
		/// Deserializes string into an object.
		/// </summary>
		/// <param name="data">Serialized object.</param>
		/// <param name="type">Type of object to deserialize.</param>
		/// <returns>Object of type T.</returns>
		public object Deserialize(string data, Type type)
        {
            return (this as IStreamSerializer).DeserializeFromString(data, type);
        }
        #endregion
    }
}
