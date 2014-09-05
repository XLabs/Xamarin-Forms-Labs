using System;
using System.Collections.Generic;

namespace Xamarin.Forms.Labs.Mvvm
{
    using Services;

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
        private static readonly Dictionary<Type, IList<Type>> TypeDictionary = new Dictionary<Type, IList<Type>>();

        /// <summary>
        /// The page cache.
        /// </summary>
        private static readonly Dictionary<string, Tuple<ViewModel, Page>> PageCache =
            new Dictionary<string, Tuple<ViewModel, Page>>();

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
        public static void Register<TView, TViewModel>(Func<IResolver, TViewModel> func = null, bool singleInstance = false)
            where TView : Page
            where TViewModel : ViewModel
        {
            if (!TypeDictionary.ContainsKey(typeof(TViewModel)))
            {
                TypeDictionary[typeof(TViewModel)] = new List<Type>();
            }

            TypeDictionary[typeof(TViewModel)].Add(typeof(TView));

            var container = Resolver.Resolve<IDependencyContainer>();

            // check if we have DI container
            if (container != null)
            {
                // register viewmodel with DI to enable non default vm constructors / service locator
                if (func == null)
                {
                    if (!singleInstance)
                    {
                        container.Register<TViewModel, TViewModel>();
                    }else{
                        //register as single instance
                        container.RegisterSingle<TViewModel, TViewModel>();
                    }
                }
                else
                {
                    container.Register(func);
                }
            }
        }

        /// <summary>
        /// Creates the page.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="initialiser">
        /// The create action.
        /// </param>
        /// <returns>
        /// Page for the ViewModel.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        /// Unknown View for ViewModel.
        /// </exception>
        public static Page CreatePage<TViewModel>(Action<TViewModel, Page> initialiser = null,bool hasNavigationPage = false)
            where TViewModel : ViewModel
        {
            return InitializePage<TViewModel>(initialiser, null, hasNavigationPage);
        }

        public static Page CreatePage<TViewModel, TView>(Action<TViewModel, Page> initialiser = null, bool hasNavigationPage = false)
            where TViewModel : ViewModel 
        {
            return InitializePage<TViewModel>(initialiser, typeof(TView), hasNavigationPage);
        }

        private static Page InitializePage<TViewModel>(Action<TViewModel, Page> initialiser = null, Type viewType = null,bool hasNavigationPage = false)
            where TViewModel : ViewModel 
        {
             
            var viewModelType = typeof(TViewModel);

            if (TypeDictionary.ContainsKey(viewModelType))
            {
                //if view is null the first view is selected
                viewType = viewType ?? TypeDictionary[viewModelType][0];
            }
            else
            {
                throw new InvalidOperationException("Unknown View for ViewModel");
            }

            Page page;
            TViewModel viewModel;
            var pageCacheKey = string.Format("{0}:{1}", viewModelType.Name, viewType.Name);

            if (EnableCache && PageCache.ContainsKey(pageCacheKey))
            {
                var cache = PageCache[pageCacheKey];
                viewModel = cache.Item1 as TViewModel;
                page = cache.Item2;
            }
            else
            {
                page = (Page)Activator.CreateInstance(viewType);

                if (hasNavigationPage)
                {
                   page  = new NavigationPage(page);
                }

                viewModel = Resolver.Resolve<TViewModel>() ?? Activator.CreateInstance<TViewModel>();

                viewModel.Navigation = new ViewModelNavigation(page.Navigation);

                if (EnableCache)
                {
                    PageCache[pageCacheKey] = new Tuple<ViewModel, Page>(viewModel, page);
                }

            }

            if (initialiser != null)
            {
                initialiser(viewModel, page);
            }


            // forcing break reference on viewmodel in order to allow initializer to do its work
            page.BindingContext = null;
            page.BindingContext = viewModel;

            return page;
        }
    }
}