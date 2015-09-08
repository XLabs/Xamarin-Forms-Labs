namespace XLabs.Platform.Device
{
	using Android.App;
	using Android.Util;

	/// <summary>
	///     Android Display implements <see cref="IDisplay" />.
	/// </summary>
	public class Display : IDisplay
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="Display" /> class.
		/// </summary>
		public Display()
		{
			var dm = Metrics;
			Height = dm.HeightPixels;
			Width = dm.WidthPixels;
			Xdpi = dm.Xdpi;
			Ydpi = dm.Ydpi;

			//FontManager = new FontManager(this);
		}

		/// <summary>
		///     Gets the metrics.
		/// </summary>
		/// <value>The metrics.</value>
		public static DisplayMetrics Metrics
		{
			get
			{
				return Application.Context.Resources.DisplayMetrics;
			}
		}

		/// <summary>
		///     Returns a <see cref="System.String" /> that represents the current <see cref="Display" />.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents the current <see cref="Display" />.</returns>
		public override string ToString()
		{
			return string.Format("[Screen: Height={0}, Width={1}, Xdpi={2:0.0}, Ydpi={3:0.0}]", Height, Width, Xdpi, Ydpi);
		}

		#region IScreen implementation

		/// <summary>
		///     Gets the screen height in pixels
		/// </summary>
		public int Height { get; private set; }

		/// <summary>
		///     Gets the screen width in pixels
		/// </summary>
		public int Width { get; private set; }

		/// <summary>
		///     Gets the screens X pixel density per inch
		/// </summary>
		public double Xdpi { get; private set; }

		/// <summary>
		///     Gets the screens Y pixel density per inch
		/// </summary>
		public double Ydpi { get; private set; }

		//public IFontManager FontManager { get; private set; }

		/// <summary>
		///     Convert width in inches to runtime pixels
		/// </summary>
		public double WidthRequestInInches(double inches)
		{
			return inches * Xdpi / Metrics.Density;
		}

		/// <summary>
		///     Convert height in inches to runtime pixels
		/// </summary>
		public double HeightRequestInInches(double inches)
		{
			return inches * Ydpi / Metrics.Density;
		}

		#endregion
	}
}