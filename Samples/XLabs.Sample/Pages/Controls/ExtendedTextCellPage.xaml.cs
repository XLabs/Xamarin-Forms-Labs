namespace XLabs.Sample.Pages.Controls
{
	using Xamarin.Forms;

	using XLabs.Sample.ViewModel;

	/// <summary>
	/// Class ExtendedCellPage.
	/// </summary>
	public partial class ExtendedTextCellPage : ContentPage
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedTextCellPage"/> class.
        /// </summary>
        public ExtendedTextCellPage()
		{
			InitializeComponent();
			BindingContext = ViewModelLocator.Main;
		}
	}
}

