using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Xamarin.Forms.Labs.Sample
{

  using Xamarin.Forms.Labs.Mvvm.Views;

  public partial class PhoneServicePage : BaseView
	{	
		public PhoneServicePage ()
		{
			InitializeComponent ();
			BindingContext = ViewModelLocator.Main;
		}
	}
}

