using Xamarin.Forms;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedScrollView), typeof(ExtendedScrollViewRenderer))]

namespace XLabs.Forms.Controls
{
    using System;
    using System.ComponentModel;
    using Xamarin.Forms.Platform.Android;

    /// <summary>
    /// Class ExtendedScrollViewRenderer.
    /// </summary>
    public class ExtendedScrollViewRenderer:ScrollViewRenderer
    {
        /// <summary>
        /// The epsilon.
        /// </summary>
        private const double Epsilon = 0.1;

        /// <summary>
        /// Handles the <see cref="E:ElementChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="VisualElementChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            this.ViewTreeObserver.ScrollChanged += (sender, ev) => {
                var scrollView = (ExtendedScrollView)this.Element;
                if(scrollView == null)
                    return;

                var bounds = new Rectangle(this.ScrollX, this.ScrollY, GetChildAt(0).Width, GetChildAt(0).Height);
                scrollView.UpdateBounds(bounds);
            };

            if(e.OldElement != null)
                e.OldElement.PropertyChanged -= OnElementPropertyChanged;

            e.NewElement.PropertyChanged += OnElementPropertyChanged;
        }

        /// <summary>
        /// Handles the <see cref="E:ElementPropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != ExtendedScrollView.PositionProperty.PropertyName)
            {
                return;
            }

            var scrollView = (ExtendedScrollView)this.Element;
            var position = scrollView.Position;

            if (Math.Abs((int) (this.ScrollY - position.Y)) < Epsilon
                && Math.Abs((int) (this.ScrollX - position.X)) < Epsilon)
            {
                return;
            }

            ScrollTo((int)position.X,(int)position.Y);
            UpdateLayout();
        }
    }

}

