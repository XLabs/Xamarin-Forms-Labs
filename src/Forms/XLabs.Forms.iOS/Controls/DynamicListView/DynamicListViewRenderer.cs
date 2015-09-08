using System;

namespace XLabs.Forms.Controls
{
	using Foundation;
	using UIKit;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	/// <summary>
	/// Class DynamicUITableViewRenderer.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class DynamicUITableViewRenderer<T> : ViewRenderer<DynamicListView<T>, UITableView>
	{
		/// <summary>
		/// The _default row height
		/// </summary>
		private const float DEFAULT_ROW_HEIGHT = 22;

		/// <summary>
		/// The _table delegate
		/// </summary>
		private TableViewDelegate _tableDelegate;
		/// <summary>
		/// The _data source
		/// </summary>
		private TableDataSource _dataSource;
		/// <summary>
		/// The _table view
		/// </summary>
		private UITableView _tableView;

		/// <summary>
		/// Gets the data source.
		/// </summary>
		/// <value>The data source.</value>
		private TableDataSource DataSource
		{
			get
			{
				return _dataSource ??
					(_dataSource =
					new TableDataSource(GetCell, RowsInSection));
			}
		}

		/// <summary>
		/// Gets the table delegate.
		/// </summary>
		/// <value>The table delegate.</value>
		private TableViewDelegate TableDelegate
		{
			get
			{
				return _tableDelegate ??
					(_tableDelegate =
					new TableViewDelegate(RowSelected, GetHeightForRow));
			}
		}

		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<DynamicListView<T>> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement == null)
			{
				_tableView = new UITableView();
				SetNativeControl(_tableView);
			}

			Unbind(e.OldElement);
			Bind(e.NewElement);

