using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer (typeof(GridView), typeof(GridViewRenderer))]
namespace XLabs.Forms.Controls
{
	using System;
	using System.Collections;
	using System.Collections.Specialized;
	using System.Linq;
	using System.Net;

	using Android.Content;
	using Android.Content.Res;
	using Android.Graphics;
	using Android.Views;
	using Android.Widget;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.Android;

	/// <summary>
	/// Class GridViewRenderer.
	/// </summary>
	public class GridViewRenderer : ViewRenderer<GridView, Android.Widget.GridView>
	{
		/// <summary>
		/// The orientation
		/// </summary>
		private readonly Android.Content.Res.Orientation _orientation = Android.Content.Res.Orientation.Undefined;
		/// <summary>
		/// Initializes a new instance of the <see cref="GridViewRenderer"/> class.
		/// </summary>
		public GridViewRenderer ()
		{
		}

		/// <summary>
		/// Called when the current configuration of the resources being used
		/// by the application have changed.
		/// </summary>
		/// <param name="newConfig">The new resource configuration.</param>
		/// <since version="Added in API level 8" />
		/// <remarks><para tool="javadoc-to-mdoc">Called when the current configuration of the resources being used
		/// by the application have changed.  You can use this to decide when
		/// to reload resources that can changed based on orientation and other
		/// configuration characterstics.  You only need to use this if you are
		/// not relying on the normal <c><see cref="T:Android.App.Activity" /></c> mechanism of
		/// recreating the activity instance upon a configuration change.</para>
		/// <para tool="javadoc-to-mdoc">
		///   <format type="text/html">
		///     <a href="http://developer.android.com/reference/android/view/View.html#onConfigurationChanged(android.content.res.Configuration)" target="_blank">[Android Documentation]</a>
		///   </format>
		/// </para></remarks>
		protected override void OnConfigurationChanged(Configuration newConfig)
		{
			base.OnConfigurationChanged(newConfig);
			if (newConfig.Orientation != _orientation)
				OnElementChanged(new ElementChangedEventArgs<GridView>(this.Element, this.Element));
		}

		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged (ElementChangedEventArgs<GridView> e)
		{
			base.OnElementChanged (e);

			var collectionView = new Android.Widget.GridView (Xamarin.Forms.Forms.Context);
			collectionView.SetGravity(GravityFlags.Center);
			collectionView.SetColumnWidth (Convert.ToInt32(Element.ItemWidth));
			collectionView.StretchMode = StretchMode.StretchColumnWidth;
	
			var metrics = Resources.DisplayMetrics;
			var spacing = (int)e.NewElement.ColumnSpacing;
			var width = metrics.WidthPixels;
			var itemWidth = (int)e.NewElement.ItemWidth;

			int noOfColumns = width / (itemWidth + spacing);
			// If possible add another row without spacing (because the number of columns will be one less than the number of spacings)
			if (width - (noOfColumns * (itemWidth + spacing)) >= itemWidth)
				noOfColumns++;

			collectionView.SetNumColumns (noOfColumns);
			collectionView.SetPadding(Convert.ToInt32(Element.Padding.Left),Convert.ToInt32(Element.Padding.Top), Convert.ToInt32(Element.Padding.Right),Convert.ToInt32(Element.Padding.Bottom));

			collectionView.SetBackgroundColor (Element.BackgroundColor.ToAndroid ());
			collectionView.SetHorizontalSpacing (Convert.ToInt32(Element.RowSpacing));
			collectionView.SetVerticalSpacing(Convert.ToInt32(Element.ColumnSpacing));

			this.Unbind(e.OldElement);
			this.Bind(e.NewElement);

			collectionView.Adapter = this.DataSource;

			collectionView.ItemClick += CollectionViewItemClick;

			base.SetNativeControl(collectionView);

		}


		/// <summary>
		/// Handles the ItemClick event of the collectionView control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="AdapterView.ItemClickEventArgs"/> instance containing the event data.</param>
		void CollectionViewItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			var item = this.Element.ItemsSource.Cast<object>().ElementAt(e.Position);
			this.Element.InvokeItemSelectedEvent(this, item);
		}

