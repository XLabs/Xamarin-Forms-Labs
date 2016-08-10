// ***********************************************************************
// Assembly         : XLabs.Forms.WinUniversal
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
using Windows.UI.Xaml;
using TextAlignment = Xamarin.Forms.TextAlignment;

namespace XLabs.Forms
{

    /// <summary>
    /// Implementation of AlignmentExtensions.
    /// </summary>
    public static class AlignmentExtensions
    {
        /// <summary>
        /// To the content vertical alignment.
        /// </summary>
        /// <param name="alignment">The alignment.</param>
        /// <returns>VerticalAlignment.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static VerticalAlignment ToContentVerticalAlignment(this TextAlignment alignment)
        {
            switch (alignment)
            {
                case TextAlignment.Center:
                    return VerticalAlignment.Center;
                case TextAlignment.End:
                    return VerticalAlignment.Bottom;
                case TextAlignment.Start:
                    return VerticalAlignment.Top;
            }

            throw new InvalidOperationException(alignment.ToString());
        }

        /// <summary>
        /// To the content horizontal alignment.
        /// </summary>
        /// <param name="alignment">The alignment.</param>
        /// <returns>HorizontalAlignment.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static HorizontalAlignment ToContentHorizontalAlignment(this TextAlignment alignment)
        {
            switch (alignment)
            {
                case TextAlignment.Center:
                    return HorizontalAlignment.Center;
                case TextAlignment.End:
                    return HorizontalAlignment.Right;
                case TextAlignment.Start:
                    return HorizontalAlignment.Left;
            }

            throw new InvalidOperationException(alignment.ToString());
        }
    }
}