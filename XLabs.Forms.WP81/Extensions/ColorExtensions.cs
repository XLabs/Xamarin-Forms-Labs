namespace XLabs.Forms
{
    using Windows.UI;
    using Windows.UI.Xaml.Media;

    /// <summary>
    /// Class ColorExtensions.
    /// </summary>
    public static class ColorExtensions
	{
		public static Brush ToBrush(this Xamarin.Forms.Color color)
		{
			return new SolidColorBrush(color.ToMediaColor());
		}

		public static Color ToMediaColor(this Xamarin.Forms.Color color)
		{
			return Color.FromArgb((byte)(color.A * 255.0), (byte)(color.R * 255.0), (byte)(color.G * 255.0), (byte)(color.B * 255.0));
		}
	}
}
