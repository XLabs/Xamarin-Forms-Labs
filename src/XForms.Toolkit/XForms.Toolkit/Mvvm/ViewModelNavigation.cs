using System.Threading.Tasks;
using Xamarin.Forms;

namespace XForms.Toolkit.Mvvm
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

		public Task PushAsync<TViewModel> ()
			where TViewModel : ViewModel
		{
			return PushAsync (ViewFactory.CreatePage<TViewModel> ());
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


		public Task PushModalAsync<TViewModel> ()
			where TViewModel : ViewModel
		{
			return PushModalAsync (ViewFactory.CreatePage<TViewModel> ());
		}

		public Task PopModalAsync ()
		{
			return implementor.PopModalAsync ();
		}
	}
}