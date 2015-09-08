namespace XLabs.Sample.Pages.Controls
{
	using Xamarin.Forms;

	using XLabs.Sample.ViewModel;

	/// <summary>
	/// Class ExtendedCellPage.
	/// </summary>
	public partial class ExtendedCellPage : ContentPage
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExtendedCellPage"/> class.
		/// </summary>
		public ExtendedCellPage()
		{
			InitializeComponent();
			BindingContext = ViewModelLocator.Main;
		}
	}
}

