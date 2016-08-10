// ***********************************************************************
// Assembly         : XLabs.Settings
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="SimpleConfigSettingsMgr.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System;
using System.Collections.Generic;

namespace XLabs.Settings
{
    /// <summary>
    /// Class SimpleConfigSettings.
    /// </summary>
    public class SimpleConfigSettingsMgr : IConfigSettingsMgr
    {
        /// <summary>
        /// The _settings
        /// </summary>
        private readonly IDictionary<string, object> _settings = new Dictionary<string, object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleConfigSettingsMgr"/> class.
        /// </summary>
        public SimpleConfigSettingsMgr()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleConfigSettingsMgr"/> class.
        /// </summary>
        /// <param name="loadFunc">The function load.</param>
        public SimpleConfigSettingsMgr(Func<IConfigSettingsMgr, bool> loadFunc)
        {
            Load(loadFunc);
        }

        /// <summary>
        /// Gets the setting value. If it is not found, the default value is returned
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The setting or the default value.</returns>
        public T GetValue<T>(string key, T defaultValue = default(T))
        {
            var result = defaultValue;
            object tmp = result;

            if (_settings.ContainsKey(key)) tmp = _settings[key];

            result = (T)Convert.ChangeType(tmp, typeof(T));

            return result;
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
            _settings[key] = value;

            return true;
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Object.</returns>
        public object this[string key]
        {
            get { return GetValue<object>(key); }
            set { SetValue(key, value); }
        }

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
        /// Refreshes this instance.
        /// </summary>
        //public void Refresh()
        //{
        //}

        /// <summary>
        /// Flushes this instance.
        /// </summary>
        public void Flush()
        {
        }
    }
}