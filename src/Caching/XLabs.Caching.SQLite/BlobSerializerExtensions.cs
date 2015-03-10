namespace XLabs.Caching.SQLite
{
    using System;
    using global::SQLite.Net;
    using Serialization;

    /// <summary>
    /// Converts <see cref="IByteSerializer"/> to <see cref="IBlobSerializer"/>.
    /// </summary>
    public static class BlobSerializerExtensions
    {
        /// <summary>
        /// Converts <see cref="IByteSerializer"/> to <see cref="IBlobSerializer"/>.
        /// </summary>
        /// <param name="serializer">Byte serializer.</param>
        /// <returns>IBlobSerializer</returns>
        public static IBlobSerializer AsBlobSerializer(this IByteSerializer serializer)
        {
            return new BlobSerializerDelegate(serializer.SerializeToBytes, serializer.Deserialize, serializer.CanDeserialize);
        }

        private static bool CanDeserialize(this IByteSerializer serializer, Type type)
        {
            return true;
        }
    }
}
