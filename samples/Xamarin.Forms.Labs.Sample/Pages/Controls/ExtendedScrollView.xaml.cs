using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Xamarin.Forms.Labs.Sample.Pages.Controls
{

  using Xamarin.Forms.Labs.Mvvm.Views;

  public partial class ExtendedScrollView : BaseView
    {    
        int _imageHeight = 200;
        bool _displayAlert = false;
        public ExtendedScrollView ()
        {
            InitializeComponent ();

            var container = new StackLayout () { 
                HorizontalOptions= LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand };
            var buttonToScroll = new Button () { Text = "Scroll to end" };
            container.Children.Add (buttonToScroll);
            for (int i = 0; i < 10; i++) {
                container.Children.Add (new Image { 
                    Aspect = Aspect.AspectFill,
                    Source = ImageSource.FromUri (new Uri("http://www.stockvault.net/data/2011/05/31/124348/small.jpg")),
                    HeightRequest = _imageHeight

                });
            }
            var sv = new Xamarin.Forms.Labs.Controls.ExtendedScrollView { Content = container, HorizontalOptions= LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand};
            sv.Scrolled+= async (ScrollView arg1, Rectangle arg2) => {
                if(arg2.Y > sv.Bounds.Height && !_displayAlert)
                {  
                    _displayAlert = true;
                   await DisplayAlert("Scroll event", "User scrolled pass bounds","Ok","cancel");
                 
                }
            };
            buttonToScroll.Clicked+= (object sender, EventArgs e) => {
                sv.Position = new Point(sv.Position.X, sv.ContentSize.Height-_imageHeight);
            };
            Content = sv;
        }

    }
}

