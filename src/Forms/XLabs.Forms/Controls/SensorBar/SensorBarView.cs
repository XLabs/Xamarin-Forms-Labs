// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="SensorBarView.cs" company="XLabs Team">
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

namespace XLabs.Forms.Controls.SensorBar
{
	/// <summary>
	/// Class SensorBarView.
	/// </summary>
	public class SensorBarView : View
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SensorBarView"/> class.
		/// </summary>
		public SensorBarView()
		{
		}

		/// <summary>
		/// The positive color property
		/// </summary>
		public static readonly BindableProperty PositiveColorProperty = BindableProperty.Create<SensorBarView, Color>(p => p.PositiveColor, Color.Green);
		/// <summary>
		/// The negative color property
		/// </summary>
		public static readonly BindableProperty NegativeColorProperty = BindableProperty.Create<SensorBarView, Color>(p => p.NegativeColor, Color.Red);
		/// <summary>
		/// The current value property
		/// </summary>
		public static readonly BindableProperty CurrentValueProperty = BindableProperty.Create<SensorBarView, double>(p => p.CurrentValue, 0);
		/// <summary>
		/// The limit property
		/// </summary>
		public static readonly BindableProperty LimitProperty = BindableProperty.Create<SensorBarView, double>(p => p.Limit, 1);

		/// <summary>
		/// Gets or sets the current value.
		/// </summary>
		/// <value>The current value.</value>
		public double CurrentValue
		{
			get
			{
				return (double)GetValue(CurrentValueProperty);
			}

			set
			{
				if (Math.Abs(value) <= this.Limit)
				{
					SetValue(CurrentValueProperty, value);
				}
			}
		}

		/// <summary>
		/// Gets or sets the limit.
		/// </summary>
		/// <value>The limit.</value>
		public double Limit
		{
			get { return (double)GetValue(LimitProperty); }
			set { SetValue(LimitProperty, value); }
		}

		/// <summary>
		/// Gets or sets the color of the positive.
		/// </summary>
		/// <value>The color of the positive.</value>
		public Color PositiveColor
		{
			get { return (Color)GetValue(PositiveColorProperty); }
			set { SetValue(PositiveColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets the color of the negative.
		/// </summary>
		/// <value>The color of the negative.</value>
		public Color NegativeColor
		{
			get { return (Color)GetValue(NegativeColorProperty); }
			set { SetValue(NegativeColorProperty, value); }
		}
	}
}

