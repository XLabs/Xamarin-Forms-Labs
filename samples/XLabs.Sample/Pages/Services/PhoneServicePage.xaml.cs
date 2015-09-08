namespace XLabs.Sample.Pages.Services
{
	using Xamarin.Forms;

	using XLabs.Sample.ViewModel;

	/// <summary>
	/// Class PhoneServicePage.
	/// </summary>
	public partial class PhoneServicePage : ContentPage
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PhoneServicePage"/> class.
		/// </summary>
		public PhoneServicePage ()
		{
			InitializeComponent ();
			BindingContext = ViewModelLocator.Main;
		}
	}
}

