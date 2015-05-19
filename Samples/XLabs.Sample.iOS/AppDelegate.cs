namespace XLabs.Sample.iOS
{
	using System.IO;

	using Foundation;
	using UIKit;

	using Xamarin.Forms;

	using XLabs.Caching;
	using XLabs.Caching.SQLite;
	using XLabs.Forms;
	using XLabs.Forms.Controls;
	using XLabs.Ioc;
	using XLabs.Platform.Device;
	using XLabs.Platform.Mvvm;
	using XLabs.Sample;
	using XLabs.Serialization;

	/// <summary>
	/// Class AppDelegate.
	/// </summary>
	/// <remarks>
	/// The UIApplicationDelegate for the application. This class is responsible for launching the
	/// User Interface of the application, as well as listening (and optionally responding) to
	/// application events from iOS.
	/// </remarks>
	[Register("AppDelegate")]
	public partial class AppDelegate : XFormsApplicationDelegate
	{
		/// <summary>
		/// The window
		/// </summary>
		private UIWindow _window;

		/// <summary>
		/// Finished the launching.
		/// </summary>
		/// <param name="app">The application.</param>
		/// <param name="options">The options.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		/// <remarks>
		/// This method is invoked when the application has loaded and is ready to run. In this
		/// method you should instantiate the window, load the UI into it and then make the window
		/// visible.
		///
		/// You have 17 seconds to return from this method, or iOS will terminate your application.
		/// </remarks>
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			this.SetIoc();

			new CalendarViewRenderer(); //added so the assembly is included

			Forms.Init();

			var formsApp = new App();

			LoadApplication (formsApp);

//			this._window = new UIWindow(UIScreen.MainScreen.Bounds)
//			{
//				RootViewController = App.GetMainPage().CreateViewController()
//			};


			base.FinishedLaunching(app, options);

			return true;
		}

		/// <summary>
		/// Sets the IoC.
		/// </summary>
		private void SetIoc()
		{
			var resolverContainer = new SimpleContainer();

			var app = new XFormsAppiOS();
			app.Init(this);

			//var documents = app.AppDataDirectory;
			//var pathToDatabase = Path.Combine(documents, "xforms.db");

			resolverContainer.Register<IDevice> (t => AppleDevice.CurrentDevice)
				.Register<IDisplay> (t => t.Resolve<IDevice> ().Display)
				.Register<IJsonSerializer, XLabs.Serialization.ServiceStack.JsonSerializer> ()
			//.Register<IJsonSerializer, Services.Serialization.SystemJsonSerializer>()
				.Register<IXFormsApp> (app)
				.Register<IDependencyContainer> (t => resolverContainer);
				/*.Register<ISimpleCache>(
					t => new SQLiteSimpleCache(new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS(),
						new SQLite.Net.SQLiteConnectionString(pathToDatabase, true), t.Resolve<IJsonSerializer>()));*/
			
			Resolver.SetResolver(resolverContainer.GetResolver());
		}
	}
}
