// ***********************************************************************
// Assembly         : XLabs.Platform.Droid
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

using System;
using Android.App;
using Android.Content;
using Android.Hardware;
using Object = Java.Lang.Object;

namespace XLabs.Platform.Device
{
    /// <summary>
    ///     Class Accelerometer.
    /// </summary>
    public partial class Accelerometer : Object, ISensorEventListener
    {
        /// <summary>
        ///     The accelerometer
        /// </summary>
        private Sensor accelerometer;

        /// <summary>
        ///     The delay
        /// </summary>
        private SensorDelay delay;

        /// <summary>
        ///     The sensor manager
        /// </summary>
        private SensorManager sensorManager;

        /// <summary>
        ///     Gets a value indicating whether this instance is supported.
        /// </summary>
        /// <value><c>true</c> if this instance is supported; otherwise, <c>false</c>.</value>
        public static bool IsSupported
        {
            get
            {
                using (var sensorManager = Application.Context.GetSystemService(Context.SensorService) as SensorManager)
                {
                    return sensorManager != null && sensorManager.GetDefaultSensor(SensorType.Accelerometer) != null;
                }
            }
        }

        #region IAccelerometer Members

        /// <summary>
        ///     Gets or sets the interval.
        /// </summary>
        /// <value>The interval.</value>
        public AccelerometerInterval Interval
        {
            get
            {
                switch (this.delay)
                {
                    case SensorDelay.Fastest:
                        return AccelerometerInterval.Fastest;
                    case SensorDelay.Game:
                        return AccelerometerInterval.Game;
                    case SensorDelay.Normal:
                        return AccelerometerInterval.Normal;
                    default:
                        return AccelerometerInterval.Ui;
                }
            }
            set
            {
                switch (value)
                {
                    case AccelerometerInterval.Fastest:
                        this.delay = SensorDelay.Fastest;
                        break;
                    case AccelerometerInterval.Game:
                        this.delay = SensorDelay.Game;
                        break;
                    case AccelerometerInterval.Normal:
                        this.delay = SensorDelay.Normal;
                        break;
                    case AccelerometerInterval.Ui:
                        this.delay = SensorDelay.Ui;
                        break;
                }
            }
        }

        #endregion

        /// <summary>
        ///     Called when the accuracy of a sensor has changed.
        /// </summary>
        /// <param name="sensor">To be added.</param>
        /// <param name="accuracy">The new accuracy of this sensor</param>
        /// <since version="Added in API level 3" />
        /// <remarks>
        ///     <para tool="javadoc-to-mdoc">
        ///         Called when the accuracy of a sensor has changed.
        ///     </para>
        ///     <para tool="javadoc-to-mdoc">
        ///         See
        ///         <c>
        ///             <see cref="T:Android.Hardware.SensorManager" />
        ///         </c>
        ///         for details.
        ///     </para>
        ///     <para tool="javadoc-to-mdoc">
        ///         <format type="text/html">
        ///             <a
        ///                 href="http://developer.android.com/reference/android/hardware/SensorEventListener.html#onAccuracyChanged(android.hardware.Sensor, int)"
        ///                 target="_blank">
        ///                 [Android Documentation]
        ///             </a>
        ///         </format>
        ///     </para>
        /// </remarks>
        void ISensorEventListener.OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
            //              throw new NotImplementedException ();
        }

        /// <summary>
        ///     Called when sensor values have changed.
        /// </summary>
        /// <param name="e">
        ///     the
        ///     <c>
        ///         <see cref="T:Android.Hardware.SensorEvent" />
        ///     </c>
        ///     .
        /// </param>
        /// <since version="Added in API level 3" />
        /// <remarks>
        ///     <para tool="javadoc-to-mdoc">
        ///         Called when sensor values have changed.
        ///     </para>
        ///     <para tool="javadoc-to-mdoc">
        ///         See
        ///         <c>
        ///             <see cref="T:Android.Hardware.SensorManager" />
        ///         </c>
        ///         for details on possible sensor types.
        ///     </para>
        ///     <para tool="javadoc-to-mdoc">
        ///         See also
        ///         <c>
        ///             <see cref="T:Android.Hardware.SensorEvent" />
        ///         </c>
        ///         .
        ///     </para>
        ///     <para tool="javadoc-to-mdoc">
        ///         <format type="text/html">
        ///             <b>NOTE:</b>
        ///         </format>
        ///         The application doesn't own the
        ///         <c>
        ///             <see cref="T:Android.Hardware.SensorEvent" />
        ///         </c>
        ///         object passed as a parameter and therefore cannot hold on to it.
        ///         The object may be part of an internal pool and may be reused by
        ///         the framework.
        ///     </para>
        ///     <para tool="javadoc-to-mdoc">
        ///         <format type="text/html">
        ///             <a
        ///                 href="http://developer.android.com/reference/android/hardware/SensorEventListener.html#onSensorChanged(android.hardware.SensorEvent)"
        ///                 target="_blank">
        ///                 [Android Documentation]
        ///             </a>
        ///         </format>
        ///     </para>
        /// </remarks>
        void ISensorEventListener.OnSensorChanged(SensorEvent e)
        {
            if (e.Sensor.Type != SensorType.Accelerometer)
            {
                return;
            }

            this.LatestReading = new Vector3(
                e.Values[0] / Gravitation,
                e.Values[1] / Gravitation,
                e.Values[2] / Gravitation);

            this.readingAvailable.Invoke(this, this.LatestReading);
        }

        /// <summary>
        ///     Starts this instance.
        /// </summary>
        partial void Start()
        {
            this.sensorManager = Application.Context.GetSystemService(Context.SensorService) as SensorManager;

            if (this.sensorManager == null)
            {
                throw new NotSupportedException("Sensor Manager not supported");
            }

            this.accelerometer = this.sensorManager.GetDefaultSensor(SensorType.Accelerometer);

            this.sensorManager.RegisterListener(this, this.accelerometer, this.delay);
        }

        /// <summary>
        ///     Stops this instance.
        /// </summary>
        partial void Stop()
        {
            if (this.sensorManager == null) return;

            this.sensorManager.UnregisterListener(this);
            this.sensorManager.Dispose();
            this.sensorManager = null;
            this.accelerometer.Dispose();
            this.accelerometer = null;
        }
    }
}