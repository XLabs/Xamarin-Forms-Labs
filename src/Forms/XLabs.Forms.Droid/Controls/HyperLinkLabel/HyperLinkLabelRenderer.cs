// ***********************************************************************
// Assembly         : XLabs.Forms.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="HyperLinkLabelRenderer.cs" company="XLabs Team">
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

using Android.Text.Util;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(HyperLinkLabel), typeof(HyperLinkLabelRenderer))]

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Class HyperLinkLabelRenderer.
    /// </summary>
    public class HyperLinkLabelRenderer : LabelRenderer
    {
        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {

                var nativeEditText = (global::Android.Widget.TextView)Control;

                Linkify.AddLinks(nativeEditText, MatchOptions.All);
            }
        }
    }
}