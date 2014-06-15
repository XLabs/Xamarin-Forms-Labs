using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace XForms.Toolkit.Sample
{	
	public partial class PhoneServicePage : ContentPage
	{	
		public PhoneServicePage ()
		{
			InitializeComponent ();
			BindingContext = ViewModelLocator.Main;
		}
	}
}

