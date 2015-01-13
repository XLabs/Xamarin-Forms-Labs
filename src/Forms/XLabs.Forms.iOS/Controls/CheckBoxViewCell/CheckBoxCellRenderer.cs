using UIKit;
using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(CheckboxCell), typeof(CheckBoxCellRenderer))]

namespace XLabs.Forms.Controls
{
	using Xamarin.Forms;

	/// <summary>
	/// Class CheckBoxCellRenderer.
	/// </summary>
	public class CheckBoxCellRenderer : ExtendedTextCellRenderer
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CheckBoxCellRenderer"/> class.
		/// </summary>
		public CheckBoxCellRenderer ()
		{
		}

	    /// <summary>
	    /// Gets the cell.
	    /// </summary>
	    /// <param name="item">The item.</param>
	    /// <param name="reusableCell">The reusable cell.</param>
	    /// <param name="tv">The table view.</param>
	    /// <returns>UITableViewCell.</returns>
	    public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			var viewCell = item as CheckboxCell;
            var nativeCell = base.GetCell(item, reusableCell, tv);
            nativeCell.SelectionStyle = UITableViewCellSelectionStyle.None;

	        if (viewCell == null) return nativeCell;

			nativeCell.Accessory = viewCell.Checked ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;

			viewCell.CheckedChanged += (s, e) => tv.ReloadData();

			return nativeCell;
		}

	}
}

