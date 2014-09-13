using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Labs.Controls;

namespace Xamarin.Forms.Labs.Sample.Pages.Controls
{

  using Xamarin.Forms.Labs.Mvvm.Views;

  public partial class PopupPage : BaseView
  {
    public PopupPage()
    {
      InitializeComponent();

      this.openButton.Clicked += openButton_Clicked;
    }

    void openButton_Clicked(object sender, EventArgs e)
    {
      var popupLayout = this.Content as PopupLayout;

      if (popupLayout.IsPopupActive)
      {
        popupLayout.DismissPopup();
      }
      else
      {
        var list = new ListView()
        {
          BackgroundColor = Color.White,
          ItemsSource = new[] { "1", "2", "3" },
          HeightRequest = this.Height * .5,
          WidthRequest = this.Width * .8
        };

        list.ItemSelected += (s, args) =>
            popupLayout.DismissPopup();

        popupLayout.ShowPopup(list);
      }
    }
  }
}
