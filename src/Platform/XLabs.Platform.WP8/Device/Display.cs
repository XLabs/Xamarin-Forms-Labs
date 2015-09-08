namespace XLabs.Platform.Device
{
	using System.Windows;

	using Microsoft.Phone.Info;

	/// <summary>
	/// Windows Phone 8 Display.
	/// </summary>
	public class Display : IDisplay
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Display" /> class.
		/// </summary>
		/// <remarks>To get accurate display reading application should enable ID_CAP_IDENTITY_DEVICE on app manifest.</remarks>
		public Display()
		{
			object physicalScreenResolutionObject;

			if (DeviceExtendedProperties.TryGetValue("PhysicalScreenResolution", out physicalScreenResolutionObject))
			{
				var physicalScreenResolution = (Size)physicalScreenResolutionObject;
				Height = (int)physicalScreenResolution.Height;
				Width = (int)physicalScreenResolution.Width;
			}
			else
			{
				var scaleFactor = Application.Current.Host.Content.ScaleFactor;
				Height = (int)(Application.Current.Host.Content.ActualHeight * scaleFactor);
				Width = (int)(Application.Current.Host.Content.ActualWidth * scaleFactor);
			}

			object rawDpiX, rawDpiY;

			if (DeviceExtendedProperties.TryGetValue("RawDpiX", out rawDpiX))
			{
				Xdpi = (double)rawDpiX;
			}

			if (DeviceExtendedProperties.TryGetValue("RawDpiY", out rawDpiY))
			{
				Ydpi = (double)rawDpiY;
			}

			//FontManager = new FontManager(this);
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents the current <see cref="Display" />.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents the current <see cref="Display" />.</returns>
		public override string ToString()
		{
			return string.Format("[Screen: Height={0}, Width={1}, Xdpi={2:0.0}, Ydpi={3:0.0}]", Height, Width, Xdpi, Ydpi);
		}

		#region IDisplay Members

		/// <summary>
		/// Gets the screen height in pixels
		/// </summary>
		/// <value>The height.</value>
		public int Height { get; private set; }

		/// <summary>
		/// Gets the screen width in pixels
		/// </summary>
		/// <value>The width.</value>
		public int Width { get; private set; }

		/// <summary>
		/// Gets the screens X pixel density per inch
		/// </summary>
		/// <value>The xdpi.</value>
		public double Xdpi { get; private set; }

		/// <summary>
		/// Gets the screens Y pixel density per inch
		/// </summary>
		/// <value>The ydpi.</value>
		public double Ydpi { get; private set; }

		/// <summary>
		/// Gets the font manager.
		/// </summary>
		/// <value>The font manager.</value>
		//public IFontManager FontManager { get; private set; }

		/// <summary>
		/// Convert width in inches to runtime pixels
		/// </summary>
		/// <param name="inches">The inches.</param>
		/// <returns>System.Double.</returns>
		public double WidthRequestInInches(double inches)
		{
			return inches * Xdpi * 100 / Application.Current.Host.Content.ScaleFactor;
		}

		/// <summary>
		/// Convert height in inches to runtime pixels
		/// </summary>
		/// <param name="inches">The inches.</param>
		/// <returns>System.Double.</returns>
		public double HeightRequestInInches(double inches)
		{
			return inches * Ydpi * 100 / Application.Current.Host.Content.ScaleFactor;
		}

		#endregion
	}
}