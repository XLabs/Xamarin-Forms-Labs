using System;
using XForms.Toolkit.Mvvm;

namespace XForms.Toolkit.Sample
{
	[ViewType(typeof(MvvmSamplePage))]
	public class MvvmSampleViewModel : ViewModel
	{
		public MvvmSampleViewModel ()
		{
		
		}


		private RelayCommand _navigateToViewModel;
		public RelayCommand NavigateToViewModel 
		{
			get
			{ 
				return _navigateToViewModel ?? new RelayCommand (
					async () => await Navigation.PushAsync<NewPageViewModel>() , 
					() => true); 
			}
		}

		private string _navigateToViewModelButtonText = "Navigate to another view model";
		public string NavigateToViewModelButtonText
		{
			get
			{
				return _navigateToViewModelButtonText;
			}
			set
			{ 
				this.ChangeAndNotify(ref _navigateToViewModelButtonText, value);
			}
		}

	}
}

