// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ExtendedContentView.cs" company="XLabs Team">
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
    /// Extended content view.
    /// </summary>
    public class ExtendedContentView : ContentView
    {
        /// <summary>
        /// The width request in inches property.
        /// </summary>
        public static readonly BindableProperty WidthRequestInInchesProperty =
            BindableProperty.Create<ExtendedContentView, double>(
                p => p.WidthRequestInInches, default(double));

        /// <summary>
        /// Gets or sets the width request in inches.
        /// </summary>
        /// <value>The width request in inches.</value>
        public double WidthRequestInInches
        {
            get
            {
                return this.GetHeightRequestInInches();
            }

            set
            {
                this.SetHeightRequestInInches(value);
                this.SetValue(WidthRequestInInchesProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the height request in inches.
        /// </summary>
        /// <value>The height request in inches.</value>
        public double HeightRequestInInches
        {
            get
            {
                return this.GetHeightRequestInInches();
            }

            set
            {
                this.SetHeightRequestInInches(value);
            }
        }
    }
}
