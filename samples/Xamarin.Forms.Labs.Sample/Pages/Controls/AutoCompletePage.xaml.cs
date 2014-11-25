namespace Xamarin.Forms.Labs.Sample
{
    /// <summary>
    /// Define the AutoCompletePage.
    /// </summary>
	public partial class AutoCompletePage : ContentPage
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoCompletePage"/> class.
        /// </summary>
		public AutoCompletePage ()
		{
			InitializeComponent ();
            BindingContext = ViewModelLocator.AutoCompleteViewModel;
		}
	}
}

