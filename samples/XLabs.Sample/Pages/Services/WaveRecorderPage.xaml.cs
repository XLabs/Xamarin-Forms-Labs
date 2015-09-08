namespace XLabs.Sample.Pages.Services
{
	using XLabs.Sample.ViewModel;

	/// <summary>
	/// Class WaveRecorderPage.
	/// </summary>
	public partial class WaveRecorderPage
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WaveRecorderPage"/> class.
		/// </summary>
		public WaveRecorderPage()
		{
			InitializeComponent();
		}

		/// <summary>
		/// When overridden, allows the application developer to customize behavior as the <see cref="T:Xamarin.Forms.Page" /> disappears.
		/// </summary>
		/// <remarks>To be added.</remarks>
		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			var vm = this.BindingContext as WaveRecorderViewModel;

			if (vm != null && vm.Stop.CanExecute(this))
			{
				vm.Stop.Execute(this);
			}
		}
	}
}
