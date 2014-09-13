﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Xamarin.Forms.Labs.Sample.Pages.Controls
{

  using Xamarin.Forms.Labs.Mvvm.Views;

  public partial class CheckBoxPage : BaseView
  {
    private bool _checked;

    public bool Checked
    {
      get { return _checked; }
      set
      {
        _checked = value;
        OnPropertyChanged("Checked");
      }
    }

    public CheckBoxPage()
    {
      InitializeComponent();

      BindingContext = this;
    }
  }
}

