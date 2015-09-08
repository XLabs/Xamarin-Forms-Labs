using System;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.Sample.Droid;
using Xamarin.Forms.Labs.Droid;

[assembly: ExportRenderer(typeof(DynamicListView<object>), typeof(BasicListRenderer))]

namespace Xamarin.Forms.Labs.Sample.Droid
{
    public class BasicListRenderer : DynamicListViewRenderer<object>
    {
        protected override Android.Views.View GetView(object item, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            return base.GetView(item, convertView, parent);
        }
    }
}

