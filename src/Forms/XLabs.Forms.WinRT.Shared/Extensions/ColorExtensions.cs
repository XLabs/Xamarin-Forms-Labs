// ***********************************************************************
// Assembly         : XLabs.Forms.WinUniversal
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ColorExtensions.cs" company="XLabs Team">
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

using Windows.UI;
using Windows.UI.Xaml.Media;

namespace XLabs.Forms
{
    /// <summary>
    /// Class ColorExtensions.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// To the brush.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>Brush.</returns>
        public static Brush ToBrush(this Xamarin.Forms.Color color)
        {
            return new SolidColorBrush(color.ToMediaColor());
        }

        /// <summary>
        /// To the color of the media.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>Color.</returns>
        public static Color ToMediaColor(this Xamarin.Forms.Color color)
        {
            return Color.FromArgb((byte)(color.A * 255.0), (byte)(color.R * 255.0), (byte)(color.G * 255.0), (byte)(color.B * 255.0));
        }
    }
}
