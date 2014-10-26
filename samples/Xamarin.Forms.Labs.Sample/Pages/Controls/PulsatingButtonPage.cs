using System;
using System.Threading.Tasks;
using Xamarin.Forms.Labs.Controls;

namespace Xamarin.Forms.Labs.Sample.Pages.Controls
{
  public class PulsatingButtonPage : ContentPage
  {
    private bool _loading;

    public PulsatingButtonPage()
    {
      var pulsatingButton = new PulsatingButton
      {
        VerticalOptions = LayoutOptions.Center,
        HorizontalOptions = LayoutOptions.Center,
        Text = "Click Me",
      };

      pulsatingButton.Clicked += PulsatingButtonOnClicked;

      Content = pulsatingButton;
    }

    private void PulsatingButtonOnClicked(object sender, EventArgs eventArgs)
    {
      var btn = sender as PulsatingButton;

      if (btn == null) return;

      btn.IsLoading = true;

      Task.Delay(TimeSpan.FromSeconds(15)).ContinueWith((result) =>
      {
        btn.IsLoading = false;
      });
    }
  }
}