namespace XLabs.Forms.Extensions
{
    using System;
    using System.Windows;
    using Windows.UI.Xaml.Media;
    using Xamarin.Forms;

    /// <summary>
    /// Class FontExtensions.
    /// </summary>
    public static class FontExtensions
    {
        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <returns>System.Double.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static double GetHeight(this Font font)
        {
            if (!font.UseNamedSize) return font.FontSize;

            switch (font.NamedSize)
            {
                case NamedSize.Micro:
                    return (double)Application.Current.Resources["PhoneFontSizeSmall"] - 3.0;
                case NamedSize.Small:
                    return (double)Application.Current.Resources["PhoneFontSizeSmall"];
                case NamedSize.Default:
                case NamedSize.Medium:
                    return (double)Application.Current.Resources["PhoneFontSizeNormal"];
                case NamedSize.Large:
                    return (double)Application.Current.Resources["PhoneFontSizeLarge"];
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Gets the font family.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <returns>FontFamily.</returns>
        public static FontFamily GetFontFamily(this Font font)
        {
            return string.IsNullOrEmpty(font.FontFamily)
                       ? (FontFamily)Application.Current.Resources["PhoneFontFamilyNormal"]
                       : new FontFamily(font.FontFamily);
        }
    }
}
