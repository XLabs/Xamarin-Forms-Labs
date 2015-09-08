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
        /// The data source
        /// </summary>
        private GridDataSource _dataSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="GridViewRenderer"/> class.
        /// </summary>
        public GridViewRenderer ()
        {
        }

        /// <summary>
        /// Rowses the in section.
        /// </summary>
        /// <param name="collectionView">The collection view.</param>
        /// <param name="section">The section.</param>
        /// <returns>System.Int32.</returns>
        public int RowsInSection(UICollectionView collectionView, nint section)
        {
            return ((ICollection) this.Element.ItemsSource).Count;
        }

        /// <summary>
        /// Items the selected.
        /// </summary>
        /// <param name="tableView">The table view.</param>
        /// <param name="indexPath">The index path.</param>
        public void ItemSelected(UICollectionView tableView, NSIndexPath indexPath)
        {
            var item = this.Element.ItemsSource.Cast<object>().ElementAt(indexPath.Row);
            this.Element.InvokeItemSelectedEvent(this, item);
        }

        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <param name="collectionView">The collection view.</param>
        /// <param name="indexPath">The index path.</param>
        /// <returns>UICollectionViewCell.</returns>
        public UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var item = this.Element.ItemsSource.Cast<object>().ElementAt(indexPath.Row);
            var viewCellBinded = (this.Element.ItemTemplate.CreateContent() as ViewCell);
            if (viewCellBinded == null) return null;

            viewCellBinded.BindingContext = item;
            return this.GetCell(collectionView, viewCellBinded, indexPath);
        }

        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <param name="collectionView">The collection view.</param>
        /// <param name="item">The item.</param>
        /// <param name="indexPath">The index path.</param>
        /// <returns>UICollectionViewCell.</returns>
        protected virtual UICollectionViewCell GetCell(UICollectionView collectionView, ViewCell item, NSIndexPath indexPath)
        {
            var collectionCell = collectionView.DequeueReusableCell(new NSString(GridViewCell.Key), indexPath) as GridViewCell;

            if (collectionCell == null) return null;

            collectionCell.ViewCell = item;

            return collectionCell;
        }

        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged (ElementChangedEventArgs<GridView> e)
        {
            base.OnElementChanged (e);
            if (e.OldElement != null)
            {
                Unbind (e.OldElement);
            }
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var collectionView = new GridCollectionView {
                        AllowsMultipleSelection = false,
                        SelectionEnable = e.NewElement.SelectionEnabled,
                        ContentInset =  new UIEdgeInsets ((float)this.Element.Padding.Top, (float)this.Element.Padding.Left, (float)this.Element.Padding.Bottom, (float)this.Element.Padding.Right),
                        BackgroundColor = this.Element.BackgroundColor.ToUIColor (),
                        ItemSize = new CoreGraphics.CGSize ((float)this.Element.ItemWidth, (float)this.Element.ItemHeight),
                        RowSpacing = this.Element.RowSpacing,
                        ColumnSpacing = this.Element.ColumnSpacing
                    };
                    
                    Bind (e.NewElement);

                    collectionView.Source = this.DataSource;
                    //collectionView.Delegate = this.GridViewDelegate;

                    SetNativeControl (collectionView);
                }
            }

        
        }

        /// <summary>
        /// Unbinds the specified old element.
        /// </summary>
        /// <param name="oldElement">The old element.</param>
        private void Unbind (GridView oldElement)
        {
            if (oldElement == null) return;

            oldElement.PropertyChanging -= this.ElementPropertyChanging;
            oldElement.PropertyChanged -= this.ElementPropertyChanged;
                
            var itemsSource = oldElement.ItemsSource as INotifyCollectionChanged;
            if (itemsSource != null) 
            {
                itemsSource.CollectionChanged -= this.DataCollectionChanged;
            }
        }

        /// <summary>
        /// Binds the specified new element.
        /// </summary>
        /// <param name="newElement">The new element.</param>
        private void Bind (GridView newElement)
        {
            if (newElement == null) return;

            newElement.PropertyChanging += this.ElementPropertyChanging;
            newElement.PropertyChanged += this.ElementPropertyChanged;

            var source = newElement.ItemsSource as INotifyCollectionChanged;
            if (source != null) 
            {
                source.CollectionChanged += this.DataCollectionChanged;
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
                var itemsSource = this.Element.ItemsSource as INotifyCollectionChanged;
                if (itemsSource != null) 
                {
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
                var itemsSource = this.Element.ItemsSource as INotifyCollectionChanged;
                if (itemsSource != null) 
                {
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
            try 
            {
                if(this.Control != null) this.Control.ReloadData();
            } 
            catch { } // todo: determine why we are hiding a possible exception here
        }

        /// <summary>
        /// Gets the data source.
        /// </summary>
        /// <value>The data source.</value>
        private GridDataSource DataSource 
        {
            get 
            {
                return _dataSource ?? (_dataSource = new GridDataSource (GetCell, RowsInSection,ItemSelected));
            }
        }

        protected override void Dispose (bool disposing)
        {
            base.Dispose (disposing);
            if (disposing && _dataSource != null)
            {
                Unbind (Element);
                _dataSource.Dispose ();
                _dataSource = null;
            }
        }
    }
}
