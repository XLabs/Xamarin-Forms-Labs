using System;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;

namespace XLabs.Forms.Converter
{
	/// <summary>
	/// Class HeightToMillimeters.
	/// </summary>
	public class HeightToMillimeters : IValueConverter
	{
		/// <summary>
		/// The display
		/// </summary>
		private static IDisplay _display;
		/// <summary>
		/// The millimeters in inch
		/// </summary>
		private const double MILLIMETERS_IN_INCH = 25.4;

		#region IValueConverter Members

		/// <summary>
		/// Implement this method to convert <paramref name="value" /> to <paramref name="targetType" /> by using <paramref name="parameter" /> and <paramref name="culture" />.
		/// </summary>
		/// <param name="value">To be added.</param>
		/// <param name="targetType">To be added.</param>
		/// <param name="parameter">To be added.</param>
		/// <param name="culture">To be added.</param>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return ToMillimeters((double)value);
		}

		/// <summary>
		/// Implement this method to convert <paramref name="value" /> back from <paramref name="targetType" /> by using <paramref name="parameter" /> and <paramref name="culture" />.
		/// </summary>
		/// <param name="value">To be added.</param>
		/// <param name="targetType">To be added.</param>
		/// <param name="parameter">To be added.</param>
		/// <param name="culture">To be added.</param>
		/// <returns>To be added.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		/// <remarks>To be added.</remarks>
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion

		/// <summary>
		/// Gets the display.
		/// </summary>
		/// <value>The display.</value>
		/// <exception cref="System.InvalidOperationException">Unable to resolve display. Please set the IDevice implementation on your IoC container.</exception>
		private static IDisplay Display
		{
			get
			{
				if ((_display ?? (_display = Resolver.Resolve<IDevice>().Display)) == null)
				{
					throw new InvalidOperationException("Unable to resolve display. Please set the IDevice implementation on your IoC container.");
				}

				return _display;
			}
		}

		/// <summary>
		/// To the millimeters.
		/// </summary>
		/// <param name="inches">The inches.</param>
		/// <returns>System.Double.</returns>
		private static double ToMillimeters(double inches)
		{
			return Display.HeightRequestInInches(inches) / MILLIMETERS_IN_INCH;
		}
	}
}
