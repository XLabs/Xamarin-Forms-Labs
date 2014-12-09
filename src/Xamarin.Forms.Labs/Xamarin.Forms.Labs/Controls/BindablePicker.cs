using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using XLabs;
using System.Linq;

namespace Xamarin.Forms.Labs.Controls
{
	public class BindablePicker : View
	{
		public BindablePicker()
		{
			this.Items = new ObservableCollection<string>();
			((ObservableCollection<string>)this.Items).CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnItemsCollectionChanged);
			this.SelectedIndexChanged += OnSelectedIndexChanged;
		}

		public Func<object, string> SourceItemLabelConverter { get; set; }

		public static BindableProperty ItemsSourceProperty =
			BindableProperty.Create<BindablePicker, IList>(o => o.ItemsSource, default(IList), propertyChanged: new BindableProperty.BindingPropertyChangedDelegate<IList> (BindablePicker.OnItemsSourceChanged));

		public static BindableProperty SelectedItemProperty =
			BindableProperty.Create<BindablePicker, object>(o => o.SelectedItem, default(object),BindingMode.TwoWay,propertyChanged: new BindableProperty.BindingPropertyChangedDelegate<object> (BindablePicker.OnSelectedItemChanged));

		public IList ItemsSource
		{
			get { return (IList)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		public object SelectedItem
		{
			get { return (object)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		private static void OnItemsSourceChanged(BindableObject bindable, IList oldvalue, IList newvalue)
		{
			var picker = bindable as BindablePicker;
			picker.Items.Clear();
			if (newvalue != null)
			{
				foreach (var item in newvalue)
				{
					if (picker.SourceItemLabelConverter != null)
						picker.Items.Add (picker.SourceItemLabelConverter (item));
					else 
						picker.Items.Add (item.ToString());
				}
			}
		}

		private void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
		{
			if (SelectedIndex < 0 || SelectedIndex > Items.Count - 1)
			{
				SelectedItem = null;
			}
			else
			{

				SelectedItem = ItemsSource[SelectedIndex];
			}
		}

		private static void OnSelectedItemChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			var picker = bindable as BindablePicker;
			if (newvalue != null) {
				string title = string.Empty;
				if (picker.SourceItemLabelConverter != null)
					title = picker.SourceItemLabelConverter (newvalue);
				else
					title = newvalue.ToString ();

				picker.SelectedIndex = picker.Items.IndexOf (title);
			} else {
				picker.SelectedIndex = -1;
			}
		}

		public static readonly BindableProperty TitleProperty = BindableProperty.Create<BindablePicker, string>((BindablePicker w) => w.Title, null, BindingMode.OneWay, null, null, null, null);
		public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create<BindablePicker, int>((BindablePicker w) => w.SelectedIndex, -1, BindingMode.TwoWay, null, delegate(BindableObject bindable, int oldvalue, int newvalue)
			{
				EventHandler selectedIndexChanged = ((BindablePicker)bindable).SelectedIndexChanged;
				if (selectedIndexChanged != null)
				{
					selectedIndexChanged(bindable, EventArgs.Empty);
				}
			}, null, new BindableProperty.CoerceValueDelegate<int>(BindablePicker.CoerceSelectedIndex));

		public event EventHandler SelectedIndexChanged;

		private static int CoerceSelectedIndex(BindableObject bindable, int value)
		{
			BindablePicker picker = (BindablePicker)bindable;
			if (picker.Items != null)
			{
				return value.Clamp(-1, picker.Items.Count-1);
			}
			return -1;
		}

		public string Title
		{
			get
			{
				return (string)base.GetValue(BindablePicker.TitleProperty);
			}
			set
			{
				base.SetValue(BindablePicker.TitleProperty, value);
			}
		}

		public ObservableCollection<string> Items
		{
			get;
			private set;
		}

		public int SelectedIndex
		{
			get
			{
				return (int)base.GetValue(BindablePicker.SelectedIndexProperty);
			}
			set
			{
				base.SetValue(BindablePicker.SelectedIndexProperty, value);
			}
		}

		private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (SelectedItem != null) {
				string title = string.Empty;
				if (SourceItemLabelConverter != null)
					title = SourceItemLabelConverter (SelectedItem);
				else
					title = SelectedItem.ToString ();

				SelectedIndex = Items.IndexOf (title);
			}
		}

		/// <summary>
		/// The HasBorder property
		/// </summary>
		public static readonly BindableProperty HasBorderProperty =
			BindableProperty.Create("HasBorder", typeof(bool), typeof(ExtendedEntry), true);

		/// <summary>
		/// Gets or sets if the border should be shown or not
		/// </summary>
		public bool HasBorder
		{
			get { return (bool)GetValue(HasBorderProperty); }
			set { SetValue(HasBorderProperty, value); }
		}	
	}
}

