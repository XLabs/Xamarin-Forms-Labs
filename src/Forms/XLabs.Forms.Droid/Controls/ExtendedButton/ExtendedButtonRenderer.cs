using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedButton), typeof(ExtendedButtonRenderer))]

namespace XLabs.Forms.Controls
{
    using System.ComponentModel;
    using Extensions;

    public class ExtendedButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            SetAlignment();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "VerticalContentAlignment":
                case "HorizontalContentAlignment":
                    SetAlignment();
                    break;
                default:
                    base.OnElementPropertyChanged(sender, e);
                    break;
            }
        }

        private void SetAlignment()
        {
            var element = this.Element as ExtendedButton;

            if (element == null || this.Control == null)
            {
                return;
            }

            this.Control.Gravity = element.VerticalContentAlignment.ToDroidVerticalGravity() |
                element.HorizontalContentAlignment.ToDroidHorizontalGravity();
        }
    }
}