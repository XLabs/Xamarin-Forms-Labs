using System;

namespace XLabs.Forms.Controls
{
    using Foundation;
    using UIKit;

    /// <summary>
    /// Class GridDataSource.
    /// </summary>
    public class GridDataSource : UICollectionViewSource
    {
        /// <summary>
        /// Delegate OnGetCell
        /// </summary>
        /// <param name="collectionView">The collection view.</param>
        /// <param name="indexPath">The index path.</param>
        /// <returns>UICollectionViewCell.</returns>
        public delegate UICollectionViewCell OnGetCell(UICollectionView collectionView, NSIndexPath indexPath);

        /// <summary>
        /// Delegate OnRowsInSection
        /// </summary>
        /// <param name="collectionView">The collection view.</param>
        /// <param name="section">The section.</param>
        /// <returns>System.Int32.</returns>
        public delegate int OnRowsInSection(UICollectionView collectionView, nint section);

        /// <summary>
        /// Delegate OnItemSelected
        /// </summary>
        /// <param name="collectionView">The collection view.</param>
        /// <param name="indexPath">The index path.</param>
        public delegate void OnItemSelected(UICollectionView collectionView, NSIndexPath indexPath);

        /// <summary>
        /// The _on get cell
        /// </summary>
        private readonly OnGetCell _onGetCell;
        /// <summary>
        /// The _on rows in section
        /// </summary>
        private readonly OnRowsInSection _onRowsInSection;
        /// <summary>
        /// The _on item selected
        /// </summary>
        private readonly OnItemSelected _onItemSelected;

        /// <summary>
        /// Initializes a new instance of the <see cref="GridDataSource"/> class.
        /// </summary>
        /// <param name="onGetCell">The on get cell.</param>
        /// <param name="onRowsInSection">The on rows in section.</param>
        /// <param name="onItemSelected">The on item selected.</param>
        public GridDataSource(OnGetCell onGetCell, OnRowsInSection onRowsInSection, OnItemSelected onItemSelected)
        {
            this._onGetCell = onGetCell;
            this._onRowsInSection = onRowsInSection;
            this._onItemSelected = onItemSelected;
        }

        #region implemented abstract members of UICollectionViewDataSource

        /// <summary>
        /// Gets the items count.
        /// </summary>
        /// <param name="collectionView">The collection view.</param>
        /// <param name="section">The section.</param>
        /// <returns>System.Int32.</returns>
        public override nint GetItemsCount (UICollectionView collectionView, nint section)
        {
            return this._onRowsInSection(collectionView, section);
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
        /// Gets the cell.
        /// </summary>
        /// <param name="collectionView">The collection view.</param>
        /// <param name="indexPath">The index path.</param>
        /// <returns>UICollectionViewCell.</returns>
        public override UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = this._onGetCell (collectionView, indexPath);

            if (((GridCollectionView) collectionView).SelectionEnable)
            {
                // todo: investigate whether this needs to be removed when cell is re-used
                cell.AddGestureRecognizer(new UITapGestureRecognizer(v =>
                {
                    ItemSelected(collectionView, indexPath);
                }));
            }
            else
            {
                cell.SelectedBackgroundView = new UIView();
            }

            return cell;
        }

        #endregion
    }
}

