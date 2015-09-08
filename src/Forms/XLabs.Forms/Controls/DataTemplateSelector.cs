using System;
using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
    public class DataTemplateSelector
    {
        public virtual DataTemplate SelectTemplate(object item, BindableObject container)
        {
            return null;
        }

    }

    public static class DataTemplateSelectorExtensions
    {
        public static Cell CellFor(this BindableObject This, object item, DataTemplateSelector selector)
        {
            if (selector != null)
            {
                var template = selector.SelectTemplate(item, This);
                if (template != null)
                {
                    var templateInstance = template.CreateContent();
                    // see if it's a view or a cell
                    var templateView = templateInstance as View;
                    var templateCell = templateInstance as Cell;

                    if (templateView == null && templateCell == null)
                        throw new InvalidOperationException("DataTemplate must be either a Cell or a View");

                    if (templateView != null) // we got a view, wrap in a cell
                        templateCell = new ViewCell { View = templateView };

                    return templateCell;
                }
            }

            return null;
        }

        public static View ViewFor(this BindableObject This, object item, DataTemplateSelector selector)
        {
            if (selector != null)
            {
                var template = selector.SelectTemplate(item, This);
                if (template != null)
                {
                    var templateInstance = template.CreateContent();
                    // see if it's a view or a cell
                    var templateView = templateInstance as View;

                    if (templateView == null)
                        throw new InvalidOperationException("DataTemplate must be a View");

                    return templateView;
                }
            }

            return null;
        }
    }
}