using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class ExtendedTimePicker.
	/// </summary>
	public class ExtendedTimePicker : TimePicker
	{
		/// <summary>
		/// The HasBorder property
		/// </summary>
		public static readonly BindableProperty HasBorderProperty =
			BindableProperty.Create("HasBorder", typeof(bool), typeof(ExtendedTimePicker), true);

		/// <summary>
		/// Initializes a new instance of the <see cref="ExtendedTimePicker"/> class.
		/// </summary>
		public ExtendedTimePicker ()
		{
		}

		/// <summary>
		/// Gets or sets if the border should be shown or not
		/// </summary>
		/// <value><c>true</c> if this instance has border; otherwise, <c>false</c>.</value>
		public bool HasBorder
		{
			get { return (bool)GetValue(HasBorderProperty); }
			set { SetValue(HasBorderProperty, value); }
		}	
	}
}

