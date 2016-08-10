// ***********************************************************************
// Assembly         : XLabs.Platform
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="IDisplay.cs" company="XLabs Team">
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
    /// Portable interface for device screen information
    /// </summary>
    public interface IDisplay
    {
        /// <summary>
        /// Gets the screen height in pixels
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Gets the screen width in pixels
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets the screens X pixel density per inch
        /// </summary>
        double Xdpi { get; }

        /// <summary>
        /// Gets the screens Y pixel density per inch
        /// </summary>
        double Ydpi { get; }

        /// <summary>
        /// Gets the scale value of the display.
        /// </summary>
        double Scale { get; }

        /// <summary>
        /// Convert width in inches to runtime pixels
        /// </summary>
        double WidthRequestInInches(double inches);

        /// <summary>
        /// Convert height in inches to runtime pixels
        /// </summary>
        double HeightRequestInInches(double inches);
    }
}

