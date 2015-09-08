namespace XLabs.Forms.Controls.MonoDroid.TimesSquare
{
	using Xamarin.Forms;
	using Xamarin.Forms.Platform.Android;

	/// <summary>
	/// Class StyleDescriptor.
	/// </summary>
	public class StyleDescriptor
	{
		/// <summary>
		/// The background color
		/// </summary>
		public Android.Graphics.Color BackgroundColor = Color.FromHex("#ffffffff").ToAndroid();
		/// <summary>
		/// The date foreground color
		/// </summary>
		public Android.Graphics.Color DateForegroundColor = Color.FromHex("#ff778088").ToAndroid();
		/// <summary>
		/// The date background color
		/// </summary>
		public Android.Graphics.Color DateBackgroundColor = Color.FromHex("#fff5f7f9").ToAndroid();
		/// <summary>
		/// The inactive date foreground color
		/// </summary>
		public Android.Graphics.Color InactiveDateForegroundColor = Color.FromHex("#40778088").ToAndroid();
		/// <summary>
		/// The inactive date background color
		/// </summary>
		public Android.Graphics.Color InactiveDateBackgroundColor = Color.FromHex("#fff5f7f9").ToAndroid();
		/// <summary>
		/// The selected date foreground color
		/// </summary>
		public Android.Graphics.Color SelectedDateForegroundColor = Color.FromHex("#ffffffff").ToAndroid();
		/// <summary>
		/// The selected date background color
		/// </summary>
		public Android.Graphics.Color SelectedDateBackgroundColor = Color.FromHex("#ff379bff").ToAndroid();
		/// <summary>
		/// The title foreground color
		/// </summary>
		public Android.Graphics.Color TitleForegroundColor = Color.FromHex("#ff778088").ToAndroid();
		/// <summary>
		/// The title background color
		/// </summary>
		public Android.Graphics.Color TitleBackgroundColor = Color.FromHex("#ffffffff").ToAndroid();
		/// <summary>
		/// The today foreground color
		/// </summary>
		public Android.Graphics.Color TodayForegroundColor = Color.FromHex("#ff778088").ToAndroid();
		/// <summary>
		/// The today background color
		/// </summary>
		public Android.Graphics.Color TodayBackgroundColor = Color.FromHex("#ccffcc").ToAndroid();
		/// <summary>
		/// The day of week label foreground color
		/// </summary>
		public Android.Graphics.Color DayOfWeekLabelForegroundColor =  Color.FromHex("#ff778088").ToAndroid();
		/// <summary>
		/// The day of week label background color
		/// </summary>
		public Android.Graphics.Color DayOfWeekLabelBackgroundColor = Color.FromHex("#ffffffff").ToAndroid();
		/// <summary>
		/// The highlighted date foreground color
		/// </summary>
		public Android.Graphics.Color HighlightedDateForegroundColor =  Color.FromHex("#ff778088").ToAndroid();
		/// <summary>
		/// The highlighted date background color
		/// </summary>
		public Android.Graphics.Color HighlightedDateBackgroundColor = Color.FromHex("#ccffcc").ToAndroid();
		/// <summary>
		/// The date separator color
		/// </summary>
		public Android.Graphics.Color DateSeparatorColor = Color.FromHex("#ffbababa").ToAndroid();
		/// <summary>
		/// The month title font
		/// </summary>
		public Android.Graphics.Typeface MonthTitleFont = null;
		/// <summary>
		/// The date label font
		/// </summary>
		public Android.Graphics.Typeface DateLabelFont = null;
		/// <summary>
		/// The should highlight days of week label
		/// </summary>
		public bool 	ShouldHighlightDaysOfWeekLabel = false;

	}



}

