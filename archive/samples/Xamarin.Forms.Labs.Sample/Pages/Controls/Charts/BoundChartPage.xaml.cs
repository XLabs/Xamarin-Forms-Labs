using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Sample.Pages.Controls.Charts
{
    public partial class BoundChartPage
    {
        public BoundChartPage()
        {
            InitializeComponent();
            this.BindingContext = new BoundChartViewModel();
        }
    }
}
