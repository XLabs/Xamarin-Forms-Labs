using System;

namespace Xamarin.Forms.Labs.Services.IO 
{
    public abstract class AppSettingsBase
    {
        /// <summary>
        /// Stores the specified value with the specified key
        /// </summary>
        /// <remarks>
        /// Note that only the following data types can be stored:
        /// int, long, string, float, double, bool, DateTime
        /// </remarks>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Set<T>(string key, T value) 
        {
            var type = typeof(T);

            if (type == typeof(int))
                PutInt(key, Box<int>(value));
            else if (type == typeof(long))
                PutLong(key, Box<long>(value));
            else if (type == typeof(string))
                PutString(key, Box<string>(value));
            else if (type == typeof(float))
                PutFloat(key, Box<float>(value));
            else if (type == typeof(double))
                PutDouble(key, Box<double>(value));
            else if (type == typeof(bool))
                PutBool(key, Box<bool>(value));
            else if (type == typeof(DateTime))
                PutDateTime(key, Box<DateTime>(value));
            else
                throw new NotSupportedException("Only primitive types are supported.");
        }

        /// <summary>
        /// Gets the value associated with the specified key or a default value if key is not found.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value to return if key is not found.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public T Get<T>(string key, T defaultValue) 
        {
            var type = typeof(T);

            if (type == typeof(int))
                return Box<T>(GetInt(key, Box<int>(defaultValue)));
            else if (type == typeof(long))
                return Box<T>(GetLong(key, Box<long>(defaultValue)));
            else if (type == typeof(string))
                return Box<T>(GetString(key, Box<string>(defaultValue)));
            else if (type == typeof(float))
                return Box<T>(GetFloat(key, Box<float>(defaultValue)));
            else if (type == typeof(double))
                return Box<T>(GetDouble(key, Box<double>(defaultValue)));
            else if (type == typeof(bool))
                return Box<T>(GetBool(key, Box<bool>(defaultValue)));
            else if (type == typeof(DateTime))
                return Box<T>(GetDateTime(key, Box<DateTime>(defaultValue)));
            else
                throw new NotSupportedException("Only primitive types are supported.");
        }

        /// <summary>
        /// Deletes the entry with the specified key.
        /// </summary>
        /// <param name="key">They key.</param>
        public abstract void Delete(string key);

        /// <summary>
        /// Determines whether the specified key exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// True if key exists otherwise, false.
        /// </returns>
        public abstract bool ContainsKey(string key);

        /// <summary>
        /// Associates the given int value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected abstract void PutInt(string key, int value);

        /// <summary>
        /// Associates the given long value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected abstract void PutLong(string key, long value);

        /// <summary>
        /// Associates the given string value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected abstract void PutString(string key, string value);

        /// <summary>
        /// Associates the given float value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected abstract void PutFloat(string key, float value);

        /// <summary>
        /// Associates the given double value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected abstract void PutDouble(string key, double value);

        /// <summary>
        /// Associates the given bool value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        protected abstract void PutBool(string key, bool value);

        /// <summary>
        /// Associates the given DateTime value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected abstract void PutDateTime(string key, DateTime value);

        /// <summary>
        /// Returns the int value associated with the given key or the default value if key does not exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        protected abstract int GetInt(string key, int defaultValue);

        /// <summary>
        /// Returns the long value associated with the given key or the default value if key does not exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        protected abstract long GetLong(string key, long defaultValue);

        /// <summary>
        /// Returns the string value associated with the given key or the default value if key does not exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        protected abstract string GetString(string key, string defaultValue);

        /// <summary>
        /// Returns the float value associated with the given key or the default value if key does not exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        protected abstract float GetFloat(string key, float defaultValue);

        /// <summary>
        /// Returns the double value associated with the given key or the default value if key does not exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        protected abstract double GetDouble(string key, double defaultValue);

        /// <summary>
        /// Returns the bool value associated with the given key or the default value if key does not exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns></returns>
        protected abstract bool GetBool(string key, bool defaultValue);

        /// <summary>
        /// Returns the DateTime value associated with the given key or the default value if key does not exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        protected abstract DateTime GetDateTime(string key, DateTime defaultValue);

        private static T Box<T>(object value) 
        {
            return (T)value;
        }
    }
}
