// ***********************************************************************
// Assembly         : XLabs.Platform
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="IAccelerometer.cs" company="XLabs Team">
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

namespace XLabs.Platform.Device
{
	/// <summary>
	/// The Accelerometer interface.
	/// </summary>
	public interface IAccelerometer
	{
		/// <summary>
		/// The reading available event handler.
		/// </summary>
		event EventHandler<EventArgs<Vector3>> ReadingAvailable;

		/// <summary>
		/// Gets the latest reading.
		/// </summary>
		/// <value>The latest reading.</value>
		Vector3 LatestReading { get; }

		/// <summary>
		/// Gets or sets the interval.
		/// </summary>
		/// <value>The interval.</value>
		AccelerometerInterval Interval { get; set; }
	}

	/// <summary>
	/// The accelerometer interval.
	/// </summary>
	public enum AccelerometerInterval
	{
		/// <summary>
		/// The fastest interval.
		/// </summary>
		Fastest = 1,

		/// <summary>
		/// The game interval, approximately 20ms.
		/// </summary>
		Game = 20,

		/// <summary>
		/// The UI interval, approximately 70ms.
		/// </summary>
		Ui = 70,

		/// <summary>
		/// The normal interval, approximately 200ms.
		/// </summary>
		Normal = 200
	}
}
