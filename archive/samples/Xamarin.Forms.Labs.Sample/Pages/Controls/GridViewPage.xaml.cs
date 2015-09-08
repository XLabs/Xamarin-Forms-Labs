using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Mvvm;

namespace Xamarin.Forms.Labs.Sample.Pages.Controls
{
    public partial class GridViewPage : BaseView
    {
        public GridViewPage ()
        {
            InitializeComponent ();
            BindingContext = ViewModelLocator.Main;
            this.grdView.ItemSelected += (sender, e) => {
                DisplayAlert ("selected value", e.Value.ToString (), "ok");
            };
        }


    }
}

