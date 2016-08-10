// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ButtonGroup.cs" company="XLabs Team">
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

using System.Collections.Generic;
using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class ButtonGroup.
	/// </summary>
	public class ButtonGroup : ContentView
	{
		/// <summary>
		/// The outline color property
		/// </summary>
		public static readonly BindableProperty OutlineColorProperty = BindableProperty.Create("OutlineColor", typeof(Color), typeof(ButtonGroup), Color.Default);
		/// <summary>
		/// The view background color property
		/// </summary>
		public static readonly BindableProperty ViewBackgroundColorProperty = BindableProperty.Create("ViewBackgroundColor", typeof(Color), typeof(ButtonGroup), Color.Default);
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
							  /// <summary>
							  /// The background color property
							  /// </summary>
		public static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create("BackgroundColor", typeof(Color), typeof(ButtonGroup), Color.Default);
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
							  /// <summary>
							  /// The selected background color property
							  /// </summary>
		public static readonly BindableProperty SelectedBackgroundColorProperty = BindableProperty.Create("SelectedBackgroundColor", typeof(Color), typeof(ButtonGroup), Color.Default);
		/// <summary>
		/// The text color property
		/// </summary>
		public static readonly BindableProperty TextColorProperty = BindableProperty.Create("TextColor", typeof(Color), typeof(ButtonGroup), Color.Default);
		/// <summary>
		/// The selected text color property
		/// </summary>
		public static readonly BindableProperty SelectedTextColorProperty = BindableProperty.Create("SelectedTextColor", typeof(Color), typeof(ButtonGroup), Color.Default);
		/// <summary>
		/// The border color property
		/// </summary>
		public static readonly BindableProperty BorderColorProperty = BindableProperty.Create("BorderColor", typeof(Color), typeof(ButtonGroup), Color.Default);
		/// <summary>
		/// The selected border color property
		/// </summary>
		public static readonly BindableProperty SelectedBorderColorProperty = BindableProperty.Create("SelectedBorderColor", typeof(Color), typeof(ButtonGroup), Color.Black);
		/// <summary>
		/// The selected frame background color property
		/// </summary>
		public static readonly BindableProperty SelectedFrameBackgroundColorProperty = BindableProperty.Create("SelectedFrameBackgroundColor", typeof(Color), typeof(ButtonGroup), Color.Black);
		/// <summary>
		/// The selected index property
		/// </summary>
		public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create<ButtonGroup, int>(p => p.SelectedIndex, 0, BindingMode.TwoWay);
		/// <summary>
		/// The items property property
		/// </summary>
		public static readonly BindableProperty ItemsPropertyProperty = BindableProperty.Create<ButtonGroup, List<string>>(p => p.Items, null, BindingMode.TwoWay);
		/// <summary>
		/// The font property
		/// </summary>
		public static readonly BindableProperty FontProperty = BindableProperty.Create("Font", typeof(Font), typeof(ButtonGroup), Font.Default);
		/// <summary>
		/// The rounded property
		/// </summary>
		public static readonly BindableProperty RoundedProperty = BindableProperty.Create("Rounded", typeof(bool), typeof(ButtonGroup), false);
		/// <summary>
		/// The is number property
		/// </summary>
		public static readonly BindableProperty IsNumberProperty = BindableProperty.Create("IsNumber", typeof(bool), typeof(ButtonGroup), false);

		/// <summary>
		/// The button layout
		/// </summary>
		private readonly WrapLayout _buttonLayout;
		/// <summary>
		/// The spacing
		/// </summary>
		private const int SPACING = 5;
		/// <summary>
		/// The padding
		/// </summary>
		private const int PADDING = 5;
		/// <summary>
		/// The button border width
		/// </summary>
		private const int BUTTON_BORDER_WIDTH = 1;
		/// <summary>
		/// The frame padding
		/// </summary>
		private const int FRAME_PADDING = 1;
		/// <summary>
		/// The button border radius
		/// </summary>
		private const int BUTTON_BORDER_RADIUS = 5;
		/// <summary>
		/// The button height
		/// </summary>
		private const int BUTTON_HEIGHT = 44;
		/// <summary>
		/// The button height wp
		/// </summary>
		private const int BUTTON_HEIGHT_WP = 72;
		/// <summary>
		/// The button half height
		/// </summary>
		private const int BUTTON_HALF_HEIGHT = 22;
		/// <summary>
		/// The button half height wp
		/// </summary>
		private const int BUTTON_HALF_HEIGHT_WP = 36;



		#region Properties
		/// <summary>
		/// Gets or sets the color of the outline.
		/// </summary>
		/// <value>The color of the outline.</value>
		public Color OutlineColor
		{
			get
			{
				return (Color)GetValue(OutlineColorProperty);
			}
			set
			{
				SetValue(OutlineColorProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the color of the view background.
		/// </summary>
		/// <value>The color of the view background.</value>
		public Color ViewBackgroundColor
		{
			get
			{
				return (Color)GetValue(ViewBackgroundColorProperty);
			}
			set
			{
				SetValue(ViewBackgroundColorProperty, value);
				_buttonLayout.BackgroundColor = value;
			}
		}

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
		/// <summary>
		/// Gets or sets the color which will fill the background of a VisualElement. This is a bindable property.
		/// </summary>
		/// <value>The color that is used to fill the background of a VisualElement. The default is <see cref="P:Xamarin.Forms.Color.Default" />.</value>
		/// <remarks>To be added.</remarks>
		public Color BackgroundColor
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
		{
			get { return (Color)GetValue(BackgroundColorProperty); }
			set
			{
				SetValue(BackgroundColorProperty, value);

				if (_buttonLayout == null)
				{
					return;
				}

				for (var iBtn = 0; iBtn < _buttonLayout.Children.Count; iBtn++)
				{
					SetSelectedState(iBtn, iBtn == SelectedIndex);
				}
			}
		}

		/// <summary>
		/// Gets or sets the color of the selected background.
		/// </summary>
		/// <value>The color of the selected background.</value>
		public Color SelectedBackgroundColor
		{
			get { return (Color)GetValue(SelectedBackgroundColorProperty); }
			set
			{
				SetValue(SelectedBackgroundColorProperty, value);

				if (_buttonLayout == null)
				{
					return;
				}

				for (var iBtn = 0; iBtn < _buttonLayout.Children.Count; iBtn++)
				{
					SetSelectedState(iBtn, iBtn == SelectedIndex);
				}
			}
		}

		/// <summary>
		/// Gets or sets the color of the text.
		/// </summary>
		/// <value>The color of the text.</value>
		public Color TextColor
		{
			get { return (Color)GetValue(TextColorProperty); }
			set { SetValue(TextColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets the color of the selected text.
		/// </summary>
		/// <value>The color of the selected text.</value>
		public Color SelectedTextColor
		{
			get { return (Color)GetValue(SelectedTextColorProperty); }
			set { SetValue(SelectedTextColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets the color of the border.
		/// </summary>
		/// <value>The color of the border.</value>
		public Color BorderColor
		{
			get { return (Color)GetValue(BorderColorProperty); }
			set { SetValue(BorderColorProperty, value); }
		}

		/// <summary>
		/// Gets or sets the color of the selected border.
		/// </summary>
		/// <value>The color of the selected border.</value>
		public Color SelectedBorderColor
		{
			get { return (Color)GetValue(SelectedBorderColorProperty); }
			set { SetValue(SelectedBorderColorProperty, value); }
		}
		/// <summary>
		/// Gets or sets the color of the selected frame background.
		/// </summary>
		/// <value>The color of the selected frame background.</value>
		public Color SelectedFrameBackgroundColor
		{
			get { return (Color)GetValue(SelectedFrameBackgroundColorProperty); }
			set { SetValue(SelectedFrameBackgroundColorProperty, value); }
		}


		/// <summary>
		/// Gets or sets the font.
		/// </summary>
		/// <value>The font.</value>
		public Font Font
		{
			get { return (Font)GetValue(FontProperty); }
			set { SetValue(FontProperty, value); }
		}

		/// <summary>
		/// Gets or sets the index of the selected.
		/// </summary>
		/// <value>The index of the selected.</value>
		public int SelectedIndex
		{
			get
			{
				return (int)GetValue(SelectedIndexProperty);
			}
			set
			{
				SetSelectedState(SelectedIndex, false);
				SetValue(SelectedIndexProperty, value);

				if (value < 0 || value >= _buttonLayout.Children.Count)
				{
					return;
				}

				SetSelectedState(value, true);
			}
		}

		/// <summary>
		/// Gets or sets the items.
		/// </summary>
		/// <value>The items.</value>
		public List<string> Items
		{
			get { return (List<string>)GetValue(ItemsPropertyProperty); }
			set
			{
				SetValue(ItemsPropertyProperty, value);

				foreach (var item in Items)
				{
					AddButton(item);
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="ButtonGroup"/> is rounded.
		/// </summary>
		/// <value><c>true</c> if rounded; otherwise, <c>false</c>.</value>
		public bool Rounded
		{
			get
			{
				return (bool)GetValue(RoundedProperty);
			}
			set
			{
				SetValue(RoundedProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is number.
		/// </summary>
		/// <value><c>true</c> if this instance is number; otherwise, <c>false</c>.</value>
		public bool IsNumber
		{
			get
			{
				return (bool)GetValue(IsNumberProperty);
			}
			set
			{
				SetValue(IsNumberProperty, value);
			}
		}

		#endregion

		/// <summary>
		/// The clicked command
		/// </summary>
		private Command _clickedCommand;

		/// <summary>
		/// Initializes a new instance of the <see cref="ButtonGroup"/> class.
		/// </summary>
		public ButtonGroup()
		{
			_buttonLayout = new WrapLayout
			{
				Spacing = SPACING,
				Padding = PADDING,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
			};

			HorizontalOptions = LayoutOptions.FillAndExpand;
			VerticalOptions = LayoutOptions.Center;
			//Padding = new Thickness(Spacing);
			Content = _buttonLayout;
			_clickedCommand = new Command(SetSelectedButton);
		}

		/// <summary>
		/// Adds the button.
		/// </summary>
		/// <param name="text">The text.</param>
		public void AddButton(string text)
		{
			var button = new Button
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = BackgroundColor,
				BorderColor = BorderColor,
				TextColor = TextColor,
				BorderWidth = BUTTON_BORDER_WIDTH,
				BorderRadius =
					Rounded
						? Device.OnPlatform(BUTTON_HALF_HEIGHT, BUTTON_HALF_HEIGHT, BUTTON_HALF_HEIGHT_WP)
						: BUTTON_BORDER_RADIUS,
				HeightRequest = Device.OnPlatform(BUTTON_HEIGHT, BUTTON_HEIGHT, BUTTON_HEIGHT_WP),
				MinimumHeightRequest = Device.OnPlatform(BUTTON_HEIGHT, BUTTON_HEIGHT, BUTTON_HEIGHT_WP),
				Font = Font,
				Command = _clickedCommand,
				CommandParameter = _buttonLayout.Children.Count,
			};

			if (IsNumber)
			{
				button.Text = string.Format("{0}", text);
				button.WidthRequest = Device.OnPlatform(44, 44, 72);
				button.MinimumWidthRequest = Device.OnPlatform(44, 44, 72);
			}
			else
			{
				button.Text = string.Format("  {0}  ", text);
			}

			var frame = new Frame
			{
				BackgroundColor = ViewBackgroundColor,
				Padding = FRAME_PADDING,
				OutlineColor = OutlineColor,
				HasShadow = false,
				Content = button,
			};

			_buttonLayout.Children.Add(frame);

			SetSelectedState(_buttonLayout.Children.Count - 1, _buttonLayout.Children.Count - 1 == SelectedIndex);
		}

		/// <summary>
		/// Sets the selected button.
		/// </summary>
		/// <param name="o">The o.</param>
		private void SetSelectedButton(object o)
		{
			SelectedIndex = (int)o;
		}

		/// <summary>
		/// Sets the state of the selected.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="isSelected">if set to <c>true</c> [is selected].</param>
		private void SetSelectedState(int index, bool isSelected)
		{
			if (index < 0 || _buttonLayout.Children.Count <= index)
			{
				return; //Out of bounds
			}

			var frame = (Frame)_buttonLayout.Children[index];

			frame.HasShadow = isSelected;

			frame.BackgroundColor = isSelected ? SelectedFrameBackgroundColor : ViewBackgroundColor;

			var button = (Button)frame.Content;

			button.BackgroundColor = isSelected ? SelectedBackgroundColor : BackgroundColor;
			button.TextColor = isSelected ? SelectedTextColor : TextColor;
			button.BorderColor = isSelected ? SelectedBorderColor : BorderColor;
		}
	}
}
