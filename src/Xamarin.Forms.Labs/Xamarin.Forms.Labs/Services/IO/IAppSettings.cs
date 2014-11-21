using System;

namespace Xamarin.Forms.Labs.Services.IO 
{
    public interface IAppSettings 
    {
        /// <summary>
        /// Stores the specified value with the specified key. If a value already
        /// exists for the specified key then it will be updated with the new value.
        /// </summary>
        /// <remarks>
        /// Note that only the following data types can be stored:
        /// int, long, string, float, double, bool, DateTime
        /// </remarks>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        void Set<T>(string key, T value);

        /// <summary>
        /// Gets the value associated with the specified key or a default value if key is not found.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value to return if key is not found.</param>
        /// <returns></returns>
        T Get<T>(string key, T defaultValue);

        /// <summary>
        /// Determines whether the specified key exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>True if key exists otherwise, false.</returns>
        bool ContainsKey(string key);

        /// <summary>
        /// Deletes the entry with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        void Delete(string key);
    }
}
