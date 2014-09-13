using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Services;
using Xamarin.Forms.Labs.Mvvm.Views;

namespace Xamarin.Forms.Labs.Sample
{	
	public partial class TextToSpeechPage : BaseView
	{	
		public TextToSpeechPage ()
		{
			InitializeComponent ();
			BindingContext = ViewModelLocator.Main;
		}
	}
}

