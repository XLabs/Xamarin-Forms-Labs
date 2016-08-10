// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ExtendedButton.cs" company="XLabs Team">
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
    /// Extends <see cref="Xamarin.Forms.Button"/>.
    /// </summary>
    public class ExtendedButton : Button
    {
        /// <summary>
        /// Bindable property for button content vertical alignment.
        /// </summary>
        public static readonly BindableProperty VerticalContentAlignmentProperty =
            BindableProperty.Create<ExtendedButton, TextAlignment>(
                p => p.VerticalContentAlignment, TextAlignment.Center);

        /// <summary>
        /// Bindable property for button content horizontal alignment.
        /// </summary>
        public static readonly BindableProperty HorizontalContentAlignmentProperty =
            BindableProperty.Create<ExtendedButton, TextAlignment>(
                p => p.HorizontalContentAlignment, TextAlignment.Center);

        /// <summary>
        /// Gets or sets the content vertical alignment.
        /// </summary>
        public TextAlignment VerticalContentAlignment
        {
            get { return this.GetValue<TextAlignment>(VerticalContentAlignmentProperty); }
            set { this.SetValue(VerticalContentAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content horizontal alignment.
        /// </summary>
        public TextAlignment HorizontalContentAlignment
        {
            get { return this.GetValue<TextAlignment>(HorizontalContentAlignmentProperty); }
            set { this.SetValue(HorizontalContentAlignmentProperty, value); }
        }
    }
}