// ***********************************************************************
// Assembly         : XLabs.Platform.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="BluetoothDevice.cs" company="XLabs Team">
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
using CoreBluetooth;

namespace XLabs.Platform.Device
{
	/// <summary>
	/// Class BluetoothDevice.
	/// </summary>
	public class BluetoothDevice : IBluetoothDevice
	{
		/// <summary>
		/// The device
		/// </summary>
		private readonly CBPeripheral device;

		/// <summary>
		/// Initializes a new instance of the <see cref="BluetoothDevice"/> class.
		/// </summary>
		/// <param name="device">The device.</param>
		public BluetoothDevice(CBPeripheral device)
		{
			this.device = device;
		}

		#region IBluetoothDevice implementation

		/// <summary>
		/// Connects this instance.
		/// </summary>
		/// <returns>System.Threading.Tasks.Task.</returns>
		/// <exception cref="NotImplementedException"></exception>
		public System.Threading.Tasks.Task Connect()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Disconnects this instance.
		/// </summary>
		/// <exception cref="NotImplementedException"></exception>
		public void Disconnect()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get
			{
				return device.Name;
			}
		}

		/// <summary>
		/// Gets the address.
		/// </summary>
		/// <value>The address.</value>
		public string Address
		{
			get
			{
				return device.Identifier.AsString();
			}
		}

		/// <summary>
		/// Gets the input stream.
		/// </summary>
		/// <value>The input stream.</value>
		/// <exception cref="NotImplementedException"></exception>
		public System.IO.Stream InputStream
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>
		/// Gets the output stream.
		/// </summary>
		/// <value>The output stream.</value>
		/// <exception cref="NotImplementedException"></exception>
		public System.IO.Stream OutputStream
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		#endregion

		/// <summary>
		/// Class BtDelegate.
		/// </summary>
		private class BtDelegate : CBPeripheralDelegate
		{

		}
	}
}