			_tableView.DataSource = DataSource;
			_tableView.Delegate = TableDelegate;
		}

		/// <summary>
		/// Unbinds the specified old element.
		/// </summary>
		/// <param name="oldElement">The old element.</param>
		private void Unbind(DynamicListView<T> oldElement)
		{
			if (oldElement != null)
			{
				oldElement.PropertyChanging += ElementPropertyChanging;
				oldElement.PropertyChanged -= ElementPropertyChanged;
				oldElement.Data.CollectionChanged += DataCollectionChanged;
			}
		}

		/// <summary>
		/// Binds the specified new element.
		/// </summary>
		/// <param name="newElement">The new element.</param>
		private void Bind(DynamicListView<T> newElement)
		{
			if (newElement != null)
			{
				newElement.PropertyChanging += ElementPropertyChanging;
				newElement.PropertyChanged += ElementPropertyChanged;
				newElement.Data.CollectionChanged += DataCollectionChanged;
			}
		}

		/// <summary>
		/// Elements the property changing.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="PropertyChangingEventArgs"/> instance containing the event data.</param>
		private void ElementPropertyChanging(object sender, PropertyChangingEventArgs e)
		{
			if (e.PropertyName == "Data")
			{
				Element.Data.CollectionChanged -= DataCollectionChanged;
			}
		}

		/// <summary>
		/// Datas the collection changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
		private void DataCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			_tableView.ReloadData();
		}

		/// <summary>
		/// Elements the property changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
		private void ElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Data")
			{
				Element.Data.CollectionChanged += DataCollectionChanged;
			}
		}

		#region UITableView weak delegate
		//[Export("tableView:cellForRowAtIndexPath:")]
		/// <summary>
		/// Gets cell for UITableView
		/// </summary>
		/// <param name="tableView">The table view.</param>
		/// <param name="indexPath">The index path.</param>
		/// <returns>The <see cref="UITableViewCell" />.</returns>
		public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var item = Element.Data[indexPath.Row];
			return GetCell(tableView, item);
		}

		/// <summary>
		/// Gets the cell.
		/// </summary>
		/// <param name="tableView">The table view.</param>
		/// <param name="item">The item.</param>
		/// <returns>UITableViewCell.</returns>
		protected virtual UITableViewCell GetCell(UITableView tableView, T item)
		{
			var cell = tableView.DequeueReusableCell(GetType().Name) ?? new UITableViewCell(UITableViewCellStyle.Value1, GetType().Name);

			cell.TextLabel.Text = item.ToString();
			return cell;
		}

		//[Export("tableView:numberOfRowsInSection:")]
		/// <summary>
		/// The rows in section.
		/// </summary>
		/// <param name="tableView">The table view.</param>
		/// <param name="section">The section.</param>
		/// <returns>The <see cref="int" />.</returns>
		public int RowsInSection(UITableView tableView, nint section)
		{
			return Element.Data.Count;
		}

		//[Export("tableView:didSelectRowAtIndexPath:")]
		/// <summary>
		/// Rows the selected.
		/// </summary>
		/// <param name="tableView">The table view.</param>
		/// <param name="indexPath">The index path.</param>
		public void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			Element.InvokeItemSelectedEvent(tableView, Element.Data[(int)indexPath.Item]);
		}

		//[Export("tableView:heightForRowAtIndexPath:")]
		/// <summary>
		/// Gets the height for row.
		/// </summary>
		/// <param name="tableView">The table view.</param>
		/// <param name="indexPath">The index path.</param>
		/// <returns>System.Single.</returns>
		public virtual float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return GetHeightForRow(tableView, Element.Data[(int)indexPath.Item]);
		}

		/// <summary>
		/// Gets the height for row.
		/// </summary>
		/// <param name="tableView">The table view.</param>
		/// <param name="item">The item.</param>
		/// <returns>System.Single.</returns>
		protected virtual float GetHeightForRow(UITableView tableView, T item)
		{
			return DEFAULT_ROW_HEIGHT;
		}
		#endregion

		/// <summary>
		/// Class TableDataSource.
		/// </summary>
		private class TableDataSource : UITableViewDataSource
		{
			/// <summary>
			/// Delegate OnGetCell
			/// </summary>
			/// <param name="tableView">The table view.</param>
			/// <param name="indexPath">The index path.</param>
			/// <returns>UITableViewCell.</returns>
			public delegate UITableViewCell OnGetCell(UITableView tableView, NSIndexPath indexPath);
			/// <summary>
			/// Delegate OnRowsInSection
			/// </summary>
			/// <param name="tableView">The table view.</param>
			/// <param name="section">The section.</param>
			/// <returns>System.Int32.</returns>
			public delegate int OnRowsInSection(UITableView tableView, nint section);

			/// <summary>
			/// The _on get cell
			/// </summary>
			private readonly OnGetCell _onGetCell;
			/// <summary>
			/// The _on rows in section
			/// </summary>
			private readonly OnRowsInSection _onRowsInSection;

			/// <summary>
			/// Initializes a new instance of the <see cref="TableDataSource" /> class.
			/// </summary>
			/// <param name="onGetCell">The on get cell.</param>
			/// <param name="onRowsInSection">The on rows in section.</param>
			public TableDataSource(OnGetCell onGetCell, OnRowsInSection onRowsInSection)
			{
				_onGetCell = onGetCell;
				_onRowsInSection = onRowsInSection;
			}

			/// <summary>
			/// Gets the cell.
			/// </summary>
			/// <param name="tableView">The table view.</param>
			/// <param name="indexPath">The index path.</param>
			/// <returns>UITableViewCell.</returns>
			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				return _onGetCell(tableView, indexPath);
			}

			/// <summary>
			/// Rowses the in section.
			/// </summary>
			/// <param name="tableView">The table view.</param>
			/// <param name="section">The section.</param>
			/// <returns>System.Int32.</returns>
			public override nint RowsInSection(UITableView tableView, nint section)
			{
				return _onRowsInSection(tableView, section);
			}
		}

		/// <summary>
		/// Class TableViewDelegate.
		/// </summary>
		private class TableViewDelegate : UITableViewDelegate
		{
			/// <summary>
			/// Delegate OnRowSelected
			/// </summary>
			/// <param name="tableView">The table view.</param>
			/// <param name="indexPath">The index path.</param>
			public delegate void OnRowSelected(UITableView tableView, NSIndexPath indexPath);
			/// <summary>
			/// Delegate OnGetHeightForRow
			/// </summary>
			/// <param name="tableView">The table view.</param>
			/// <param name="indexPath">The index path.</param>
			/// <returns>System.Single.</returns>
			public delegate float OnGetHeightForRow(UITableView tableView, NSIndexPath indexPath);

			/// <summary>
			/// The _on row selected
			/// </summary>
			private readonly OnRowSelected _onRowSelected;
			/// <summary>
			/// The _on get height for row
			/// </summary>
			private readonly OnGetHeightForRow _onGetHeightForRow;

			/// <summary>
			/// Initializes a new instance of the <see cref="TableViewDelegate" /> class.
			/// </summary>
			/// <param name="onRowSelected">The on row selected.</param>
			/// <param name="onGetHeightForRow">The on get height for row.</param>
			public TableViewDelegate(OnRowSelected onRowSelected, OnGetHeightForRow onGetHeightForRow)
			{
				_onRowSelected = onRowSelected;
				_onGetHeightForRow = onGetHeightForRow;
			}

			/// <summary>
			/// Gets the height for row.
			/// </summary>
			/// <param name="tableView">The table view.</param>
			/// <param name="indexPath">The index path.</param>
			/// <returns>System.Single.</returns>
			public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
			{
				return _onGetHeightForRow(tableView, indexPath);
			}

			/// <summary>
			/// Rows the selected.
			/// </summary>
			/// <param name="tableView">The table view.</param>
			/// <param name="indexPath">The index path.</param>
			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
				_onRowSelected(tableView, indexPath);
			}
		}
	}
}