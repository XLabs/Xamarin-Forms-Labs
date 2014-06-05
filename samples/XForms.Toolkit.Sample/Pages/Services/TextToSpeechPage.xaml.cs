using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XForms.Toolkit.Services;

namespace XForms.Toolkit.Sample
{	
	public partial class TextToSpeechPage : ContentPage
	{	
		public TextToSpeechPage ()
		{
			InitializeComponent ();
			btnSpeak.Clicked += (s,e) =>{

				DependencyService.Get<ITextToSpeechService>().Speak("Hello from XForms Toolkit");
			};
		}
	}
}

