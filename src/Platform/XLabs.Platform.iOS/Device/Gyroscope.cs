// ***********************************************************************
// Assembly         : XLabs.Platform.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Gyroscope.cs" company="XLabs Team">
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

using CoreMotion;
using Foundation;

namespace XLabs.Platform.Device
{
	/// <summary>
	/// Class Gyroscope.
	/// </summary>
	public partial class Gyroscope
	{
		/// <summary>
		/// The _motion manager
		/// </summary>
		private CMMotionManager _motionManager;

		/// <summary>
		/// Gets or sets the interval.
		/// </summary>
		/// <value>The interval.</value>
		public AccelerometerInterval Interval { get; set; }

		/// <summary>
		/// Gets a value indicating whether this instance is supported.
		/// </summary>
		/// <value><c>true</c> if this instance is supported; otherwise, <c>false</c>.</value>
		public static bool IsSupported
		{
			get
			{
				return new CMMotionManager().GyroAvailable;
			}
		}

		/// <summary>
		/// Starts this instance.
		/// </summary>
		partial void Start()
		{
			_motionManager = new CMMotionManager();
			_motionManager.GyroUpdateInterval = (long)Interval / 1000;
			_motionManager.StartGyroUpdates(NSOperationQueue.MainQueue, OnUpdate);
		}

		/// <summary>
		/// Stops this instance.
		/// </summary>
		partial void Stop()
		{
			_motionManager.StopGyroUpdates();
			_motionManager = null;
		}

		/// <summary>
		/// Called when [update].
		/// </summary>
		/// <param name="gyroData">The gyro data.</param>
		/// <param name="error">The error.</param>
		private void OnUpdate(CMGyroData gyroData, NSError error)
		{
			if (error != null)
			{
				this.readingAvailable.Invoke(
					this,
					new Vector3(gyroData.RotationRate.x, gyroData.RotationRate.y, gyroData.RotationRate.z));
			}
		}
	}
}