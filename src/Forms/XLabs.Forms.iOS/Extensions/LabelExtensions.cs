namespace XLabs.Forms.Extensions
{
	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	using XLabs.Platform.Extensions;

	/// <summary>
	/// Class LabelExtensions.
	/// </summary>
	public static class LabelExtensions
	{
		/// <summary>
		/// Adjusts the height.
		/// </summary>
		/// <param name="label">The label.</param>
		public static void AdjustHeight(this Label label)
		{
			label.HeightRequest = label.Text.StringHeight(label.Font.ToUIFont(), (float)label.Width);
		}
	}
}

