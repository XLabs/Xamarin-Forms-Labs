// ***********************************************************************
// Assembly         : XLabs.Platform
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="IDevice.cs" company="XLabs Team">
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
using System.Threading.Tasks;
using XLabs.Enums;
using XLabs.Platform.Services;
using XLabs.Platform.Services.IO;
using XLabs.Platform.Services.Media;

namespace XLabs.Platform.Device
{
    /// <summary>
    /// Abstracted device interface.
    /// </summary>
    public interface IDevice
    {
        /// <summary>
        /// Gets Unique Id for the device.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the display information for the device.
        /// </summary>
        IDisplay Display { get; }

        /// <summary>
        /// Gets the phone service for this device.
        /// </summary>
        /// <value>Phone service instance if available, otherwise null.</value>
        IPhoneService PhoneService { get; }

        /// <summary>
        /// Gets the battery for the device.
        /// </summary>
        IBattery Battery { get; }

        /// <summary>
        /// Gets the accelerometer.
        /// </summary>
        /// <value>The accelerometer instance if available, otherwise null.</value>
        IAccelerometer Accelerometer { get; }

        /// <summary>
        /// Gets the gyroscope.
        /// </summary>
        /// <value>The gyroscope instance if available, otherwise null.</value>
        IGyroscope Gyroscope { get; }

        /// <summary>
        /// Gets the picture chooser.
        /// </summary>
        /// <value>The picture chooser.</value>
        IMediaPicker MediaPicker { get; }

        /// <summary>
        /// Gets the network service.
        /// </summary>
        /// <value>The network service.</value>
        INetwork Network { get; }

        /// <summary>
        /// Gets the bluetooth hub service.
        /// </summary>
        /// <value>The bluetooth hub service if available, otherwise null.</value>
        IBluetoothHub BluetoothHub { get; }

        /// <summary>
        /// Gets the default microphone for the device
        /// </summary>
        /// <value>The default microphone if available, otherwise null.</value>
        IAudioStream Microphone { get; }

        /// <summary>
        /// Gets the file manager for the device.
        /// </summary>
        /// <value>Device file manager.</value>
        IFileManager FileManager { get; }

        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        /// <value>The name of the device.</value>
        string Name { get; }

        /// <summary>
        /// Gets the firmware version.
        /// </summary>
        string FirmwareVersion { get; }

        /// <summary>
        /// Gets the hardware version.
        /// </summary>
        string HardwareVersion { get; }

        /// <summary>
        /// Gets the manufacturer.
        /// </summary>
        string Manufacturer { get; }

        /// <summary>
        /// Gets the total memory in bytes.
        /// </summary>
        long TotalMemory { get; }

        /// <summary>
        /// Gets the ISO Language Code
        /// </summary>
        string LanguageCode { get; }

        /// <summary>
        /// Gets the UTC offset
        /// </summary>
        double TimeZoneOffset { get; }

        /// <summary>
        /// Gets the timezone name
        /// </summary>
        string TimeZone { get; }
        
        /// <summary>
        /// Gets the <see cref="Orientation"/> of the device.
        /// </summary>
        Orientation Orientation { get; }

        /// <summary>
        /// Starts the default app associated with the URI for the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>The launch operation.</returns>
        Task<bool> LaunchUriAsync(Uri uri);
    }
}

