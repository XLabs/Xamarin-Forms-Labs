// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="FontManager.cs" company="XLabs Team">
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

using System.Collections.Generic;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Platform.Device;

namespace XLabs.Forms.Services
{
    /// <summary>
    /// Class FontManager.
    /// </summary>
    public partial class FontManager : IFontManager
    {
        /// <summary>
        /// The _display
        /// </summary>
        private readonly IDisplay display;

        /// <summary>
        /// Initializes a new instance of the <see cref="FontManager"/> class.
        /// </summary>
        /// <param name="display">The display.</param>
        public FontManager(IDisplay display)
        {
            this.display = display;
        }

        #region IFontManager Members

        /// <summary>
        /// Gets all available system fonts.
        /// </summary>
        /// <value>The available fonts.</value>
        public IEnumerable<string> AvailableFonts
        {
            get
            {
                return UIFont.FamilyNames;
            }
        }

        /// <summary>
        /// Gets height for the font.
        /// </summary>
        /// <param name="font">Font whose height is calculated.</param>
        /// <returns>Height of the font in inches.</returns>
        public double GetHeight(Font font)
        {
            return font.ToUIFont().Ascender 
                * UIScreen.MainScreen.Scale 
                * this.display.ScreenHeightInches() 
                / this.display.Height;
        }

        #endregion
    }
}