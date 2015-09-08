using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.Sample.WP.DynamicListView;
using Xamarin.Forms.Labs.WP8.Controls;

[assembly: ExportRenderer(typeof(DynamicListView<object>), typeof(BasicListRenderer))]

namespace Xamarin.Forms.Labs.Sample.WP.DynamicListView
{
    public class BasicListRenderer : DynamicListViewRenderer<object>
    {
        private const string Xaml = @"<DataTemplate
                    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                    xmlns:Views=""clr-namespace:Xamarin.Forms.Labs.Sample.WP.DynamicListView;assembly=Xamarin.Forms.Labs.Sample.WP""
                    >
                    <Views:BasicListContentControl Content=""{Binding}"">
                            <Views:BasicListContentControl.DateTimeTemplate>
                                <DataTemplate>
                                    <Grid>
                                        <TextBlock Text='{Binding}' FontSize='30' />
                                    </Grid>
                                </DataTemplate>
                            </Views:BasicListContentControl.DateTimeTemplate>
                            <Views:BasicListContentControl.StringTemplate>
                                <DataTemplate>
                                    <Grid>
                                        <TextBlock Text='{Binding}' FontSize='24' />
                                    </Grid>
                                </DataTemplate>
                            </Views:BasicListContentControl.StringTemplate>
                    </Views:BasicListContentControl>        
                </DataTemplate>";

        protected override System.Windows.DataTemplate Template
        {
            get
            {
                return (System.Windows.DataTemplate)XamlReader.Load(Xaml);
            }
        }

        protected override System.Windows.DataTemplate TemplateForItem(object item)
        {
            return base.TemplateForItem(item);
        }
    }

    public class BasicListContentControl : ContentControl
    {
        public System.Windows.DataTemplate DateTimeTemplate { get; set; }

        public System.Windows.DataTemplate StringTemplate { get; set; }

        public virtual System.Windows.DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is DateTime)
            {
                return DateTimeTemplate;
            }

            if (item is string)
            {
                return StringTemplate;
            }

            return null;
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            ContentTemplate = SelectTemplate(newContent, this);
        }
    }
}
