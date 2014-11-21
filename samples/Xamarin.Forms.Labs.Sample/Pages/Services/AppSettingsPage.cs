using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Labs.Services.IO;

namespace Xamarin.Forms.Labs.Sample.Pages.Services {
    public class AppSettingsPage : ContentPage 
    {
        public AppSettingsPage() 
        {
            this.Title = "AppSettings";
            var appSettings = DependencyService.Get<IAppSettings>();

            var settingsStored = appSettings.Get<bool>("SETTINGS_STORED", false);
            if (!settingsStored) 
            {
                appSettings.Set("SETTINGS_STORED", true);
                appSettings.Set("theInt", Int32.MaxValue);
                appSettings.Set("theLong", Int64.MaxValue);
                appSettings.Set("theString", "Hello");
                appSettings.Set("theFloat", float.MaxValue);
                appSettings.Set("theDouble", double.MaxValue);
                appSettings.Set("theBool", true);
                appSettings.Set("theDate", DateTime.UtcNow);
            }

            var lblSettingsState = new Label { Text = settingsStored ? "Settings already saved." : "New settings saved." };
            var lblSettingsValues = new Label { Text = "Settings Values:" };

            var lblTheInt = new Label { Text = String.Format("theInt: {0}", appSettings.Get<int>("theInt", 0).ToString()) };
            var lblTheLong = new Label { Text = String.Format("theLong: {0}", appSettings.Get<long>("theLong", 0).ToString()) };
            var lblTheString = new Label { Text = String.Format("theString: {0}", appSettings.Get<string>("theString", null).ToString()) };
            var lblTheFloat = new Label { Text = String.Format("theFloat: {0}", appSettings.Get<float>("theFloat", 0).ToString()) };
            var lblTheDouble = new Label { Text = String.Format("theDouble: {0}", appSettings.Get<double>("theDouble", 0).ToString()) };
            var lblTheBool = new Label { Text = String.Format("theBool: {0}", appSettings.Get<bool>("theBool", false).ToString()) };
            var lblTheDate = new Label { Text = String.Format("theDate: {0}", appSettings.Get<DateTime>("theDate", DateTime.MinValue).ToString()) };

            var stack = new StackLayout();
            stack.Children.Add(lblSettingsState);
            stack.Children.Add(lblSettingsValues);

            stack.Children.Add(lblTheInt);
            stack.Children.Add(lblTheLong);
            stack.Children.Add(lblTheString);
            stack.Children.Add(lblTheFloat);
            stack.Children.Add(lblTheDouble);
            stack.Children.Add(lblTheBool);
            stack.Children.Add(lblTheDate);

            this.Content = stack;
        }
    }
}
