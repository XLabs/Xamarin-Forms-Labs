namespace XLabs.Sample.Pages.Controls
{
	using Xamarin.Forms;

	/// <summary>
	/// Class SegmentedControlViewPage.
	/// </summary>
	public partial class SegmentedControlViewPage : ContentPage
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SegmentedControlViewPage"/> class.
		/// </summary>
		public SegmentedControlViewPage()
		{
			InitializeComponent();

			Filter.SelectedItem = 1;
		}
	}
}
