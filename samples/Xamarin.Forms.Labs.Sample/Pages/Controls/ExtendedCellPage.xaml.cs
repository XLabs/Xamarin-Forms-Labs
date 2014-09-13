using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Xamarin.Forms.Labs.Sample
{
  using Xamarin.Forms.Labs.Mvvm.Views;

  public partial class ExtendedCellPage : BaseView
    {
        public ExtendedCellPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.Main;
        }
    }
}

