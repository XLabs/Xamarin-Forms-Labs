// ***********************************************************************
// Assembly         : XLabs.Forms.Droid
// Author           : XLabs Team
// Created          : 01-06-2016
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-06-2016
// ***********************************************************************
// <copyright file="TextViewRenderHelper.cs" company="XLabs Team">
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
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Xamarin.Forms.Platform.Android;
using XLabs.Forms.Extensions;

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Helper class for rendering TextView like controls.
    /// 
    /// </summary>
    internal class TextViewRenderHelper
    {
        public Context Context { get; private set; }

        public TextViewRenderHelper(Context context)
        {
            Context = context;
        }

        /// <summary>
        /// Gets the color of the span.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="defaultColors">The default colors.</param>
        /// <returns>Android.Graphics.Color.</returns>
        public Android.Graphics.Color GetSpanColor(Xamarin.Forms.Color color, ColorStateList defaultColors)
        {
            if (color == Xamarin.Forms.Color.Default)
                return new Android.Graphics.Color(defaultColors.DefaultColor);

            return color.ToAndroid();
        }

        /// <summary>
        /// Load the font from assets
        /// </summary>
        /// <param name="fontName">name of the font</param>
        /// <returns></returns>
        private Typeface LoadFontFromAsset(string fontName)
        {
            try
            {
                return Typeface.CreateFromAsset(Context.Assets, "fonts/" + fontName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("not found in assets. Exception: {0}", ex));
                return null;
            }
        }

        /// <summary>
        /// Load the font from file
        /// </summary>
        /// <param name="fontName">name of the font</param>
        /// <returns></returns>
        private Typeface LoadFontFromFile(string fontName)
        {
            try
            {
                return Typeface.CreateFromFile("fonts/" + fontName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("not found by file. Exception: {0}", ex));
                return null;
            }
        }

        /// <summary>
        /// Load the font
        /// </summary>
        /// <param name="fontName"></param>
        /// <returns></returns>
        public Typeface TrySetFont(string fontName)
        {
            var tp = TypefaceCache.SharedCache.RetrieveTypeface(fontName);
            if (tp == null)
                tp = LoadFontFromAsset(fontName);
            if (tp == null)
                tp = LoadFontFromFile(fontName);
            if (tp == null)
                tp = Typeface.Default;
            
            TypefaceCache.SharedCache.StoreTypeface(fontName, tp);

            return tp;
        }
    }
}
