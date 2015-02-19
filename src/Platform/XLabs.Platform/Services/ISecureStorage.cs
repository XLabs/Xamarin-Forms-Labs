namespace XLabs.Platform.Services
{
    /// <summary>
    /// Interface for securely storing data.
    /// </summary>
    public interface ISecureStorage
    {
        /// <summary>
        /// Stores data.
        /// </summary>
        /// <param name="key">Key for the data.</param>
        /// <param name="dataBytes">Data bytes to store.</param>
        void Store(string key, byte[] dataBytes);

        /// <summary>
        /// Retrieves stored data.
        /// </summary>
        /// <param name="key">Key for the data.</param>
        /// <returns>Byte array of stored data.</returns>
        byte[] Retrieve(string key);

        /// <summary>
        /// Deletes data.
        /// </summary>
        /// <param name="key">Key for the data to be deleted.</param>
        void Delete(string key);
    }
}
