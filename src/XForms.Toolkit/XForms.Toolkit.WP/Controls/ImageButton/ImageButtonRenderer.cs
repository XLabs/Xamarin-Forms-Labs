using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using XForms.Toolkit.Controls;
using XForms.Toolkit.Enums;
using XForms.Toolkit.WP.Controls.ImageButton;
using Image = System.Windows.Controls.Image;
using TextAlignment = System.Windows.TextAlignment;

[assembly: ExportRenderer(typeof(ImageButton), typeof(ImageButtonRenderer))]
namespace XForms.Toolkit.WP.Controls.ImageButton
{
    public class ImageButtonRenderer : ButtonRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            var sourceButton = this.Element as Toolkit.Controls.ImageButton;
            var targetButton = this.Control;
            if (sourceButton != null && targetButton != null && !String.IsNullOrEmpty(sourceButton.Image))
            {
                var stackPanel = new StackPanel();
                stackPanel.Orientation = (sourceButton.Orientation == ImageOrientation.ImageOnTop ||
                    sourceButton.Orientation == ImageOrientation.ImageOnBottom) ?
                    Orientation.Vertical : Orientation.Horizontal;

                var image = GetImage(sourceButton.Image, sourceButton.ImageHeightRequest, sourceButton.ImageWidthRequest);
                SetImageMargin(image, sourceButton.Orientation);

                var label = new TextBlock();
                label.TextAlignment = TextAlignment.Left;
                label.FontSize = 16;
                label.VerticalAlignment = VerticalAlignment.Center;
                label.Text = sourceButton.Text;

                if (sourceButton.Orientation == ImageOrientation.ImageOnTop ||
                    sourceButton.Orientation == ImageOrientation.ImageToLeft)
                {
                    stackPanel.Children.Add(image);
                    stackPanel.Children.Add(label);
                }
                else
                {
                    stackPanel.Children.Add(label);
                    stackPanel.Children.Add(image);
                }

                targetButton.Content = stackPanel;
            }

        }

        private Image GetImage(string imageName, int height, int width)
        {
            var image = new Image();
            var uri = new Uri("images/" + imageName + ".png", UriKind.Relative);
            var bmp = new BitmapImage(uri);
            image.Source = bmp;
            image.Height = height / 2;
            image.Width = width / 2;
            return image;
        }

        private void SetImageMargin(Image image, ImageOrientation orientation)
        {
            int left = 0;
            int top = 0;
            int right = 0;
            int bottom = 0;

            switch (orientation)
            {
                case ImageOrientation.ImageToLeft:
                    right = 10;
                    break;
                case ImageOrientation.ImageOnTop:
                    bottom = 10;
                    break;
                case ImageOrientation.ImageToRight:
                    left = 10;
                    break;
                case ImageOrientation.ImageOnBottom:
                    top = 10;
                    break;
            }

            image.Margin = new System.Windows.Thickness(left, top, right, bottom);
        }
    }
}