		/// <summary>
		/// Unbinds the specified old element.
		/// </summary>
		/// <param name="oldElement">The old element.</param>
		private void Unbind(GridView oldElement)
		{
			if (oldElement != null)
			{
				oldElement.PropertyChanging += ElementPropertyChanging;
				oldElement.PropertyChanged -= ElementPropertyChanged;
				if (oldElement.ItemsSource is INotifyCollectionChanged) {
					(oldElement.ItemsSource as INotifyCollectionChanged).CollectionChanged -= DataCollectionChanged;
				}
			}
		}

		/// <summary>
		/// Binds the specified new element.
		/// </summary>
		/// <param name="newElement">The new element.</param>
		private void Bind(GridView newElement)
		{
			if (newElement != null)
			{
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
		private void ElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "ItemsSource")
			{
				if (this.Element.ItemsSource is INotifyCollectionChanged) {
					(this.Element.ItemsSource as INotifyCollectionChanged).CollectionChanged -= DataCollectionChanged;
				}
			}
		}
		/// <summary>
		/// Elements the property changing.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="PropertyChangingEventArgs"/> instance containing the event data.</param>
		private void ElementPropertyChanging(object sender, PropertyChangingEventArgs e)
		{
			if (e.PropertyName == "ItemsSource")
			{
				if (this.Element.ItemsSource is INotifyCollectionChanged) {
					(this.Element.ItemsSource as INotifyCollectionChanged).CollectionChanged += DataCollectionChanged;
				}
			}
		}

