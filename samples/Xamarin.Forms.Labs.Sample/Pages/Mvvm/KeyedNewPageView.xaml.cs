using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Mvvm.Views;

namespace Xamarin.Forms.Labs.Sample
{	
  public partial class KeyedNewPageView : BaseView
  {
    public KeyedNewPageView()
    {
      InitializeComponent ();

      ViewId.Text = Guid.NewGuid().ToString();
    }
  }
}

