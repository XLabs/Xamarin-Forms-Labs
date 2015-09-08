namespace XLabs.Forms
{
	using Xamarin.Forms;

	using XLabs.Ioc;
	using XLabs.Platform.Device;

	/// <summary>
	/// Class ViewExtensions.
	/// </summary>
	public static class ViewExtensions
	{
		/// <summary>
		/// Gets the display.
		/// </summary>
		/// <value>The display.</value>
		private static IDisplay Display
		{
			get
			{
				return Resolver.Resolve<IDisplay>() ?? 
					Resolver.Resolve<IDevice>().Display;
			}
		}

		/// <summary>
		/// The width in inches
		/// </summary>
		private static double? _widthInInches;
		/// <summary>
		/// The height in inches
		/// </summary>
		private static double? _heightInInches;

		/// <summary>
		/// Gets the width in inches.
		/// </summary>
		/// <value>The width in inches.</value>
		private static double WidthInInches
		{
			get
			{
				if (_widthInInches.HasValue)
				{
					return _widthInInches.Value;
				}
				
				_widthInInches = Display.WidthRequestInInches(1);
				return _widthInInches.Value;
			}
		}

		/// <summary>
		/// Gets the height in inches.
		/// </summary>
		/// <value>The height in inches.</value>
		private static double HeightInInches
		{
			get
			{
				if (_widthInInches.HasValue)
				{
					return _heightInInches.Value;
				}

				_heightInInches = Display.WidthRequestInInches(1);
				return _heightInInches.Value;
			}
		}

		/// <summary>
		/// Gets the width request in inches.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <returns>System.Double.</returns>
		public static double GetWidthRequestInInches(this View view)
		{
			return view.WidthRequest / WidthInInches;
		}

		/// <summary>
		/// Sets the width request in inches.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <param name="inches">The inches.</param>
		public static void SetWidthRequestInInches(this View view, double inches)
		{
			view.WidthRequest = inches * WidthInInches;
		}

		/// <summary>
		/// Gets the height request in inches.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <returns>System.Double.</returns>
		public static double GetHeightRequestInInches(this View view)
		{
			return view.HeightRequest / HeightInInches;
		}

		/// <summary>
		/// Sets the height request in inches.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <param name="inches">The inches.</param>
		public static void SetHeightRequestInInches(this View view, double inches)
		{
			view.HeightRequest = inches * HeightInInches;
		}
	}
}