		/// <summary>
		/// Datas the collection changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
		private void DataCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			//  Control.ReloadData();
		}

		/// <summary>
		/// The data source
		/// </summary>
		private GridDataSource _dataSource;
		/// <summary>
		/// Gets the data source.
		/// </summary>
		/// <value>The data source.</value>
		private GridDataSource DataSource
		{
			get
			{
				return _dataSource ??
					(_dataSource =
						new GridDataSource( this.GetCell,this.RowsInSection));
			}
		}

		/// <summary>
		/// Rowses the in section.
		/// </summary>
		/// <returns>System.Int32.</returns>
		public int RowsInSection()
		{
			return (this.Element.ItemsSource as ICollection).Count;
		}

		/// <summary>
		/// Gets the cell.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="convertView">The convert view.</param>
		/// <param name="parent">The parent.</param>
		/// <returns>Android.Views.View.</returns>
		public  global::Android.Views.View GetCell(int position, global::Android.Views.View convertView, ViewGroup parent)
		{
			var item = this.Element.ItemsSource.Cast<object>().ElementAt(position);
			var viewCellBinded = (Element.ItemTemplate.CreateContent () as ViewCell);
			viewCellBinded.BindingContext = item;
			var view = RendererFactory.GetRenderer (viewCellBinded.View);
			// Platform.SetRenderer (viewCellBinded.View, view);
			view.ViewGroup.LayoutParameters = new  Android.Widget.GridView.LayoutParams (Convert.ToInt32(this.Element.ItemWidth), Convert.ToInt32(this.Element.ItemHeight));
			view.ViewGroup.SetBackgroundColor (global::Android.Graphics.Color.Blue);
			return view.ViewGroup;
			//                this.AddView (this.view.ViewGroup);

			//            GridViewCellRenderer render = new GridViewCellRenderer ();
			//           
			//            return render.GetCell (viewCellBinded, convertView, parent, this.Context);;
			//          //  view.LayoutParameters = new GridView.LayoutParams (this.Element.ItemWidth,this.Element.ItemHeight);
			//            return view;
			//            ImageView imageView;
			//
			//            if (convertView == null) {  // if it's not recycled, initialize some attributes
			//                imageView = new ImageView (Forms.Context);
			//                imageView.LayoutParameters = new GridView.LayoutParams (85, 85);
			//                imageView.SetScaleType (ImageView.ScaleType.CenterCrop);
			//                imageView.SetPadding (8, 8, 8, 8);
			//            } else {
			//                imageView = (ImageView)convertView;
			//            }
			//            var imageBitmap = GetImageBitmapFromUrl("http://xamarin.com/resources/design/home/devices.png");
			//            imageView.SetImageBitmap(imageBitmap);
			//            return imageView;
		}
		/// <summary>
		/// Gets the image bitmap from URL.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <returns>Bitmap.</returns>
		private Bitmap GetImageBitmapFromUrl(string url)
		{
			Bitmap imageBitmap = null;

			using (var webClient = new WebClient())
			{
				var imageBytes = webClient.DownloadData(url);
				if (imageBytes != null && imageBytes.Length > 0)
				{
					imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
				}
			}

			return imageBitmap;
		}
	}

	public class GridDataSource : BaseAdapter
	{
		Context _context;

		public delegate global::Android.Views.View OnGetCell(int position, global::Android.Views.View convertView, ViewGroup parent);
		public delegate int OnRowsInSection();

		private readonly OnGetCell _onGetCell;
		private readonly OnRowsInSection _onRowsInSection;

		public GridDataSource(OnGetCell onGetCell, OnRowsInSection onRowsInSection)
		{
			this._onGetCell = onGetCell;
			this._onRowsInSection = onRowsInSection;
		}

		public GridDataSource (Context c)
		{
			_context = c;
		}

		public override int Count {
			get { return _onRowsInSection (); }
		}

		public override Java.Lang.Object GetItem (int position)
		{
			return null;
		}

		public override long GetItemId (int position)
		{
			return 0;
		}

		public override global::Android.Views.View GetView (int position, global::Android.Views.View convertView, ViewGroup parent)
		{
			return _onGetCell (position, convertView, parent);
		}

	}

	public class GridViewCellRenderer : CellRenderer
	{
		//
		// Methods
		//
		protected override global::Android.Views.View GetCellCore (Cell item, global::Android.Views.View convertView, ViewGroup parent, Context context)
		{
			ViewCell viewCell = (ViewCell)item;
			GridViewCellRenderer.ViewCellContainer viewCellContainer = convertView as GridViewCellRenderer.ViewCellContainer;
			if (viewCellContainer != null) {
				viewCellContainer.Update (viewCell);
				return viewCellContainer;
			}

			IVisualElementRenderer renderer = RendererFactory.GetRenderer (viewCell.View);
			//   Platform.SetRenderer (viewCell.View, renderer);
			// viewCell.View.IsPlatformEnabled = true;
			return new GridViewCellRenderer.ViewCellContainer (context, renderer, viewCell,parent);
		}

		//
		// Nested Types
		//
		private class ViewCellContainer : ViewGroup
		{

			IVisualElementRenderer _view;
			global::Android.Views.View _parent;
			ViewCell _viewCell;
			public ViewCellContainer (Context context, IVisualElementRenderer view, ViewCell viewCell, global::Android.Views.View parent) : base (context)
			{

				this._view = view;
				this._parent = parent;
				//                this.unevenRows = unevenRows;
				//                this.rowHeight = rowHeight;
				this._viewCell = viewCell;
				this.AddView (view.ViewGroup);
			}

			public void Update (ViewCell cell)
			{
				IVisualElementRenderer visualElementRenderer = this.GetChildAt (0) as IVisualElementRenderer;
				//              Type type = Registrar.Registered.GetHandlerType (cell.View.GetType ()) ?? typeof(RendererFactory.DefaultRenderer);
				//                if (visualElementRenderer != null && visualElementRenderer.GetType () == type) {
				//                    this.viewCell = cell;
				//                    visualElementRenderer.SetElement (cell.View);
				//                    Platform.SetRenderer (cell.View, this.view);
				//                    cell.View.IsPlatformEnabled = true;
				//                    this.Invalidate ();
				//                    return;
				//                }
				//                this.RemoveView (this.view.ViewGroup);
				//                Platform.SetRenderer (this.viewCell.View, null);
				//                this.viewCell.View.IsPlatformEnabled = false;
				//                this.view.ViewGroup.Dispose ();
				//                this.viewCell = cell;
				//                this.view = RendererFactory.GetRenderer (this.viewCell.View);
				//                Platform.SetRenderer (this.viewCell.View, this.view);
				//                this.AddView (this.view.ViewGroup);
			}
			//
			protected override void OnLayout (bool changed, int l, int t, int r, int b)
			{
				double width = base.Context.FromPixels ((double)(r - l));
				double height = base.Context.FromPixels ((double)(b - t));
				this._view.Element.Layout (new Rectangle (0, 0, width, height));
				this._view.UpdateLayout ();
			}
			//
			//            protected override void OnMeasure (int widthMeasureSpec, int heightMeasureSpec)
			//            {
			//                int size = View.MeasureSpec.GetSize (widthMeasureSpec);
			//                int measuredHeight;
			//               measuredHeight = (int)base.Context.ToPixels ((this.ParentRowHeight == -1) ? 44 : ((double)this.ParentRowHeight));
			//                base.SetMeasuredDimension (size, measuredHeight);
			//            }
		}
	}
}