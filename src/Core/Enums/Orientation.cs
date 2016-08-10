// ***********************************************************************
// Assembly         : XLabs.Core
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Orientation.cs" company="XLabs Team">
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

namespace XLabs.Enums
{
    /// <summary>
    /// Enum Orientation
    /// </summary>
    [Flags]
    public enum Orientation
    {
        /// <summary>
        /// The none
        /// </summary>
        None = 0,
        /// <summary>
        /// The portrait
        /// </summary>
        Portrait = 1,
        /// <summary>
        /// The landscape
        /// </summary>
        Landscape = 2,
        /// <summary>
        /// The portrait up
        /// </summary>
        PortraitUp = 5,
        /// <summary>
        /// The portrait down
        /// </summary>
        PortraitDown = 9,
        /// <summary>
        /// The landscape left
        /// </summary>
        LandscapeLeft = 18,
        /// <summary>
        /// The landscape right
        /// </summary>
        LandscapeRight = 34,
    }
}
