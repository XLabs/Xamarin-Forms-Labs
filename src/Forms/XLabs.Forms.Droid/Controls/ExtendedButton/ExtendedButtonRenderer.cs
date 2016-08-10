// ***********************************************************************
// Assembly         : XLabs.Forms.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ExtendedButtonRenderer.cs" company="XLabs Team">
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
using Xamarin.Forms.Platform.Android;
using XLabs.Forms.Controls;
using XLabs.Forms.Extensions;

[assembly: ExportRenderer(typeof(ExtendedButton), typeof(ExtendedButtonRenderer))]

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Class ExtendedButtonRenderer.
    /// </summary>
    public class ExtendedButtonRenderer : ButtonRenderer
    {
        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            UpdateAlignment();
            UpdateFont();
        }

        /// <summary>
        /// Handles the <see cref="E:ElementPropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == ExtendedButton.VerticalContentAlignmentProperty.PropertyName ||
                e.PropertyName == ExtendedButton.HorizontalContentAlignmentProperty.PropertyName)
            {
                UpdateAlignment();
            }
            else if (e.PropertyName == Button.FontProperty.PropertyName)
            {
                UpdateFont();
            }
             
            base.OnElementPropertyChanged(sender, e);
        }

        /// <summary>
        /// Updates the font
        /// </summary>
        private void UpdateFont()
        {
            Control.Typeface = Element.Font.ToExtendedTypeface(Context);
        }

        /// <summary>
        /// Sets the alignment.
        /// </summary>
        private void UpdateAlignment()
        {
            var element = this.Element as ExtendedButton;

            if (element == null || this.Control == null)
            {
                return;
            }

            this.Control.Gravity = element.VerticalContentAlignment.ToDroidVerticalGravity() |
                element.HorizontalContentAlignment.ToDroidHorizontalGravity();
        }
    }
}