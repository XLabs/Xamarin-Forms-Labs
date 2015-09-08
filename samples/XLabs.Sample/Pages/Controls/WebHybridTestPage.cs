using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace XLabs.Sample.Pages.Controls
{
    public class WebHybridTestPage : ContentPage
    {
        public WebHybridTestPage()
        {
            var stack = new StackLayout { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
            var hwv = new HybridWebView { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };

            stack.Children.Add(hwv);
            this.Content = stack;

            hwv.Uri = new Uri("http://test.padrose.co.uk/hvw/test1.html");

            hwv.RegisterCallback("dataCallback", t =>
                Device.BeginInvokeOnMainThread(() =>
                {
                    /**********************************/
                    //THIS WILL WORK FOR PAGE 1 ONLY
                    /*********************************/
                    System.Diagnostics.Debug.WriteLine("!!!!!!!!!!!!!!!!! dataCallback: " + t);
                })
            );

            hwv.LoadFinished += (s, e) =>
            {
                /***********************************/
                //THIS WILL WORK FOR PAGE 1 ONLY
                //WEAK REFERENCE LOST???
                /***********************************/
                System.Diagnostics.Debug.WriteLine("(!!!!!!!!!!!!!!!!!!!! LoadFinished");
            };
        }


    }
}
