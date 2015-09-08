using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.iOS.Controls;
using Xamarin.Forms.Labs.Sample.iOS;


[assembly: ExportRenderer(typeof(DynamicListView<object>), typeof(BasicListRenderer))]

namespace Xamarin.Forms.Labs.Sample.iOS
{
    public class BasicListRenderer : DynamicUITableViewRenderer<object>
    {
        protected override UITableViewCell GetCell(UITableView tableView, object item)
        {
            if (item is string)
            {
                return base.GetCell(tableView, item);
            }
            
            if (item is DateTime)
            {
                var cell = new UITableViewCell(UITableViewCellStyle.Value1, this.GetType().Name);

                cell.TextLabel.Text = ((DateTime)item).ToShortDateString();
                cell.DetailTextLabel.Text = ((DateTime)item).ToShortTimeString();
                return cell;
            }

            throw new NotImplementedException();
        }

        public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            var item = this.Element.Data[indexPath.Item];
            if (item is string)
            {
                return base.GetHeightForRow(tableView, indexPath);
            }
            else if (item is DateTime)
            {
                return 44f;
            }

            throw new NotImplementedException();
        }
    }
}