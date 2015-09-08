namespace XLabs.Platform
{
#if WINDOWS_PHONE_APP
    using Windows.UI;
    using Windows.UI.Xaml.Media;
#else
    using System.Windows.Media;
#endif

    /// <summary>
    /// Class ColorExtensions.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// To the brush.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>Brush.</returns>
        public static Brush ToBrush(this Color color)
        {
            return new SolidColorBrush(color.ToMediaColor());
        }

        /// <summary>
        /// To the color of the media.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>Color.</returns>
        public static Color ToMediaColor(this Color color)
        {
            return Color.FromArgb(
                (byte)(color.A * 255.0),
                (byte)(color.R * 255.0),
                (byte)(color.G * 255.0),
                (byte)(color.B * 255.0));
        }
    }
}