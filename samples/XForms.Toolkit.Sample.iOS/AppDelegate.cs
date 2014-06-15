using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;
using XForms.Toolkit.iOS.Controls.Calendar;
using XForms.Toolkit.Services;
using XForms.Toolkit.Services.Serialization;

namespace XForms.Toolkit.Sample.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;

		//
		// This method is invoked when the application has loaded and is ready to run. In this
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
            SetIoc();

			new CalendarViewRenderer (); //added so the assembly is included

			Forms.Init();

			window = new UIWindow(UIScreen.MainScreen.Bounds);

			window.RootViewController = App.GetMainPage().CreateViewController();

			window.MakeKeyAndVisible();

			return true;

		}

        private static void SetIoc()
        {
            var resolverContainer = new SimpleContainer();

            resolverContainer.Register<IDevice>(t => AppleDevice.CurrentDevice)
                .Register<IJsonSerializer, Services.Serialization.ServiceStackV3.JsonSerializer>();

            Resolver.SetResolver(resolverContainer.GetResolver());
        }
	}
}

