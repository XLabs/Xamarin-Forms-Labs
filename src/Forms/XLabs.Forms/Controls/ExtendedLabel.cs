// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ExtendedLabel.cs" company="XLabs Team">
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
	/// Class ExtendedLabel.
	/// </summary>
	public class ExtendedLabel : Label
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExtendedLabel"/> class.
		/// </summary>
		public ExtendedLabel()
		{
		}

		/// <summary>
		/// The font name android property.
		/// </summary>
		[Obsolete("This is now obsolete. Please rather use FontName and FriendlyFontName to cover all platforms.")]
		public static readonly BindableProperty FontNameAndroidProperty =
			BindableProperty.Create<ExtendedLabel, string>(
				p => p.FontNameAndroid, string.Empty);

		/// <summary>
		/// Gets or sets the font name android.
		/// </summary>
		/// <value>The font name android.</value>
		[Obsolete("This is now obsolete. Please rather use FontName and FriendlyFontName to cover all platforms.")]
		public string FontNameAndroid
		{
			get
			{
				return (string)GetValue(FontNameAndroidProperty);
			}
			set
			{
				SetValue(FontNameAndroidProperty, value);
			}
		}

		/// <summary>
		/// The font name ios property.
		/// </summary>
		[Obsolete("This is now obsolete. Please rather use FontName and FriendlyFontName to cover all platforms.")]
		public static readonly BindableProperty FontNameIosProperty =
			BindableProperty.Create<ExtendedLabel, string>(
				p => p.FontNameIOS, string.Empty);

		/// <summary>
		/// Gets or sets the font name ios.
		/// </summary>
		/// <value>The font name ios.</value>
		[Obsolete]
		public string FontNameIOS
		{
			get
			{
				return (string)GetValue(FontNameIosProperty);
			}
			set
			{
				SetValue(FontNameIosProperty, value);
			}
		}

		/// <summary>
		/// The font name property.
		/// </summary>
		public static readonly BindableProperty FontNameProperty =
			BindableProperty.Create<ExtendedLabel, string>(
				p => p.FontName, string.Empty);

		/// <summary>
		/// Gets or sets the name of the font file including extension. If no extension given then ttf is assumed.
		/// Fonts need to be included in projects accoring to the documentation.
		/// </summary>
		/// <value>The full name of the font file including extension.</value>
		public string FontName
		{
			get
			{
				return (string)GetValue(FontNameProperty);
			}
			set
			{
				SetValue(FontNameProperty, value);
			}
		}

		/// <summary>
		/// The friendly font name property. This can be found on the first line of the font or in the font preview. 
		/// This is only required on Windows Phone. If not given then the file name excl. the extension is used.
		/// </summary>
		public static readonly BindableProperty FriendlyFontNameProperty =
			BindableProperty.Create<ExtendedLabel, string>(
				p => p.FriendlyFontName, string.Empty);

		/// <summary>
		/// Gets or sets the name of the font.
		/// </summary>
		/// <value>The name of the font.</value>
		public string FriendlyFontName
		{
			get
			{
				return (string)GetValue(FriendlyFontNameProperty);
			}
			set
			{
				SetValue(FriendlyFontNameProperty, value);
			}
		}

		/// <summary>
		/// The is underlined property.
		/// </summary>
		public static readonly BindableProperty IsUnderlineProperty =
			BindableProperty.Create<ExtendedLabel, bool>(p => p.IsUnderline, false);

		/// <summary>
		/// Gets or sets a value indicating whether the text in the label is underlined.
		/// </summary>
		/// <value>A <see cref="bool"/> indicating if the text in the label should be underlined.</value>
		public bool IsUnderline
		{
			get
			{
				return (bool)GetValue(IsUnderlineProperty);
			}
			set
			{
				SetValue(IsUnderlineProperty, value);
			}
		}

		/// <summary>
		/// The is underlined property.
		/// </summary>
		public static readonly BindableProperty IsStrikeThroughProperty =
			BindableProperty.Create<ExtendedLabel, bool>(p => p.IsStrikeThrough, false);

		/// <summary>
		/// Gets or sets a value indicating whether the text in the label is underlined.
		/// </summary>
		/// <value>A <see cref="bool"/> indicating if the text in the label should be underlined.</value>
		public bool IsStrikeThrough
		{
			get
			{
				return (bool)GetValue(IsStrikeThroughProperty);
			}
			set
			{
				SetValue(IsStrikeThroughProperty, value);
			}
		}

		/// <summary>
		/// This is the drop shadow property
		/// </summary>
		public static readonly BindableProperty IsDropShadowProperty =
			BindableProperty.Create<ExtendedLabel, bool>(p => p.IsDropShadow, false);

		/// <summary>
		/// Gets or sets a value indicating whether this instance is drop shadow.
		/// </summary>
		/// <value><c>true</c> if this instance is drop shadow; otherwise, <c>false</c>.</value>
		public bool IsDropShadow 
		{
			get 
			{
				return (bool)GetValue (IsDropShadowProperty);
			}
			set 
			{
				SetValue (IsDropShadowProperty, value);
			}
		}

		/// <summary>
		/// This is the drop shadow color property
		/// </summary>
		public static readonly BindableProperty DropShadowColorProperty =
			BindableProperty.Create<ExtendedLabel, Color>(p => p.DropShadowColor, default(Color));

		/// <summary>
		/// Gets or sets the color of the drop shadow.
		/// </summary>
		/// <value>The color of the drop shadow.</value>
		public Color DropShadowColor 
		{
			get 
			{
				return (Color)GetValue (DropShadowColorProperty);
			}
			set 
			{
				SetValue (DropShadowColorProperty, value);
			}
		}

		/// <summary>
		/// The placeholder property.
		/// </summary>
		public static readonly BindableProperty PlaceholderProperty = 
			BindableProperty.Create<ExtendedLabel, string>(p => p.Placeholder, default(string));

		/// <summary>
		/// Gets or sets the string value that is used when the label's Text property is empty.
		/// </summary>
		/// <value>The placeholder string.</value>
		public string Placeholder
		{
			get { return (string)GetValue(PlaceholderProperty); }
			set 
			{
				SetValue(FormattedPlaceholderProperty, null);
				SetValue(PlaceholderProperty, value); 
			}
		}

		/// <summary>
		/// The formatted placeholder property.
		/// </summary>
		public static readonly BindableProperty FormattedPlaceholderProperty = 
			BindableProperty.Create<ExtendedLabel, FormattedString>(p => p.FormattedPlaceholder, default(FormattedString));

		/// <summary>
		/// Gets or sets the FormattedString value that is used when the label's Text property is empty.
		/// </summary>
		/// <value>The placeholder FormattedString.</value>
		public FormattedString FormattedPlaceholder
		{
			get { return (FormattedString)GetValue(FormattedPlaceholderProperty); }
			set 
			{  
				SetValue(PlaceholderProperty, null);
				SetValue(FormattedPlaceholderProperty, value);
			}
		}

	}
}