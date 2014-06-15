using System;
using Xamarin.Forms;
using XForms.Toolkit.Controls;
using XForms.Toolkit.Sample.Pages.Controls;
using XForms.Toolkit.Mvvm;
using System.Diagnostics;
using XForms.Toolkit.Services.Serialization;
using XForms.Toolkit.Services;
using System.Collections.Generic;

namespace XForms.Toolkit.Sample
{
	public class App
	{
		public static Page GetMainPage ()
		{	
			//Register our views with our view models
			ViewFactory.Register<MvvmSamplePage,MvvmSampleViewModel> ();
			ViewFactory.Register<NewPageView,NewPageViewModel> ();
			ViewFactory.Register<GeolocatorPage,GeolocatorViewModel> ();

			var mainTab = new ExtendedTabbedPage () { Title="XForms Toolkit Samples" };
			var mainPage = new NavigationPage (mainTab);
			mainTab.CurrentPageChanged += () => {
				Debug.WriteLine (string.Format ("ExtendedTabbedPage CurrentPageChanged {0}", mainTab.CurrentPage.Title));
			};

			var controls = GetControlsPage (mainPage);
			var services = GetServicesPage (mainPage);
			var mvvm =  ViewFactory.CreatePage(typeof(MvvmSampleViewModel));
			mainTab.Children.Add (controls);
			mainTab.Children.Add (services);
			mainTab.Children.Add (mvvm);

			return mainPage;
		}

		static ContentPage GetServicesPage (NavigationPage mainPage)
		{
			var services = new ContentPage ();
			services.Title = "Services";
			var lstServices = new ListView ();
			lstServices.ItemsSource = new List<string> () {
				"TextToSpeech",
				"DeviceExtended",
				"PhoneService",
				"GeoLocator"
			};
			lstServices.ItemSelected += (sender, e) =>  {
				switch (e.SelectedItem.ToString ().ToLower ()) {
				case "texttospeech":
					mainPage.Navigation.PushAsync (new TextToSpeechPage ());
					break;
				case "deviceextended":
					mainPage.Navigation.PushAsync (new ExtendedDeviceInfoPage (Resolver.Resolve<IDevice> ()));
					break;
				case "phoneservice":
					mainPage.Navigation.PushAsync (new PhoneServicePage ());
					break;
				case "geolocator":
					mainPage.Navigation.PushAsync (ViewFactory.CreatePage(typeof(GeolocatorViewModel)));
					break;
				default:
					break;
				}
			};
			services.Content = lstServices;
			return services;
		}

		static ContentPage GetControlsPage (NavigationPage mainPage)
		{
			var controls = new ContentPage ();
			controls.Title = "Controls";
			var lstControls = new ListView ();
			lstControls.ItemsSource = new List<string> () {
				"Calendar",
				"Autocomplete",
				"Buttons",
				"Labels",
				"HybridWebView"
			};
			lstControls.ItemSelected += (sender, e) => {
				switch (e.SelectedItem.ToString ().ToLower ()) {
				case "calendar":
					mainPage.Navigation.PushAsync (new CalendarPage ());
					break;
				case "autocomplete":
					Device.OnPlatform (() => mainPage.Navigation.PushAsync (new AutoCompletePage ()),
                        null, () => mainPage.Navigation.PushAsync(new AutoCompletePage()));
					break;
				case "buttons":
					mainPage.Navigation.PushAsync (new ButtonPage ());
					break;
				case "labels":
					mainPage.Navigation.PushAsync (new ExtendedLabelPage ());
					break;
				case "hybridwebview":
                    mainPage.Navigation.PushAsync(new CanvasWebHybrid());
//					mainPage.Navigation.PushAsync (new ContentPage () {
//						
//						Content = new HybridWebView (Resolver.Resolve<IJsonSerializer>()) {
//							Uri = new Uri ("https://github.com/XForms/XForms-Toolkit"), 
//
//							HorizontalOptions = LayoutOptions.FillAndExpand,
//							VerticalOptions = LayoutOptions.FillAndExpand
//						}
//					});
					break;
				default:
					break;
				}
			};
			controls.Content = lstControls;
			return controls;
		}
	}
}

