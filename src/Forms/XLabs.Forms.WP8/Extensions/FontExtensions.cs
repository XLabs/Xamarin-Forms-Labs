namespace XLabs.Forms.Extensions
{
    using System;
    using System.Windows;
    using System.Windows.Media;

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
                    return (double)System.Windows.Application.Current.Resources[(object)"PhoneFontSizeSmall"] - 3.0;
                case NamedSize.Small:
                    return (double)System.Windows.Application.Current.Resources[(object)"PhoneFontSizeSmall"];
                case NamedSize.Default:
                case NamedSize.Medium:
                    return (double)System.Windows.Application.Current.Resources[(object)"PhoneFontSizeNormal"];
                case NamedSize.Large:
                    return (double)System.Windows.Application.Current.Resources[(object)"PhoneFontSizeLarge"];
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
                       ? (FontFamily)System.Windows.Application.Current.Resources[(object)"PhoneFontFamilyNormal"]
                       : new FontFamily(font.FontFamily);
        }
    }
}
