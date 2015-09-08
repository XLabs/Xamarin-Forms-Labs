using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace XLabs.Sample.Pages.Controls
{
    public class IconButtonPage : ContentPage
    {
        public IconButtonPage()
        {
            this.Title = "Icon Button (Font anwsome)";
            IconButton btn1 = new IconButton()
            {
                Icon = "\uf1d8",
                Text = "Text and icon",
                TextColor = Color.White,
                IconColor = Color.White,
                BackgroundColor = Color.Gray


            };

            IconButton btn2 = new IconButton()
            {
                Icon = "\uf1d8",
                Text = "Different color and size",
                IconColor = Color.Red,
                TextColor = Color.White,
                IconSize = 40,
                BackgroundColor = Color.Gray,
                FontSize = 20
            };

            IconButton btn3 = null;
            Device.OnPlatform(null, () =>
            {
                //Orientation boottom and top only supported on Android
                btn3 = new IconButton()
                {
                    Icon = "\uf1d8",
                    Text = "Orientation icon on top",
                    TextColor = Color.White,
                    IconColor = Color.White,
                    BackgroundColor = Color.Gray,
                    FontSize = 20,
                    Orientation = Enums.ImageOrientation.ImageOnTop

                };
            }, null);
            IconButton btn4 = new IconButton()
            {
                Icon = "\uf1d8",
                Text = "Orientation icon on the right",
                TextColor = Color.White,
                IconColor = Color.White,
                BackgroundColor = Color.Gray,
                FontSize = 20,
                Orientation = Enums.ImageOrientation.ImageToRight

            };

            IconButton btn5 = null;
            Device.OnPlatform(null, () =>
            {
                //Orientation boottom and top only supported on Android
                btn5 = new IconButton()
                {
                    Icon = "\uf1d8",
                    Text = "Orientation icon on bottom",
                    TextColor = Color.White,
                    IconColor = Color.White,
                    BackgroundColor = Color.Gray,
                    FontSize = 20,
                    Orientation = Enums.ImageOrientation.ImageOnBottom,


                };
            }, null);

            IconButton btn6 = new IconButton()
            {
                Icon = "\uf1d8",
                Text = "TextAlignment Center",
                TextColor = Color.White,
                IconColor = Color.White,
                FontSize = 20,
                BackgroundColor = Color.Gray,
                TextAlignement = TextAlignment.Center


            };
            IconButton btn7 = new IconButton()
            {
                Icon = "\uf1d8",
                Text = "TextAlignment End",
                TextColor = Color.White,
                IconColor = Color.White,
                FontSize = 20,
                BackgroundColor = Color.Gray,
                TextAlignement = TextAlignment.End


            };
            IconButton btn8 = new IconButton()
            {
                Icon = "\uf1d8",
                Text = " TextAlignment Start",
                TextColor = Color.White,
                IconColor = Color.White,
                FontSize = 20,
                BackgroundColor = Color.Gray,
                TextAlignement = TextAlignment.Start


            };

            IconButton btn9 = new IconButton()
            {
                Icon = "\uf1d8",
                Text = "With icon separator",
                ShowIconSeparator = true,
                TextColor = Color.Red,
                IconColor = Color.White,
                IconSize = 20,
                BackgroundColor = Color.Gray,
                FontSize = 20
            };

         
           

            StackLayout content = null;
            Device.OnPlatform(() =>
            {
                content = new StackLayout()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Children ={
                        btn1,
                        btn2,
                        btn4,
                        btn6,
                        btn7,
                        btn8,
                        btn9,
                        
                    }
                };
            }, () =>
            {
                content = new StackLayout()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Children ={
                        btn1,
                        btn2,
                        btn3,
                        btn4,
                        btn5,
                        btn6,
                        btn7,
                        btn8,
                        btn9,
                        
                    }
                };
            }, null);
            this.Content = content;
        }
    }
}
