namespace XLabs.Sample.Pages.Controls
{
	using Xamarin.Forms;

	using XLabs.Sample.ViewModel;

	/// <summary>
	/// Class AutoCompletePage.
	/// </summary>
	public partial class AutoCompletePage : ContentPage
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AutoCompletePage"/> class.
		/// </summary>
		public AutoCompletePage ()
		{
			InitializeComponent ();
            BindingContext = ViewModelLocator.AutoCompleteViewModel;
		}
	}
}

