using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XForms.Toolkit.Mvvm
{
	class NavigationConverter : IValueConverter
	{
		public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException ();
		}

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return new ViewModelNavigation ((INavigation) value);
		}
	}

	public class BaseView : ContentPage
	{
		public BaseView ()
		{
			SetBinding (NavigationProperty, new Binding ("Navigation", converter: new NavigationConverter ()));
		}
	}
}
