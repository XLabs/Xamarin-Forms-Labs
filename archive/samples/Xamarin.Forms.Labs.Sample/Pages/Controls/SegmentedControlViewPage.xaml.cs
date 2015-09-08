using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Sample.Pages.Controls
{
    public partial class SegmentedControlViewPage : ContentPage
    {
        public SegmentedControlViewPage()
        {
            InitializeComponent();

            Filter.SelectedItem = 1;
        }
    }
}
