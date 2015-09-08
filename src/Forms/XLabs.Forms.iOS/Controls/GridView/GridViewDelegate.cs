namespace XLabs.Forms.Controls
{
	using Foundation;
	using UIKit;

	/// <summary>
	/// Class GridViewDelegate.
	/// </summary>
	public class GridViewDelegate: UICollectionViewDelegate
	{
		/// <summary>
		/// Delegate OnItemSelected
		/// </summary>
		/// <param name="tableView">The table view.</param>
		/// <param name="indexPath">The index path.</param>
		public delegate void OnItemSelected (UICollectionView tableView, NSIndexPath indexPath);

		/// <summary>
		/// The _on item selected
		/// </summary>
		private readonly OnItemSelected _onItemSelected;

		/// <summary>
		/// Initializes a new instance of the <see cref="GridViewDelegate"/> class.
		/// </summary>
		/// <param name="onItemSelected">The on item selected.</param>
		public GridViewDelegate (OnItemSelected onItemSelected)
		{
		    this._onItemSelected = onItemSelected;
		}

		/// <summary>
		/// Items the selected.
		/// </summary>
		/// <param name="collectionView">The collection view.</param>
		/// <param name="indexPath">The index path.</param>
		public override void ItemSelected (UICollectionView collectionView, NSIndexPath indexPath)
		{
		    this._onItemSelected (collectionView, indexPath);
		}

		/// <summary>
		/// Items the highlighted.
		/// </summary>
		/// <param name="collectionView">The collection view.</param>
		/// <param name="indexPath">The index path.</param>
		public override void ItemHighlighted(UICollectionView collectionView, NSIndexPath indexPath)
		{
		    this._onItemSelected.Invoke(collectionView, indexPath);
		}
	}
}

