// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ExtendedSwitch.cs" company="XLabs Team">
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
	///     Class ExtendedSwitch.
	/// </summary>
	public class ExtendedSwitch : Switch
	{
		/// <summary>
		///     Identifies the Switch tint color bindable property.
		/// </summary>
		public static readonly BindableProperty TintColorProperty =
			BindableProperty.Create<ExtendedSwitch, Color>(
				p => p.TintColor, Color.Black);

		/// <summary>
		///     Gets or sets the color of the tint.
		/// </summary>
		/// <value>The color of the tint.</value>
		public Color TintColor
		{
			get { return (Color)GetValue(TintColorProperty); }

			set { SetValue(TintColorProperty, value); }
		}
	}
}