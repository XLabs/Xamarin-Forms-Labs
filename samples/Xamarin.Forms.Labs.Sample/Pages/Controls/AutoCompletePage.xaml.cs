using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Xamarin.Forms.Labs.Sample
{
  using Xamarin.Forms.Labs.Mvvm.Views;

  public partial class AutoCompletePage : BaseView
	{	
		public AutoCompletePage ()
		{
			InitializeComponent ();
			BindingContext = ViewModelLocator.Main;
		}
	}
}

