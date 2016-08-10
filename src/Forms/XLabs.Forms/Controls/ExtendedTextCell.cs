// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ExtendedTextCell.cs" company="XLabs Team">
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
using XLabs.Enums;

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class ExtendedTextCell.
	/// </summary>
	public class ExtendedTextCell : TextCell
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExtendedTextCell"/> class.
		/// </summary>
		public ExtendedTextCell()
		{
		}

		/// <summary>
		/// The detail location property
		/// </summary>
		public static readonly BindableProperty DetailLocationProperty = BindableProperty.Create<ExtendedTextCell, TextCellDetailLocation> (p => p.DetailLocation, default(TextCellDetailLocation));

		/// <summary>
		/// Gets or sets the detail location.
		/// </summary>
		/// <value>The detail location.</value>
		public TextCellDetailLocation DetailLocation {
			get { return (TextCellDetailLocation)GetValue (DetailLocationProperty); }
			set { SetValue (DetailLocationProperty, value); }
		}

		/// <summary>
		/// The font size property
		/// </summary>
		public static readonly BindableProperty FontSizeProperty =
			BindableProperty.Create<ExtendedTextCell, double>(
				p => p.FontSize, -1);

		/// <summary>
		/// Gets or sets the size of the font.
		/// </summary>
		/// <value>The size of the font.</value>
		public double FontSize
		{
			get { return (double)GetValue(FontSizeProperty); }
			set { SetValue(FontSizeProperty, value); }
		}

		/// <summary>
		/// The font name android property
		/// </summary>
		public static readonly BindableProperty FontNameAndroidProperty =
			BindableProperty.Create<ExtendedTextCell, string>(
				p => p.FontNameAndroid, string.Empty);

		/// <summary>
		/// Gets or sets the font name android.
		/// </summary>
		/// <value>The font name android.</value>
		public string FontNameAndroid
		{
			get { return (string)GetValue(FontNameAndroidProperty); }
			set { SetValue(FontNameAndroidProperty, value); }
		}

		/// <summary>
		/// The font name ios property
		/// </summary>
		public static readonly BindableProperty FontNameIosProperty =
			BindableProperty.Create<ExtendedTextCell, string>(
				p => p.FontNameIos, string.Empty);

		/// <summary>
		/// Gets or sets the font name ios.
		/// </summary>
		/// <value>The font name ios.</value>
		public string FontNameIos
		{
			get { return (string)GetValue(FontNameIosProperty); }
			set { SetValue(FontNameIosProperty, value); }
		}

		/// <summary>
		/// The font name wp property
		/// </summary>
		public static readonly BindableProperty FontNameWpProperty =
			BindableProperty.Create<ExtendedTextCell, string>(
				p => p.FontNameWp, string.Empty);

		/// <summary>
		/// Gets or sets the font name wp.
		/// </summary>
		/// <value>The font name wp.</value>
		public string FontNameWp
		{
			get { return (string)GetValue(FontNameWpProperty); }
			set { SetValue(FontNameWpProperty, value); }
		}


		/// <summary>
		/// The font name property
		/// </summary>
		public static readonly BindableProperty FontNameProperty =
			BindableProperty.Create<ExtendedTextCell, string>(
				p => p.FontName, string.Empty);

		/// <summary>
		/// Gets or sets the name of the font.
		/// </summary>
		/// <value>The name of the font.</value>
		public string FontName
		{
			get { return (string)GetValue(FontNameProperty); }
			set
			{
				SetValue(FontNameProperty, value);

			}
		}

		/// <summary>
		/// The background color property
		/// </summary>
		public static readonly BindableProperty BackgroundColorProperty =
			BindableProperty.Create<ExtendedTextCell, Color>(p => p.BackgroundColor, Color.Transparent);

		/// <summary>
		/// Gets or sets the color of the background.
		/// </summary>
		/// <value>The color of the background.</value>
		public Color BackgroundColor
		{
			get { return (Color)GetValue(BackgroundColorProperty); }
			set { SetValue(BackgroundColorProperty, value); }
		}

		/// <summary>
		/// The separator color property
		/// </summary>
		public static readonly BindableProperty SeparatorColorProperty =
			BindableProperty.Create<ExtendedTextCell, Color>(p => p.SeparatorColor, Color.FromRgba(199, 199, 204, 255));

		/// <summary>
		/// Gets or sets the color of the separator.
		/// </summary>
		/// <value>The color of the separator.</value>
		public Color SeparatorColor
		{
			get { return (Color)GetValue(SeparatorColorProperty); }
			set { SetValue(SeparatorColorProperty, value); }
		}

		/// <summary>
		/// The separator padding property
		/// </summary>
		public static readonly BindableProperty SeparatorPaddingProperty =
			BindableProperty.Create<ExtendedTextCell, Thickness>(p => p.SeparatorPadding, default(Thickness));

		/// <summary>
		/// Gets or sets the separator padding.
		/// </summary>
		/// <value>The separator padding.</value>
		public Thickness SeparatorPadding
		{
			get { return (Thickness)GetValue(SeparatorPaddingProperty); }
			set { SetValue(SeparatorPaddingProperty, value); }
		}


		/// <summary>
		/// The show separator property
		/// </summary>
		public static readonly BindableProperty ShowSeparatorProperty =
			BindableProperty.Create<ExtendedTextCell, bool>(p => p.ShowSeparator, true);

		/// <summary>
		/// Gets or sets a value indicating whether [show separator].
		/// </summary>
		/// <value><c>true</c> if [show separator]; otherwise, <c>false</c>.</value>
		public bool ShowSeparator
		{
			get { return (bool)GetValue(ShowSeparatorProperty); }
			set { SetValue(ShowSeparatorProperty, value); }
		}


		/// <summary>
		/// The show disclousure property
		/// </summary>
		public static readonly BindableProperty ShowDisclousureProperty =
			BindableProperty.Create<ExtendedTextCell, bool>(p => p.ShowDisclousure, true);

		/// <summary>
		/// Gets or sets a value indicating whether [show disclousure].
		/// </summary>
		/// <value><c>true</c> if [show disclousure]; otherwise, <c>false</c>.</value>
		public bool ShowDisclousure
		{
			get { return (bool)GetValue(ShowDisclousureProperty); }
			set { SetValue(ShowDisclousureProperty, value); }
		}

		/// <summary>
		/// The disclousure image property
		/// </summary>
		public static readonly BindableProperty DisclousureImageProperty =
			BindableProperty.Create<ExtendedTextCell, string>(
				p => p.DisclousureImage, string.Empty);

		/// <summary>
		/// Gets or sets the disclousure image.
		/// </summary>
		/// <value>The disclousure image.</value>
		public string DisclousureImage
		{
			get { return (string)GetValue(DisclousureImageProperty); }
			set { SetValue(DisclousureImageProperty, value); }
		}
	}
}

