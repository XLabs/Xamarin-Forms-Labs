using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Controls
{
    public class RepeaterView<T> : StackLayout
    {
        public RepeaterView()
        {
            this.Spacing = 0;
        }

        public ObservableCollection<T> ItemsSource
        {
            get { return (ObservableCollection<T>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create<RepeaterView<T>, ObservableCollection<T>>(p => p.ItemsSource, new ObservableCollection<T>(), BindingMode.OneWay, null, ItemsChanged);

        private static void ItemsChanged(BindableObject bindable, ObservableCollection<T> oldValue, ObservableCollection<T> newValue)
        {
            var control = bindable as RepeaterView<T>;
            control.ItemsSource.CollectionChanged += control.ItemsSource_CollectionChanged;
            control.Children.Clear();

            foreach (var item in newValue)
            {
                var cell = control.ItemTemplate.CreateContent();
                var view = ((ViewCell)cell).View;
                view.BindingContext = item;
                control.Children.Add(view);
            }
        }

        void ItemsSource_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset) 
			{
                this.Children.Clear();
                this.UpdateChildrenLayout();
                this.InvalidateLayout();
            } else {
                if (e.OldItems != null) {
	                this.Children.RemoveAt(e.OldStartingIndex);
	                this.UpdateChildrenLayout();
	                this.InvalidateLayout();
	            }

	            if (e.NewItems != null)
	            {
	                foreach (T item in e.NewItems)
	                {
	                    var cell = this.ItemTemplate.CreateContent();
	                        var viewCell = (ViewCell) cell;
	                        viewCell.BindingContext = item;
	                        viewCell.View.BindingContext = item;
	                        this.Children.Insert(ItemsSource.IndexOf(item), viewCell.View);
	                }

	                this.UpdateChildrenLayout();
	                this.InvalidateLayout();
	            }
	        }
        }

        public static readonly BindableProperty ItemTemplateProperty =
        BindableProperty.Create<RepeaterView<T>, DataTemplate>(p => p.ItemTemplate, default(DataTemplate));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
    }
}
