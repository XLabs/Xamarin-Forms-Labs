using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedLabel), typeof(ExtendedLabelRenderer))]

namespace XLabs.Forms.Controls
{
	using System.IO;

	using Foundation;
	using UIKit;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	/// <summary>
	/// The extended label renderer.
	/// </summary>
	public class ExtendedLabelRenderer : LabelRenderer
	{
		/// <summary>
		/// The on element changed callback.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);
			if (e.NewElement != null) {
				var view = (ExtendedLabel)e.NewElement;
				UpdateUi (view, Control);
			}
			
		}

		/// <summary>
		/// Raises the element property changed event.
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">The event arguments</param>
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			var view = (ExtendedLabel)Element;

			if (e.PropertyName == ExtendedLabel.IsUnderlineProperty.PropertyName ||
				e.PropertyName == ExtendedLabel.IsDropShadowProperty.PropertyName ||
				e.PropertyName == ExtendedLabel.IsStrikeThroughProperty.PropertyName ||
                e.PropertyName == ExtendedLabel.PlaceholderProperty.PropertyName ||
                e.PropertyName == ExtendedLabel.TextProperty.PropertyName
				) {
					UpdateUi (view,Control);
			}
			
		}

		/// <summary>
		/// Updates the UI.
		/// </summary>
		/// <param name="view">
		/// The view.
		/// </param>
		/// <param name="control">
		/// The control.
		/// </param>
		private void UpdateUi(ExtendedLabel view, UILabel control)
		{
			if (view == null || control == null)
				return;

            // if text is null or empty, try to set the "placeholder"
            if (string.IsNullOrEmpty(view.Text) && !string.IsNullOrEmpty(view.Placeholder))
            {
                this.Control.Text = view.Placeholder;
                this.Control.TextColor = Color.Gray.ToUIColor();

                return;
            }

			//Do not create attributed string if it is not necesarry
			if (!view.IsUnderline && !view.IsStrikeThrough && !view.IsDropShadow)
			{
				return;
			}

			var underline = view.IsUnderline ? NSUnderlineStyle.Single : NSUnderlineStyle.None;
			var strikethrough = view.IsStrikeThrough ? NSUnderlineStyle.Single : NSUnderlineStyle.None;

			NSShadow dropShadow = null;

			if (view.IsDropShadow)
			{
				dropShadow = new NSShadow
				{
					ShadowColor = UIColor.DarkGray,
					ShadowBlurRadius = 1.4f,
					ShadowOffset = new CoreGraphics.CGSize(new CoreGraphics.CGPoint(0.3f, 0.8f))
				};
			}

			// For some reason, if we try and convert Color.Default to a UIColor, the resulting color is
			// either white or transparent. The net result is the ExtendedLabel does not display.
			// Only setting the control's TextColor if is not Color.Default will prevent this issue.
			if (view.TextColor != Color.Default)
			{
				control.TextColor = view.TextColor.ToUIColor();
			}

		
			control.AttributedText = new NSMutableAttributedString(control.Text,
																   control.Font,
																   underlineStyle: underline,
																   strikethroughStyle: strikethrough,
																   shadow: dropShadow);
		
		}

	}
}

