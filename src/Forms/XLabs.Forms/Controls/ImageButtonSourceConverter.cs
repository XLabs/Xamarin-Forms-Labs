// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ImageButtonSourceConverter.cs" company="XLabs Team">
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
using System.Globalization;
using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Creates an <see cref="ImageSource"/> from the a string
    /// that is either a file or a URI.
    /// </summary>
    public class ImageSourceConverter : TypeConverter
    {
        /// <summary>
        /// Checks to see if the type attempted to be converted from is a string.
        /// </summary>
        /// <param name="sourceType">The type that is attempting to be converted.</param>
        /// <returns>Returns true if the sourceType is a <see cref="string"/>.</returns>
        public override bool CanConvertFrom(Type sourceType)
        {
            return sourceType == typeof(string);
        }

        /// <summary>
        /// Converts the string value into a <see cref="ImageSource"/> either from a file or URI.
        /// </summary>
        /// <param name="culture">The current culture being used.</param>
        /// <param name="value">The string value to convert.</param>
        /// <returns>Returns a <see cref="ImageSource"/> loaded from the value.</returns>
        public override object ConvertFrom(CultureInfo culture, object value)
        {
            if (value == null)
            {
                return null;
            }

            var str = value as string;
            if (str != null)
            {
                Uri result;
                if (!Uri.TryCreate(str, UriKind.Absolute, out result) || !(result.Scheme != "file"))
                {
                    return ImageSource.FromFile(str);
                }
                return ImageSource.FromUri(result);
            }
            throw new InvalidOperationException(
                string.Format("Cannot convert \"{0}\" into {1}",
                    new[] { value, typeof(ImageSource) }));
        }
    }
}