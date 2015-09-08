namespace XLabs.Sample.Pages.Services
{
	using Xamarin.Forms;

	using XLabs.Sample.ViewModel;

	/// <summary>
	/// Class TextToSpeechPage.
	/// </summary>
	public partial class TextToSpeechPage : ContentPage
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TextToSpeechPage"/> class.
		/// </summary>
		public TextToSpeechPage ()
		{
			InitializeComponent ();
			BindingContext = ViewModelLocator.Main;
		}
	}
}

