namespace XLabs.Sample.Pages.Controls
{
	using Xamarin.Forms;

	using XLabs.Forms.Controls;
	using XLabs.Sample.ViewModel;

	public partial class ExtendedLabelPage : ContentPage
	{
		public ExtendedLabelPage()
		{
			InitializeComponent();

			BindingContext = ViewModelLocator.Main;

			var label3 = new ExtendedLabel
			{
				Text = "From code, Strikethrough and using Font property",
				TextColor = Device.OnPlatform(Color.Black,Color.White, Color.White),
				IsUnderline = false,
				IsStrikeThrough = true
			};

			var label4 = new ExtendedLabel
			{
				IsDropShadow = true,
				Text = "From code, Dropshadow with TextColor",
				TextColor = Color.Green
			};

			var label5 = new Label
			{
				Text = "Standard Label created using code",
			};

		
			stkRoot.Children.Add(label4);
			stkRoot.Children.Add(label3);
			stkRoot.Children.Add(label5);
		}
	}
}

