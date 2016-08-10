// ***********************************************************************
// Assembly         : XLabs.Platform.WP81
// Author           : XLabs Team
// Created          : 01-02-2016
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

using System;

namespace XLabs.Platform.Device
{
	/// <summary>
	/// Class Gyroscope.
	/// </summary>
	public partial class Gyroscope : IGyroscope
	{
		/// <summary>
		/// Occurs when [reading available].
		/// </summary>
		private event EventHandler<EventArgs<Vector3>> readingAvailable;

		/// <summary>
		/// Initializes a new instance of the <see cref="Gyroscope"/> class.
		/// </summary>
		public Gyroscope()
		{
			this.Interval = AccelerometerInterval.Ui;
		}

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
		/// Gets the latest reading vector
		/// </summary>
		/// <value>Rotation values in radians per second</value>
		public Vector3 LatestReading
		{
			get;
			private set;
		}

		partial void Start();

		partial void Stop();
	}
}
