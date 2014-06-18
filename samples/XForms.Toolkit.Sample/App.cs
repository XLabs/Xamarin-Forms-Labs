// ***********************************************************************
// Assembly         : XForms.Toolkit.Sample
// Author           : 
// Created          : 
//
// Last Modified By : Shawn Anderson
// Last Modified On : 06-16-2014
// ***********************************************************************
// <copyright file="App.cs" company="">
//     Copyright (c) 2014 . All rights reserved.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Xamarin.Forms;
using XForms.Toolkit.Controls;
using XForms.Toolkit.Sample.Pages.Controls;
using XForms.Toolkit.Mvvm;
using System.Diagnostics;
using XForms.Toolkit.Services;
using System.Collections.Generic;

namespace XForms.Toolkit.Sample
{
	/// <summary>
	/// Class App.
	/// </summary>
	public class App
	{
		public static void Init()
		{
			var app = Resolver.Resolve<IXFormsApp>();
			if (app == null) return;

			app.Closing += (o, e) =>
			{
				System.Diagnostics.Debug.WriteLine("Application Closing");
			};
			app.Error += (o, e) =>
			{
				System.Diagnostics.Debug.WriteLine("Application Error");
			};
			app.Initialize += (o, e) =>
			{
				System.Diagnostics.Debug.WriteLine("Application Initialized");
			};
			app.Resumed += (o, e) =>
			{
				System.Diagnostics.Debug.WriteLine("Application Resumed");
			};
			app.Rotation += (o, e) =>
			{
				System.Diagnostics.Debug.WriteLine("Application Rotated");
			};
			app.Startup += (o, e) =>
			{
				System.Diagnostics.Debug.WriteLine("Application Startup");
			};
			app.Suspended += (o, e) =>
			{
				System.Diagnostics.Debug.WriteLine("Application Suspended");
			};
		}

		/// <summary>
		/// Gets the main page.
		/// </summary>
		/// <returns>Page.</returns>
		public static Page GetMainPage()
		{
			//Register our views with our view models
			ViewFactory.Register<MvvmSamplePage, MvvmSampleViewModel>();
			ViewFactory.Register<NewPageView, NewPageViewModel>();
			ViewFactory.Register<GeolocatorPage, GeolocatorViewModel>();
			ViewFactory.Register<CameraPage, CameraViewModel>();

			var mainTab = new ExtendedTabbedPage() { Title = "XForms Toolkit Samples" };
			var mainPage = new NavigationPage(mainTab);
			mainTab.CurrentPageChanged += () =>
			{
				Debug.WriteLine(string.Format("ExtendedTabbedPage CurrentPageChanged {0}", mainTab.CurrentPage.Title));
			};

			var controls = GetControlsPage(mainPage);
			var services = GetServicesPage(mainPage);
			var mvvm = ViewFactory.CreatePage(typeof(MvvmSampleViewModel));
			mainTab.Children.Add(controls);
			mainTab.Children.Add(services);
			mainTab.Children.Add(mvvm);

			return mainPage;
		}

		/// <summary>
		/// Gets the services page.
		/// </summary>
		/// <param name="mainPage">The main page.</param>
		/// <returns>ContentPage.</returns>
		static ContentPage GetServicesPage(NavigationPage mainPage)
		{
            var services = new ContentPage { Title = "Services" };
			var lstServices = new ListView
			{
				ItemsSource = new List<string>()
				{
					"TextToSpeech",
					"DeviceExtended",
					"PhoneService",
					"GeoLocator",
					"Camera",
					"Accelerometer",
                    "Display"
				}
			};

			lstServices.ItemSelected += (sender, e) =>
			{
				switch (e.SelectedItem.ToString().ToLower())
				{
					case "texttospeech":
						mainPage.Navigation.PushAsync(new TextToSpeechPage());
						break;
					case "deviceextended":
						mainPage.Navigation.PushAsync(new ExtendedDeviceInfoPage(Resolver.Resolve<IDevice>()));
						break;
					case "phoneservice":
						mainPage.Navigation.PushAsync(new PhoneServicePage());
						break;
					case "geolocator":
						mainPage.Navigation.PushAsync(ViewFactory.CreatePage(typeof(GeolocatorViewModel)));
						break;
					case "camera":
						mainPage.Navigation.PushAsync(ViewFactory.CreatePage(typeof(CameraViewModel)));
						break;
					case "accelerometer":
						mainPage.Navigation.PushAsync(new AcceleratorSensorPage());
						break;
                    case "display":
                        mainPage.Navigation.PushAsync(new AbsoluteLayoutWithDisplayInfoPage(Resolver.Resolve<IDisplay>()));
                        break;
					default:
						break;
				}
			};
			services.Content = lstServices;
			return services;
		}

		/// <summary>
		/// Gets the controls page.
		/// </summary>
		/// <param name="mainPage">The main page.</param>
		/// <returns>ContentPage.</returns>
		static ContentPage GetControlsPage(NavigationPage mainPage)
		{
			ContentPage controls = new ContentPage();
			controls.Title = "Controls";
			var lstControls = new ListView
			{
				ItemsSource = new List<string>()
				{
					"Calendar",
					"Autocomplete",
					"Buttons",
					"Labels",
					"HybridWebView"
				}
			};
			lstControls.ItemSelected += (sender, e) =>
			{
				switch (e.SelectedItem.ToString().ToLower())
				{
					case "calendar":
						mainPage.Navigation.PushAsync(new CalendarPage());
						break;
					case "autocomplete":
						Device.OnPlatform(() => mainPage.Navigation.PushAsync(new AutoCompletePage()),
							null, () => mainPage.Navigation.PushAsync(new AutoCompletePage()));
						break;
					case "buttons":
						mainPage.Navigation.PushAsync(new ButtonPage());
						break;
					case "labels":
						mainPage.Navigation.PushAsync(new ExtendedLabelPage());
						break;
					case "hybridwebview":
						mainPage.Navigation.PushAsync(new CanvasWebHybrid());
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

