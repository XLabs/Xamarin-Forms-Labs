using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.Mvvm;
using Xamarin.Forms.Labs.Sample.Pages.Controls;
using Xamarin.Forms.Labs.Sample.Pages.Controls.Charts;
using Xamarin.Forms.Labs.Sample.Pages.Services;
using Xamarin.Forms.Labs.Services;

namespace Xamarin.Forms.Labs.Sample
{

  using Xamarin.Forms.Labs.Mvvm.Views;
  using Xamarin.Forms.Labs.Sample.ViewModel;

  using ViewFactory = Xamarin.Forms.Labs.Mvvm.Views.ViewFactory;

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
      ViewFactory.Instance.Register<MvvmSamplePage, MvvmSampleViewModel>();
      ViewFactory.Instance.Register<NewPageView, NewPageViewModel>(null, true); // create a singleton instance
      ViewFactory.Instance.Register<KeyedNewPageView, NewPageViewModel>("key", true); // create a singleton instance
      ViewFactory.Instance.Register<GeolocatorPage, GeolocatorViewModel>();
      ViewFactory.Instance.Register<CameraPage, CameraViewModel>();
      ViewFactory.Instance.Register<CacheServicePage, CacheServiceViewModel>();
      ViewFactory.Instance.Register<SoundPage, SoundServiceViewModel>();
      ViewFactory.Instance.Register<RepeaterViewPage, RepeaterViewViewModel>();
      ViewFactory.Instance.Register<WaveRecorderPage, WaveRecorderViewModel>();

      // because we cant add a reference to Mvvm to the Labs project this has to be a BaseTabbedView
      var mainTab = new BaseTabbedView()
      {
        Title = "Xamarin Forms Labs",
        //SwipeEnabled = true,
        //TintColor = Color.White,
        //BarTintColor = Color.Blue,
        //Badges = { "1", "2", "3" },
        //TabBarBackgroundImage = "ToolbarGradient2.png",
        //TabBarSelectedImage = "blackbackground.png",
      };

      var mainPage = new BaseNavigationView(mainTab);
      var container = Resolver.Resolve<IDependencyContainer>();
      container.Register<INavigable>(mainPage);
    //  mainTab.CurrentPageChanged += () => Debug.WriteLine("ExtendedTabbedPage CurrentPageChanged {0}", mainTab.CurrentPage.Title);
      container.Register<IAlertable>(mainPage);

      var controls = GetControlsPage(mainPage);
      var services = GetServicesPage(mainPage);
      var charts = GetChartingPage(mainPage);

      var mvvm = ViewFactory.Instance.CreateView<MvvmSampleViewModel>();

      mainTab.Children.Add(controls);
      mainTab.Children.Add(services);
      mainTab.Children.Add(charts);
      mainTab.Children.Add(mvvm as Page);

      return mainPage;
    }

    /// <summary>
    /// Gets the services page.
    /// </summary>
    /// <param name="mainPage">The main page.</param>
    /// <returns>Content Page.</returns>
    private static BaseView GetServicesPage(INavigable mainPage)
    {
      var services = new BaseView
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
                     "Bluetooth",
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
            await mainPage.PushAsync(new TextToSpeechPage());
            break;
          case "deviceextended":
            await mainPage.PushAsync(new ExtendedDeviceInfoPage(Resolver.Resolve<IDevice>()));
            break;
          case "phoneservice":
            await mainPage.PushAsync(new PhoneServicePage());
            break;
          case "geolocator":
            await mainPage.PushAsync(ViewFactory.Instance.CreateView<GeolocatorViewModel>());
            break;
          case "camera":
            await mainPage.PushAsync(ViewFactory.Instance.CreateView<CameraViewModel>());
            break;
          case "accelerometer":
            await mainPage.PushAsync(new AcceleratorSensorPage());
            break;
          case "display":
            await mainPage.PushAsync(new AbsoluteLayoutWithDisplayInfoPage(Resolver.Resolve<IDisplay>()));
            break;
          case "cache":
            await mainPage.PushAsync(ViewFactory.Instance.CreateView<CacheServiceViewModel>());
            break;
          case "sound":
            await mainPage.PushAsync(ViewFactory.Instance.CreateView<SoundServiceViewModel>());
            break;
          case "bluetooth":
            await mainPage.PushAsync(new BluetoothPage());
            break;
          case "fontmanager":
            await mainPage.PushAsync(new FontManagerPage(Resolver.Resolve<IDisplay>()));
            break;
          case "nfc":
            await mainPage.PushAsync(new NfcDevicePage());
            break;
          case "waverecorder":
            await mainPage.PushAsync(ViewFactory.Instance.CreateView<WaveRecorderViewModel>());
            break;
          case "email":
            await mainPage.PushAsync(new EmailPage());
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
    private static BaseView GetControlsPage(INavigable mainPage)
    {
      var controls = new BaseView
            {
              Title = "Controls",
              Icon = Device.OnPlatform("settings20_32.png", "settings20.png", "Images/settings20.png"),
            };

      var lstControls = new ListView
      {
        ItemsSource = new List<string>
                 {
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
                     "CheckBox",
                     "ImageGallery",
                     "CameraView",
                     "Slider",
                     "Segment",
                     "Popup",
                     "Entries"
                 }
      };

      lstControls.ItemSelected += async (sender, e) =>
      {
        switch (e.SelectedItem.ToString().ToLower())
        {
          case "calendar":
            await mainPage.PushAsync(new CalendarPage());
            break;
          case "autocomplete":
            await mainPage.PushAsync(new AutoCompletePage());
            break;
          case "buttons":
            await mainPage.PushAsync(new ButtonPage());
            break;
          case "labels":
            await mainPage.PushAsync(new ExtendedLabelPage());
            break;
          case "cells":
            await mainPage.PushAsync(new ExtendedCellPage());
            break;
          case "hybridwebview":
            await mainPage.PushAsync(new CanvasWebHybrid());
            break;
          case "webimage":
            await mainPage.PushAsync(new WebImagePage());
            break;
          case "dynamiclistview":
            await mainPage.PushAsync(new Xamarin.Forms.Labs.Sample.Pages.Controls.DynamicList.DynamicListView());
            break;
          case "gridview":
            await mainPage.PushAsync(new GridViewPage());
            break;
          case "extendedscrollview":
            await mainPage.PushAsync(new Pages.Controls.ExtendedScrollView());
            break;
          case "repeaterview":
            await mainPage.PushAsync(new RepeaterViewPage());
            break;
          case "checkbox":
            await mainPage.PushAsync(new CheckBoxPage());
            break;
          case "imagegallery":
            await mainPage.PushAsync(new Pages.Controls.ImageGallery());
            break;
          case "cameraview":
            await mainPage.PushAsync(new CameraViewPage());
            break;
          case "slider":
            await mainPage.PushAsync(new ExtendedSliderPage());
            break;
          case "segment":
            await mainPage.PushAsync(new SegmentPage());
            break;
          case "popup":
            await mainPage.PushAsync(new PopupPage());
            break;
          case "entries":
            await mainPage.PushAsync(new ExtendedEntryPage());
            break;
          default:
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
    private static BaseView GetChartingPage(INavigable mainPage)
    {
      var controls = new BaseView
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
            await mainPage.PushAsync(new BarChartPage());
            break;
          case "line":
            await mainPage.PushAsync(new LineChartPage());
            break;
          case "combination":
            await mainPage.PushAsync(new CombinationPage());
            break;
          case "pie":
            await mainPage.PushAsync(new PieChartPage());
            break;
          case "databound combination":
            await mainPage.PushAsync(new BoundChartPage());
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

