using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace XLabs.Platform.Droid
{
    public static class ViewGroupExtensions
    {
        public static Tuple<View, PointF> GetTouchedView(this ViewGroup viewGroup, PointF point, PointF offset = null)
        {
            offset = offset ?? new PointF();

            for (var n = 0; n < viewGroup.ChildCount; n++)
            {
                var view = viewGroup.GetChildAt(n);

                if (view.IsHit(point))
                {
                    var vg = view as ViewGroup;
                    if (vg != null)
                    {
                        offset = new PointF(offset.X + vg.Left, offset.Y + vg.Top);
                        var p = new PointF(point.X - vg.Left, point.Y - vg.Top);
                        return GetTouchedView(vg, p, offset);
                    }
                    else if (string.IsNullOrWhiteSpace(view.ContentDescription))
                    {
                        return new Tuple<View, PointF>(viewGroup, offset);
                    }

                    return new Tuple<View, PointF>(view, offset);
                }
            }

            return null;
        }
    }
}