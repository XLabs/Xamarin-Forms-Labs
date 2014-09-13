using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Labs.Mvvm.Views;

namespace Xamarin.Forms.Labs.Sample.Pages.Controls
{


  public partial class WaveRecorderPage : BaseView
  {
    public WaveRecorderPage()
    {
      InitializeComponent();
    }

    protected override void OnDisappearing()
    {
      base.OnDisappearing();

      var vm = this.BindingContext as WaveRecorderViewModel;

      if (vm != null && vm.Stop.CanExecute(this))
      {
        vm.Stop.Execute(this);
      }
    }
  }
}
