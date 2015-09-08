namespace XLabs.Sample.Pages.Controls
{
	using System;

	using Xamarin.Forms;

	using XLabs.Forms.Controls;

	/// <summary>
	/// Class ExtendedScrollViewPage.
	/// </summary>
	public partial class ExtendedScrollViewPage : ContentPage
	{
		/// <summary>
		/// The imag e_ height
		/// </summary>
		private const int IMAGE_HEIGHT = 200;

		/// <summary>
		/// The _display alert
		/// </summary>
		bool _displayAlert = false;
		/// <summary>
		/// Initializes a new instance of the <see cref="ExtendedScrollViewPage"/> class.
		/// </summary>
		public ExtendedScrollViewPage()
		{
			InitializeComponent();

			var container = new StackLayout()
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			var buttonToScroll = new Button() { Text = "Scroll to end" };

			container.Children.Add(buttonToScroll);

			for (var i = 0; i < 10; i++)
			{
				container.Children.Add(new Image
				{
					Aspect = Aspect.AspectFill,
					Source = ImageSource.FromUri(new Uri("http://www.stockvault.net/data/2011/05/31/124348/small.jpg")),
					HeightRequest = IMAGE_HEIGHT

				});
			}
			var sv = new ExtendedScrollView
			{
				Content = container,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			sv.Scrolled += async (arg1, arg2) =>
			{
				if (!(arg2.Y > sv.Bounds.Height) || _displayAlert)
				{
					return;
				}

				_displayAlert = true;
				await DisplayAlert("Scroll event", "User scrolled pass bounds", "Ok", "cancel");
			};

			buttonToScroll.Clicked += (sender, e) =>
			{
				sv.Position = new Point(sv.Position.X, sv.ContentSize.Height - IMAGE_HEIGHT);
			};

			Content = sv;
		}

	}
}

