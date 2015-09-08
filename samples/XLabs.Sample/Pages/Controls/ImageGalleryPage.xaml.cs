namespace XLabs.Sample.Pages.Controls
{
	using Xamarin.Forms;

	using XLabs.Sample.ViewModel;

	/// <summary>
	/// Class ImageGalleryPage.
	/// </summary>
	public partial class ImageGalleryPage : ContentPage
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ImageGalleryPage"/> class.
		/// </summary>
		public ImageGalleryPage()
		{
			InitializeComponent();
			BindingContext = new MainViewModel();
		}

		/// <summary>
		/// When overridden, allows application developers to customize behavior immediately prior to the <see cref="T:Xamarin.Forms.Page" /> becoming visible.
		/// </summary>
		/// <remarks>To be added.</remarks>
		protected override void OnAppearing()
		{
		   (BindingContext as MainViewModel).AddImages();

			base.OnAppearing();
		}
	}
}

