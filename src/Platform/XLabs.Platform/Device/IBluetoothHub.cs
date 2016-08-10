// ***********************************************************************
// Assembly         : XLabs.Platform
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="IBluetoothHub.cs" company="XLabs Team">
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

using System.Collections.Generic;
using System.Threading.Tasks;

namespace XLabs.Platform.Device
{
    /// <summary>
    /// Interface IBluetoothHub
    /// </summary>
    public interface IBluetoothHub
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="IBluetoothHub"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        bool Enabled { get; }

        /// <summary>
        /// Gets the paired devices.
        /// </summary>
        /// <returns>Task&lt;IReadOnlyList&lt;IBluetoothDevice&gt;&gt;.</returns>
        Task<IReadOnlyList<IBluetoothDevice>> GetPairedDevices();

        /// <summary>
        /// Opens the settings.
        /// </summary>
        /// <returns>Task.</returns>
        Task OpenSettings();
    }
}