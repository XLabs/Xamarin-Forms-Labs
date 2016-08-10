// ***********************************************************************
// Assembly         : XLabs.Platform.WinUniversal
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

using Windows.Devices.Sensors;

namespace XLabs.Platform.Device
{
    using AccelerometerSensor = Windows.Devices.Sensors.Accelerometer;

    /// <summary>
    /// Class Accelerometer.
    /// </summary>
    public partial class Accelerometer
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
            var accelerometer = AccelerometerSensor.GetDefault();

            if (accelerometer == null) return;

            accelerometer.ReportInterval = (uint) this.Interval;

            accelerometer.ReadingChanged += AccelerometerOnReadingChanged;
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        partial void Stop()
        {
            var accelerometer = AccelerometerSensor.GetDefault();

            if (accelerometer != null)
            {
                accelerometer.ReadingChanged -= AccelerometerOnReadingChanged;
            }
        }

        private void AccelerometerOnReadingChanged(AccelerometerSensor sender, AccelerometerReadingChangedEventArgs args)
        {
            LatestReading = args.Reading.AsVector3();
            readingAvailable.Invoke(sender, LatestReading);
        }
    }
}