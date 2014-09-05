using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Forms.Labs.Mvvm
{
    public class ViewModelNavigation
    {
        readonly INavigation implementor;

        public ViewModelNavigation(INavigation implementor)
        {
            this.implementor = implementor;
        }

        // This method can be considered unclean in the pure MVVM sense, 
        // however it has been handy to me at times
        public Task PushAsync(Page page)
        {
            return implementor.PushAsync(page);
        }

        /// <summary>
        /// Pushes the asynchronous.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the t view model.</typeparam>
        /// <param name="activateAction">The activate action.</param>
        /// <returns>Task.</returns>
        public Task PushAsync<TViewModel>(Action<TViewModel, Page> activateAction = null, bool hasNavigationPage = false)
            where TViewModel : ViewModel
        {
            return PushAsync(ViewFactory.CreatePage<TViewModel>(activateAction, hasNavigationPage));
        }

        /// <summary>
        /// Pushes the asynchronous.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the t view model.</typeparam>
        /// <typeparam name="TView">The type of the TView.</typeparam>
        /// <param name="activateAction">The activate action.</param>
        /// <returns>Task.</returns>
        public Task PushAsync<TViewModel, TView>(Action<TViewModel, Page> activateAction = null, bool hasNavigationBar = false)
            where TViewModel : ViewModel where TView : Page
        {
            return PushAsync(ViewFactory.CreatePage<TViewModel, TView>(activateAction, hasNavigationBar));
        }

        public Task PopAsync()
        {
            return implementor.PopAsync();
        }

        public Task PopToRootAsync()
        {
            return implementor.PopToRootAsync();
        }

        // This method can be considered unclean in the pure MVVM sense, 
        // however it has been handy to me at times
        public Task PushModalAsync(Page page)
        {
            return implementor.PushModalAsync(page);
        }


        /// <summary>
        /// Pushes the modal asynchronous.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the t view model.</typeparam>
        /// <param name="activateAction">The create action.</param>
        /// <returns>Task.</returns>
        public Task PushModalAsync<TViewModel>(Action<TViewModel, Page> activateAction = null, bool hasNavigationBar = false)
            where TViewModel : ViewModel
        {
            return PushModalAsync(ViewFactory.CreatePage<TViewModel>(activateAction, hasNavigationBar));
        }

        /// <summary>
        /// Pushes the modal asynchronous. 
        /// </summary>
        /// <typeparam name="TViewModel">The type of the t view model.</typeparam>
        /// <typeparam name="TView">The type of the TView.</typeparam>
        /// <param name="activateAction">The create action.</param>
        /// <returns>Task.</returns>
        public Task PushModalAsync<TViewModel, TView>(Action<TViewModel, Page> activateAction = null, bool hasNavigationBar = false)
            where TViewModel : ViewModel where TView : Page
        {
            return PushModalAsync(ViewFactory.CreatePage<TViewModel, TView>(activateAction, hasNavigationBar));
        }


        public Task PopModalAsync()
        {
            return implementor.PopModalAsync();
        }
    }
}