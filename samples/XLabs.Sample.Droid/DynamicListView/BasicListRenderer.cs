using Xamarin.Forms;
using XLabs.Forms.Controls;
using XLabs.Sample.Droid.DynamicListView;

[assembly: ExportRenderer(typeof(DynamicListView<object>), typeof(BasicListRenderer))]

namespace XLabs.Sample.Droid.DynamicListView
{
	public class BasicListRenderer : DynamicListViewRenderer<object>
    {
        protected override Android.Views.View GetView(object item, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            return base.GetView(item, convertView, parent);
        }
    }
}

