// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ExtendedEditor.cs" company="XLabs Team">
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
using System.Runtime.CompilerServices;
using Xamarin.Forms;

[assembly: 
	InternalsVisibleTo("XLabs.Forms.Droid"),
	InternalsVisibleTo("XLabs.Forms.iOS"),
	InternalsVisibleTo("XLabs.Forms.WP8")]

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class ExtendedEditor.
	/// </summary>
	public class ExtendedEditor : Editor
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExtendedEditor"/> class.
		/// </summary>
		public ExtendedEditor ()
		{
		}

		/// <summary>
		/// The font property
		/// </summary>
		public static readonly BindableProperty FontProperty = BindableProperty.Create<ExtendedEditor, Font> (p => p.Font, default(Font));

		/// <summary>
		/// Gets or sets the font.
		/// </summary>
		/// <value>The font.</value>
		public Font Font 
		{
			get { return (Font)GetValue (FontProperty); }
			set { SetValue (FontProperty, value); }
		}

		/// <summary>
		/// The left swipe
		/// </summary>
		public EventHandler LeftSwipe;
		/// <summary>
		/// The right swipe
		/// </summary>
		public EventHandler RightSwipe;

		/// <summary>
		/// Handles the <see cref="E:LeftSwipe" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		internal void OnLeftSwipe(object sender, EventArgs e)
		{
			var handler = this.LeftSwipe;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		/// <summary>
		/// Handles the <see cref="E:RightSwipe" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		internal void OnRightSwipe(object sender, EventArgs e)
		{
			var handler = this.RightSwipe;
			if (handler != null)
			{
				handler(this, e);
			}
		}
	}
}

