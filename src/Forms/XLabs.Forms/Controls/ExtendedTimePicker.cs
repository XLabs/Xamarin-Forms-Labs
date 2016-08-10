// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ExtendedTimePicker.cs" company="XLabs Team">
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
	/// Class ExtendedTimePicker.
	/// </summary>
	public class ExtendedTimePicker : TimePicker
	{
		/// <summary>
		/// The HasBorder property
		/// </summary>
		public static readonly BindableProperty HasBorderProperty =
			BindableProperty.Create("HasBorder", typeof(bool), typeof(ExtendedTimePicker), true);

		/// <summary>
		/// The MinimumTime property
		/// </summary>
		public static readonly BindableProperty MinimumTimeProperty =
			BindableProperty.Create("MinimumTime", typeof(TimeSpan), typeof(ExtendedTimePicker), new TimeSpan(0, 0, 0));

		/// <summary>
		/// The MaximumTime property
		/// </summary>
		public static readonly BindableProperty MaximumTimeProperty =
			BindableProperty.Create("MaximumTime", typeof(TimeSpan), typeof(ExtendedTimePicker), new TimeSpan(24, 0, 0));

		/// <summary>
		/// Initializes a new instance of the <see cref="ExtendedTimePicker"/> class.
		/// </summary>
		public ExtendedTimePicker ()
		{
		}

		/// <summary>
		/// Gets or sets if the border should be shown or not
		/// </summary>
		/// <value><c>true</c> if this instance has border; otherwise, <c>false</c>.</value>
		public bool HasBorder
		{
			get { return (bool)GetValue(HasBorderProperty); }
			set { SetValue(HasBorderProperty, value); }
		}

		/// <summary>
		/// Gets or sets the minimum time
		/// </summary>
		/// <value>The minimum time.</value>
		public TimeSpan MinimumTime
		{
			get { return (TimeSpan)GetValue(MinimumTimeProperty); }
			set { SetValue(MinimumTimeProperty, value); }
		}

		/// <summary>
		/// Gets or sets the maximum time
		/// </summary>
		/// <value>The maximum time.</value>
		public TimeSpan MaximumTime
		{
			get { return (TimeSpan)GetValue(MaximumTimeProperty); }
			set { SetValue(MaximumTimeProperty, value); }
		}
	}
}

