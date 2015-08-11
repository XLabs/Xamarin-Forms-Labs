using System;

namespace XLabs.Settings
{
    public static class ConfigSettingsMgr
    {
        private static IConfigSettingsMgr _instance;

        private static IConfigSettingsMgr Instance
        {
            get
            {
                if (!IsSet)
                {
                    throw new InvalidOperationException("IConfigSettings has not been set. Please set it by calling ConfigSettings.SetInstance(configSettingssInstnace) method.");
                }

                return _instance;
            }
            set
            {
                if (IsSet)
                {
                    throw new InvalidOperationException("IConfigSettings can only be set once.");
                }

                _instance = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether resolver has been set.
        /// </summary>
        public static bool IsSet
        {
            get { return _instance != null; }
        }

        /// <summary>
        /// Sets the config settings instance.
        /// </summary>
        /// <param name="instance">Instance of IConfigSettings implementation.</param>
        /// <exception cref="InvalidOperationException">Instance can only be set once to prevent mix-ups.</exception>
        public static void SetInstance(IConfigSettingsMgr instance)
        {
            Instance = instance;
        }

        /// <summary>
        /// Gets the value of the specified setting.  If none is defined, the default value is returned
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>T.</returns>
        public static T GetValue<T>(string key, T defaultValue = default(T))
        {
            return Instance.GetValue(key, defaultValue);
        }

        /// <summary>
        /// Gets the value of the specified setting.  If none is defined, a null value is returned
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.String.</returns>
        public static string GetValue(string key, string defaultValue = null)
        {
            return GetValue<string>(key, defaultValue);
        }
    }
}