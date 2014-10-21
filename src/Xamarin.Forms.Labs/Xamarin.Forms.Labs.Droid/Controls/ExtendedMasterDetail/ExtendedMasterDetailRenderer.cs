using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.Droid.Controls.ExtendedMasterDetail;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedMasterDetailPage), typeof(ExtendedMasterDetailRenderer))]
namespace Xamarin.Forms.Labs.Droid.Controls.ExtendedMasterDetail
{
	/// <summary> The ExtendedMasterDetailRenderer is to be used with the ExtendedMasterDetailPage to change properties like the DrawerWidth of the Master Page</summary>
	public class ExtendedMasterDetailRenderer : MasterDetailRenderer
	{
		private bool _FirstDone;
		public override void AddView(Android.Views.View child)
		{
			//This is a bit of a hack but works well with 1.2.3.6257 (for sure)
			if (_FirstDone)
			{
				ExtendedMasterDetailPage page = (ExtendedMasterDetailPage)this.Element;
				LayoutParams p = (LayoutParams)child.LayoutParameters;
				p.Width = page.DrawerWidth;
				base.AddView(child, p);
			}
			else
			{
				_FirstDone = true;
				base.AddView(child);
			}
		}

	}
}