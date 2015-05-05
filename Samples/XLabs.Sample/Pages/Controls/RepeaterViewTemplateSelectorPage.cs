namespace XLabs.Sample.Pages.Controls
{
    using Xamarin.Forms;

    using XLabs.Forms.Controls;
    using XLabs.Sample.ViewModel;

    /// <summary>
    /// Class RepeaterViewPage.
    /// </summary>
    public class RepeaterViewTemplateSelectorPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepeaterViewPage"/> class.
        /// </summary>
        public RepeaterViewTemplateSelectorPage()
		{
			var viewModel = new RepeaterViewTemplateSelectorViewModel();
			BindingContext = viewModel;

		    var thingTempate = new DataTemplate(() =>
		    {
		        var nameLabel = new Label {Font = Font.SystemFontOfSize(NamedSize.Medium)};
                nameLabel.SetBinding(Label.TextProperty, RepeaterViewTemplateSelectorViewModel.ThingsNamePropertyName);

		        var descriptionLabel = new Label {Font = Font.SystemFontOfSize(NamedSize.Small)};
                descriptionLabel.SetBinding(Label.TextProperty, RepeaterViewTemplateSelectorViewModel.ThingsDescriptionPropertyName);

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
		    });
            // Make the Thing2 template have green text
            var thing2Tempate = new DataTemplate(() =>
		    {
		        var nameLabel = new Label {Font = Font.SystemFontOfSize(NamedSize.Medium), TextColor = Color.Green};
                nameLabel.SetBinding(Label.TextProperty, RepeaterViewTemplateSelectorViewModel.ThingsNamePropertyName);

		        var descriptionLabel = new Label {Font = Font.SystemFontOfSize(NamedSize.Small), TextColor = Color.Green};
                descriptionLabel.SetBinding(Label.TextProperty, RepeaterViewTemplateSelectorViewModel.ThingsDescriptionPropertyName);

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
		    });

            var templateSelector = new TemplateSelector();
            templateSelector.Add(thingTempate, typeof(Thing));
            templateSelector.Add(thing2Tempate, typeof(Thing2));

			var repeater = new RepeaterView
			{
				Spacing = 10,
				ItemsSource = viewModel.Things,
                ItemTemplateSelector = templateSelector
			};

			var removeButton = new Button
			{
				Text = "Remove 1st Item",      
				HorizontalOptions = LayoutOptions.Start
			};

            removeButton.SetBinding(Button.CommandProperty, RepeaterViewTemplateSelectorViewModel.RemoveFirstItemCommandName);

			var addButton = new Button
			{
				Text = "Add New Thing",
				HorizontalOptions = LayoutOptions.Start
			};

            addButton.SetBinding(Button.CommandProperty, RepeaterViewTemplateSelectorViewModel.AddThingItemCommandName);

            var addThing2Button = new Button
            {
                Text = "Add New Thing2",
                HorizontalOptions = LayoutOptions.Start
            };

            addThing2Button.SetBinding(Button.CommandProperty, RepeaterViewTemplateSelectorViewModel.AddThing2ItemCommandName);

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
                    new Label
                    {
                        Text=" with Template Selector",
						Font = Font.SystemFontOfSize(NamedSize.Medium),
                        FontAttributes = FontAttributes.Italic
                    },
					repeater,
					removeButton,
					addButton,
                    addThing2Button
				}
			};

			viewModel.LoadData();
		}
    }
}
