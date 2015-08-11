using UIKit;
using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(AccessoryViewCell), typeof(AccessoryViewCellRenderer))]

namespace XLabs.Forms.Controls
{
    using CoreGraphics;

    using Xamarin.Forms;
    using Xamarin.Forms.Platform.iOS;

    /// <summary>
    /// Class AccessoryViewCellRenderer.
    /// </summary>
    public class AccessoryViewCellRenderer : ExtendedTextCellRenderer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccessoryViewCellRenderer"/> class.
        /// </summary>
        public AccessoryViewCellRenderer ()
        {
        }

        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="reusableCell">The reusable table view cell.</param>
        /// <param name="tv">The table view.</param>
        /// <returns>MonoTouch.UIKit.UITableViewCell.</returns>
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UIKit.UITableView tv)
        {
            var viewCell = item as AccessoryViewCell;
            var nativeCell = base.GetCell(item, reusableCell, tv);

            if (viewCell != null)
            {
                var frame = new CGRect (0, 0, (float)viewCell.AccessoryView.WidthRequest, (float)viewCell.AccessoryView.HeightRequest);
                var nativeView = RendererFactory.GetRenderer (viewCell.AccessoryView).NativeView;
                nativeView.Frame = frame;
                nativeView.Bounds = frame;
                nativeCell.AccessoryView = nativeView;
            }

            return nativeCell;
        }
    }
}

