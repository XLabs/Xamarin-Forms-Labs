// ***********************************************************************
// Assembly         : XLabs.Platform
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="INfcDevice.cs" company="XLabs Team">
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

namespace XLabs.Platform.Services
{
	/// <summary>
	/// Interface INfcDevice
	/// </summary>
	public interface INfcDevice
	{
		//string DeviceId { get; }

		/// <summary>
		/// Gets a value indicating whether this instance is enabled.
		/// </summary>
		/// <value><c>true</c> if this instance is enabled; otherwise, <c>false</c>.</value>
		bool IsEnabled { get; }

		/// <summary>
		/// Occurs when [device in range].
		/// </summary>
		event EventHandler<EventArgs<INfcDevice>> DeviceInRange;

		/// <summary>
		/// Occurs when [device out of range].
		/// </summary>
		event EventHandler<EventArgs<INfcDevice>> DeviceOutOfRange;

		//byte[] Message { get; set; }

		/// <summary>
		/// Publishes the URI.
		/// </summary>
		/// <param name="uri">The URI.</param>
		/// <returns>Guid.</returns>
		Guid PublishUri(Uri uri);

		/// <summary>
		/// Unpublishes the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		void Unpublish(Guid id);
	}

	/// <summary>
	/// Enum NdefType
	/// </summary>
	public enum NdefType
	{
		/// <summary>
		/// The URI
		/// </summary>
		Uri = 0x01
	}
}