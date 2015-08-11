using System;
using Refractored.Xam.Settings;

namespace XLabs.Settings.XamSettings
{
    /// <summary>
    /// Implementation of ConfigSettings for XamSettings.
    /// </summary>
    public class XamSettingsConfigSettings : IConfigSettingsMgr
    {
        /// <summary>
        /// Gets the setting value. If it is not found, the default value is returned
        /// </summary>
        /// <typeparam name="T">the type of the setting</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The setting or the default value.</returns>
        public T GetValue<T>(string key, T defaultValue)
        {
            return CrossSettings.Current.GetValueOrDefault(key, defaultValue);
        }

        /// <summary>
        /// Sets the setting value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if setting the value was a success, <c>false</c> otherwise.</returns>
        public bool SetValue<T>(string key, T value)
        {
            return CrossSettings.Current.AddOrUpdateValue(key, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object" /> with the specified setting value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Object.</returns>
        public object this[string key]
        {
            get { return GetValue<object>(key, null); }
            set { SetValue(key, value); }
        }

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        //public void Refresh()
        //{
        //}

        /// <summary>
        /// Loads the configuration settings.
        /// </summary>
        /// <param name="loadFunc">The optional function to execute during the load process.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Load(Func<IConfigSettingsMgr, bool> loadFunc = null)
        {
            if (loadFunc != null)
            {
                loadFunc(this);
            }
        }

        /// <summary>
        /// Flushes this instance.
        /// </summary>
        public void Flush()
        {
        }
    }
}
