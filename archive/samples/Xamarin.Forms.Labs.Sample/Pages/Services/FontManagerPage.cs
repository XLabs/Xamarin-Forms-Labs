using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Labs;
using Xamarin.Forms.Labs.Services;

namespace Xamarin.Forms.Labs.Sample.Pages.Services
{
    public class FontManagerPage : ContentPage
    {
        public FontManagerPage(IDisplay display)
        {
            var stack = new StackLayout();

            foreach (var namedSize in Enum.GetValues(typeof(NamedSize)))
            {
                var font = Font.SystemFontOfSize((NamedSize)namedSize);

                var height = display.FontManager.GetHeight(font);

                var heightRequest = display.HeightRequestInInches(height);

                var label = new Label()
                {
                    Font = font,
                    HeightRequest = heightRequest + 10,
                    Text = string.Format("System font {0} is {1:0.000}in tall.", namedSize, height),
                    XAlign = TextAlignment.Center
                };

                stack.Children.Add(label);
            }

            var f = Font.SystemFontOfSize(24);

            var inchFont = display.FontManager.FindClosest(f.FontFamily, 0.25);

            stack.Children.Add(new Label() 
                {
                    Text = "The below text should be 1/4 inch height from its highest point to lowest.",
                    XAlign = TextAlignment.Center
                });

            stack.Children.Add(new Label()
                {
                    Text = "ftlgjp",
                    Font = inchFont,
                    XAlign = TextAlignment.Center
                });

            this.Content = stack;
        }
    }
}
