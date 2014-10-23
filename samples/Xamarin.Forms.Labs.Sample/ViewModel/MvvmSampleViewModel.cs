using Xamarin.Forms.Labs.Mvvm;

namespace Xamarin.Forms.Labs.Sample
{
    /// <summary>
    /// The MVVM sample view model.
    /// </summary>
    [ViewType(typeof(MvvmSamplePage))]
	public class MvvmSampleViewModel : ViewModel
	{
	    private Command navigateToViewModel;
        private string navigateToViewModelButtonText = "Navigate to another view model";

	    /// <summary>
	    /// Gets the navigate to view model.
	    /// </summary>
	    /// <value>
	    /// The navigate to view model.
	    /// </value>
	    public Command NavigateToViewModel 
		{
			get
			{
			    return navigateToViewModel ?? (navigateToViewModel = new Command(
			                                                               async () => await Navigation.PushAsync<NewPageViewModel>(),
			                                                               () => true));
			}
		}

        /// <summary>
        /// Gets or sets the navigate to view model button text.
        /// </summary>
        /// <value>
        /// The navigate to view model button text.
        /// </value>
        public string NavigateToViewModelButtonText
		{
			get
			{
				return navigateToViewModelButtonText;
			}
			set
			{ 
				this.SetProperty(ref navigateToViewModelButtonText, value);
			}
		}
	}
}

