using System;
using Xamarin.Forms;
using XLabs.Forms.Controls;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace XLabs.Sample
{
    public class WebPageLoadSample : ContentPage
    {
        private HybridWebView hybrid;
        private readonly List<Uri> pages;
        private readonly Label addressLabel;
        private CancellationTokenSource source;
        private int index = 0;

        public WebPageLoadSample()
        {
            this.pages = new List<Uri>(new[]
            {
                new Uri("http://www.openstreetmap.org "),
                new Uri("https://www.google.pl/maps"),
                new Uri("https://www.bing.com/maps/ "),
            });
            
            this.Padding = new Thickness(0, 20, 0, 0);

            this.hybrid = new HybridWebView()
            {
                BackgroundColor = Color.Red,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                StyleId = "HybridWebView",
            };


            var mainLayout = new RelativeLayout
            {
                StyleId = "MainLayout",
            };

            this.addressLabel = new Label()
            {
                StyleId = "UrlLoaded",
                FontSize = 8
            };

            var labelHeight = Device.OnPlatform(10, 10, 20);

            mainLayout.Children.Add(this.addressLabel,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent(p => p.Width),
                Constraint.Constant(labelHeight));

            mainLayout.Children.Add(this.hybrid,
                Constraint.Constant(0),
                Constraint.RelativeToView(addressLabel, (p, v) => v.Y + v.Height),
                Constraint.RelativeToParent(p => 
                    {
                        return p.Width;
                    }),
                Constraint.RelativeToView(addressLabel, (p, v) => p.Height - v.Height));

            this.Content = mainLayout;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.source = new CancellationTokenSource();

            CircleUrls(this.source.Token);
        }

        protected override void OnDisappearing()
        {
            this.source.Cancel();
            base.OnDisappearing();
        }

        private async Task CircleUrls(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await LoadPage(this.pages[index++], token);
                index %= this.pages.Count;
                await Task.Delay(10000);
            }
        }

        public Task<bool> LoadPage(Uri page, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<bool>();

            EventHandler e = null;

            e = (sender, args) =>
            {
                if (!token.IsCancellationRequested) 
                {
                    Device.BeginInvokeOnMainThread(() => 
                    {
                        this.addressLabel.Text = page.ToString();
                        this.addressLabel.BackgroundColor = Color.Green;
                    });
                }

                tcs.SetResult(true);

                this.hybrid.LoadFinished -= e;
            };

            this.hybrid.LoadFinished += e;

            Device.BeginInvokeOnMainThread(() =>
            {
                this.addressLabel.Text = string.Empty;
                this.addressLabel.BackgroundColor = Color.Yellow;
                this.hybrid.Uri = page;
                
            });

            return tcs.Task;
        }
    }
}

