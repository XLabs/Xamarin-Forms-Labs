// ***********************************************************************
// Assembly         : XLabs.Platform.Droid
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

using Android.App;
using Android.Util;

namespace XLabs.Platform.Device
{
    /// <summary>
    ///     Android Display implements <see cref="IDisplay" />.
    /// </summary>
    public class Display : IDisplay
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Display" /> class.
        /// </summary>
        public Display()
        {
            var dm = Metrics;
            Height = dm.HeightPixels;
            Width = dm.WidthPixels;
            Xdpi = dm.Xdpi;
            Ydpi = dm.Ydpi;

            //FontManager = new FontManager(this);
        }

        /// <summary>
        ///     Gets the metrics.
        /// </summary>
        /// <value>The metrics.</value>
        public static DisplayMetrics Metrics
        {
            get
            {
                return Application.Context.Resources.DisplayMetrics;
            }
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents the current <see cref="Display" />.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents the current <see cref="Display" />.</returns>
        public override string ToString()
        {
            return string.Format("[Screen: Height={0}, Width={1}, Xdpi={2:0.0}, Ydpi={3:0.0}]", Height, Width, Xdpi, Ydpi);
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

        //public IFontManager FontManager { get; private set; }

        /// <summary>
        ///     Convert width in inches to runtime pixels
        /// </summary>
        public double WidthRequestInInches(double inches)
        {
            return inches * Xdpi / Metrics.Density;
        }

        /// <summary>
        ///     Convert height in inches to runtime pixels
        /// </summary>
        public double HeightRequestInInches(double inches)
        {
            return inches * Ydpi / Metrics.Density;
        }

        /// <summary>
        /// Gets the scale value of the display.
        /// </summary>
        /// <value>The scale.</value>
        public double Scale
        {
            get { return Metrics.Density; }
        }
        #endregion

    }
}