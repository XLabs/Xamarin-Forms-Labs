using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(WebImage), typeof(WebImageRenderer))]
namespace XLabs.Forms.Controls
{
	using System;
	using System.Windows.Media.Imaging;

	using Xamarin.Forms.Platform.WinPhone;

	using Image = Xamarin.Forms.Image;

	/// <summary>
	/// Class WebImageRenderer.
	/// </summary>
	public class WebImageRenderer : ImageRenderer
	{
		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);

			var webImage = (WebImage)Element;

			SetNativeControl(GetImageFromWeb(webImage.ImageUrl));
		}

		/// <summary>
		/// Gets the image from web.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <returns>System.Windows.Controls.Image.</returns>
		private System.Windows.Controls.Image GetImageFromWeb(string url)
		{
			var image = new System.Windows.Controls.Image();

			var uri = new Uri(url, UriKind.Absolute);

			image.Source = new BitmapImage(uri);

			return image;
		}
	}
}