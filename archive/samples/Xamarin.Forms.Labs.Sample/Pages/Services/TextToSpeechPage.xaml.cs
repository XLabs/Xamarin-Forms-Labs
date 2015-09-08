using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Services;

namespace Xamarin.Forms.Labs.Sample
{	
	public partial class TextToSpeechPage : ContentPage
	{	
		public TextToSpeechPage ()
		{
			InitializeComponent ();
			BindingContext = ViewModelLocator.Main;
		}
	}
}

