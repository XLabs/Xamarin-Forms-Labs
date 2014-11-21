using Android.App;
using Android.Content;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Services.IO;

[assembly: Dependency(typeof(Xamarin.Forms.Labs.Droid.Services.IO.AppSettings))]
namespace Xamarin.Forms.Labs.Droid.Services.IO 
{
    public class AppSettings : AppSettingsBase, IAppSettings 
    {
        private readonly ISharedPreferences pref;

        // Do not change below value! <Guid("D13490CB-345F-45EB-87E0-4731118403BC")>
        private const string prefId = "D13490CB-345F-45EB-87E0-4731118403BC";

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettings"/> class.
        /// </summary>
        public AppSettings() 
        {
            pref = Application.Context.GetSharedPreferences(prefId, FileCreationMode.Private);
        }

        /// <summary>
        /// Deletes the entry with the specified key.
        /// </summary>
        /// <param name="key">They key.</param>
        public override void Delete(string key) 
        {
            pref.Edit().Remove(key).Commit();
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
            return pref.Contains(key);
        }

        /// <summary>
        /// Associates the given int value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected override void PutInt(string key, int value) 
        {
            pref.Edit().PutInt(key, value).Commit();
        }

        /// <summary>
        /// Associates the given long value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected override void PutLong(string key, long value) 
        {
            pref.Edit().PutLong(key, value).Commit();
        }

        /// <summary>
        /// Associates the given string value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected override void PutString(string key, string value) 
        {
            pref.Edit().PutString(key, value).Commit();
        }

        /// <summary>
        /// Associates the given float value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected override void PutFloat(string key, float value) 
        {
            pref.Edit().PutFloat(key, value).Commit();
        }

        /// <summary>
        /// Associates the given double value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected override void PutDouble(string key, double value) 
        {
            pref.Edit().PutLong(key, Java.Lang.Double.DoubleToLongBits(value)).Commit();
        }

        /// <summary>
        /// Associates the given bool value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        protected override void PutBool(string key, bool value) 
        {
            pref.Edit().PutBoolean(key, value).Commit();
        }

        /// <summary>
        /// Associates the given DateTime value with the given key in the app settings store.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected override void PutDateTime(string key, DateTime value) 
        {
            pref.Edit().PutLong(key, value.Ticks).Commit();
        }

        /// <summary>
        /// Returns the int value associated with the given key or the default value if key does not exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        protected override int GetInt(string key, int defaultValue) 
        {
            return pref.GetInt(key, defaultValue);
        }

        /// <summary>
        /// Returns the long value associated with the given key or the default value if key does not exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        protected override long GetLong(string key, long defaultValue) 
        {
            return pref.GetLong(key, defaultValue);
        }

        /// <summary>
        /// Returns the string value associated with the given key or the default value if key does not exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        protected override string GetString(string key, string defaultValue) 
        {
            return pref.GetString(key, defaultValue);
        }

        /// <summary>
        /// Returns the float value associated with the given key or the default value if key does not exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        protected override float GetFloat(string key, float defaultValue) 
        {
            return pref.GetFloat(key, defaultValue);
        }

        /// <summary>
        /// Returns the double value associated with the given key or the default value if key does not exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        protected override double GetDouble(string key, double defaultValue) 
        {
            return Java.Lang.Double.LongBitsToDouble(pref.GetLong(key, Java.Lang.Double.DoubleToLongBits(defaultValue)));
        }

        /// <summary>
        /// Returns the bool value associated with the given key or the default value if key does not exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns></returns>
        protected override bool GetBool(string key, bool defaultValue) 
        {
            return pref.GetBoolean(key, defaultValue);
        }

        /// <summary>
        /// Returns the DateTime value associated with the given key or the default value if key does not exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        protected override DateTime GetDateTime(string key, DateTime defaultValue) 
        {
            var value = pref.GetLong(key, -1);
            if (value == -1) 
            {
                return defaultValue;
            } 
            else 
            {
                return new DateTime(value);
            }
        }
    }
}