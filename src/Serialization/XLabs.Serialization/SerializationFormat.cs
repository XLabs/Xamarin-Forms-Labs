namespace XLabs.Serialization
{
    /// <summary>
    /// Serialization format type.
    /// </summary>
    public enum SerializationFormat
    {
        /// <summary>
        /// Custom undefined format.
        /// </summary>
        Custom,

        /// <summary>
        /// JSON format.
        /// </summary>
        Json,

        /// <summary>
        /// XML format.
        /// </summary>
        Xml,

        /// <summary>
        /// ProtoBuffer format.
        /// </summary>
        ProtoBuffer,

        /// <summary>
        /// Custom binary format.
        /// </summary>
        Binary
    }
}
