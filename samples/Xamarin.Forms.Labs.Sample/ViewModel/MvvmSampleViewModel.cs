namespace Xamarin.Forms.Labs.Sample
{
  using Xamarin.Forms.Labs.Mvvm.Views;

  /// <summary>
  /// The MVVM sample view model.
  /// </summary>
  public class MvvmSampleViewModel : Mvvm.ViewModels.ViewModel
  {
    private Command navigateToViewModel;
    private Command navigateToKeyedViewModel;
    private Command navigateModallyToViewModel;
    private string navigateToViewModelButtonText = "Navigate to another view model";
    private string navigateToKeyedViewModelButtonText = "Navigate to a keyed view model";
    private string navigateModallyText = "Navigate to a view model modally";

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
                                                               async () => await NavigationService.PushAsync<NewPageViewModel>(),
                                                               () => true));
      }
    }

    /// <summary>
    /// Gets the navigate to view model.
    /// </summary>
    /// <value>
    /// The navigate to view model.
    /// </value>
    public Command NavigateToKeyedViewModel
    {
      get
      {
        return navigateToKeyedViewModel ?? (navigateToKeyedViewModel = new Command(
                                                                         // use cached version of the view
                                                                         async () => await NavigationService.PushAsync<NewPageViewModel>("key", null, true),
                                                                         () => true));
      }
    }

    /// <summary>
    /// Gets the navigate to view model.
    /// </summary>
    /// <value>
    /// The navigate to view model.
    /// </value>
    public Command NavigateModallyToViewModel
    {
      get
      {
        return navigateModallyToViewModel ?? (navigateModallyToViewModel = new Command(
                                                                         // use cached version of the view
                                                                         async () => await NavigationService.PushModalAsync<NewPageViewModel>(typeof(BaseNavigationView), "key", null, true),
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
        SetProperty(ref navigateToViewModelButtonText, value);
      }
    }

    /// <summary>
    /// Gets or sets the navigate to view model button text.
    /// </summary>
    /// <value>
    /// The navigate to view model button text.
    /// </value>
    public string NavigateToKeyedViewModelButtonText
    {
      get
      {
        return navigateToKeyedViewModelButtonText;
      }
      set
      {
        SetProperty(ref navigateToKeyedViewModelButtonText, value);
      }
    }

    /// <summary>
    /// Gets or sets the navigate to view model button text.
    /// </summary>
    /// <value>
    /// The navigate to view model button text.
    /// </value>
    public string NavigateModallyText
    {
      get
      {
        return navigateModallyText;
      }
      set
      {
        SetProperty(ref navigateModallyText, value);
      }
    }
  }
}