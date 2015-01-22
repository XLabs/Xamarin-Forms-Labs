using System;
using System.Collections;
using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class GridView.
	/// </summary>
	public class GridView : ContentView
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GridView"/> class.
		/// </summary>
		public GridView ()
		{
			SelectionEnabled = true;
		}


		//
		// Static Fields
		//
		/// <summary>
		/// The items source property
		/// </summary>
		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create ("ItemsSource", typeof(IEnumerable), typeof(GridView), null, BindingMode.OneWay, null, null, null, null);

        /// <summary>
        /// The selected item property
        /// </summary>
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create("SelectedItem", typeof(object), typeof(GridView), null, BindingMode.TwoWay, null, null, null, null);
        
        /// <summary>
		/// The item template property
		/// </summary>
		public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create ("ItemTemplate", typeof(DataTemplate), typeof(GridView), null, BindingMode.OneWay, null, null, null, null);

		/// <summary>
		/// The row spacing property
		/// </summary>
		public static readonly BindableProperty RowSpacingProperty = BindableProperty.Create ("RowSpacing", typeof(double), typeof(GridView), (double)0, BindingMode.OneWay, null, null, null, null);

		/// <summary>
		/// The column spacing property
		/// </summary>
		public static readonly BindableProperty ColumnSpacingProperty = BindableProperty.Create ("ColumnSpacing", typeof(double), typeof(GridView), (double)0, BindingMode.OneWay, null, null, null, null);

		/// <summary>
		/// The item width property
		/// </summary>
		public static readonly BindableProperty ItemWidthProperty = BindableProperty.Create ("ItemWidth", typeof(double), typeof(GridView), (double)100, BindingMode.OneWay, null, null, null, null);

		/// <summary>
		/// The item height property
		/// </summary>
		public static readonly BindableProperty ItemHeightProperty = BindableProperty.Create ("ItemHeight", typeof(double), typeof(GridView), (double)100, BindingMode.OneWay, null, null, null, null);

		//
		// Properties
		//
		/// <summary>
		/// Gets or sets the items source.
		/// </summary>
		/// <value>The items source.</value>
		public IEnumerable ItemsSource {
			get {
				return (IEnumerable)base.GetValue (GridView.ItemsSourceProperty);
			}
			set {
				base.SetValue (GridView.ItemsSourceProperty, value);
			}
		}

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>The selected item.</value>
        public object SelectedItem
        {
            get
            {
                return (object)base.GetValue(GridView.SelectedItemProperty);
            }
            set
            {
                base.SetValue(GridView.SelectedItemProperty, value);
            }
        }

		/// <summary>
		/// Gets or sets the item template.
		/// </summary>
		/// <value>The item template.</value>
		public DataTemplate ItemTemplate {
			get {
				return (DataTemplate)base.GetValue (GridView.ItemTemplateProperty);
			}
			set {
				base.SetValue (GridView.ItemTemplateProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the row spacing.
		/// </summary>
		/// <value>The row spacing.</value>
		public double RowSpacing {
			get {
				return (double)base.GetValue (GridView.RowSpacingProperty);
			}
			set {
				base.SetValue (GridView.RowSpacingProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the column spacing.
		/// </summary>
		/// <value>The column spacing.</value>
		public double ColumnSpacing {
			get {
				return (double)base.GetValue (GridView.ColumnSpacingProperty);
			}
			set {
				base.SetValue (GridView.ColumnSpacingProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the width of the item.
		/// </summary>
		/// <value>The width of the item.</value>
		public double ItemWidth {
			get {
				return (double)base.GetValue (GridView.ItemWidthProperty);
			}
			set {
				base.SetValue (GridView.ItemWidthProperty, value);
			}
		}

		/// <summary>
		/// Gets or sets the height of the item.
		/// </summary>
		/// <value>The height of the item.</value>
		public double ItemHeight {
			get {
				return (double)base.GetValue (GridView.ItemHeightProperty);
			}
			set {
				base.SetValue (GridView.ItemHeightProperty, value);
			}
		}

		/// <summary>
		/// Occurs when item is selected.
		/// </summary>
		public event EventHandler<EventArgs<object>> ItemSelected;

		/// <summary>
		/// Invokes the item selected event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="item">Item.</param>
		public void InvokeItemSelectedEvent (object sender, object item)
		{
            this.SelectedItem = item;

			if (this.ItemSelected != null) {
				this.ItemSelected.Invoke (sender, new EventArgs<object> (item));
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether [selection enabled].
		/// </summary>
		/// <value><c>true</c> if [selection enabled]; otherwise, <c>false</c>.</value>
		public bool SelectionEnabled {
			get;
			set;
		}
	}
}
