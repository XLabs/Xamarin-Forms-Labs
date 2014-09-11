namespace Xamarin.Forms.Labs.Mvvm.ViewModels
{
  #region Usings

  using System.Threading.Tasks;

  using Xamarin.Forms.Labs.Data;
  using Xamarin.Forms.Labs.Mvvm.Services;
  using Xamarin.Forms.Labs.Mvvm.Views;
  using Xamarin.Forms.Labs.Services;

  #endregion

  /// <summary>
  /// View model base class.
  /// </summary>
  /// <example>
  /// To implement observable property:
  /// private object propertyBackField;
  /// public object Property
  /// {
  ///   get { return this.propertyBackField; }
  /// set
  /// {
  ///   SetProperty(ref this.propertyBackField, value);
  /// }
  /// </example>
  public abstract class ViewModel : ObservableObject,
                                    IViewModel
  {
    private static INavigationService _navigationService;
    private static IAlertable _alertService;
    private bool _isBusy;

    /// <summary>
    /// Gets or sets a value indicating whether this instance is busy.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is busy; otherwise, <c>false</c>.
    /// </value>
    public bool IsBusy
    {
      get
      {
        return _isBusy;
      }
      set
      {
        SetProperty(ref _isBusy, value);
      }
    }

    #region Navigation event handlers

    /// <inheritdoc />
    public virtual void OnAppearing(INavigable view)
    {}

    /// <inheritdoc />
    public virtual bool OnDisappearing(INavigable view)
    {
      return true;
    }

    /// <inheritdoc />
    public async Task DisplayAlert(string title, string message, string buttonText = "Ok")
    {
      if (AlertService != null) await AlertService.DisplayAlert(title, message, buttonText);
    }

    #endregion

    /// <inheritdoc />
    public INavigationService NavigationService
    {
      get
      {
        return _navigationService ?? (_navigationService = Resolver.Resolve<INavigationService>() ?? DefaultNavigationService.CreateService());
      }
    }

    /// <inheritdoc />
    public IAlertable AlertService
    {
      get
      {
        return _alertService ?? (_alertService = Resolver.Resolve<IAlertable>());
      }
    }
  }
}