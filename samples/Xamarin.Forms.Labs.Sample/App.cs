using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.Mvvm;
using Xamarin.Forms.Labs.Sample.Pages.Controls;
using Xamarin.Forms.Labs.Sample.Pages.Controls.Charts;
using Xamarin.Forms.Labs.Sample.Pages.Services;
using Xamarin.Forms.Labs.Services;
using XLabs.Ioc;

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
        public static void Init()
        {

            var app = Resolver.Resolve<IXFormsApp>();
            if (app == null)
            {
                return;
            }

            app.Closing += (o, e) => Debug.WriteLine("Application Closing");
            app.Error += (o, e) => Debug.WriteLine("Application Error");
            app.Initialize += (o, e) => Debug.WriteLine("Application Initialized");
            app.Resumed += (o, e) => Debug.WriteLine("Application Resumed");
            app.Rotation += (o, e) => Debug.WriteLine("Application Rotated");
            app.Startup += (o, e) => Debug.WriteLine("Application Startup");
            app.Suspended += (o, e) => Debug.WriteLine("Application Suspended");
        }

        /// <summary>
        /// Gets the main page.
        /// </summary>
        /// <returns>The Main Page.</returns>
        public static Page GetMainPage()
        {
            // Register our views with our view models
            ViewFactory.Register<MvvmSamplePage, MvvmSampleViewModel>();
            ViewFactory.Register<NewPageView, NewPageViewModel>();
            ViewFactory.Register<GeolocatorPage, GeolocatorViewModel>();
            ViewFactory.Register<CameraPage, CameraViewModel>();
            ViewFactory.Register<CacheServicePage, CacheServiceViewModel>();
            ViewFactory.Register<SoundPage, SoundServiceViewModel>();
            ViewFactory.Register<RepeaterViewPage, RepeaterViewViewModel>();
            ViewFactory.Register<WaveRecorderPage, WaveRecorderViewModel>();

            var mainTab = new ExtendedTabbedPage()
            {
                Title = "Xamarin Forms Labs",
                SwipeEnabled = true,
                TintColor = Color.White,
                BarTintColor = Color.Blue,
                Badges = { "1", "2", "3" },
                TabBarBackgroundImage = "ToolbarGradient2.png",
                TabBarSelectedImage = "blackbackground.png",
            };

            var mainPage = new NavigationPage(mainTab);

            mainTab.CurrentPageChanged += () => Debug.WriteLine("ExtendedTabbedPage CurrentPageChanged {0}", mainTab.CurrentPage.Title);

            var controls = GetControlsPage(mainPage);
            var services = GetServicesPage(mainPage);
            var charts = GetChartingPage(mainPage);

            var mvvm = ViewFactory.CreatePage<MvvmSampleViewModel>();

            mainTab.Children.Add(controls);
            mainTab.Children.Add(services);
            mainTab.Children.Add(charts);
            mainTab.Children.Add(mvvm);

            return mainPage;
        }

        /// <summary>
        /// Gets the services page.
        /// </summary>
        /// <param name="mainPage">The main page.</param>
        /// <returns>Content Page.</returns>
        private static ContentPage GetServicesPage(VisualElement mainPage)
        {
            var services = new ContentPage
            {
                Title = "Services",
                Icon = Device.OnPlatform("services1_32.png", "services1_32.png", "Images/services1_32.png"),
            };
            var lstServices = new ListView
            {
                ItemsSource = new List<string>() {
                    "TextToSpeech",
                    "DeviceExtended",
                    "PhoneService",
                    "GeoLocator",
                    "Camera",
                    "Accelerometer",
                    "Display",
                    "Cache",
                    "Sound",
                    //"Bluetooth",
                    "FontManager",
                    "NFC",
                    //"WaveRecorder",
                    "Email"
                }
            };

            lstServices.ItemSelected += async (sender, e) =>
            {
                switch (e.SelectedItem.ToString().ToLower())
                {
                    case "texttospeech":
                        await mainPage.Navigation.PushAsync(new TextToSpeechPage());
                        break;
                    case "deviceextended":
                        await mainPage.Navigation.PushAsync(new ExtendedDeviceInfoPage(Resolver.Resolve<IDevice>()));
                        break;
                    case "phoneservice":
                        await mainPage.Navigation.PushAsync(new PhoneServicePage());
                        break;
                    case "geolocator":
                        await mainPage.Navigation.PushAsync(ViewFactory.CreatePage<GeolocatorViewModel>());
                        break;
                    case "camera":
                        await mainPage.Navigation.PushAsync(ViewFactory.CreatePage<CameraViewModel>());
                        break;
                    case "accelerometer":
                        await mainPage.Navigation.PushAsync(new AcceleratorSensorPage());
                        break;
                    case "display":
                        await mainPage.Navigation.PushAsync(new AbsoluteLayoutWithDisplayInfoPage(Resolver.Resolve<IDisplay>()));
                        break;
                    case "cache":
                        await mainPage.Navigation.PushAsync(ViewFactory.CreatePage<CacheServiceViewModel>());
                        break;
                    case "sound":
                        await mainPage.Navigation.PushAsync(ViewFactory.CreatePage<SoundServiceViewModel>());
                        break;
                    //case "bluetooth":
                    //    await mainPage.Navigation.PushAsync(new BluetoothPage());
                    //    break;
                    case "fontmanager":
                        await mainPage.Navigation.PushAsync(new FontManagerPage(Resolver.Resolve<IDisplay>()));
                        break;
                    case "nfc":
                        await mainPage.Navigation.PushAsync(new NfcDevicePage());
                        break;
                    case "waverecorder":
                        await mainPage.Navigation.PushAsync(ViewFactory.CreatePage<WaveRecorderViewModel>());
                        break;
                    case "email":
                        await mainPage.Navigation.PushAsync(new EmailPage());
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
        private static ContentPage GetControlsPage(VisualElement mainPage)
        {
            var controls = new ContentPage
            {
                Title = "Controls",
                Icon = Device.OnPlatform("settings20_32.png", "settings20.png", "Images/settings20.png"),
            };

            var lstControls = new ListView
            {
                ItemsSource = new List<string>
                {
                    "Autocomplete",
                    "Buttons",
                    "ButtonGroup",
                    "Calendar",
                    "CameraView",
                    "CheckBox",
                    "DynamicListView",
                    "ExtendedCells",
                    "ExtendedEntries",
                    "ExtendedLabels",
                    "ExtendedScrollView",
                    "ExtendedSlider",
                    "GridView",
                    "HybridWebView",
                    "ImageGallery",
                    "Popup",
                    "RepeaterView",
                    "Segment",
                    "Separator",
                    "WebImage",
                    "SvgImage",
                    "PulsatingButton",
                }
            };

            lstControls.ItemSelected += async (sender, e) =>
            {
                switch (e.SelectedItem.ToString().ToLower())
                {
                    case "autocomplete":
                        await mainPage.Navigation.PushAsync(new AutoCompletePage());
                        break;
                    case "buttons":
                        await mainPage.Navigation.PushAsync(new ButtonPage());
                        break;
                    case "calendar":
                        await mainPage.Navigation.PushAsync(new CalendarPage());
                        break;
                    case "cameraview":
                        await mainPage.Navigation.PushAsync(new CameraViewPage());
                        break;
                    case "checkbox":
                        await mainPage.Navigation.PushAsync(new CheckBoxPage());
                        break;
                    case "dynamiclistview":
                        await mainPage.Navigation.PushAsync(new Xamarin.Forms.Labs.Sample.Pages.Controls.DynamicList.DynamicListView());
                        break;
                    case "extendedcells":
                        await mainPage.Navigation.PushAsync(new ExtendedCellPage());
                        break;
                    case "extendedentries":
                        await mainPage.Navigation.PushAsync(new ExtendedEntryPage());
                        break;
                    case "extendedlabels":
                        await mainPage.Navigation.PushAsync(new ExtendedLabelPage());
                        break;
                    case "extendedscrollview":
                        await mainPage.Navigation.PushAsync(new Pages.Controls.ExtendedScrollView());
                        break;
                    case "extendedslider":
                        await mainPage.Navigation.PushAsync(new ExtendedSliderPage());
                        break;
                    case "gridview":
                        await mainPage.Navigation.PushAsync(new GridViewPage());
                        break;
                    case "hybridwebview":
                        await mainPage.Navigation.PushAsync(new CanvasWebHybrid());
                        break;
                    case "imagegallery":
                        await mainPage.Navigation.PushAsync(new Pages.Controls.ImageGallery());
                        break;
                    case "popup":
                        await mainPage.Navigation.PushAsync(new PopupPage());
                        break;
                    case "repeaterview":
                        await mainPage.Navigation.PushAsync(new RepeaterViewPage());
                        break;
                    case "segment":
                        await mainPage.Navigation.PushAsync(new SegmentPage());
                        break;
                    case "separator":
                        await mainPage.Navigation.PushAsync(new SeparatorPage());
                        break;
                    case "webimage":
                        await mainPage.Navigation.PushAsync(new WebImagePage());
                        break;
                    case "buttongroup":
                        await mainPage.Navigation.PushAsync(new ButtonGroupPage());
                        break;
                    case "svgimage":
                        await mainPage.Navigation.PushAsync(new SvgImagePage());
                        break;
                    case "pulsatingbutton":
                        await mainPage.Navigation.PushAsync(new PulsatingButtonPage());
                        break;
                }
            };
            controls.Content = lstControls;
            return controls;
        }

        /// <summary>
        /// Gets the charting page.
        /// </summary>
        /// <param name="mainPage">The main page.</param>
        /// <returns>Content Page.</returns>
        private static ContentPage GetChartingPage(VisualElement mainPage)
        {
            var controls = new ContentPage
            {
                Title = "Charts",
                Icon = Device.OnPlatform("pie30_32.png", "pie30_32.png", "Images/pie30_32.png"),
            };
            var lstControls = new ListView
            {
                ItemsSource = new List<string>() {
                    "Bar",
                    "Line",
                    "Combination",
                    "Pie",
                    "Databound combination"
                }
            };
            lstControls.ItemSelected += async (sender, e) =>
            {
                switch (e.SelectedItem.ToString().ToLower())
                {
                    case "bar":
                        await mainPage.Navigation.PushAsync(new BarChartPage());
                        break;
                    case "line":
                        await mainPage.Navigation.PushAsync(new LineChartPage());
                        break;
                    case "combination":
                        await mainPage.Navigation.PushAsync(new CombinationPage());
                        break;
                    case "pie":
                        await mainPage.Navigation.PushAsync(new PieChartPage());
                        break;
                    case "databound combination":
                        await mainPage.Navigation.PushAsync(new BoundChartPage());
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

