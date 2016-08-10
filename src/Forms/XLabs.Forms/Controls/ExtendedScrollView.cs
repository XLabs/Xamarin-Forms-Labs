// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ExtendedScrollView.cs" company="XLabs Team">
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
	/// Class ExtendedScrollView.
	/// </summary>
	public class ExtendedScrollView : ScrollView
	{
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
							  /// <summary>
							  /// Occurs when [scrolled].
							  /// </summary>
		public event Action<ScrollView, Rectangle> Scrolled;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

		/// <summary>
		/// Updates the bounds.
		/// </summary>
		/// <param name="bounds">The bounds.</param>
		public void UpdateBounds(Rectangle bounds)
		{
			Position = bounds.Location;
			if (Scrolled != null)
				Scrolled (this, bounds);
		}

		/// <summary>
		/// The position property
		/// </summary>
		public static readonly BindableProperty PositionProperty = 
			BindableProperty.Create<ExtendedScrollView,Point>(
				p => p.Position, default(Point));

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		/// <value>The position.</value>
		public Point Position {
			get { return (Point)GetValue(PositionProperty); }
			set { SetValue(PositionProperty, value); }
		}

		/// <summary>
		/// The animate scroll property
		/// </summary>
		public static readonly BindableProperty AnimateScrollProperty = 
			BindableProperty.Create<ExtendedScrollView,bool>(
				p => p.AnimateScroll,true);

		/// <summary>
		/// Gets or sets a value indicating whether [animate scroll].
		/// </summary>
		/// <value><c>true</c> if [animate scroll]; otherwise, <c>false</c>.</value>
		public bool AnimateScroll {
			get { return (bool)GetValue (AnimateScrollProperty); }
			set { SetValue (AnimateScrollProperty, value); }
		}

	}
}

