namespace XLabs.Sample.Pages.Controls
{
	using Xamarin.Forms;

	using XLabs.Forms.Controls;
	using XLabs.Sample.ViewModel;

	/// <summary>
	/// Class RepeaterViewPage.
	/// </summary>
	public class RepeaterViewPage : ContentPage
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RepeaterViewPage"/> class.
		/// </summary>
		public RepeaterViewPage()
		{
			var viewModel = new RepeaterViewViewModel();
			BindingContext = viewModel;

			var repeater = new RepeaterView
			{
				Spacing = 10,
				ItemsSource = viewModel.Things,
				ItemTemplate = new DataTemplate(() =>
				{
					var nameLabel = new Label { Font = Font.SystemFontOfSize(NamedSize.Medium) };
					nameLabel.SetBinding(Label.TextProperty, RepeaterViewViewModel.ThingsNamePropertyName);

					var descriptionLabel = new Label { Font = Font.SystemFontOfSize(NamedSize.Small) };
					descriptionLabel.SetBinding(Label.TextProperty, RepeaterViewViewModel.ThingsDescriptionPropertyName);

					ViewCell cell = new ViewCell
					{
						View = new StackLayout
						{
							Spacing = 0,
							Children =
							{
								nameLabel,
								descriptionLabel
							}
						}
					};

					return cell;
				})
			};

			var removeButton = new Button
			{
				Text = "Remove 1st Item",      
				HorizontalOptions = LayoutOptions.Start
			};

			removeButton.SetBinding(Button.CommandProperty, RepeaterViewViewModel.RemoveFirstItemCommandName);

			var addButton = new Button
			{
				Text = "Add New Item",
				HorizontalOptions = LayoutOptions.Start
			};

			addButton.SetBinding(Button.CommandProperty, RepeaterViewViewModel.AddItemCommandName);

			Content = new StackLayout
			{
				Padding = 20,
				Spacing = 5,
				Children = 
				{
					new Label 
					{ 
						Text = "RepeaterView Demo", 
						Font = Font.SystemFontOfSize(NamedSize.Large)
					},
					repeater,
					removeButton,
					addButton
				}
			};

			viewModel.LoadData();
		}
	}
}
