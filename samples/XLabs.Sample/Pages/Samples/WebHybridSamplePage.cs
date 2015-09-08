namespace XLabs.Sample.Pages.Samples
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices.WindowsRuntime;
    using Forms.Controls;
    using Ioc;
    using Serialization;
    using Xamarin.Forms;

    public class WebHybridSamplePage : ContentPage
    {
        private HybridWebView hwv;
        public WebHybridSamplePage()
        {
            this.BackgroundColor = Color.White;
            var stack = new StackLayout { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
            hwv = new HybridWebView { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };

            stack.Children.Add(hwv);
            this.Content = stack;

            hwv.RegisterCallback("dataCallback", t =>
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.DisplayAlert("Data callback", t, "OK");
                })
            );

            //hwv.RegisterNativeFunction("funcCallback", s => new object[] {"Func return data for " + s});

            hwv.RegisterCallback("sendObject", s =>
            {
                var serializer = Resolver.Resolve<IJsonSerializer>();

                var o = serializer.Deserialize<SendObject>(s);

                System.Diagnostics.Debug.WriteLine(o.X);
                System.Diagnostics.Debug.WriteLine(o.Y);
            });
        }

        #region Overrides of Page
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var assembly = this.GetType().GetTypeInfo().Assembly;
            using (var reader = new StreamReader(assembly.GetManifestResourceStream("XLabs.Sample.Data.WebHybridTest.html")))
            {
                var str = reader.ReadToEnd();
                this.hwv.LoadContent(str);
            }
        }

        #endregion
    }

    public class SendObject
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}