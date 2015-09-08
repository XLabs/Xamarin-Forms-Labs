using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(HyperLinkLabel), typeof(HyperLinkLabelRenderer))]

namespace XLabs.Forms.Controls
{
	using Foundation;
	using UIKit;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	/// <summary>
	/// Class HyperLinkLabelRenderer.
	/// </summary>
	public class HyperLinkLabelRenderer : LabelRenderer
	{
		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement == null)
			{
				var label = (UILabel)Control;
				label.TextColor = UIColor.Blue;
				label.BackgroundColor = UIColor.Clear;
				label.UserInteractionEnabled = true;
				var tapXamarin = new UITapGestureRecognizer();

				tapXamarin.AddTarget(() =>
				{
					var hyperLinkLabel = Element as HyperLinkLabel;
					UIApplication.SharedApplication.OpenUrl(new NSUrl(GetNavigationUri(hyperLinkLabel.NavigateUri)));
				});

				tapXamarin.NumberOfTapsRequired = 1;
				tapXamarin.DelaysTouchesBegan = true;
				label.AddGestureRecognizer(tapXamarin);
			}
		}

		/// <summary>
		/// Gets the navigation URI.
		/// </summary>
		/// <param name="uri">The URI.</param>
		/// <returns>System.String.</returns>
		private string GetNavigationUri(string uri)
		{
			if (uri.Contains("@") && !uri.StartsWith("mailto:"))
			{
				return string.Format("{0}{1}", "mailto:", uri);
			}
			else if (uri.StartsWith("www."))
			{
				return string.Format("{0}{1}", @"http://", uri);
			}
			return uri;
		}
	}
}