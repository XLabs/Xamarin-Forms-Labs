namespace Xamarin.Forms.Labs.Mvvm.Views
{
  #region Usings

  using System;
  using System.Collections.Generic;

  using Xamarin.Forms.Labs.Mvvm.ViewModels;
  using Xamarin.Forms.Labs.Services;

  #endregion

  /// <summary>
  /// Manages the mapping between view models and views and creates the view on demand. 
  /// </summary>
  public class ViewFactory : IViewFactory
  {
    /// <summary>
    /// Gets a reference to the singleton instance of the default <see cref="IViewFactory"/>.
    /// </summary>
    public static IViewFactory Instance
    {
      get
      {
        if (_instance == null) CreateFactory();

        return _instance;
      }
      protected set
      {
        _instance = value;
      }
    }

    private static IViewFactory _instance;

    /// <summary>
    /// The dictionary mapping view models and views.
    /// </summary>
    private static readonly Dictionary<Tuple<Type, string>, Type> TypeDictionary = new Dictionary<Tuple<Type, string>, Type>();

    /// <summary>
    /// The resolved view cache.
    /// </summary>
    private static readonly Dictionary<string, Tuple<IViewModel, INavigable, INavigable>> ViewCache = new Dictionary<string, Tuple<IViewModel, INavigable, INavigable>>();

    /// <inheritdoc />
    public bool EnableCache { get; set; }

    /// <inheritdoc />
    public void Register<TView, TViewModel>(string key = null, bool singleInstance = false)
      where TView : INavigable
      where TViewModel : class, IViewModel, new()
    {
      var dictKey = new Tuple<Type, string>(typeof(TViewModel), key ?? "");
      TypeDictionary[dictKey] = typeof(TView);

      var container = Resolver.Resolve<IDependencyContainer>();

      // check if we have DI container
      if (container != null)
      {
        // register view model with DI to enable non default VM constructors or service locator.
        // either as a singleton instance or not
        if (singleInstance)
          container.RegisterSingle<TViewModel, TViewModel>();
        else
          container.Register<TViewModel, TViewModel>();
      }
    }

    /// <inheritdoc />
    public INavigable CreateView<TViewModel>(string key = null, Action<TViewModel, INavigable> initialiser = null, bool useCache = false)
      where TViewModel : class, IViewModel, new()
    {
      Type viewType;
      var dictKey = new Tuple<Type, string>(typeof(TViewModel), key ?? "");

      if (TypeDictionary.ContainsKey(dictKey))
        viewType = TypeDictionary[dictKey];
      else
        throw new InvalidOperationException(string.Format("No View registered for ViewModel({0}) and key {1}.", typeof(TViewModel).Name, key ?? "(none)"));

      INavigable view;
      INavigable wrappingView = null;
      TViewModel viewModel;
      var cacheKey = string.Format("{0}:{1}:{2}", typeof(TViewModel).Name, viewType.Name, key ?? "");

      if ((EnableCache || useCache) && ViewCache.ContainsKey(cacheKey))
      {
        var cache = ViewCache[cacheKey];
        viewModel = cache.Item1 as TViewModel;
        view = cache.Item2;
        wrappingView = cache.Item3;
      }
      else
      {
        view = Activator.CreateInstance(viewType) as INavigable;
        viewModel = Resolver.Resolve<TViewModel>() ?? Activator.CreateInstance<TViewModel>();

        if (EnableCache || useCache)
          ViewCache[cacheKey] = new Tuple<IViewModel, INavigable, INavigable>(viewModel, view, null);
      }

      if (initialiser != null) initialiser(viewModel, view);

      // forcing break reference on view model in order to allow initializer to do its work
      view.BindingContext = null;
      view.BindingContext = viewModel;

      return view;
    }

    /// <inheritdoc />
    public INavigable CreateView<TViewModel>(Type wrappingViewType, string key = null, Action<TViewModel, INavigable> initialiser = null, bool useCache = false)
      where TViewModel : class, IViewModel, new()
    {
      Type viewType;
      var dictKey = new Tuple<Type, string>(typeof(TViewModel), key ?? "");

      if (TypeDictionary.ContainsKey(dictKey))
        viewType = TypeDictionary[dictKey];
      else
        throw new InvalidOperationException(string.Format("No View registered for ViewModel({0}) and key {1}.", typeof(TViewModel).Name, key ?? "(none)"));

      INavigable wrappedView;
      INavigable wrappingView = null;
      TViewModel viewModel;
      var cacheKey = string.Format("{0}:{1}:{2}", typeof(TViewModel).Name, viewType.Name, key ?? "");

      if ((EnableCache || useCache) && ViewCache.ContainsKey(cacheKey))
      {
        var cache = ViewCache[cacheKey];
        viewModel = cache.Item1 as TViewModel;
        wrappedView = cache.Item2;
        wrappingView = cache.Item3;
      }
      else
      {
        wrappedView = Activator.CreateInstance(viewType) as INavigable;
        viewModel = Resolver.Resolve<TViewModel>() ?? Activator.CreateInstance<TViewModel>();
      }
      if (wrappingViewType != null && wrappingView == null)
      {
        wrappingView = Activator.CreateInstance(wrappingViewType, wrappedView) as INavigable;
      }

      if (EnableCache || useCache)
        ViewCache[cacheKey] = new Tuple<IViewModel, INavigable, INavigable>(viewModel, wrappedView, wrappingView);

      if (initialiser != null) initialiser(viewModel, wrappedView);

      // forcing break reference on view model in order to allow initializer to do its work
      wrappedView.BindingContext = null;
      wrappedView.BindingContext = viewModel;

      return wrappingView ?? wrappedView;
    }

    /// <summary>
    /// Create and register the default <see cref="IViewFactory"/>.
    /// </summary>
    public static IViewFactory CreateFactory()
    {
      if (_instance != null) return _instance;

      var ioc = Resolver.Resolve<IDependencyContainer>();
      if (ioc == null) return null;

      _instance = Resolver.Resolve<IViewFactory>() ?? new ViewFactory();
      ioc.Register(_instance);

      return _instance;
    }
  }
}