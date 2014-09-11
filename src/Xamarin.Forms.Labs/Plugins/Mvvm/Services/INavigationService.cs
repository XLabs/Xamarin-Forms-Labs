namespace Xamarin.Forms.Labs.Mvvm.Services
{
  #region Usings

  using System;
  using System.Threading.Tasks;

  using Xamarin.Forms.Labs.Mvvm.ViewModels;
  using Xamarin.Forms.Labs.Mvvm.Views;

  #endregion

  /// <summary>
  /// The interface used by all navigation services to navigate to another view model (and thus view).
  /// </summary>
  public interface INavigationService
  {
    /// <summary>
    /// The reference to the <see cref="INavigable"/> that the navigation service uses to navigate to another view.
    /// </summary>
    INavigable Navigable { get; }

    /// <summary>
    /// The reference to the <see cref="IViewFactory"/> that the navigation service uses to create a view.
    /// </summary>
    IViewFactory ViewFactory { get; }

    /// <summary>
    /// Asynchronously push the view mapped to the view model.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the <see cref="ViewModel"/> to use.</typeparam>
    /// <param name="key">A extra key in case the same view model is mapped to multiple views.</param>
    /// <param name="initialiser">An <see cref="Action"/> to execute when the view is activated.</param>
    /// <param name="useCache">Use the cache for this view instance. The <see cref="IViewFactory.EnableCache"/> is a global setting that if true always uses the cache.</param>
    /// <returns>True if successful, false otherwise.</returns>
    Task<bool> PushAsync<TViewModel>(string key = null, Action<IViewModel, INavigable> initialiser = null, bool useCache = false)
      where TViewModel : class, IViewModel, new();

    /// <summary>
    /// Asynchronously pop the view that is currently on display.
    /// </summary>
    /// <returns>The view that was on display.</returns>
    Task<INavigable> PopAsync();

    /// <summary>
    /// Asynchronously push the modal view mapped to the view model.
    /// </summary>
    /// <typeparam name="TViewModel">TheMvvm.IViewModelef="ViewModel"/> to use.</typeparam>
    /// <param name="key">A extra key in case the same view model is mapped to multiple views.</param>
    /// <param name="initialiser">An <see cref="Action"/> to execute when the view is activated.</param>
    /// <param name="useCache">Use the cache for this view instance. The <see cref="IViewFactory.EnableCache"/> is a global setting that if true always uses the cache.</param>
    /// <returns>True if successful, false otherwise.</returns>
    Task<bool> PushModalAsync<TViewModel>(string key = null, Action<IViewModel, INavigable> initialiser = null, bool useCache = false)
      where TViewModel : class, IViewModel, new();

    /// <summary>
    /// Asynchronously pop the modal view that is currently on display.
    /// </summary>
    /// <returns>The view that was on display.</returns>
    Task<INavigable> PopModalAsync();
  }
}