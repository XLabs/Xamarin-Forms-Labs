namespace Xamarin.Forms.Labs.Mvvm.Services
{
  #region Usings

  using System;
  using System.Threading.Tasks;

  using Xamarin.Forms.Labs.Mvvm.ViewModels;
  using Xamarin.Forms.Labs.Mvvm.Views;
  using Xamarin.Forms.Labs.Services;

  #endregion

  /// <summary>
  /// The default <see cref="INavigationService"/> to use if none other is registered with IoC.
  /// </summary>
  /// <remarks>
  /// This can also be used during unit testing as long as the <see cref="INavigable"/> 
  /// and <see cref="IViewFactory"/> have been set to a mocking version of these interfaces.
  /// </remarks>
  public class DefaultNavigationService : INavigationService
  {
    /// <summary>
    /// Gets a reference to the singleton instance of the default <see cref="INavigationService"/>.
    /// </summary>
    public static DefaultNavigationService Instance { get; protected set; }

    /// <inheritdoc />
    public INavigable Navigable { get; set; }

    /// <inheritdoc />
    public IViewFactory ViewFactory { get; set; }

    /// <inheritdoc />
    public async Task<bool> PushAsync<TViewModel>(string key = null, Action<IViewModel, INavigable> initialiser = null, bool useCache = false)
      where TViewModel : class, IViewModel, new()
    {
      if (Instance == null || Instance.Navigable == null || ViewFactory == null) return false;

      var view = ViewFactory.CreateView<TViewModel>(key, initialiser, useCache);
      if (view == null) return false;

      await Instance.Navigable.PushAsync(view);

      return true;
    }

    /// <inheritdoc />
    public async Task<INavigable> PopAsync()
    {
      if (Instance == null || Instance.Navigable == null) return null;

      return await Instance.Navigable.PopAsync();
    }

    /// <inheritdoc />
    public async Task<bool> PushModalAsync<TViewModel>(string key = null, Action<IViewModel, INavigable> initialiser = null, bool useCache = false)
      where TViewModel : class, IViewModel, new()
    {
      if (Instance == null || Instance.Navigable == null || ViewFactory == null) return false;

      var view = ViewFactory.CreateView<TViewModel>(key, initialiser, useCache);
      if (view == null) return false;

      await Instance.Navigable.PushModalAsync(view);

      return true;
    }
    
    public async Task<bool> PushModalAsync<TViewModel>(Type wrapperType, string key = null, Action<IViewModel, INavigable> initialiser = null, bool useCache = false)
      where TViewModel : class, IViewModel, new()
    {
      if (Instance == null || Instance.Navigable == null || ViewFactory == null) return false;

      var view = ViewFactory.CreateView<TViewModel>(wrapperType, key, initialiser, useCache);
      if (view == null) return false;

      await Instance.Navigable.PushModalAsync(view);

      return true;
    }

    /// <inheritdoc />
    public async Task<INavigable> PopModalAsync()
    {
      if (Instance == null || Instance.Navigable == null) return null;

      return await Instance.Navigable.PopModalAsync();
    }

    /// <summary>
    /// Create the default <see cref="INavigationService"/>. />
    /// </summary>
    public static INavigationService CreateService()
    {
      if (Instance != null) return Instance;

      var ioc = Resolver.Resolve<IDependencyContainer>();
      if (ioc == null) return null;

      var nav = Resolver.Resolve<INavigable>();
      if (nav == null) return null;

      var factory = Resolver.Resolve<IViewFactory>() ?? new ViewFactory();

      Instance = new DefaultNavigationService
                   {
                     Navigable = nav,
                     ViewFactory = factory
                   };
      ioc.Register<INavigationService>(Instance);

      return Instance;
    }
  }
}