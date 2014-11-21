using System;
using System.IO.IsolatedStorage;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Services.IO;

[assembly: Dependency(typeof(Xamarin.Forms.Labs.WP8.Services.IO.AppSettings))]
namespace Xamarin.Forms.Labs.WP8.Services.IO 
{
    public class AppSettings : AppSettingsBase, IAppSettings 
    {
        private readonly IsolatedStorageSettings isoSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettings"/> class.
        /// </summary>
        public AppSettings() 
        {
            isoSettings = IsolatedStorageSettings.ApplicationSettings;
        }

        /// <summary>
        /// Deletes the entry with the specified key.
        /// </summary>
        /// <param name="key">They key.</param>
        public override void Delete(string key) 
        {
            isoSettings.Remove(key);
            isoSettings.Save();
        }

        /// <summary>
        /// Determines whether the specified key exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// True if key exists otherwise, false.
        /// </returns>
        public override bool ContainsKey(string key) 
        {
            return isoSettings.Contains(key);
        }

        /// <summary>
        /// Associates the given int value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected override void PutInt(string key, int value) 
        {
            isoSettings[key] = value;
            isoSettings.Save();
        }

        /// <summary>
        /// Associates the given long value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected override void PutLong(string key, long value) 
        {
            isoSettings[key] = value;
            isoSettings.Save();
        }

        /// <summary>
        /// Associates the given string value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected override void PutString(string key, string value) 
        {
            isoSettings[key] = value;
            isoSettings.Save();
        }

        /// <summary>
        /// Associates the given float value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected override void PutFloat(string key, float value) 
        {
            isoSettings[key] = value;
            isoSettings.Save();
        }

        /// <summary>
        /// Associates the given double value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected override void PutDouble(string key, double value) 
        {
            isoSettings[key] = value;
            isoSettings.Save();
        }

        /// <summary>
        /// Associates the given bool value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        protected override void PutBool(string key, bool value) 
        {
            isoSettings[key] = value;
            isoSettings.Save();
        }

        /// <summary>
        /// Associates the given DateTime value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected override void PutDateTime(string key, DateTime value) 
        {
            isoSettings[key] = value;
            isoSettings.Save();
        }

        /// <summary>
        /// Returns the int value associated with the given key or the default value if key does not exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        protected override int GetInt(string key, int defaultValue) 
        {
            return isoSettings.Contains(key)
              ? (int)isoSettings[key]
              : defaultValue;
        }

        /// <summary>
        /// Returns the long value associated with the given key or the default value if key does not exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        protected override long GetLong(string key, long defaultValue) 
        {
            return isoSettings.Contains(key)
              ? (long)isoSettings[key]
              : defaultValue;
        }

        /// <summary>
        /// Returns the string value associated with the given key or the default value if key does not exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        protected override string GetString(string key, string defaultValue) 
        {
            return isoSettings.Contains(key)
              ? (string)isoSettings[key]
              : defaultValue;
        }

        /// <summary>
        /// Returns the float value associated with the given key or the default value if key does not exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        protected override float GetFloat(string key, float defaultValue) 
        {
            return isoSettings.Contains(key)
              ? (float)isoSettings[key]
              : defaultValue;
        }

        /// <summary>
        /// Returns the double value associated with the given key or the default value if key does not exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        protected override double GetDouble(string key, double defaultValue) 
        {
            return isoSettings.Contains(key)
              ? (double)isoSettings[key]
              : defaultValue;
        }

        /// <summary>
        /// Returns the bool value associated with the given key or the default value if key does not exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns></returns>
        protected override bool GetBool(string key, bool defaultValue) 
        {
            return isoSettings.Contains(key)
              ? (bool)isoSettings[key]
              : defaultValue;
        }

        /// <summary>
        /// Returns the DateTime value associated with the given key or the default value if key does not exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        protected override DateTime GetDateTime(string key, DateTime defaultValue) 
        {
            return isoSettings.Contains(key)
              ? (DateTime)isoSettings[key]
              : defaultValue;
        }
    }
}
