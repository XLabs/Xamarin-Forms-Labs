using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
	using XLabs.Enums;

	/// <summary>
	/// Creates a button with text and a FontAwesome icon
	/// The icon can be on the left, above, on the right or below the text.
	/// </summary>
	public class IconButton : Button
	{
		/// <summary>
		/// Backing field for the orientation property.
		/// </summary>
		public static readonly BindableProperty OrientationProperty =
			BindableProperty.Create<IconButton, ImageOrientation>(
				p => p.Orientation, ImageOrientation.ImageToLeft);


		/// <summary>
		/// Gets or sets The orientation of the image relative to the text.
		/// </summary> 
		/// <value>
		/// The Orientation property gets/sets the value of the backing field, OrientationProperty.
		/// </value> 
		public ImageOrientation Orientation
		{
			get { return (ImageOrientation)GetValue(OrientationProperty); }
			set { SetValue(OrientationProperty, value); }
		}

		/// <summary>
		/// Backing field for the icon property
		/// </summary>
		public static readonly BindableProperty IconProperty = 
			BindableProperty.Create<IconButton, string>(
			p => p.Icon, default(string));

		/// <summary>
		/// Gets or sets the icon. A set of FontAwesome icons have been included in <see cref="Icons"/>, and
		/// more can be added from the FontAwesome cheatsheet (http://fortawesome.github.io/Font-Awesome/cheatsheet/)
		/// </summary>
		/// <value>
		/// The Icon property gets/sets the value of the backing field, IconProperty
		/// </value>
		public string Icon
		{
			get { return (string)GetValue(IconProperty); }
			set { SetValue(IconProperty, value); }
		}

		/// <summary>
		/// Backing field for the icon color property
		/// </summary>
		public static readonly BindableProperty IconColorProperty =
			BindableProperty.Create<IconButton, Color>(
				p => p.IconColor, default(Color));

		/// <summary>
		/// Gets or sets the icon's color
		/// </summary>
		/// <value>
		/// The IconColor property gets/sets the value of the backing field, IconColorProperty
		/// </value>
		public Color IconColor { get { return (Color) GetValue(IconColorProperty); } 
			set{ SetValue(IconColorProperty, value);} 
		}

		/// <summary>
		/// Backing field for the icon size property
		/// </summary>
		public static readonly BindableProperty IconSizeProperty =
			BindableProperty.Create<IconButton, float>(
				p => p.IconSize, default(float));

		/// <summary>
		/// Gets or set's the font size of the icon
		/// </summary>
		/// <value>
		/// The IconSize property gets/sets the value of the backing field, IconSizeProperty
		/// </value>
		public float IconSize
		{
			get { return (float)GetValue(IconSizeProperty); }
			set { SetValue(IconSizeProperty, value); }
		}

		/// <summary>
		/// Backing field for the icon font name property
		/// </summary>
		public static readonly BindableProperty IconFontNameProperty = 
			BindableProperty.Create<IconButton, string>(
			p => p.IconFontName, default(string));

		/// <summary>
		/// Gets or set's the font name for the icon - currently this will default to using the FontAwesome font (http://fortawesome.github.io/Font-Awesome/cheatsheet/).
		/// Be sure that the fontawesome-webfont.ttf is in your iOS project's Resources folder, and that the build action for it is set to Bundle Resource and Copy Always to Output.
		/// Also, an entry to your iOS's info.plist must be made. If you are using Visual Studio 2013, this can be done manually by editing the info.plist as XML. Simply add an entry
		/// like this inside of the <dict></dict> element:
		/// 
		/// <key>UIAppFonts</key>
		/// <array>
		///     <string>Fonts/Roboto-Light.ttf</string> <!-- Inside the Resources/Fonts folder -->
		///     <string>Fonts/fontawesome-webfont.ttf</string> <!-- Inside the Resources/Fonts folder -->
		///     <string>AnotherFont.ttf</string> <!-- Inside the Resources folder -->
		/// </array>
		/// 
		/// </summary>
		/// <value>
		/// The IconFontName property gets/sets the value of the backing field, IconFontNameProperty
		/// </value>
		public string IconFontName
		{
			get { return (string)GetValue(IconFontNameProperty); }
			set { SetValue(IconFontNameProperty, value); }
		}
	}
}
