using Xamarin.Forms.Labs.Controls;

namespace Xamarin.Forms.Labs.Sample.Pages.Controls
{
    public class SegmentPage : ContentPage
    {
        public SegmentPage()
        {
            var segment = new SegmentControl
            {
                TintColor = Color.Green
            };

            segment.AddSegment("Green");
            segment.AddSegment("Yellow");
            segment.AddSegment("Red");

            segment.SelectedSegmentChanged += (sender, segmentIndex) =>
            {
                switch (segmentIndex)
                {
                    case 0:
                        segment.TintColor = Color.Green;
                        break;
                    case 1:
                        segment.TintColor = Color.Yellow;
                        break;
                    case 2:
                        segment.TintColor = Color.Red;
                        break;
                }
            };

            segment.SelectedSegment = 1;

            Content = new StackLayout
            {
                Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 10),
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = { segment },
            };
        }
    }
}
