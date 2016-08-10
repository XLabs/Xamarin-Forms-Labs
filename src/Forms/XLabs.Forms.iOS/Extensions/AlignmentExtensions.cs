// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="AlignmentExtensions.cs" company="XLabs Team">
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
using UIKit;
using Xamarin.Forms;

namespace XLabs.Forms.Extensions
{
    /// <summary>
    /// Class AlignmentExtensions.
    /// </summary>
    public static class AlignmentExtensions
    {
        /// <summary>
        /// To the content vertical alignment.
        /// </summary>
        /// <param name="alignment">The alignment.</param>
        /// <returns>UIControlContentVerticalAlignment.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static UIControlContentVerticalAlignment ToContentVerticalAlignment(this TextAlignment alignment)
        {
            switch (alignment)
            {
                case TextAlignment.Center:
                    return UIControlContentVerticalAlignment.Center;
                case TextAlignment.End:
                    return UIControlContentVerticalAlignment.Bottom;
                case TextAlignment.Start:
                    return UIControlContentVerticalAlignment.Top;
            }

            throw new InvalidOperationException(alignment.ToString());
        }

        /// <summary>
        /// To the content horizontal alignment.
        /// </summary>
        /// <param name="alignment">The alignment.</param>
        /// <returns>UIControlContentHorizontalAlignment.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static UIControlContentHorizontalAlignment ToContentHorizontalAlignment(this TextAlignment alignment)
        {
            switch (alignment)
            {
                case TextAlignment.Center:
                    return UIControlContentHorizontalAlignment.Center;
                case TextAlignment.End:
                    return UIControlContentHorizontalAlignment.Right;
                case TextAlignment.Start:
                    return UIControlContentHorizontalAlignment.Left;
            }

            throw new InvalidOperationException(alignment.ToString());
        }
    }
}