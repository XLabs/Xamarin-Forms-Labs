namespace XLabs.Forms.Controls
{
	using CoreGraphics;

	using Foundation;
	using UIKit;

	/// <summary>
	/// Class GridCollectionView.
	/// </summary>
	public class GridCollectionView : UICollectionView
	{
		/// <summary>
		/// Gets or sets a value indicating whether [selection enable].
		/// </summary>
		/// <value><c>true</c> if [selection enable]; otherwise, <c>false</c>.</value>
		public bool SelectionEnable {
			get;
			set;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="GridCollectionView"/> class.
		/// </summary>
		public GridCollectionView () : this (default(CGRect))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GridCollectionView"/> class.
		/// </summary>
		/// <param name="frm">The FRM.</param>
		public GridCollectionView (CGRect frm) : base (default(CGRect), new UICollectionViewFlowLayout () { })
		{
			AutoresizingMask = UIViewAutoresizing.All;
			ContentMode = UIViewContentMode.ScaleToFill;
			RegisterClassForCell (typeof(GridViewCell), new NSString (GridViewCell.Key));

		}

		/// <summary>
		/// Cells for item.
		/// </summary>
		/// <param name="indexPath">The index path.</param>
		/// <returns>UICollectionViewCell.</returns>
		public override UICollectionViewCell CellForItem(NSIndexPath indexPath)
		{
			return base.CellForItem(indexPath);
		}

		/// <summary>
		/// Draws the specified rect.
		/// </summary>
		/// <param name="rect">The rect.</param>
		public override void Draw (CGRect rect)
		{
			CollectionViewLayout.InvalidateLayout ();

			base.Draw (rect);
		}

		/// <summary>
		/// Gets or sets the row spacing.
		/// </summary>
		/// <value>The row spacing.</value>
		public double RowSpacing {
			get { 
				return (double)(CollectionViewLayout as UICollectionViewFlowLayout).MinimumLineSpacing;
			}
			set {
				(CollectionViewLayout as UICollectionViewFlowLayout).MinimumLineSpacing = (float)value;
			}
		}

		/// <summary>
		/// Gets or sets the column spacing.
		/// </summary>
		/// <value>The column spacing.</value>
		public double ColumnSpacing {
			get { 
				return (double)(CollectionViewLayout as UICollectionViewFlowLayout).MinimumInteritemSpacing;
			}
			set {
				(CollectionViewLayout as UICollectionViewFlowLayout).MinimumInteritemSpacing = (float)value;
			}
		}

		/// <summary>
		/// Gets or sets the size of the item.
		/// </summary>
		/// <value>The size of the item.</value>
		public CGSize ItemSize {
			get { 
				return (CollectionViewLayout as UICollectionViewFlowLayout).ItemSize;
			}
			set {
				(CollectionViewLayout as UICollectionViewFlowLayout).ItemSize = value;
			}
		}
	}
}

