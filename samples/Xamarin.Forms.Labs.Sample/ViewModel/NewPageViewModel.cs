using System;
using Xamarin.Forms.Labs.Mvvm;


namespace Xamarin.Forms.Labs.Sample
{
	//[ViewType(typeof(NewPageView))] can specify this
    public class NewPageViewModel : Xamarin.Forms.Labs.Mvvm.ViewModel
	{
		public NewPageViewModel ()
		{
			NewPage =@"This page was created by the view factory and binded to the viewmodel and injected a navigation context using the following code:
						ViewFactory.Register<NewPageView,NewPageViewModel> ();
						We can also navigate to this page from any view model using the following code: 
						await Navigation.PushAsync<NewPageViewModel>() ";

		}

		private string _newPage =string.Empty;
		public string NewPage
		{
			get
			{
				return _newPage;
			}
			set
						
			{
				this.SetProperty(ref _newPage, value);
			}
		}

		private string _pageTitle ="New Page";
		public string PageTitle
		{
			get
			{
				return _pageTitle;
			}
			set
			{
				this.SetProperty(ref _pageTitle, value);
			}
		}
	}
}

