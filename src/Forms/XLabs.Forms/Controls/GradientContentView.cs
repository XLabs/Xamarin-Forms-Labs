// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="GradientContentView.cs" company="XLabs Team">
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
    /// Enum GradientOrientation
    /// </summary>
    public enum GradientOrientation
    {
        /// <summary>
        /// The vertical
        /// </summary>
        Vertical,
        /// <summary>
        /// The horizontal
        /// </summary>
        Horizontal
    }

    /// <summary>
    /// ContentView that allows you to have a Gradient for
    /// the background. Let there be Gradients!
    /// </summary>
    public class GradientContentView : ContentView
    {
        /// <summary>
        /// Start color of the gradient
        /// Defaults to White
        /// </summary>
        public GradientOrientation Orientation
        {
            get { return (GradientOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// The orientation property
        /// </summary>
        public static readonly BindableProperty OrientationProperty =
            BindableProperty.Create<GradientContentView, GradientOrientation>(x => x.Orientation, GradientOrientation.Vertical, BindingMode.OneWay);

        /// <summary>
        /// Start color of the gradient
        /// Defaults to White
        /// </summary>
        public Color StartColor
        {
            get { return (Color)GetValue(StartColorProperty); }
            set { SetValue(StartColorProperty, value); }
        }

        /// <summary>
        /// Using a BindableProperty as the backing store for StartColor.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly BindableProperty StartColorProperty =
            BindableProperty.Create<GradientContentView, Color>(x => x.StartColor, Color.White, BindingMode.OneWay);

        /// <summary>
        /// End color of the gradient
        /// Defaults to Black
        /// </summary>
        public Color EndColor
        {
            get { return (Color)GetValue(EndColorProperty); }
            set { SetValue(EndColorProperty, value); }
        }

        /// <summary>
        /// Using a BindableProperty as the backing store for EndColor.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly BindableProperty EndColorProperty =
            BindableProperty.Create<GradientContentView, Color>(x => x.EndColor, Color.Black, BindingMode.OneWay);
    }
}
