// ***********************************************************************
// Assembly         : XLabs.Platform.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="StringExtensions.cs" company="XLabs Team">
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
using CoreGraphics;
using Foundation;
using UIKit;

namespace XLabs.Platform.Extensions
{
    /// <summary>
    /// Class StringExtensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Gets the height of a string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="font">The font.</param>
        /// <param name="width">The width.</param>
        /// <returns>nfloat.</returns>
        public static nfloat StringHeight(this string text, UIFont font, nfloat width)
        {
            return text.StringRect(font, width).Height;
        }

        /// <summary>
        /// To the native string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>NSString.</returns>
        public static NSString ToNativeString(this string text)
        {
            return new NSString(text);
        }

        /// <summary>
        /// Gets the rectangle of a string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="font">The font.</param>
        /// <param name="width">The width.</param>
        /// <returns>CGRect.</returns>
        public static CGRect StringRect(this string text, UIFont font, nfloat width)
        {
            return text.ToNativeString().GetBoundingRect(
                new CGSize(width, nfloat.MaxValue),
                NSStringDrawingOptions.OneShot,//.UsesFontLeading | NSStringDrawingOptions.UsesLineFragmentOrigin,
                new UIStringAttributes { Font = font },
                null);
        }

        /// <summary>
        /// Strings the size.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="font">The font.</param>
        /// <returns>CGSize.</returns>
        public static CGSize StringSize(this string text, UIFont font)
        {
            return text.ToNativeString().GetSizeUsingAttributes(new UIStringAttributes { Font = font });
        }
    }
}