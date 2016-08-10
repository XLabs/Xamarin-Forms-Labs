// ***********************************************************************
// Assembly         : XLabs.Settings.XamSettings
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="XamSettingsConfigSettings.cs" company="XLabs Team">
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
using Plugin.Settings;

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
