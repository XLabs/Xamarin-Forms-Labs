namespace XLabs.Sample.Pages.Controls
{
	using XLabs.Forms.Mvvm;
	using XLabs.Sample.ViewModel;

	/// <summary>
	/// Class GridViewPage.
	/// </summary>
	public partial class GridViewPage : BaseView
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GridViewPage"/> class.
		/// </summary>
		public GridViewPage ()
		{
			InitializeComponent ();
			BindingContext = ViewModelLocator.Main;
			this.GrdView.ItemSelected += (sender, e) => {
				DisplayAlert ("selected value", e.Value.ToString (), "ok");
			};
		}


	}
}

