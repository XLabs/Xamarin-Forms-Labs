namespace XLabs.Sample.Pages.Controls
{
	using Xamarin.Forms;

	using XLabs.Sample.ViewModel;

	/// <summary>
	/// Class GestureSample.
	/// </summary>
	public partial class GestureSample  : ContentPage
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GestureSample"/> class.
		/// </summary>
		public GestureSample()
		{
			InitializeComponent();
			BindingContext = new GestureSampleVm();
		}
	}
}
