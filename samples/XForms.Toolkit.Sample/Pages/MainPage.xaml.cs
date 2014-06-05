using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XForms.Toolkit.Sample.Pages.Controls;

namespace XForms.Toolkit.Sample
{	
	public partial class MainPage : TabbedPage
	{	
		public MainPage ()
		{
			InitializeComponent ();

			var controls = new CarouselPage ();
			controls.Title = "Controls";
			controls.Children.Add (new CalendarPage ());

			var services = new CarouselPage ();
			services.Title = "Services";
			services.Children.Add (new TextToSpeechPage ());

		    var buttons = new CarouselPage ();
		    buttons.Title = "Buttons";
            buttons.Children.Add (new ButtonPage ());

			this.Children.Add (controls);
			this.Children.Add (services);
		    this.Children.Add (buttons);
		}
	}
}

