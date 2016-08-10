// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="WebImage.cs" company="XLabs Team">
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

using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Class WebImage.
    /// </summary>
    public class WebImage : Image
    {
        /// <summary>
        /// The image URL property
        /// </summary>
        public static readonly BindableProperty ImageUrlProperty = BindableProperty.Create<WebImage, string>(p => p.ImageUrl, default(string));

        /// <summary>
        /// The URL of the image to display from the web
        /// </summary>
        public string ImageUrl
        {
            get { return (string)GetValue(ImageUrlProperty); }
            set { SetValue(ImageUrlProperty, value); }
        }

        /// <summary>
        /// The default image property
        /// </summary>
        public static readonly BindableProperty DefaultImageProperty = BindableProperty.Create<WebImage, string>(p => p.DefaultImage, default(string));

        /// <summary>
        /// The path to the local image to display if the <c>ImageUrl</c> can't be loaded
        /// </summary>
        public string DefaultImage
        {
            get { return (string)GetValue(DefaultImageProperty); }
            set { SetValue(DefaultImageProperty, value); }
        }

    }
}
