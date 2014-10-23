using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.Mvvm;
using Xamarin.Forms.Labs.Sample.Pages.Controls;
using Xamarin.Forms.Labs.Sample.ViewModel;
using Xamarin.Forms.Labs.Services;

namespace Xamarin.Forms.Labs.Sample
{
    /// <summary>
    /// Class App.
    /// </summary>
    public class App
    {
        /// <summary>
        /// Initializes the application.
        /// </summary>
        public static void Init ()
        {

            var app = Resolver.Resolve<IXFormsApp> ();
            if (app == null) {
                return;
            }
            
            app.Closing += (o, e) => Debug.WriteLine ("Application Closing");
            app.Error += (o, e) => Debug.WriteLine ("Application Error");
            app.Initialize += (o, e) => Debug.WriteLine ("Application Initialized");
            app.Resumed += (o, e) => Debug.WriteLine ("Application Resumed");
            app.Rotation += (o, e) => Debug.WriteLine ("Application Rotated");
            app.Startup += (o, e) => Debug.WriteLine ("Application Startup");
            app.Suspended += (o, e) => Debug.WriteLine ("Application Suspended");
        }

        /// <summary>
        /// Gets the main page.
        /// </summary>
        /// <returns>The Main Page.</returns>
        public static Page GetMainPage ()
        {
            // Register our views with our view models
            ViewFactory.Register<MvvmSamplePage, MvvmSampleViewModel> ();
            ViewFactory.Register<NewPageView, NewPageViewModel> ();
            ViewFactory.Register<GeolocatorPage, GeolocatorViewModel> ();
            ViewFactory.Register<CameraPage, CameraViewModel> ();
            ViewFactory.Register<CacheServicePage, CacheServiceViewModel> ();
            ViewFactory.Register<SoundPage, SoundServiceViewModel> ();
            ViewFactory.Register<RepeaterViewPage, RepeaterViewViewModel>();
            ViewFactory.Register<RepeaterViewXAMLPage, RepeaterViewXAMLViewModel>();

            var mainTab = new ExtendedTabbedPage () { Title = "Xamarin Forms Labs" };
            var mainPage = new NavigationPage (mainTab);
            mainTab.CurrentPageChanged += () => Debug.WriteLine ("ExtendedTabbedPage CurrentPageChanged {0}", mainTab.CurrentPage.Title);

            var controls = GetControlsPage (mainPage);
            var services = GetServicesPage (mainPage);
            var mvvm = ViewFactory.CreatePage<MvvmSampleViewModel> ();
            mainTab.Children.Add (controls);
            mainTab.Children.Add (services);
            mainTab.Children.Add (mvvm);

            return mainPage;
        }

        /// <summary>
        /// Gets the services page.
        /// </summary>
        /// <param name="mainPage">The main page.</param>
        /// <returns>Content Page.</returns>
        private static ContentPage GetServicesPage (VisualElement mainPage)
        {
            var services = new ContentPage { Title = "Services" };
            var lstServices = new ListView {
                ItemsSource = new List<string> () {
                    "TextToSpeech",
                    "DeviceExtended",
                    "PhoneService",
                    "GeoLocator",
                    "Camera",
                    "Accelerometer",
                    "Display",
                    "Cache",
                    "Sound"
                }
            };

            lstServices.ItemSelected += async (sender, e) => {
                switch (e.SelectedItem.ToString ().ToLower ()) {
                case "texttospeech":
                    await mainPage.Navigation.PushAsync (new TextToSpeechPage ());
                    break;
                case "deviceextended":
                    await mainPage.Navigation.PushAsync (new ExtendedDeviceInfoPage (Resolver.Resolve<IDevice> ()));
                    break;
                case "phoneservice":
                    await mainPage.Navigation.PushAsync (new PhoneServicePage ());
                    break;
                case "geolocator":
                    await mainPage.Navigation.PushAsync (ViewFactory.CreatePage<GeolocatorViewModel> ());
                    break;
                case "camera":
                    await mainPage.Navigation.PushAsync (ViewFactory.CreatePage<CameraViewModel> ());
                    break;
                case "accelerometer":
                    await mainPage.Navigation.PushAsync (new AcceleratorSensorPage ());
                    break;
                case "display":
                    await mainPage.Navigation.PushAsync (new AbsoluteLayoutWithDisplayInfoPage (Resolver.Resolve<IDisplay> ()));
                    break;
                case "cache":
                    await mainPage.Navigation.PushAsync (ViewFactory.CreatePage<CacheServiceViewModel> ());
                    break;
                case "sound":
                    await mainPage.Navigation.PushAsync (ViewFactory.CreatePage<SoundServiceViewModel> ());
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
        /// <returns>Content Page.</returns>
        private static ContentPage GetControlsPage (VisualElement mainPage)
        {
            var controls = new ContentPage { Title = "Controls" };
            var lstControls = new ListView {
                ItemsSource = new List<string> () {
                    "Calendar",
                    "Autocomplete",
                    "Buttons",
                    "Labels",
                    "Cells",
                    "HybridWebView",
                    "WebImage",
                    "DynamicListView",
                    "GridView",
                    "ExtendedScrollView",
                    "RepeaterView",
                    "RepeaterViewXAML"
                }
            };
            lstControls.ItemSelected += async (sender, e) => {
                switch (e.SelectedItem.ToString ().ToLower ()) {
                case "calendar":
                    await mainPage.Navigation.PushAsync (new CalendarPage ());
                    break;
                case "autocomplete":
                    await mainPage.Navigation.PushAsync (new AutoCompletePage ());
                    break;
                case "buttons":
                    await mainPage.Navigation.PushAsync (new ButtonPage ());
                    break;
                case "labels":
                    await mainPage.Navigation.PushAsync (new ExtendedLabelPage ());
                    break;
                case "cells":
                    await mainPage.Navigation.PushAsync (new ExtendedCellPage ());
                    break;
                case "hybridwebview":
                    await mainPage.Navigation.PushAsync (new CanvasWebHybrid ());
                    break;
                case "webimage":
                    await mainPage.Navigation.PushAsync (new WebImagePage ());
                    break;
                case "dynamiclistview":
                    await mainPage.Navigation.PushAsync (new Xamarin.Forms.Labs.Sample.Pages.Controls.DynamicList.DynamicListView ());
                    break;
                case "gridview":
                    await mainPage.Navigation.PushAsync (new Xamarin.Forms.Labs.Sample.Pages.Controls.GridViewPage ());
                    break;
                case "extendedscrollview":
                    await mainPage.Navigation.PushAsync (new Xamarin.Forms.Labs.Sample.Pages.Controls.ExtendedScrollView ());
                    break;
                case "repeaterview":
                    await mainPage.Navigation.PushAsync (new Xamarin.Forms.Labs.Sample.Pages.Controls.RepeaterViewPage ());
                    break;
                case "repeaterviewxaml":
                    await mainPage.Navigation.PushAsync(new Xamarin.Forms.Labs.Sample.Pages.Controls.RepeaterViewXAMLPage());
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

