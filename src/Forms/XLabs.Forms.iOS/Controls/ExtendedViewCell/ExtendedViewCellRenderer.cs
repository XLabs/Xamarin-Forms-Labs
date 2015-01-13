using Xamarin.Forms;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedViewCell), typeof(ExtendedViewCellRenderer))]

namespace XLabs.Forms.Controls
{
	using System;
	using CoreGraphics;
	using UIKit;
	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	/// <summary>
	/// Class ExtendedViewCellRenderer.
	/// </summary>
	public class ExtendedViewCellRenderer : ViewCellRenderer
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
			var extendedCell = (ExtendedViewCell)item;
            var cell = base.GetCell(item, reusableCell,tv);
			if (cell != null) 
            {
				cell.BackgroundColor = extendedCell.BackgroundColor.ToUIColor();
				cell.SeparatorInset = new UIEdgeInsets(
                    (float)extendedCell.SeparatorPadding.Top, 
                    (float)extendedCell.SeparatorPadding.Left,
					(float)extendedCell.SeparatorPadding.Bottom, 
                    (float)extendedCell.SeparatorPadding.Right);

				if (extendedCell.ShowDisclousure) 
                {
					cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
					if (!string.IsNullOrEmpty (extendedCell.DisclousureImage)) 
                    {
						var detailDisclosureButton = UIButton.FromType (UIButtonType.Custom);
						detailDisclosureButton.SetImage (UIImage.FromBundle (extendedCell.DisclousureImage), UIControlState.Normal);
						detailDisclosureButton.SetImage (UIImage.FromBundle (extendedCell.DisclousureImage), UIControlState.Selected);

						detailDisclosureButton.Frame = new CGRect (0f, 0f, 30f, 30f);
						detailDisclosureButton.TouchUpInside += (object sender, EventArgs e) => 
                        {
								try 
                                {
									var index = tv.IndexPathForCell (cell);
									tv.SelectRow (index, true, UITableViewScrollPosition.None);
									tv.Source.RowSelected (tv, index);
								} 
                                catch ( Foundation.You_Should_Not_Call_base_In_This_Method ex) 
                                {
									Console.Write("Xamarin Forms Labs Weird stuff : You_Should_Not_Call_base_In_This_Method happend");
								}
						};
						cell.AccessoryView = detailDisclosureButton;
					}
				}
			}

			if(!extendedCell.ShowSeparator)
				tv.SeparatorStyle = UITableViewCellSeparatorStyle.None;

			tv.SeparatorColor = extendedCell.SeparatorColor.ToUIColor();

			return cell;
		}
	}
}

