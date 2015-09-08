namespace XLabs.Sample.Pages.Services
{
	using Xamarin.Forms;

	using XLabs.Platform.Device;

	/// <summary>
	/// Class AbsoluteLayoutWithDisplayInfoPage.
	/// </summary>
	public class AbsoluteLayoutWithDisplayInfoPage : ContentPage
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AbsoluteLayoutWithDisplayInfoPage" /> class.
		/// </summary>
		/// <param name="display">The display.</param>
		public AbsoluteLayoutWithDisplayInfoPage(IDisplay display)
		{
			this.Title = "Absolute Layout With Display Info";
			var abs = new AbsoluteLayout();
			var inchX = display.WidthRequestInInches(1);
			var inchY = display.HeightRequestInInches(1);
			var originX = display.WidthRequestInInches(display.ScreenWidthInches() / 2);
			var originY = display.HeightRequestInInches(display.ScreenHeightInches() / 2);

			abs.Children.Add(new Label() { Text = "1\"x\"1\" blue frame" });

			abs.Children.Add(new Frame()
				{
					BackgroundColor = Color.Navy,
				},
				new Rectangle(originX - inchX/2, originY - inchY/2, inchX, inchY));

			abs.Children.Add(new Frame()
				{
					BackgroundColor = Color.White
				},
				new Rectangle(originX - inchX/16, originY - inchY/16, inchX/8, inchY/8));

			this.Content = abs;
		}
	}
}
