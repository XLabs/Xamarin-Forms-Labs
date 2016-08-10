// ***********************************************************************
// Assembly         : XLabs.Settings
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ConfigSettingsMgr.cs" company="XLabs Team">
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