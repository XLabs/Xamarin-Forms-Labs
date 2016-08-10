// ***********************************************************************
// Assembly         : XLabs.Platform.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="DeviceInfo.cs" company="XLabs Team">
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
namespace XLabs.Platform.Device
{
    /// <summary>
    /// Apple device base information.
    /// </summary>
    public class DeviceInfo
    {
        internal DeviceInfo(DeviceType type, int majorVersion, int minorVersion)
        {
            this.Type = type;
            this.MajorVersion = majorVersion;
            this.MinorVersion = minorVersion;
        }

        /// <summary>
        /// Gets the type of device.
        /// </summary>
        public DeviceType Type { get; private set; }

        /// <summary>
        /// Device major version.
        /// </summary>
        public int MajorVersion { get; private set; }

        /// <summary>
        /// Device minor version.
        /// </summary>
        public int MinorVersion { get; private set; }
    }
}