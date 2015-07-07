using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XLabs.Forms.Services;
using XLabs.Ioc;

namespace XLabs.Forms.Mvvm
{
	using XLabs.Platform.Services;

	/// <summary>
	/// Class ViewTypeAttribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class ViewTypeAttribute : Attribute
	{
		/// <summary>
		/// Gets the type of the view.
		/// </summary>
		/// <value>The type of the view.</value>
		public Type ViewType { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ViewTypeAttribute"/> class.
		/// </summary>
		/// <param name="viewType">Type of the view.</param>
		public ViewTypeAttribute(Type viewType)
		{
			ViewType = viewType;
		}
	}

	/// <summary>
	/// Class ViewFactory.
	/// </summary>
	public static class ViewFactory
	{
		/// <summary>
		/// The type dictionary.
		/// </summary>
		private static readonly Dictionary<Type, Type> TypeDictionary = new Dictionary<Type, Type>();

		/// <summary>
		/// The page cache.
		/// </summary>
		private static readonly Dictionary<string, Tuple<IViewModel, object>> PageCache =
			new Dictionary<string, Tuple<IViewModel, object>>();

		/// <summary>
		/// Gets or sets a value indicating whether [enable cache].
		/// </summary>
		/// <value><c>true</c> if [enable cache]; otherwise, <c>false</c>.</value>
		public static bool EnableCache { get; set; }

		/// <summary>
		/// Registers this instance.
		/// </summary>
		/// <typeparam name="TView">The type of the t view.</typeparam>
		/// <typeparam name="TViewModel">The type of the t view model.</typeparam>
		/// <param name="func">Function which returns an instance of the t view model.</param>
		public static void Register<TView, TViewModel>(Func<IResolver, TViewModel> func = null)
			where TView : class
			where TViewModel : class, IViewModel
		{
			TypeDictionary[typeof(TViewModel)] = typeof(TView);

			var container = Resolver.Resolve<IDependencyContainer>();

			// check if we have DI container
			if (container != null)
			{
				// register viewmodel with DI to enable non default vm constructors / service locator
				if (func == null)
					container.Register<TViewModel, TViewModel>();
				else 
					container.Register(func);
			}
		}

		/// <summary>
		/// Creates the page.
		/// </summary>
		/// <param name="viewModelType">Type of the view model.</param>
		/// <param name="initialiser">The initialiser.</param>
		/// <returns>System.Object.</returns>
		/// <exception cref="System.InvalidOperationException">Unknown View for ViewModel</exception>
		public static object CreatePage(Type viewModelType, Action<object, object> initialiser = null)
		{
			Type viewType;

			if (TypeDictionary.ContainsKey(viewModelType))
			{
				viewType = TypeDictionary[viewModelType];
			}
			else
			{
				throw new InvalidOperationException("Unknown View for ViewModel");
			}

			object page;
			IViewModel viewModel;
			var pageCacheKey = string.Format("{0}:{1}", viewModelType.Name, viewType.Name);

			if (EnableCache && PageCache.ContainsKey(pageCacheKey))
			{
				var cache = PageCache[pageCacheKey];
				viewModel = cache.Item1;
				page = cache.Item2;
			}
			else
			{
				viewModel = (Resolver.Resolve(viewModelType) ?? Activator.CreateInstance(viewModelType)) as IViewModel;

				page = Activator.CreateInstance(viewType);

				if (EnableCache)
				{
					PageCache[pageCacheKey] = new Tuple<IViewModel, object>(viewModel, page);
				}
			}

			viewModel.NavigationService = Resolver.Resolve<INavigationService>();
		
			if (initialiser != null)
			{
				initialiser(viewModel, page);
			}

			var pageBindable = page as BindableObject;
			if (pageBindable != null)
			{
				// forcing break reference on viewmodel in order to allow initializer to do its work
				pageBindable.BindingContext = null;
				pageBindable.BindingContext = viewModel;
			}
			var formsPage = page as Page;
			if (formsPage != null) {
				viewModel.Navigation = new ViewModelNavigation(formsPage.Navigation);
			}


			return page;
		}

        /// <summary>
        /// Creates the page.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <typeparam name="TPage">The type of the t page.</typeparam>
        /// <param name="initialiser">The create action.</param>
        /// <returns>Page for the ViewModel.</returns>
        /// <exception cref="System.InvalidOperationException">Unknown View for ViewModel.</exception>
        public static object CreatePage<TViewModel>(Action<TViewModel, object> initialiser = null)
            where TViewModel : class, IViewModel
            {
            Action<object, object> i = (o1, o2) =>
            {
                if (initialiser != null)
                    {
                    initialiser((TViewModel)o1, o2);
                    }
            };

            return CreatePage(typeof(TViewModel), i);
            }

	    /// <summary>
	    /// Creates the page.
	    /// </summary>
	    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
	    /// <typeparam name="TPage">The type of the t page.</typeparam>
	    /// <param name="initialiser">The create action.</param>
	    /// <returns>Page for the ViewModel.</returns>
	    /// <exception cref="System.InvalidOperationException">Unknown View for ViewModel.</exception>
	    public static TPage CreatePage<TViewModel, TPage>(Action<TViewModel, TPage> initialiser = null)
	        where TViewModel : class, IViewModel where TPage : Page
		{
			Action<object, object> i = (o1, o2) =>
			{
				if (initialiser != null)
				{
					initialiser((TViewModel) o1, (TPage) o2);
				}
			};

			return (TPage) CreatePage(typeof (TViewModel), i);
		}
	}
}
