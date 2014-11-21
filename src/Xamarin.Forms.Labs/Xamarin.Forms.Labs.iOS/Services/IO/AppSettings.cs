using MonoTouch.Foundation;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Services.IO;

[assembly: Dependency(typeof(Xamarin.Forms.Labs.iOS.Services.IO.AppSettings))]
namespace Xamarin.Forms.Labs.iOS.Services.IO 
{
    public class AppSettings : AppSettingsBase, IAppSettings 
    {
        private readonly NSUserDefaults userDefaults;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettings"/> class.
        /// </summary>
        public AppSettings() 
        {
            userDefaults = NSUserDefaults.StandardUserDefaults;
        }

        /// <summary>
        /// Deletes the entry with the specified key.
        /// </summary>
        /// <param name="key">They key.</param>
        public override void Delete(string key) 
        {
            userDefaults.RemoveObject(key);
            userDefaults.Synchronize();
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
            return userDefaults[key] != null; // Indexer calls setObject/objectForKey internally - http://forums.xamarin.com/discussion/5793/nsuserdefaults-missing-method-objectforkey
        }

        /// <summary>
        /// Associates the given int value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected override void PutInt(string key, int value) 
        {
            userDefaults.SetInt(value, key);
            userDefaults.Synchronize();
        }

        /// <summary>
        /// Associates the given long value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected override void PutLong(string key, long value) 
        {
            userDefaults[key] = NSNumber.FromInt64(value);
            userDefaults.Synchronize();
        }

        /// <summary>
        /// Associates the given string value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected override void PutString(string key, string value) 
        {
            userDefaults.SetString(value, key);
            userDefaults.Synchronize();
        }

        /// <summary>
        /// Associates the given float value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected override void PutFloat(string key, float value) 
        {
            userDefaults.SetFloat(value, key);
            userDefaults.Synchronize();
        }

        /// <summary>
        /// Associates the given double value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected override void PutDouble(string key, double value) 
        {
            userDefaults.SetDouble(value, key);
            userDefaults.Synchronize();
        }

        /// <summary>
        /// Associates the given bool value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        protected override void PutBool(string key, bool value) 
        {
            userDefaults.SetBool(value, key);
            userDefaults.Synchronize();
        }

        /// <summary>
        /// Associates the given DateTime value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected override void PutDateTime(string key, DateTime value) 
        {
            userDefaults[key] = (NSDate)value;
            userDefaults.Synchronize();
        }

        /// <summary>
        /// Returns the int value associated with the given key or the default value if key does not exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        protected override int GetInt(string key, int defaultValue) 
        {
            return userDefaults[key] != null
              ? userDefaults.IntForKey(key)
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
            var v = userDefaults[key];
            if (v != null) 
            {
                var nsNumber = (NSNumber)v;
                return (long)nsNumber;
            } 
            else 
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Returns the string value associated with the given key or the default value if key does not exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        protected override string GetString(string key, string defaultValue) 
        {
            return userDefaults[key] != null
              ? userDefaults.StringForKey(key)
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
            return userDefaults[key] != null
              ? userDefaults.FloatForKey(key)
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
            return userDefaults[key] != null
              ? userDefaults.DoubleForKey(key)
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
            return userDefaults[key] != null
                ? userDefaults.BoolForKey(key)
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
            var v = userDefaults[key];
            if (v != null)
            {
                var nsDate = (NSDate)v;
                return nsDate;
            } 
            else 
            {
                return defaultValue;
            }
        }
    }
}
