namespace XLabs.Forms.Extensions
{
	using CoreGraphics;
	using System.IO;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	using Foundation;
	using UIKit;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	using XLabs.Platform.Extensions;

	/// <summary>
	/// Class UiImageExtensions.
	/// </summary>
	public static class UiImageExtensions
	{
		/// <summary>
		/// Adds the text.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="text">The text.</param>
		/// <param name="point">The point.</param>
		/// <param name="font">The font.</param>
		/// <param name="color">The color.</param>
		/// <returns>Task&lt;ImageSource&gt;.</returns>
		public static async Task<ImageSource> AddText(
			this StreamImageSource source,
			string text,
			CGPoint point,
			Font font,
			Color color)
		{
			var token = new CancellationTokenSource();
			var stream = await source.Stream(token.Token);
			var image = UIImage.LoadFromData(NSData.FromStream(stream));

			var bytes = image.AddText(text, point, font.ToUIFont(), color.ToUIColor()).AsPNG().ToArray();

			return ImageSource.FromStream(() => new MemoryStream(bytes));
		}

		/// <summary>
		/// Adds the text.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="text">The text.</param>
		/// <param name="point">The point.</param>
		/// <param name="font">The font.</param>
		/// <param name="color">The color.</param>
		/// <param name="alignment">The alignment.</param>
		/// <returns>UIImage.</returns>
		public static UIImage AddText(
			this UIImage image,
			string text,
			CGPoint point,
			UIFont font,
			UIColor color,
			UITextAlignment alignment = UITextAlignment.Left)
		{
			//var labelRect = new RectangleF(point, new SizeF(image.Size.Width - point.X, image.Size.Height - point.Y));
			var h = text.StringHeight(font, image.Size.Width);
			var labelRect = new CGRect(point, new CGSize(image.Size.Width - point.X, h));

			var label = new UILabel(labelRect)
				            {
					            Font = font,
					            Text = text,
					            TextColor = color,
					            TextAlignment = alignment,
					            BackgroundColor = UIColor.Clear
				            };

			var labelImage = label.ToNativeImage();

			using (var context = image.Size.ToBitmapContext())
			{
				var rect = new CGRect(new CGPoint(0, 0), image.Size);
				context.DrawImage(rect, image.CGImage);
				context.DrawImage(labelRect, labelImage.CGImage);
				context.StrokePath();
				return UIImage.FromImage(context.ToImage());
			}
		}
	}
}