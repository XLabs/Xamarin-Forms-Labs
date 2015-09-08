using Xamarin.Forms;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(GradientContentView), typeof(GradientContentViewRenderer))]

namespace XLabs.Forms.Controls
{
    using CoreAnimation;
    using CoreGraphics;
    using UIKit;
    using Xamarin.Forms.Platform.iOS;

    class GradientContentViewRenderer : VisualElementRenderer<ContentView>
    {
        /// <summary>
        /// Gets the underlying element typed as an <see cref="GradientContentView"/>.
        /// </summary>
        private GradientContentView GradientContentView
        {
            get { return (GradientContentView)Element; }
        }

        protected CAGradientLayer GradientLayer { get; set; }

        /// <summary>
        /// Setup the gradient layer
        /// </summary>
        /// <param name="e"></param>
        protected override void OnElementChanged(ElementChangedEventArgs<ContentView> e)
        {
            base.OnElementChanged(e);

            if (GradientContentView != null &&
                NativeView != null)
            {
                // Create a gradient layer and add it to the 
                // underlying UIView
                GradientLayer = new CAGradientLayer();
                GradientLayer.Frame = NativeView.Bounds;
                GradientLayer.Colors = new CGColor[] 
                { 
                    GradientContentView.StartColor.ToCGColor(), 
                    GradientContentView.EndColor.ToCGColor() 
                };

                SetOrientation();

                NativeView.Layer.InsertSublayer(GradientLayer, 0);
            }
        }

        /// <summary>
        /// Update the underlying controls as needed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (GradientLayer != null && GradientContentView != null)
            {
                // Turn off Animations
                CATransaction.Begin();
                CATransaction.DisableActions = true;
             
                if (e.PropertyName == GradientContentView.StartColorProperty.PropertyName)
                    GradientLayer.Colors[0] = GradientContentView.StartColor.ToCGColor();

                if (e.PropertyName == GradientContentView.EndColorProperty.PropertyName)
                    GradientLayer.Colors[1] = GradientContentView.EndColor.ToCGColor();

                if (e.PropertyName == VisualElement.WidthProperty.PropertyName ||
                    e.PropertyName == VisualElement.HeightProperty.PropertyName)
                    GradientLayer.Frame = NativeView.Bounds;

                if (e.PropertyName == GradientContentView.OrientationProperty.PropertyName)
                    SetOrientation();

                CATransaction.Commit();
            }
        }
        void SetOrientation()
        {
            if (GradientContentView.Orientation == GradientOrientation.Horizontal)
            {
                GradientLayer.StartPoint = new CGPoint(0, 0.5);
                GradientLayer.EndPoint = new CGPoint(1, 0.5);
            }
            else
            {
                GradientLayer.StartPoint = new CGPoint(0.5, 0);
                GradientLayer.EndPoint = new CGPoint(0.5, 1);
            }
        }

    }
}