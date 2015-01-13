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

			var view = (ExtendedLabel)Element;

			UpdateUi(view, Control);
			SetPlaceholder(view);
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

			if (e.PropertyName == Label.TextProperty.PropertyName)
			{
				SetPlaceholder(view);
			}

			if (e.PropertyName == Label.FormattedTextProperty.PropertyName)
			{
				SetPlaceholder(view);
			}

			if (e.PropertyName == ExtendedLabel.PlaceholderProperty.PropertyName)
			{
				SetPlaceholder(view);
			}

			if (e.PropertyName == ExtendedLabel.FormattedPlaceholderProperty.PropertyName)
			{
				SetPlaceholder(view);
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
			// Prefer font set through Font property.
			if (view.Font == Font.Default)
			{
				if (view.FontSize > 0)
				{
					control.Font = UIFont.FromName(control.Font.Name, (float)view.FontSize);
				}

				if (!string.IsNullOrEmpty(view.FontName))
				{
					var fontName = Path.GetFileNameWithoutExtension(view.FontName);

					var font = UIFont.FromName(fontName, control.Font.PointSize);

					if (font != null)
					{
						control.Font = font;
					}
				}

				#region ======= This is for backward compatability with obsolete attrbute 'FontNameIOS' ========
				if (!string.IsNullOrEmpty(view.FontNameIOS))
				{
					var font = UIFont.FromName(view.FontNameIOS, (view.FontSize > 0) ? (float)view.FontSize : 12.0f);

					if (font != null)
					{
						control.Font = font;
					}
				}
				#endregion ====== End of obsolete section ==========================================================
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
																   shadow: dropShadow); ;
		}

		private void SetPlaceholder(ExtendedLabel view)
		{
			if (!string.IsNullOrWhiteSpace(view.Text))
			{
				var formattedString = view.FormattedText ?? view.Text;

				Control.AttributedText = formattedString.ToAttributed(view.Font, view.TextColor);

				LayoutSubviews();

				return;
			}

			if (string.IsNullOrWhiteSpace(view.Placeholder) && view.FormattedPlaceholder == null)
			{
				return;
			}

			var formattedPlaceholder = view.FormattedPlaceholder ?? view.Placeholder;

			Control.AttributedText = formattedPlaceholder.ToAttributed(view.Font, view.TextColor);

			LayoutSubviews();
		}
	}
}

