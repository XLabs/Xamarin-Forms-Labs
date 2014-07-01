using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Labs.WP8.Controls.ExtendedTextCell;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(TextCellRenderer), typeof(ExtendedTextCellRenderer))]

namespace Xamarin.Forms.Labs.WP8.Controls.ExtendedTextCell
{
    public class ExtendedTextCellRenderer : TextCellRenderer
    {
        public override System.Windows.DataTemplate GetTemplate(Cell cell)
        {
            return base.GetTemplate(cell);
        }
    }
}
