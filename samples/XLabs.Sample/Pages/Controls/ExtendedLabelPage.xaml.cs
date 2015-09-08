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

			var label = new ExtendedLabel
			{
				Text = "From code, using Device.OnPlatform, Underlined",
				FontName = "Open 24 Display St.ttf",
				FriendlyFontName = Device.OnPlatform("", "", "Open 24 Display St"),
				IsUnderline = true,
				FontSize = 22,
			};

			var label2 = new ExtendedLabel
			{
				Text = "From code, Strikethrough",
				FontName = "Open 24 Display St.ttf",
				FriendlyFontName = Device.OnPlatform("", "", "Open 24 Display St"),
				IsUnderline = false,
				IsStrikeThrough = true,
				FontSize = 22,
			};

			var font = Font.OfSize("Open 24 Display St", 22);

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
				DropShadowColor = Color.Red,
				Text = "From code, Dropshadow with TextColor",
				TextColor = Color.Green
			};

			var label5 = new Label
			{
				Text = "Standard Label created using code",
			};

			label3.Font = font;

			stkRoot.Children.Add(label4);
			stkRoot.Children.Add(label3);
			stkRoot.Children.Add(label2);
			stkRoot.Children.Add(label);
			stkRoot.Children.Add(label5);
		}
	}
}

