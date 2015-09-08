using Android.Graphics;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XLabs.Forms.Controls;
using XLabs.Platform;
using Application = Android.App.Application;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(DragContentView), typeof(DragContentViewRenderer))]

namespace XLabs.Forms.Controls
{
    public class DragContentViewRenderer : ViewRenderer<DragContentView, View>
    {
        private Xamarin.Forms.View touchedElement;
        private View touchedView;

        private PointF homePosition;
        private PointF offsetLocation;

        public DragContentViewRenderer()
        {
            this.Touch += HandleTouch;
        }

        private void HandleTouch(object sender, TouchEventArgs e)
        {
            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    var loc = GetPointF(e.Event);
                    var o = this.GetTouchedView(loc);
                    this.touchedView = o != null ? o.Item1 : null;

                    if (this.touchedView == this.Element.Content.GetNativeContent())
                    {
                        this.touchedView = null;
                    }

                    if (this.touchedView != null)
                    {
                        this.touchedElement = this.Element.Content.FindFormsViewFromAccessibilityId(this.touchedView);
                        this.homePosition = new PointF(this.touchedView.GetX(), this.touchedView.GetY());
                        this.offsetLocation = new PointF(loc.X - o.Item2.X + homePosition.X, loc.Y - o.Item2.Y + homePosition.Y);
                    }
                    return;
                case MotionEventActions.Up:
                    this.touchedView = null;
                    this.touchedElement = null;
                    break;
                case MotionEventActions.Cancel:
                    this.touchedView = null;
                    this.touchedElement = null;
                    break;
                case MotionEventActions.Move:
                    if (this.touchedView != null)
                    {
                        var newLoc = GetPointF(e.Event);

                        var density = Application.Context.Resources.DisplayMetrics.Density;
                        var x = (newLoc.X - this.offsetLocation.X) / density;
                        var y = (newLoc.Y - this.offsetLocation.Y) / density;

                        if (this.touchedElement != null)
                        {
                            var f2 = new Rectangle(new Xamarin.Forms.Point(x, y),
                                new Size(this.touchedElement.Width, this.touchedElement.Height));

                            this.touchedElement.Layout(f2);
                        }
                        //else
                        //{
                        //    this.touchedView.SetX(x);
                        //    this.touchedView.SetY(y);
                        //}
                    }
                    return;
            }
        }

        private static PointF GetPointF(MotionEvent e)
        {
            return new PointF(e.GetX(), e.GetY());
        }
    }
}