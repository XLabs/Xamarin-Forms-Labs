using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Controls
{
	/// <summary> The ExtendedMasterDetailPage can be used wtih the ExtendedMasterDetailRenderer to (for now) change the drawer width of the Master page. </summary>
	public class ExtendedMasterDetailPage : MasterDetailPage
	{
		/// <summary> The DrawerWidthProperty is the static BindableProperty declaration For DrawerWidth </summary>
		public static readonly BindableProperty DrawerWidthProperty = BindableProperty.Create<ExtendedMasterDetailPage, int>(p => p.DrawerWidth, default(int));

		/// <summary> The DrawerWidth property can be used to change the width of the DrawerLayout on the Android System (iOS to come soon) </summary>
		public int DrawerWidth
		{
			get { return (int)GetValue(DrawerWidthProperty); }
			set { SetValue(DrawerWidthProperty, value); }
		}
	}
}
