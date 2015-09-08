using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace XLabs.Sample.Pages.Controls
{
    public class IconLabelPage : ContentPage
    {
        public IconLabelPage()
        {
            this.Title = "Icon Label (Font anwsome label)";
            this.BackgroundColor = Color.Black;
            IconLabel label1 = new IconLabel()
            {
                Icon = "\uf1d8",
                Text = "Text and icon",
                TextColor = Color.White,
                IconColor = Color.White
             
            };

            IconLabel label2 = new IconLabel()
            {
                Icon = "\uf1d8",
                Text = "Different color and size",
                IconColor = Color.Red,
                TextColor = Color.White,
                IconSize = 50,
                FontSize = 20
            };
           
            IconLabel label3 = null;
            Device.OnPlatform(null,() =>
            {
                //Orientation boottom and top only supported on Android
                label3 = new IconLabel()
                {
                    Icon = "\uf1d8",
                    Text = "Orientation icon on top",
                    TextColor = Color.White,
                    IconColor = Color.White,
                    FontSize = 20,
                    Orientation = Enums.ImageOrientation.ImageOnTop

                };
            },null);
            IconLabel label4 = new IconLabel()
            {
                Icon = "\uf1d8",
                Text = "Orientation icon on the right",
                TextColor = Color.White,
                IconColor = Color.White,
                FontSize = 20,
                Orientation = Enums.ImageOrientation.ImageToRight

            };
            
            IconLabel label5 = null;
            Device.OnPlatform(null,() =>
            {
                //Orientation boottom and top only supported on Android
                label5 = new IconLabel()
                {
                    Icon = "\uf1d8",
                    Text = "Orientation icon on bottom",
                    TextColor = Color.White,
                    IconColor = Color.White,
                    FontSize = 20,
                    Orientation = Enums.ImageOrientation.ImageOnBottom,
                

                };
            },null);

            IconLabel label6 = new IconLabel()
            {
                Icon = "\uf1d8",
                Text = "TextAlignment Center",
                TextColor = Color.White,
                IconColor = Color.White,
                FontSize = 20,
                TextAlignement = TextAlignment.Center


            };
            IconLabel label7 = new IconLabel()
            {
                Icon = "\uf1d8",
                Text = "TextAlignment End",
                TextColor = Color.White,
                IconColor = Color.White,
                FontSize = 20,
                TextAlignement = TextAlignment.End


            };
            IconLabel label8 = new IconLabel()
            {
                Icon = "\uf1d8",
                Text = " TextAlignment Start",
                TextColor = Color.White,
                IconColor = Color.White,
                FontSize = 20,
                TextAlignement = TextAlignment.Start


            };

            IconLabel label9 = new IconLabel()
            {
                Icon = "\uf1d8",
                Text = "With icon separator",
                ShowIconSeparator = true,
                TextColor = Color.Red,
                IconColor = Color.White,
                IconSize = 20,
                FontSize = 20
            };

            string up = "\uf102";
            string down = "\uf103";
            IconLabel SwitchIcon = new IconLabel()
            {
                Icon = up,
               
                IconColor = Color.Blue,
                IconSize = 30,
                
            };

            Button switchBtn = new Button()
            {
                Text = "Toggle icon up/down",
                TextColor = Color.White
            };
            switchBtn.Clicked += (s, a) =>
            {
                SwitchIcon.Icon = SwitchIcon.Icon == up ? down : up;

            };

            StackLayout content = null;
            Device.OnPlatform(()=>
            { 
                content = new StackLayout()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Children ={
                        label1,
                        label2,
                        label4,
                        label6,
                        label7,
                        label8,
                        label9,
                          new StackLayout()
                        {
                            Orientation = StackOrientation.Vertical,
                            Children=
                            {
                                switchBtn,
                                SwitchIcon
                            }
                        }
                    }
                };
            }, () =>
            {
                content = new StackLayout()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Children ={
                        label1,
                        label2,
                        label3,
                        label4,
                        label5,
                        label6,
                        label7,
                        label8,
                        label9,
                        new StackLayout()
                        {
                            Orientation = StackOrientation.Vertical,
                            Children=
                            {
                                switchBtn,
                                SwitchIcon
                            }
                        }
                    }
                };
            },null);
            this.Content = content;
        }
    }
}
