// ***********************************************************************
// Assembly         : XLabs.Platform.WinUniversal
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

using Windows.Devices.Sensors;

namespace XLabs.Platform.Device
{
    /// <summary>
    /// Class Gyroscope.
    /// </summary>
    public partial class Gyroscope
    {
        /// <summary>
        /// Gets or sets the interval.
        /// </summary>
        /// <value>The interval.</value>
        public AccelerometerInterval Interval { get; set; }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        partial void Start()
        {
            var gyroscope = Gyrometer.GetDefault();

            if (gyroscope == null) return;

            gyroscope.ReportInterval = (uint)Interval;

            gyroscope.ReadingChanged += GyroscopeReadingChanged;
        }

        /// <summary>
        /// Gyroscope reading has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The gyroscope event arguments.</param>
        private void GyroscopeReadingChanged(Gyrometer sender, GyrometerReadingChangedEventArgs args)
        {
            LatestReading = args.Reading.AsVector3();
            readingAvailable.Invoke(this, this.LatestReading);
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        partial void Stop()
        {
            var gyroscope = Gyrometer.GetDefault();

            if (gyroscope != null)
            {
                gyroscope.ReadingChanged -= GyroscopeReadingChanged;
            }
        }
    }
}