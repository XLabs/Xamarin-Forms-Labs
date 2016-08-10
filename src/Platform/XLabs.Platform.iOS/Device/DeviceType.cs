// ***********************************************************************
// Assembly         : XLabs.Platform.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="DeviceType.cs" company="XLabs Team">
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
    /// Apple device type enumeration.
    /// </summary>
    public enum DeviceType
    {
        /// <summary>
        /// Device is an iPhone.
        /// </summary>
        Phone,
        
        /// <summary>
        /// Device is an iPad.
        /// </summary>
        Pad,

        /// <summary>
        /// Device is an iPod.
        /// </summary>
        Pod,

        /// <summary>
        /// Device is a simulator.
        /// </summary>
        Simulator
    }
}