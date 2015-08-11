using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedEntryCell), typeof(ExtendedEntryCellRenderer))]
namespace XLabs.Forms.Controls
{
    using UIKit;

    using Xamarin.Forms;
    using Xamarin.Forms.Platform.iOS;

    /// <summary>
    /// Class ExtendedEntryCellRenderer.
    /// </summary>
    public class ExtendedEntryCellRenderer : EntryCellRenderer
    {
        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="reusableCell">The reusable TableView cell.</param>
        /// <param name="tv">The TableView.</param>
        /// <returns>UITableViewCell.</returns>
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var entryCell = (ExtendedEntryCell)item;
            var cell = base.GetCell (item, reusableCell, tv);
            
            if (cell != null)
            {
                var textField = (UITextField)cell.ContentView.Subviews [0]; 
                textField.SecureTextEntry = entryCell.IsPassword;
            }

            return cell;
        }
    } 
}

