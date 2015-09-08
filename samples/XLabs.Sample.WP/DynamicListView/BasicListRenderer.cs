using Xamarin.Forms;

using XLabs.Forms.Controls;
using XLabs.Sample.WP.DynamicListView;

[assembly: ExportRenderer(typeof(DynamicListView<object>), typeof(BasicListRenderer))]

namespace XLabs.Sample.WP.DynamicListView
{
	using System;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Markup;

	using XLabs.Forms.Controls;

	using DataTemplate = System.Windows.DataTemplate;

	public class BasicListRenderer : DynamicListViewRenderer<object>
	{
		private const string Xaml = @"<DataTemplate
					xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
					xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
					xmlns:Views=""clr-namespace:XLLabs.Sample.WP.DynamicListView;assembly=XLabs.Forms.Sample.WP""
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

		protected override DataTemplate Template
		{
			get
			{
				return (DataTemplate)XamlReader.Load(Xaml);
			}
		}

		protected override DataTemplate TemplateForItem(object item)
		{
			return base.TemplateForItem(item);
		}
	}

	public class BasicListContentControl : System.Windows.Controls.ContentControl
	{
		public DataTemplate DateTimeTemplate { get; set; }

		public DataTemplate StringTemplate { get; set; }

		public virtual DataTemplate SelectTemplate(object item, DependencyObject container)
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
