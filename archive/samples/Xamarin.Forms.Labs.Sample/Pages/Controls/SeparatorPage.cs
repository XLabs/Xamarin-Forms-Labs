using System;
using Xamarin.Forms.Labs.Controls;

namespace Xamarin.Forms.Labs.Sample
{
	public class SeparatorPage : ContentPage
	{
		public SeparatorPage()
		{
			var mainLayout = new StackLayout();

			var stackLayout = new StackLayout();
			var stackLabel = new Label() {
				Text = "Separators in stack layout"
			};
			stackLayout.Children.Add(stackLabel);
			var separator = new Separator() {
				Color = Color.Red
			};
			stackLayout.Children.Add(separator);
			var stackLabel2 = new Label() {
				Text = "Thicker"
			};
			stackLayout.Children.Add(stackLabel2);
			separator = new Separator() {
				Color = Color.Green,
				Thickness = 5
			};
			stackLayout.Children.Add(separator);
			stackLabel2 = new Label() {
				Text = "Bigger after spacing"
			};
			stackLayout.Children.Add(stackLabel2);
			separator = new Separator() {
				Color = Color.Blue,
				SpacingBefore = 2,
				SpacingAfter = 20
			};
			stackLayout.Children.Add(separator);
			stackLabel2 = new Label() {
				Text = "No spacing"
			};
			stackLayout.Children.Add(stackLabel2);
			separator = new Separator() {
				Color = Color.Blue,
				SpacingBefore = 0,
				SpacingAfter = 0
			};
			stackLayout.Children.Add(separator);
			stackLabel2 = new Label() {
				Text = "Bigger before spacing"
			};
			stackLayout.Children.Add(stackLabel2);
			separator = new Separator() {
				Color = Color.Blue,
				SpacingBefore = 20,
				SpacingAfter = 2
			};
			stackLayout.Children.Add(separator);
			stackLabel2 = new Label() {
				Text = "Dashed"
			};
			stackLayout.Children.Add(stackLabel2);
			separator = new Separator() {
				Color = Color.Red,
				StrokeType = StrokeType.Dashed

			};
			stackLayout.Children.Add(separator);
			stackLabel2 = new Label() {
				Text = "Dotted"
			};
			stackLayout.Children.Add(stackLabel2);
			separator = new Separator() {
				Color = Color.Red,
				StrokeType = StrokeType.Dotted

			};
			stackLayout.Children.Add(separator);

			//stackLayout.VerticalOptions = LayoutOptions.FillAndExpand;
			stackLayout.BackgroundColor = Color.Gray.MultiplyAlpha(0.2);

			mainLayout.Children.Add(stackLayout);

			Content = new ScrollView(){Content =  mainLayout};
		}
	}
}

