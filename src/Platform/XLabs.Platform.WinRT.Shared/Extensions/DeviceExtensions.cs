// ***********************************************************************
// Assembly         : XLabs.Platform.WinUniversal
// Author           : XLabs Team
// Created          : 01-01-2016
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="DeviceExtensions.cs" company="XLabs Team">
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

using System.Threading.Tasks;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Geolocation;

namespace XLabs.Platform
{
	/// <summary>
	///     Class DeviceExtensions.
	/// </summary>
	public static class DeviceExtensions
	{
		/// <summary>
		///     Drives to.
		/// </summary>
		/// <param name="device">The device.</param>
		/// <param name="position">The position.</param>
		/// <returns>Task&lt;System.Boolean&gt;.</returns>
		public static Task<bool> DriveTo(this IDevice device, Position position)
		{
			return device.LaunchUriAsync(position.DriveToLink());
		}
	}
}