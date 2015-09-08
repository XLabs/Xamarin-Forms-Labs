using Xamarin.Forms;

using XLabs.Forms.Controls;
using XLabs.Sample.iOS.DynamicListView;

[assembly: ExportRenderer(typeof(DynamicListView<object>), typeof(BasicListRenderer))]

namespace XLabs.Sample.iOS.DynamicListView
{
	using System;

	using Foundation;
	using UIKit;

	/// <summary>
	/// Class BasicListRenderer.
	/// </summary>
	public class BasicListRenderer : DynamicUITableViewRenderer<object>
    {
		/// <summary>
		/// Gets the cell.
		/// </summary>
		/// <param name="tableView">The table view.</param>
		/// <param name="item">The item.</param>
		/// <returns>UITableViewCell.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
	
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

		/// <summary>
		/// Gets the height for row.
		/// </summary>
		/// <param name="tableView">The table view.</param>
		/// <param name="indexPath">The index path.</param>
		/// <returns>System.Single.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
			var item = this.Element.Data[(int)indexPath.Item];
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