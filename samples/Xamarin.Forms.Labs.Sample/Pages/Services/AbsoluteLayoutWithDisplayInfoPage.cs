using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Forms.Labs.Sample
{

  using Xamarin.Forms.Labs.Mvvm.Views;

  public class AbsoluteLayoutWithDisplayInfoPage : BaseView
  {
    public AbsoluteLayoutWithDisplayInfoPage(IDisplay display)
    {
      this.Title = "Absolute Layout With Display Info";
      var abs = new AbsoluteLayout();
      var inchX = display.WidthRequestInInches(1);
      var inchY = display.HeightRequestInInches(1);
      var originX = display.WidthRequestInInches(display.ScreenWidthInches() / 2);
      var originY = display.HeightRequestInInches(display.ScreenHeightInches() / 2);

      abs.Children.Add(new Label() { Text = "1\"x\"1\" blue frame" });

      abs.Children.Add(new Frame()
          {
            BackgroundColor = Color.Navy,
          },
          new Rectangle(originX - inchX / 2, originY - inchY / 2, inchX, inchY));

      abs.Children.Add(new Frame()
          {
            BackgroundColor = Color.White
          },
          new Rectangle(originX - inchX / 16, originY - inchY / 16, inchX / 8, inchY / 8));

      this.Content = abs;
    }
  }
}
