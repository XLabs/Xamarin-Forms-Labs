//// ***********************************************************************
//// Assembly         : XLabs.Platform.WinUniversal
//// Author           : XLabs Team
//// Created          : 01-01-2016
////
//// Last Modified By : XLabs Team
//// Last Modified On : 01-04-2016
//// ***********************************************************************
//// <copyright file="BluetoothHub.cs" company="XLabs Team">
////     Copyright (c) XLabs Team. All rights reserved.
//// </copyright>
//// <summary>
////       This project is licensed under the Apache 2.0 license
////       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
////
////       XLabs is a open source project that aims to provide a powerfull and cross
////       platform set of controls tailored to work with Xamarin Forms.
//// </summary>
//// ***********************************************************************
////

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Windows.Networking.Proximity;

//namespace XLabs.Platform.Device
//{
//	/// <summary>
//	/// Class BluetoothHub.
//	/// </summary>
//	public class BluetoothHub : IBluetoothHub
//	{
//		/// <summary>
//		/// Initializes a new instance of the <see cref="BluetoothHub"/> class.
//		/// </summary>
//		public BluetoothHub()
//		{
//		}

//		/// <summary>
//		/// Gets a value indicating whether this <see cref="IBluetoothHub" /> is enabled.
//		/// </summary>
//		/// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
//		public bool Enabled
//		{
//			get
//			{
//				return PeerFinder.AllowBluetooth;
//			}
//		}

//		/// <summary>
//		/// Gets the open settings.
//		/// </summary>
//		/// <value>The open settings.</value>
//		public Task OpenSettings()
//		{
//			return Task.Run(() => Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-bluetooth:///")));
//		}

//		/// <summary>
//		/// Gets the paired devices.
//		/// </summary>
//		/// <returns>Task&lt;IReadOnlyList&lt;IBluetoothDevice&gt;&gt;.</returns>
//		public async Task<IReadOnlyList<IBluetoothDevice>> GetPairedDevices()
//		{
//			PeerFinder.AlternateIdentities["Bluetooth:PAIRED"] = string.Empty;
//			var devices = await PeerFinder.FindAllPeersAsync();
//			return devices.Select(a => new BluetoothDevice(a)).ToList();
//		}
//	}
//}