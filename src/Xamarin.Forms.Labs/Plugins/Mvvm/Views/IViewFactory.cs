namespace Xamarin.Forms.Labs.Mvvm.Views
{
  #region Usings

  using System;

  using Xamarin.Forms.Labs.Mvvm.ViewModels;

  #endregion

  /// <summary>
  /// Interface used by view factories to manage the mapping between view models and views and creates the view on demand. 
  /// </summary>
  public interface IViewFactory
  {
    /// <summary>
    /// Gets or sets a value indicating whether to cache resolved <see cref="INavigable"/>s.
    /// </summary>
    bool EnableCache { get; set; }

    /// <summary>
    /// Registers a instance of a mapping between a <see cref="ViewModels.ViewModel"/> and a <see cref="INavigable"/> using
    /// the key in case multiple views are mapped to the same view model.
    /// </summary>
    /// <typeparam name="TView">The type of the <see cref="INavigable"/>.</typeparam>
    /// <typeparam name="TViewModel">The type of the <see cref="ViewModels.ViewModel"/>.</typeparam>
    /// <param name="key">The key to use in case multiple <see cref="INavigable"/>s are mapped to the same <see cref="ViewModels.ViewModel"/>.</param>
    /// <param name="singleInstance">Indicates that only a singleton instance of the <see cref="ViewModels.ViewModel"/> will be resolved.</param>
    void Register<TView, TViewModel>(string key = null, bool singleInstance = false)
      where TView : INavigable
      where TViewModel : class, IViewModel, new();

    /// <summary>
    /// Create the <see cref="INavigable"/> mapped to the <see cref="ViewModels.ViewModel"/> using the extra key if required.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the <see cref="ViewModels.ViewModel"/></typeparam>
    /// <param name="key">The key to use in case multiple <see cref="INavigable"/> are mapped to the same <see cref="ViewModels.ViewModel"/>.</param>
    /// <param name="initialiser">An action to execute when the <see cref="INavigable"/> is created.</param>
    /// <param name="useCache">Use the cache for this view instance. The <see cref="EnableCache"/> is a global setting that if true always uses the cache.</param>
    INavigable CreateView<TViewModel>(string key = null, Action<TViewModel, INavigable> initialiser = null, bool useCache = false)
      where TViewModel : class, IViewModel, new();
  }
}