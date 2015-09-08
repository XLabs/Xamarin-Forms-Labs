namespace XLabs.Platform.Device
{
	using System;

	/// <summary>
	/// Class DisplayExtensions.
	/// </summary>
	public static class DisplayExtensions
	{
		/// <summary>
		/// Screens the size inches.
		/// </summary>
		/// <param name="screen">The screen.</param>
		/// <returns>System.Double.</returns>
		public static double ScreenSizeInches(this IDisplay screen)
		{
			return Math.Sqrt(Math.Pow(screen.ScreenWidthInches(), 2) + Math.Pow(screen.ScreenHeightInches(), 2));
		}

		/// <summary>
		/// Screens the width inches.
		/// </summary>
		/// <param name="screen">The screen.</param>
		/// <returns>System.Double.</returns>
		public static double ScreenWidthInches(this IDisplay screen)
		{
			return screen.Width / screen.Xdpi;
		}

		/// <summary>
		/// Screens the height inches.
		/// </summary>
		/// <param name="screen">The screen.</param>
		/// <returns>System.Double.</returns>
		public static double ScreenHeightInches(this IDisplay screen)
		{
			return screen.Height / screen.Ydpi;
		}
	}
}

