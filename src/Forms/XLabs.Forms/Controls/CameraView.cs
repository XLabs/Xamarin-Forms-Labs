// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="CameraView.cs" company="XLabs Team">
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
using XLabs.Platform.Services.Media;

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Class CameraView.
    /// </summary>
    public class CameraView : View
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CameraView"/> class.
        /// </summary>
        public CameraView()
        {
        }

        /// <summary>
        /// The camera device to use.
        /// </summary>
        public static readonly BindableProperty CameraProperty =
            BindableProperty.Create<CameraView, CameraDevice>(
                p => p.Camera, CameraDevice.Rear);

        /// <summary>
        /// Gets or sets the camera device to use.
        /// </summary>
        public CameraDevice Camera
        {
            get { return this.GetValue<CameraDevice>(CameraProperty); }
            set { this.SetValue(CameraProperty, value); }
        }
    }
}

