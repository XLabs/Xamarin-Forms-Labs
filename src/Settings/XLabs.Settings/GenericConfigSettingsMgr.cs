using System;

namespace XLabs.Settings
{
    public class GenericConfigSettingsMgr : IConfigSettingsMgr
    {
		/// <summary>
		/// The _get setting function
		/// </summary>
		readonly Func<string, string> _getSettingFunction;
        readonly Func<string, string, bool> _setSettingFunction;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericConfigSettingsMgr" /> class.
        /// </summary>
        /// <param name="getSettingFunc">The get setting function.</param>
        /// <param name="setSettungFunc">The set settung function.</param>
        public GenericConfigSettingsMgr(Func<string, string> getSettingFunc, Func<string, string, bool> setSettungFunc)
		{
			_getSettingFunction = getSettingFunc;
		    _setSettingFunction = setSettungFunc;
		}

		#region Implementation of IConfigurationManager

        /// <summary>
        /// Gets the setting.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>T.</returns>
		public T GetValue<T>(string key, T defaultValue = default(T))
		{
			var result = GetValue(key);

			var tmp = (T) Convert.ChangeType(result, typeof (T));
            
            if (tmp == null)
            {
                tmp = defaultValue;
            }

            return tmp;
		}

        /// <summary>
        /// Gets the setting.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.String.</returns>
        public string GetValue(string key, string defaultValue = null)
        {
            return _getSettingFunction(key);
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
            return _setSettingFunction(key, value as string);
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Object.</returns>
        public object this[string key]
        {
            get { return GetValue(key); }
            set { SetValue(key, value); }
        }

        /// <summary>
        /// Loads the configuration settings.
        /// </summary>
        /// <param name="loadFunc">The optional function to execute during the load process.</param>
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

        #endregion
    }
}