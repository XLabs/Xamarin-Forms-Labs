// ***********************************************************************
// Assembly         : XLabs.Platform
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Sensor.cs" company="XLabs Team">
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
	/// Class DeviceSensor.
	/// </summary>
	public abstract class DeviceSensor : ISensor
	{
		/// <summary>
		/// Occurs when [reading available].
		/// </summary>
		protected event EventHandler<EventArgs<Vector3>> readingAvailable;

		/// <summary>
		/// Occurs when [reading available].
		/// </summary>
		public event EventHandler<EventArgs<Vector3>> ReadingAvailable
		{
			add
			{
				if (readingAvailable == null)
				{
					Start();
				}
				readingAvailable += value;
			}
			remove
			{
				readingAvailable -= value;
				if (readingAvailable == null)
				{
					Stop();
				}
			}
		}

		/// <summary>
		/// Gets the latest reading.
		/// </summary>
		/// <value>The latest reading.</value>
		public Vector3 LatestReading
		{
			get;
			protected set;
		}

		/// <summary>
		/// Starts this instance.
		/// </summary>
		protected abstract void Start();

		/// <summary>
		/// Stops this instance.
		/// </summary>
		protected abstract void Stop();

		/// <summary>
		/// Gets or sets the interval.
		/// </summary>
		/// <value>The interval.</value>
		public abstract AccelerometerInterval Interval { get; set;}
	}
}
