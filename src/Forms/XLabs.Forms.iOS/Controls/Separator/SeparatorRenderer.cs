// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="SeparatorRenderer.cs" company="XLabs Team">
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

using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(Separator), typeof(SeparatorRenderer))]

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Class SeparatorRenderer.
    /// </summary>
    public class SeparatorRenderer : ViewRenderer<Separator,UISeparator>
    {
        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Separator> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    BackgroundColor = Color.Transparent.ToUIColor ();
                    SetNativeControl (new UISeparator (Bounds));
                }
            }
            
            SetProperties ();
        }

        /// <summary>
        /// Handles the <see cref="E:ElementPropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            SetProperties();
        }

        /// <summary>
        /// Sets the properties.
        /// </summary>
        private void SetProperties()
        {
            if (Control == null || Element == null)
                return;

            var separator = Control;
            separator.Thickness = Element.Thickness;
            separator.StrokeColor = Element.Color.ToUIColor();
            separator.StrokeType = Element.StrokeType;
            separator.Orientation = Element.Orientation;
            separator.SpacingBefore = Element.SpacingBefore;
            separator.SpacingAfter = Element.SpacingAfter;
        }
    }
}

