namespace Xamarin.Forms.Labs.Mvvm.ViewModels
{
  #region Usings

  using System.Threading.Tasks;

  using Xamarin.Forms.Labs.Mvvm.Services;
  using Xamarin.Forms.Labs.Mvvm.Views;

  #endregion

  /// <summary>
  /// The interface all view models need to implement.
  /// </summary>
  public interface IViewModel : IAlertable
  {
    /// <summary>
    /// Gets a reference to the <see cref="INavigationService"/>. 
    /// </summary>
    INavigationService NavigationService { get; }

    /// <summary>
    /// Gets a reference to the <see cref="IAlertable"/>. 
    /// </summary>
    IAlertable AlertService { get; }

    /// <summary>
    /// Called when the view associated with the view model is about to become visible.
    /// </summary>
    void OnAppearing(INavigable nav);

    /// <summary>
    /// Called when the view associated with the view model is (about to be?) hidden/closed.
    /// </summary>
    /// <remarks>
    /// Added a return type of bool in the hope some day that Xamarin.Forms introduce a cancelable navigate from.
    /// </remarks>
    bool OnDisappearing(INavigable nav);
  }
}