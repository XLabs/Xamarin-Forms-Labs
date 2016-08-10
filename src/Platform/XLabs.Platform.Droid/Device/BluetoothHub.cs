// ***********************************************************************
// Assembly         : XLabs.Platform.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="BluetoothHub.cs" company="XLabs Team">
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
using System.Linq;
using System.Threading.Tasks;
using Android.Bluetooth;
using Android.Content;

namespace XLabs.Platform.Device
{
    /// <summary>
    /// Class BluetoothHub.
    /// </summary>
    public class BluetoothHub : IBluetoothHub
    {
        /// <summary>
        /// The _adapter
        /// </summary>
        private readonly BluetoothAdapter _adapter;

        /// <summary>
        /// Initializes a new instance of the <see cref="BluetoothHub"/> class.
        /// </summary>
        public BluetoothHub()
            : this(BluetoothAdapter.DefaultAdapter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BluetoothHub"/> class.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        public BluetoothHub(BluetoothAdapter adapter)
        {
            _adapter = adapter;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IBluetoothHub" /> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get
            {
                return _adapter.IsEnabled;
            }
        }

        #region IBluetoothHub implementation

        /// <summary>
        /// Gets the paired devices.
        /// </summary>
        /// <returns>Task&lt;IReadOnlyList&lt;IBluetoothDevice&gt;&gt;.</returns>
        public async Task<IReadOnlyList<IBluetoothDevice>> GetPairedDevices()
        {
            return await Task.Factory.StartNew(() => _adapter.BondedDevices.Select(a => new AndroidBluetoothDevice(a)).ToList());
        }

        /// <summary>
        /// Gets the open settings.
        /// </summary>
        /// <value>The open settings.</value>
        public Task OpenSettings()
        {
            return Task.Run(() => this.StartActivity(new Intent(BluetoothAdapter.ActionRequestEnable)));
        }

        #endregion
    }
}