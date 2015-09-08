using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Enums;

namespace XLabs.Forms.Controls
{
    public class IconLabel : Label
    {
        /// <summary>
        /// Backing field for the orientation property.
        /// </summary>
        public static readonly BindableProperty TextAlignementProperty =
            BindableProperty.Create<IconLabel, TextAlignment>(
                p => p.TextAlignement, TextAlignment.Center);


        /// <summary>
        /// Gets or sets The TextAlignment of both icon and text relative to container.
        /// </summary> 
        /// <value>
        /// The Orientation property gets/sets the value of the backing field, OrientationProperty.
        /// </value> 
        public TextAlignment TextAlignement
        {
            get { return (TextAlignment)GetValue(TextAlignementProperty); }
            set { SetValue(TextAlignementProperty, value); }
        }
        /// <summary>
        /// Backing field for the orientation property.
        /// </summary>
        public static readonly BindableProperty OrientationProperty =
            BindableProperty.Create<IconLabel, ImageOrientation>(
                p => p.Orientation, ImageOrientation.ImageToLeft);


        /// <summary>
        /// Gets or sets The orientation of the image relative to the text.
        /// </summary> 
        /// <remarks>
        /// On iOS only left and right are supported
        /// </remarks>
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
            BindableProperty.Create<IconLabel, string>(
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
        /// Backing field for the ShowIconSeparator property
        /// </summary>
        public static readonly BindableProperty ShowIconSeparatorProperty =
            BindableProperty.Create<IconLabel, bool>(
            p => p.ShowIconSeparator, default(bool));


        /// <summary>
        /// Indicate if | separator must be place between the icon and the text
        /// </summary>
        /// <value>
        /// 
        /// </value>
        public bool ShowIconSeparator
        {
            get { return (bool)GetValue(ShowIconSeparatorProperty); }
            set { SetValue(ShowIconSeparatorProperty, value); }
        }

        /// <summary>
        /// Backing field for the icon color property
        /// </summary>
        public static readonly BindableProperty IconColorProperty =
            BindableProperty.Create<IconLabel, Color>(
                p => p.IconColor, default(Color));

        /// <summary>
        /// Gets or sets the icon's color
        /// </summary>
        /// <value>
        /// The IconColor property gets/sets the value of the backing field, IconColorProperty
        /// </value>
        public Color IconColor
        {
            get { return (Color)GetValue(IconColorProperty); }
            set { SetValue(IconColorProperty, value); }
        }

        /// <summary>
        /// Backing field for the icon size property
        /// </summary>
        public static readonly BindableProperty IconSizeProperty =
            BindableProperty.Create<IconLabel, double>(
                p => p.IconSize, default(double));

        /// <summary>
        /// Gets or set's the font size of the icon
        /// </summary>
        /// <value>
        /// The IconSize property gets/sets the value of the backing field, IconSizeProperty
        /// </value>
        public double IconSize
        {
            get { return (double)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }

        /// <summary>
        /// Backing field for the icon font name property
        /// </summary>
        public static readonly BindableProperty IconFontNameProperty =
            BindableProperty.Create<IconLabel, string>(
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
