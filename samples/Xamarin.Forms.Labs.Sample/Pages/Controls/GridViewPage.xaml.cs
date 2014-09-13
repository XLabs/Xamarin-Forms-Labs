using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Mvvm.Views;

namespace Xamarin.Forms.Labs.Sample.Pages.Controls
{
    public partial class GridViewPage : BaseView
    {
        public GridViewPage ()
        {
            InitializeComponent ();
            BindingContext = ViewModelLocator.Main;
            this.grdView.ItemSelected += (object sender, EventArgs<object> e) => {
                DisplayAlert ("selected value", e.Value.ToString (), "ok", null);
            };
        }


    }
}

