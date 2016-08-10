// ***********************************************************************
// Assembly         : XLabs.Forms.WP8
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
using Xamarin.Forms;
using XLabs.Platform.Device;
using XLabs.Platform.Services;

namespace XLabs.Forms.Services
{

    /// <summary>
    /// Class FontManager.
    /// </summary>
    public partial class FontManager : IFontManager
    {
        private readonly IDisplay _display;

        private readonly FontService _fontService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FontManager"/> class.
        /// </summary>
        /// <param name="display">The display.</param>
        public FontManager(IDisplay display)
        {
            _display = display;
            _fontService = new FontService();
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
                // ReSharper disable once LoopCanBeConvertedToQuery
                // returning WinRT method call directly doesn't work so do NOT refactor!!!
                foreach (var fontName in _fontService.GetFontNames())
                {
                    yield return fontName;
                }
            }
        }

        /// <summary>
        /// Gets height for the font.
        /// </summary>
        /// <param name="font">Font whose height is calculated.</param>
        /// <returns>Height of the font in inches.</returns>
        public double GetHeight(Font font)
        {
            var multiplier = Application.Current.MainPage.Scale / 100d;
            return multiplier * font.GetHeight() / _display.Ydpi;
        }

        #endregion


    }
}
