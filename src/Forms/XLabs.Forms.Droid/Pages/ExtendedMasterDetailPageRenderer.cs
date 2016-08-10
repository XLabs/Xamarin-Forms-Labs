// ***********************************************************************
// Assembly         : XLabs.Forms.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ExtendedMasterDetailPageRenderer.cs" company="XLabs Team">
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
using Xamarin.Forms.Platform.Android;
using XLabs.Forms.Pages;

[assembly: ExportRenderer(typeof(ExtendedMasterDetailPage), typeof(ExtendedMasterDetailRenderer))]

namespace XLabs.Forms.Pages
{
    /// <summary>
    /// Class ExtendedMasterDetailRenderer.
    /// </summary>
    public class ExtendedMasterDetailRenderer : MasterDetailRenderer
    {
    }
}
