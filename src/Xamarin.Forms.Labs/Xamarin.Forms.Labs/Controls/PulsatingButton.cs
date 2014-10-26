using System;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Controls
{
  public class PulsatingButton : Button
  {
    public PulsatingButton()
    {
      LoadingText = "Loading...";
    }

    public static readonly BindableProperty IsLoadingProperty =
      BindableProperty.Create<PulsatingButton, bool>(p => p.IsLoading, false, BindingMode.TwoWay, null, PropertyChanged);

    private new static async void PropertyChanged(BindableObject bindable, bool oldValue, bool newValue)
    {
      var btn = bindable as PulsatingButton;

      if (btn == null)
        return;

      if (newValue)
        await btn.StartAnimation();
    }

    public bool IsLoading
    {
      get { return (bool)GetValue(IsLoadingProperty); }
      set { SetValue(IsLoadingProperty, value); }
    }

    public string LoadingText { get; set; }

    public async Task StartAnimation()
    {
      const double percentIncrease = 0.25;
      var originalText = string.Empty;

      Device.BeginInvokeOnMainThread(() =>
      {
        originalText = Text;
        Text = LoadingText;
        IsEnabled = false;
      });

      var oldBounds = Bounds;

      var heightIncrease = oldBounds.Height * percentIncrease;
      var widthIncrease = oldBounds.Width * percentIncrease;

      var newBounds = new Rectangle(oldBounds.X - (widthIncrease / 2), oldBounds.Y - (heightIncrease / 2),
        oldBounds.Width + widthIncrease, oldBounds.Height + heightIncrease);

      const uint length = 400;

      while (IsLoading)
      {
        await this.LayoutTo(newBounds, length, Easing.BounceOut);

        await Task.Delay(TimeSpan.FromMilliseconds(1000));

        await this.LayoutTo(oldBounds, length);

        await Task.Delay(TimeSpan.FromMilliseconds(1000));
      }

      Device.BeginInvokeOnMainThread(() =>
      {
        Text = originalText;
        IsEnabled = true;
      });
    }
  }
}
