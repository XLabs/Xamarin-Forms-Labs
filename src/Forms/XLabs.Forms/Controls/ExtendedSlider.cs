// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ExtendedSlider.cs" company="XLabs Team">
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
using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class ExtendedSlider.
	/// </summary>
	public class ExtendedSlider : Slider
	{
		/// <summary>
		/// The current step value property
		/// </summary>
		public static readonly BindableProperty CurrentStepValueProperty =
								BindableProperty.Create<ExtendedSlider, double>(p => p.StepValue, 1.0f);

		/// <summary>
		/// Gets or sets the step value.
		/// </summary>
		/// <value>The step value.</value>
		public double StepValue
		{
			get { return (double)GetValue(CurrentStepValueProperty); }

			set { SetValue(CurrentStepValueProperty, value); }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExtendedSlider"/> class.
		/// </summary>
		public ExtendedSlider()
		{
			ValueChanged += OnSliderValueChanged;
		}

		/// <summary>
		/// Handles the <see cref="E:SliderValueChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ValueChangedEventArgs"/> instance containing the event data.</param>
		private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
		{
			var newStep = Math.Round(e.NewValue / StepValue);

			Value = newStep * StepValue;
		}
	}
}
