// ***********************************************************************
// Assembly         : XLabs.Settings
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="IConfigSettingsMgr.cs" company="XLabs Team">
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
    public interface IConfigSettingsMgr
    {
        /// <summary>
        /// Gets the setting value. If it is not found, the default value is returned
        /// </summary>
        /// <typeparam name="T">the type of the setting</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The setting or the default value.</returns>
        T GetValue<T>(string key, T defaultValue = default(T));        

        /// <summary>
        /// Sets the setting value.
        /// </summary>
        /// <typeparam name="T">the type of the setting</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if setting the value was a success, <c>false</c> otherwise.</returns>
        bool SetValue<T>(string key, T value);

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> with the specified setting value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Object.</returns>
        object this[string key] { get; set; }

        /// <summary>
        /// Loads the configuration settings.
        /// </summary>
        /// <param name="loadFunc">The optional function to execute during the load process.</param>
        void Load(Func<IConfigSettingsMgr, bool> loadFunc = null);

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        //void Refresh();

        /// <summary>
        /// Flushes this instance.
        /// </summary>
        void Flush();
    }
}
