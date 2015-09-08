using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Labs.Sample;

namespace Xamarin.Forms.Labs.Sample
{
	public class ViewModelLocator
    {
		private static MainViewModel _main;
		public static MainViewModel Main
        {
            get
            {
				if (_main == null)
					_main = new MainViewModel ();
				return _main;
            }
        }
    }
}
