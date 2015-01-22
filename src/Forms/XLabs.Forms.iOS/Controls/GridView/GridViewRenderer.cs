using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer (typeof(GridView), typeof(GridViewRenderer))]
namespace XLabs.Forms.Controls
{
	using System;
	using System.Collections;
	using System.Collections.Specialized;
	using System.Linq;

	using Foundation;
	using UIKit;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	/// <summary>
	/// Class GridViewRenderer.
	/// </summary>
	public class GridViewRenderer: ViewRenderer<GridView,GridCollectionView>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GridViewRenderer"/> class.
		/// </summary>
		public GridViewRenderer ()
		{
		}

		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged (ElementChangedEventArgs<GridView> e)
		{
			base.OnElementChanged (e);

			var collectionView = new GridCollectionView ();
			collectionView.AllowsMultipleSelection = false;
			collectionView.SelectionEnable = e.NewElement.SelectionEnabled;
			//set padding
			collectionView.ContentInset = new UIEdgeInsets ((float)Element.Padding.Top, (float)Element.Padding.Left, (float)Element.Padding.Bottom, (float)Element.Padding.Right);

			collectionView.BackgroundColor = Element.BackgroundColor.ToUIColor ();
			collectionView.ItemSize = new CoreGraphics.CGSize ((float)Element.ItemWidth, (float)Element.ItemHeight);
			collectionView.RowSpacing = Element.RowSpacing;
			collectionView.ColumnSpacing = Element.ColumnSpacing;

			Unbind (e.OldElement);
			Bind (e.NewElement);

			collectionView.Source = DataSource;
			//collectionView.Delegate = this.GridViewDelegate;

			SetNativeControl (collectionView);

		}

		/// <summary>
		/// Unbinds the specified old element.
		/// </summary>
		/// <param name="oldElement">The old element.</param>
		private void Unbind (GridView oldElement)
		{
			if (oldElement != null) {
				oldElement.PropertyChanging += ElementPropertyChanging;
				oldElement.PropertyChanged -= ElementPropertyChanged;
				
				var itemsSource = oldElement.ItemsSource as INotifyCollectionChanged;
				if (itemsSource != null) {
					itemsSource.CollectionChanged -= DataCollectionChanged;
				}
			}
		}

		/// <summary>
		/// Binds the specified new element.
		/// </summary>
		/// <param name="newElement">The new element.</param>
		private void Bind (GridView newElement)
		{
			if (newElement != null) {
				newElement.PropertyChanging += ElementPropertyChanging;
				newElement.PropertyChanged += ElementPropertyChanged;
				if (newElement.ItemsSource is INotifyCollectionChanged) {
					(newElement.ItemsSource as INotifyCollectionChanged).CollectionChanged += DataCollectionChanged;
				}
			}
		}

		/// <summary>
		/// Elements the property changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
		private void ElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "ItemsSource")
			{
				var itemsSource = Element.ItemsSource as INotifyCollectionChanged;
				if (itemsSource != null) {
					itemsSource.CollectionChanged -= DataCollectionChanged;
				}
			}
		}

		/// <summary>
		/// Elements the property changing.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="PropertyChangingEventArgs"/> instance containing the event data.</param>
		private void ElementPropertyChanging (object sender, PropertyChangingEventArgs e)
		{
			if (e.PropertyName == "ItemsSource")
			{
				var itemsSource = Element.ItemsSource as INotifyCollectionChanged;
				if (itemsSource != null) {
					itemsSource.CollectionChanged += DataCollectionChanged;
				}
			}
		}

		/// <summary>
		/// Datas the collection changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
		private void DataCollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			try {
				if(Control != null)
					Control.ReloadData();
			} catch (Exception ex) {

			}
		}

		/// <summary>
		/// The _data source
		/// </summary>
		private GridDataSource _dataSource;

		/// <summary>
		/// Gets the data source.
		/// </summary>
		/// <value>The data source.</value>
		private GridDataSource DataSource {
			get {
				return _dataSource ??
				(_dataSource =
						new GridDataSource (GetCell, RowsInSection,ItemSelected));
			}
		}

		/// <summary>
		/// The _grid view delegate
		/// </summary>
		private GridViewDelegate _gridViewDelegate;

		/// <summary>
		/// Gets the grid view delegate.
		/// </summary>
		/// <value>The grid view delegate.</value>
		private GridViewDelegate GridViewDelegate {
			get {
				return _gridViewDelegate ??
				(_gridViewDelegate =
						new GridViewDelegate (ItemSelected));
			}
		}

		/// <summary>
		/// Rowses the in section.
		/// </summary>
		/// <param name="collectionView">The collection view.</param>
		/// <param name="section">The section.</param>
		/// <returns>System.Int32.</returns>
		public int RowsInSection (UICollectionView collectionView, nint section)
		{
            if (Element.ItemsSource == null)
            {
                return 0;
            }

			return ((ICollection)Element.ItemsSource).Count;
		}

		/// <summary>
		/// Items the selected.
		/// </summary>
		/// <param name="tableView">The table view.</param>
		/// <param name="indexPath">The index path.</param>
		public void ItemSelected (UICollectionView tableView, NSIndexPath indexPath)
		{
			var item = Element.ItemsSource.Cast<object>().ElementAt(indexPath.Row);
			Element.InvokeItemSelectedEvent(this, item);

		}

		/// <summary>
		/// Gets the cell.
		/// </summary>
		/// <param name="collectionView">The collection view.</param>
		/// <param name="indexPath">The index path.</param>
		/// <returns>UICollectionViewCell.</returns>
		public UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath)
		{
            if (indexPath == null)
            {
                return null;
            }

			var item = Element.ItemsSource.Cast<object> ().ElementAt (indexPath.Row);
			var viewCellBinded = (Element.ItemTemplate.CreateContent () as ViewCell);
			if (viewCellBinded != null)
			{
				viewCellBinded.BindingContext = item;

				return GetCell (collectionView, viewCellBinded, indexPath);
			}

			return null;
		}

		/// <summary>
		/// Gets the cell.
		/// </summary>
		/// <param name="collectionView">The collection view.</param>
		/// <param name="item">The item.</param>
		/// <param name="indexPath">The index path.</param>
		/// <returns>UICollectionViewCell.</returns>
		protected virtual UICollectionViewCell GetCell (UICollectionView collectionView, ViewCell item, NSIndexPath indexPath)
		{
			var collectionCell = collectionView.DequeueReusableCell (new NSString (GridViewCell.Key), indexPath) as GridViewCell;

			if (collectionCell != null)
			{
				collectionCell.ViewCell = item;

				return collectionCell as UICollectionViewCell;
			}

			return null;
		}
		   }
}
