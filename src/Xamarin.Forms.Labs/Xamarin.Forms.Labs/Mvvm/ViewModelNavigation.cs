using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Forms.Labs.Mvvm
{
	public class ViewModelNavigation
	{
		readonly INavigation implementor;

		public ViewModelNavigation (INavigation implementor)
		{
			this.implementor = implementor;
		}

		// This method can be considered unclean in the pure MVVM sense, 
		// however it has been handy to me at times
		public Task PushAsync (Page page)
		{
			return implementor.PushAsync (page);
		}

		/// <summary>
		/// Pushes the asynchronous.
		/// </summary>
		/// <typeparam name="TViewModel">The type of the t view model.</typeparam>
		/// <param name="activateAction">The activate action.</param>
		/// <returns>Task.</returns>
		public Task PushAsync<TViewModel>(Action<ViewModel, Page> activateAction = null)
			where TViewModel : ViewModel
		{
			return PushAsync(ViewFactory.CreatePage<TViewModel>(activateAction));
		}


		public Task PopAsync ()
		{
			return implementor.PopAsync ();
		}

		public Task PopToRootAsync ()
		{
			return implementor.PopToRootAsync ();
		}

		// This method can be considered unclean in the pure MVVM sense, 
		// however it has been handy to me at times
		public Task PushModalAsync (Page page)
		{
			return implementor.PushModalAsync (page);
		}


		/// <summary>
		/// Pushes the modal asynchronous.
		/// </summary>
		/// <typeparam name="TViewModel">The type of the t view model.</typeparam>
		/// <param name="activateAction">The create action.</param>
		/// <returns>Task.</returns>
		public Task PushModalAsync<TViewModel>(Action<ViewModel, Page> activateAction = null)
			where TViewModel : ViewModel
		{
			return PushModalAsync(ViewFactory.CreatePage<TViewModel>(activateAction));
		}

		public Task PopModalAsync ()
		{
			return implementor.PopModalAsync ();
		}
	}
}