// ***********************************************************************
// Assembly         : XLabs.Platform.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Accelerometer.cs" company="XLabs Team">
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

using UIKit;

namespace XLabs.Platform.Device
{
	/// <summary>
	/// The accelerometer.
	/// </summary>
	public partial class Accelerometer
	{
		/// <summary>
		/// The _interval
		/// </summary>
		private AccelerometerInterval _interval = AccelerometerInterval.Ui;

		/// <summary>
		/// Gets or sets the interval.
		/// </summary>
		/// <value>The interval.</value>
		public AccelerometerInterval Interval
		{
			get
			{
				return _interval;
			}
			set
			{
				if (_interval != value)
				{
					_interval = value;
					UIAccelerometer.SharedAccelerometer.UpdateInterval = ((long)_interval) / 1000d;
				}
			}
		}

		/// <summary>
		/// Starts this instance.
		/// </summary>
		partial void Start()
		{
			UIAccelerometer.SharedAccelerometer.Acceleration += HandleAcceleration;
		}

		/// <summary>
		/// Stops this instance.
		/// </summary>
		partial void Stop()
		{
			UIAccelerometer.SharedAccelerometer.Acceleration -= HandleAcceleration;
		}

		/// <summary>
		/// Handles the acceleration.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="UIAccelerometerEventArgs"/> instance containing the event data.</param>
		private void HandleAcceleration(object sender, UIAccelerometerEventArgs e)
		{
			readingAvailable.Invoke(sender, new Vector3(e.Acceleration.X, e.Acceleration.Y, e.Acceleration.Z));
		}
	}
}