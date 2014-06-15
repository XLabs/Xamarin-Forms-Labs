using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XForms.Toolkit.Mvvm;

namespace XForms.Toolkit.Sample
{	
	public partial class MvvmSamplePage : BaseView
	{	
		public MvvmSamplePage ()
		{
			InitializeComponent ();
			this.BindingContext = new MvvmSampleViewModel ();
		}
	}
}

