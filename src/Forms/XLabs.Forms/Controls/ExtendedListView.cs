using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
    public class ExtendedListView : ListView
    {
        public static readonly BindableProperty ItemTemplateSelectorProperty = BindableProperty.Create<ExtendedListView, DataTemplateSelector>(x => x.ItemTemplateSelector, default(DataTemplateSelector), propertyChanged: OnDataTemplateSelectorChanged);

        private DataTemplateSelector currentItemSelector;
        public DataTemplateSelector ItemTemplateSelector
        {
            get
            {
                return (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty);
            }
            set
            {
                SetValue(ItemTemplateSelectorProperty, value);
            }
        }

        private static void OnDataTemplateSelectorChanged(BindableObject bindable, DataTemplateSelector oldvalue, DataTemplateSelector newvalue)
        {
            ((ExtendedListView)bindable).OnDataTemplateSelectorChanged(oldvalue, newvalue);
        }

        protected virtual void OnDataTemplateSelectorChanged(DataTemplateSelector oldValue, DataTemplateSelector newValue)
        {
            // check to see we don't have an ItemTemplate set
            if (ItemTemplate != null && newValue != null)
                throw new ArgumentException("Cannot set both ItemTemplate and ItemTemplateSelector", "ItemTemplateSelector");

            currentItemSelector = newValue;
        }

        protected override Cell CreateDefault(object item)
        {
            var cell = this.CellFor(item, currentItemSelector);

            return cell ?? base.CreateDefault(item);
        }
        
    }

}
