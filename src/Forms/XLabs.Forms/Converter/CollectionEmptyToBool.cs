using System;
using System.Collections;
using System.Linq;
using Xamarin.Forms;

namespace XLabs.Forms.Converter
{
	/// <summary>
	/// Inverts a boolean value
	/// </summary>    
	/// <remarks>Removed unneeded default ctor</remarks>
	public class CollectionEmptyToBool : IValueConverter
	{

		/// <summary>
		/// Converts a boolean to it's negated value/>.
		/// </summary>
		/// <param name="value">The boolean to negate.</param>
		/// <param name="targetType">not used.</param>
		/// <param name="parameter">not used.</param>
		/// <param name="culture">not used.</param>
		/// <returns>Negated boolean value.</returns>
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var ie = value as IEnumerable;
			var reverse = parameter != null && parameter.ToString().ToLower() == "invert";
			var val = ie==null || IsEmpty(ie);

			return reverse ? val : !val;
		}

		/// <summary>
		/// Converts a negated value back to it's non negated value....silly I know
		/// </summary>
		/// <param name="value">The value to be un negated.</param>
		/// <param name="targetType">not used.</param>
		/// <param name="parameter">not used.</param>
		/// <param name="culture">not used.</param>
		/// <returns>The original unnegated value.</returns>
		/// <remarks>To be added.</remarks>
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new  NotImplementedException();
		}

		/// <summary>
		/// Determines whether the specified en is empty.
		/// </summary>
		/// <param name="en">The en.</param>
		/// <returns><c>true</c> if the specified en is empty; otherwise, <c>false</c>.</returns>
		private bool IsEmpty(IEnumerable en)
		{
			return !en.Cast<object>().Any();
		}
	}
}

