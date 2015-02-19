namespace XLabs.Platform.Services
{
    using System;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Security.Cryptography;
    using System.Threading;

    /// <summary>
    /// Implements <see cref="ISecureStorage"/> for WP using <see cref="IsolatedStorageFile"/> and <see cref="ProtectedData"/>.
    /// </summary>
    public class SecureStorage : ISecureStorage
    {
        private static IsolatedStorageFile File { get { return IsolatedStorageFile.GetUserStoreForApplication(); } }

        #region ISecureStorage Members

        public void Store(string key, byte[] dataBytes)
        {
            var mutex = new Mutex(false, key);

            try
            {
                mutex.WaitOne();
                using (var stream = new IsolatedStorageFileStream(key, FileMode.Create, FileAccess.Write, File))
                {
                    var data = ProtectedData.Protect(dataBytes, null);
                    stream.Write(data, 0, data.Length);
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        public byte[] Retrieve(string key)
        {
            var mutex = new Mutex(false, key);

            try
            {
                mutex.WaitOne();
                if (!File.FileExists(key))
                {
                    throw new Exception(string.Format("No entry found for key {0}.", key));
                }

                using (var stream = new IsolatedStorageFileStream(key, FileMode.Open, FileAccess.Read, File))
                {
                    var data = new byte[stream.Length];
                    stream.Read(data, 0, data.Length);
                    return ProtectedData.Unprotect(data, null);
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        public void Delete(string key)
        {
            var mutex = new Mutex(false, key);

            try
            {
                mutex.WaitOne();
                File.DeleteFile(key);
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        #endregion
    }
}
