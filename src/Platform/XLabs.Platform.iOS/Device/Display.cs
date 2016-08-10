// ***********************************************************************
// Assembly         : XLabs.Platform.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Display.cs" company="XLabs Team">
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

using UIKit;

namespace XLabs.Platform.Device
{
    /// <summary>
    ///     Apple Display class.
    /// </summary>
    public class Display : IDisplay
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Display" /> class.
        /// </summary>
        /// <param name="height">Height in pixels.</param>
        /// <param name="width">Width in pixels.</param>
        /// <param name="xdpi">Pixel density for X.</param>
        /// <param name="ydpi">Pixel density for  Y.</param>
        internal Display(int height, int width, double xdpi, double ydpi)
        {
            Height = height;
            Width = width;
            Xdpi = xdpi;
            Ydpi = ydpi;

            //FontManager = new FontManager(this);
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents the current <see cref="Display" />.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents the current <see cref="Display" />.</returns>
        public override string ToString()
        {
            return string.Format("[Screen: Height={0}, Width={1}, Xdpi={2}, Ydpi={3}]", Height, Width, Xdpi, Ydpi);
        }

        #region IScreen implementation

        /// <summary>
        ///     Gets the screen height in pixels
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        ///     Gets the screen width in pixels
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        ///     Gets the screens X pixel density per inch
        /// </summary>
        public double Xdpi { get; private set; }

        /// <summary>
        ///     Gets the screens Y pixel density per inch
        /// </summary>
        public double Ydpi { get; private set; }

        /// <summary>
        /// Gets the scale value of the display.
        /// </summary>
        public double Scale
        {
            get
            {
                return UIScreen.MainScreen.Scale;
            }
        }

        /// <summary>
        ///     Convert width in inches to runtime pixels
        /// </summary>
        public double WidthRequestInInches(double inches)
        {
            return inches * Xdpi / UIScreen.MainScreen.Scale;
        }

        /// <summary>
        ///     Convert height in inches to runtime pixels
        /// </summary>
        public double HeightRequestInInches(double inches)
        {
            return inches * Ydpi / UIScreen.MainScreen.Scale;
        }

        #endregion
    }
}