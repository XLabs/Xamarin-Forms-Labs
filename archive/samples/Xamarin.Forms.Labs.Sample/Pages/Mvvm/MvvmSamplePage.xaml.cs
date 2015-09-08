using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Mvvm;

namespace Xamarin.Forms.Labs.Sample
{
    public partial class MvvmSamplePage : BaseView
    {
        public MvvmSamplePage()
		{
			InitializeComponent ();
			BindingContext = new MvvmSampleViewModel ();

            Icon = Device.OnPlatform("pie27_32.png", "pie27_32.png", "Images/pie27_32.png");
		}
    }
}

