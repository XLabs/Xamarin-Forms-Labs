// ***********************************************************************
// Assembly         : XLabs.Platform
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="IBluetoothDevice.cs" company="XLabs Team">
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

using System.IO;
using System.Threading.Tasks;

namespace XLabs.Platform.Device
{
	/// <summary>
	/// Interface IBluetoothDevice
	/// </summary>
	public interface IBluetoothDevice
	{
		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		string Name { get; }
		/// <summary>
		/// Gets the address.
		/// </summary>
		/// <value>The address.</value>
		string Address { get; }

		/// <summary>
		/// Gets the input stream.
		/// </summary>
		/// <value>The input stream.</value>
		Stream InputStream { get; }
		/// <summary>
		/// Gets the output stream.
		/// </summary>
		/// <value>The output stream.</value>
		Stream OutputStream { get; }

		/// <summary>
		/// Connects this instance.
		/// </summary>
		/// <returns>Task.</returns>
		Task Connect();
		/// <summary>
		/// Disconnects this instance.
		/// </summary>
		void Disconnect(); 
	}
}