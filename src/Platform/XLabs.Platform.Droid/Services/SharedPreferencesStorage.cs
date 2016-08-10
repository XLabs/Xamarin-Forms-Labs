// ***********************************************************************
// Assembly         : XLabs.Platform.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="SharedPreferencesStorage.cs" company="XLabs Team">
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

using System.Text;
using Android.App;
using Android.Content;

namespace XLabs.Platform.Services
{
    /// <summary>
    /// Implements <see cref="ISecureStorage"/> for Android using <see cref="ISharedPreferences"/>.
    /// </summary>
    public class SharedPreferencesStorage : ISecureStorage
    {
        private readonly ISharedPreferences preferences;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharedPreferencesStorage"/> class.
        /// </summary>
        /// <param name="preferenceKey">Preferences key to use.</param>
        public SharedPreferencesStorage(string preferenceKey)
        {
            this.preferences = Application.Context.GetSharedPreferences(preferenceKey, FileCreationMode.Private);
        }

        #region ISecureStorage Members

        /// <summary>
        /// Stores data.
        /// </summary>
        /// <param name="key">Key for the data.</param>
        /// <param name="dataBytes">Data bytes to store.</param>
        public void Store(string key, byte[] dataBytes)
        {
            using (var editor = this.preferences.Edit())
            {
                editor.PutString(key, Encoding.UTF8.GetString(dataBytes));
                editor.Commit();
            }
        }

        /// <summary>
        /// Retrieves stored data.
        /// </summary>
        /// <param name="key">Key for the data.</param>
        /// <returns>Byte array of stored data.</returns>
        public byte[] Retrieve(string key)
        {
            return Encoding.UTF8.GetBytes(this.preferences.GetString(key, string.Empty));
        }

        /// <summary>
        /// Deletes data.
        /// </summary>
        /// <param name="key">Key for the data to be deleted.</param>
        public void Delete(string key)
        {
            if (!this.preferences.Contains(key))
            {
                return;
            }

            using (var editor = this.preferences.Edit())
            {
                editor.Remove(key);
                editor.Commit();
            }
        }

        /// <summary>
        /// Checks if the storage contains a key.
        /// </summary>
        /// <param name="key">The key to search.</param>
        /// <returns>True if the storage has the key, otherwise false.</returns>
        public bool Contains(string key)
        {
            return this.preferences.Contains(key);
        }

        #endregion
    }
}