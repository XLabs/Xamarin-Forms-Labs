using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// A view that renders its content based on a data template. Typical usage is to either set an explicit 
    /// <see cref="BindableObject.BindingContext"/> on this element or use an inhereted one, then set a display.
    /// </summary>
    public class ContentControl : ContentView
    {
        public static readonly BindableProperty ContentTemplateProperty = BindableProperty.Create<ContentControl, DataTemplate>(x => x.ContentTemplate, null, propertyChanged: OnContentTemplateChanged);

        private static void OnContentTemplateChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var cp = (ContentControl)bindable;

            var template = cp.ContentTemplate;
            if (template != null)
            {
                var content = (View)template.CreateContent();
                cp.Content = content;
            }
            else
            {
                cp.Content = null;
            }
        }

        /// <summary>
        /// A <see cref="DataTemplate"/> used to render the view. This property is bindable.
        /// </summary>
        public DataTemplate ContentTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ContentTemplateProperty);
            }
            set
            {
                SetValue(ContentTemplateProperty, value);
            }
        }

    }
}
