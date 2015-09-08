namespace XLabs.Platform
{
	using System.IO;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Media.Imaging;

	/// <summary>
	/// Class UiElementExtensions.
	/// </summary>
	public static class UiElementExtensions
	{
		/// <summary>
		/// Streams to JPEG.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <param name="stream">The stream.</param>
		/// <returns>Task.</returns>
		public static Task StreamToJpeg(this UIElement view, Stream stream)
		{
			return
				Task.Run(() => view.ToBitmap().SaveJpeg(stream, (int)view.RenderSize.Width, (int)view.RenderSize.Height, 0, 100));
		}

		/// <summary>
		/// To the bitmap.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <returns>WriteableBitmap.</returns>
		public static WriteableBitmap ToBitmap(this UIElement view)
		{
			return new WriteableBitmap(view, null);
		}
	}
}