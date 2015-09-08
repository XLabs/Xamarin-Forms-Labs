using Xamarin.Forms;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(GradientContentView), typeof(GradientContentViewRenderer))]
namespace XLabs.Forms.Controls
{
    using Xamarin.Forms.Platform.Android;
    using Xamarin.Forms;
    using Android.Graphics.Drawables.Shapes;
    using Android.Graphics.Drawables;
    using Android.Graphics;

    class GradientContentViewRenderer : ViewRenderer<GradientContentView, Android.Views.View>
    {

        public GradientDrawable GradientDrawable { get; set; }
        /// <summary>
        /// Gets the underlying element typed as an <see cref="GradientContentView"/>.
        /// </summary>
        private GradientContentView GradientContentView
        {
            get { return (GradientContentView)Element; }
        }

        /// <summary>
        /// Setup the gradient layer
        /// </summary>
        /// <param name="e"></param>
        protected override void OnElementChanged(ElementChangedEventArgs<GradientContentView> e)
        {
            base.OnElementChanged(e);

            if (GradientContentView != null)
            {
                //ShapeDrawable sd = new ShapeDrawable(new RectShape());
                GradientDrawable = new GradientDrawable();
                GradientDrawable.SetColors(new int[] {GradientContentView.StartColor.ToAndroid(), GradientContentView.EndColor.ToAndroid() });
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (GradientDrawable != null && GradientContentView != null)
            {

                if (e.PropertyName == GradientContentView.StartColorProperty.PropertyName ||
                    e.PropertyName == GradientContentView.EndColorProperty.PropertyName)
                {
                    GradientDrawable.SetColors(new int[] { GradientContentView.StartColor.ToAndroid(), GradientContentView.EndColor.ToAndroid() });
                    Invalidate();
                }

                if (e.PropertyName == VisualElement.WidthProperty.PropertyName ||
                    e.PropertyName == VisualElement.HeightProperty.PropertyName ||
                    e.PropertyName == GradientContentView.OrientationProperty.PropertyName)
                {
                    Invalidate();
                }
            }
        }

        protected override bool DrawChild(Canvas canvas, global::Android.Views.View child, long drawingTime)
        {
            GradientDrawable.Bounds = canvas.ClipBounds;
            GradientDrawable.SetOrientation(GradientContentView.Orientation == GradientOrientation.Vertical ? GradientDrawable.Orientation.TopBottom 
                : GradientDrawable.Orientation.LeftRight);
            GradientDrawable.Draw(canvas);
            return base.DrawChild(canvas, child, drawingTime);
        }
    }
}