namespace XLabs.Sample.Pages.Mvvm
{
	using Xamarin.Forms;

	using XLabs.Forms.Mvvm;
	using XLabs.Sample.ViewModel;

	/// <summary>
	/// Class MvvmSamplePage.
	/// </summary>
	public partial class MvvmSamplePage : BaseView
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MvvmSamplePage"/> class.
		/// </summary>
		public MvvmSamplePage()
		{
			InitializeComponent ();
			BindingContext = new MvvmSampleViewModel ();

			Icon = Device.OnPlatform("pie27_32.png", "pie27_32.png", "Images/pie27_32.png");
		}
	}
}

