using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(CircleImage), typeof(CircleImageRenderer))]
namespace XLabs.Forms.Controls
{
    using System;
    using System.Windows.Media;

    using Xamarin.Forms;
    using Xamarin.Forms.Platform.WinPhone;

    /// <summary>
    /// Class CircleImageRenderer.
    /// </summary>
    public class CircleImageRenderer : ImageRenderer
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="CircleImageRenderer"/> class.
        /// </summary>
        public CircleImageRenderer()
        {
        }

        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
        }

        /// <summary>
        /// Handles the <see cref="E:ElementPropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control != null && Control.Clip == null)
            {
                var min = Math.Min(Element.Width, Element.Height) / 2.0f;

                if (min <= 0)
                    return;

                Control.Clip = new EllipseGeometry
                {
                    Center = new System.Windows.Point(min, min),
                    RadiusX = min,
                    RadiusY = min
                };
            }
        }
    }
}

