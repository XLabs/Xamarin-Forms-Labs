// ***********************************************************************
// Assembly         : XLabs.Forms.WinUniversal
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="FontExtensions.cs" company="XLabs Team">
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
using Windows.UI.Xaml.Media;
using Xamarin.Forms;

namespace XLabs.Forms
{
    /// <summary>
    /// Class FontExtensions.
    /// </summary>
    public static class FontExtensions
    {
        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <returns>System.Double.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static double GetHeight(this Font font)
        {
            if (!font.UseNamedSize) return font.FontSize;

            switch (font.NamedSize)
            {
                case NamedSize.Micro:
                    return (double)Application.Current.Resources["PhoneFontSizeSmall"] - 3.0;
                case NamedSize.Small:
                    return (double)Application.Current.Resources["PhoneFontSizeSmall"];
                case NamedSize.Default:
                case NamedSize.Medium:
                    return (double)Application.Current.Resources["PhoneFontSizeNormal"];
                case NamedSize.Large:
                    return (double)Application.Current.Resources["PhoneFontSizeLarge"];
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Gets the font family.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <returns>FontFamily.</returns>
        public static FontFamily GetFontFamily(this Font font)
        {
            return string.IsNullOrEmpty(font.FontFamily)
                       ? (FontFamily)Application.Current.Resources["PhoneFontFamilyNormal"]
                       : new FontFamily(font.FontFamily);
        }
    }
}
