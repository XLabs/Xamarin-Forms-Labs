using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(WebImage), typeof(WebImageRenderer))]
namespace XLabs.Forms.Controls
{
	using System.Net;

	using Foundation;
	using UIKit;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	using XLabs.Platform.Services;

	/// <summary>
	/// Class WebImageRenderer.
	/// </summary>
	public class WebImageRenderer : ImageRenderer
	{

		/// <summary>
		/// Gets the underlying control typed as an <see cref="WebImage" />
		/// </summary>
		/// <value>The web image.</value>
		private WebImage WebImage
		{
			get { return (WebImage)Element; }
		}


		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);

			UIImage image;
			var networkStatus = Reachability.InternetConnectionStatus();

			var isReachable = networkStatus != NetworkStatus.NotReachable;

			if (isReachable)
			{
				image = GetImageFromWeb(WebImage.ImageUrl);
			}
			else
			{
				image = string.IsNullOrEmpty(WebImage.DefaultImage)
					? new UIImage()
					: UIImage.FromBundle(WebImage.DefaultImage);
			}

			var imageView = new UIImageView(image);

			SetNativeControl(imageView);
		}


		/// <summary>
		/// Gets the image from web.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <returns>UIImage.</returns>
		private UIImage GetImageFromWeb(string url)
		{
			using (var webclient = new WebClient())
			{
				var imageBytes = webclient.DownloadData(url);

				return UIImage.LoadFromData(NSData.FromArray(imageBytes));
			}
		}
	}
}