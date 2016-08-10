// ***********************************************************************
// Assembly         : XLabs.Forms.Droid
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
using Android.Views;
using TextAlignment = Xamarin.Forms.TextAlignment;

namespace XLabs.Forms.Extensions
{
    using DroidTextAlignment = Android.Views.TextAlignment;

    /// <summary>
    /// Class AlignmentExtensions.
    /// </summary>
    public static class AlignmentExtensions
    {
        /// <summary>
        /// To the droid text alignment.
        /// </summary>
        /// <param name="alignment">The alignment.</param>
        /// <returns>DroidTextAlignment.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static DroidTextAlignment ToDroidTextAlignment(this TextAlignment alignment)
        {
            switch (alignment)
            {
                case TextAlignment.Center:
                    return DroidTextAlignment.Center;
                case TextAlignment.End:
                    return DroidTextAlignment.ViewEnd;
                case TextAlignment.Start:
                    return DroidTextAlignment.ViewStart;
            }

            throw new InvalidOperationException(alignment.ToString());
        }

        /// <summary>
        /// To the droid horizontal gravity.
        /// </summary>
        /// <param name="alignment">The alignment.</param>
        /// <returns>GravityFlags.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static GravityFlags ToDroidHorizontalGravity(this TextAlignment alignment)
        {
            switch (alignment)
            {
            case TextAlignment.Center:
                return GravityFlags.CenterHorizontal;
            case TextAlignment.End:
                return GravityFlags.Right;
            case TextAlignment.Start:
                return GravityFlags.Left;
            }

            throw new InvalidOperationException(alignment.ToString());
        }

        /// <summary>
        /// To the droid vertical gravity.
        /// </summary>
        /// <param name="alignment">The alignment.</param>
        /// <returns>GravityFlags.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static GravityFlags ToDroidVerticalGravity(this TextAlignment alignment)
        {
            switch (alignment)
            {
                case TextAlignment.Center:
                    return GravityFlags.CenterVertical;
                case TextAlignment.End:
                    return GravityFlags.Bottom;
                case TextAlignment.Start:
                    return GravityFlags.Top;
            }

            throw new InvalidOperationException(alignment.ToString());
        }
    }
}