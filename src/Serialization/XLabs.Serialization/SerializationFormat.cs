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
        Binary,

        /// <summary>
        /// MessagePack format.
        /// </summary>
        /// <remarks>
        /// Website: http://msgpack.org/
        /// Specs: https://github.com/msgpack/msgpack/blob/master/spec.md
        /// </remarks>
        MsgPack
    }
}
