// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ExtendedDatePicker.cs" company="XLabs Team">
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
	/// Class ExtendedDatePicker.
	/// </summary>
	public class ExtendedDatePicker : DatePicker
	{
		/// <summary>
		/// The HasBorder property
		/// </summary>
		public static readonly BindableProperty HasBorderProperty =
			BindableProperty.Create("HasBorder", typeof(bool), typeof(ExtendedDatePicker), true);

		/// <summary>
		/// Gets or sets if the border should be shown or not
		/// </summary>
		public bool HasBorder
		{
			get { return (bool)GetValue(HasBorderProperty); }
			set { SetValue(HasBorderProperty, value); }
		}	
	}
}

