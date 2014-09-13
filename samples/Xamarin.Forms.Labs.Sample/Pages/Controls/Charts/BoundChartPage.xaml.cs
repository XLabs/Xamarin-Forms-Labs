using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Sample.Pages.Controls.Charts
{
  using Xamarin.Forms.Labs.Mvvm.Views;

  public partial class BoundChartPage : BaseView
  {
    public BoundChartPage()
    {
      InitializeComponent();
      this.BindingContext = new BoundChartViewModel();
    }
  }
}
