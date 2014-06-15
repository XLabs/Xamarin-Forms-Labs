using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using XForms.Toolkit.Services;
using XForms.Toolkit.Services.Serialization;


namespace XForms.Toolkit.Sample.Droid
{
	[Activity (Label = "XForms.Toolkit.Sample.Droid", MainLauncher = true)]
	public class MainActivity : AndroidActivity
	{
        private static bool initialized;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate(bundle);

            if (!initialized)
            {
                SetIoc();
            }

			Xamarin.Forms.Forms.Init(this, bundle);
		
			SetPage(App.GetMainPage());
		}

        private static void SetIoc()
        {
            var resolverContainer = new SimpleContainer();

            resolverContainer.Register<IDevice>(t => AndroidDevice.CurrentDevice)
                .Register<IJsonSerializer, Services.Serialization.ServiceStackV3.JsonSerializer>();

            Resolver.SetResolver(resolverContainer.GetResolver());

            initialized = true;
        }
	}
}


