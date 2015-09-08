using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(WebImage), typeof(WebImageRenderer))]
namespace XLabs.Forms.Controls
{
	using System.Net;

	using Android.Graphics;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.Android;

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
			get { return (WebImage) Element; }
		}


		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);

			var targetImageView = this.Control;

			Bitmap image = null;
			var networkStatus = Reachability.InternetConnectionStatus();
			var isReachable = networkStatus != NetworkStatus.NotReachable;

			if (isReachable)
			{
				image = GetImageFromWeb(WebImage.ImageUrl);
			}
			else
			{
				if (!string.IsNullOrEmpty(WebImage.DefaultImage))
				{
					var handler = new FileImageSourceHandler();
					image = handler.LoadImageAsync(ImageSource.FromFile(WebImage.DefaultImage), this.Context).Result;
				}
			}

			targetImageView.SetImageBitmap(image);
		}

		/// <summary>
		/// Gets the image from web.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <returns>Bitmap.</returns>
		private Bitmap GetImageFromWeb(string url)
		{
			Bitmap imageBitmap = null;
			using (var webClient = new WebClient())
			{
				var imageBytes = webClient.DownloadData(url);
				if (imageBytes != null && imageBytes.Length > 0)
				{
					imageBitmap = Android.Graphics.BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
				}
			}

			return imageBitmap;
		}
	}
}