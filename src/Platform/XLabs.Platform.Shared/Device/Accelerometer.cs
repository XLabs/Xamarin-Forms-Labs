// ***********************************************************************
// Assembly         : XLabs.Platform.WP81
// Author           : XLabs Team
// Created          : 01-02-2016
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

using System;

namespace XLabs.Platform.Device
{
    /// <summary>
    /// Class Accelerometer.
    /// </summary>
    public partial class Accelerometer : IAccelerometer
    {
        /// <summary>
        /// Gravitational force is 9.81 m/s^2
        /// </summary>
        public const double Gravitation = 9.81;

        /// <summary>
        /// Initializes a new instance of the <see cref="Accelerometer" /> class.
        /// </summary>
        public Accelerometer()
        {
            this.Interval = AccelerometerInterval.Ui;
        }

        /// <summary>
        /// The reading available event handler.
        /// </summary>
        public event EventHandler<EventArgs<Vector3>> ReadingAvailable
        {
            add
            {
                if (this.readingAvailable == null)
                {
                    this.Start();
                }

                this.readingAvailable += value;
            }

            remove
            {
                this.readingAvailable -= value;
                if (this.readingAvailable == null)
                {
                    this.Stop();
                }
            }
        }

        /// <summary>
        /// Occurs when [reading available].
        /// </summary>
        private event EventHandler<EventArgs<Vector3>> readingAvailable;

        /// <summary>
        /// Gets the latest reading.
        /// </summary>
        /// <value>The latest reading.</value>
        public Vector3 LatestReading
        {
            get;
            private set;
        }

        partial void Start();

        partial void Stop();
    }
}
