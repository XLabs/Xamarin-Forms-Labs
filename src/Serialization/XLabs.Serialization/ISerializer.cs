namespace XLabs.Serialization
{
    /// <summary>
    /// Common serializer interface.
    /// </summary>
    public interface ISerializer : IByteSerializer, IStreamSerializer, IStringSerializer
    {
        /// <summary>
        /// Gets the serialization format.
        /// </summary>
        /// <value>Serialization format.</value>
        SerializationFormat Format { get; }

        /// <summary>
        /// Cleans memory.
        /// </summary>
        void Flush();
    }
}
