// ***********************************************************************
// Assembly         : XLabs.Platform.WinUniversal
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="VectorExtensions.cs" company="XLabs Team">
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

namespace XLabs.Platform
{
    /// <summary>
    /// Class VectorExtensions.
    /// </summary>
    public static class VectorExtensions
    {
        /// <summary>
        /// Returns the Accelerometer Reading as a Vector3
        /// </summary>
        /// <param name="reading">The reading.</param>
        /// <returns>Vector3.</returns>
        public static Vector3 AsVector3(this AccelerometerReading reading)
        {
            return new Vector3(reading.AccelerationX, reading.AccelerationY, reading.AccelerationZ);
        }

        /// <summary>
        /// Returns the Gyrometer Reading as a Vector3
        /// </summary>
        /// <param name="reading">The reading.</param>
        /// <returns>Vector3.</returns>
        public static Vector3 AsVector3(this GyrometerReading reading)
        {
            return new Vector3(reading.AngularVelocityX, reading.AngularVelocityY, reading.AngularVelocityZ);
        }
    }
}