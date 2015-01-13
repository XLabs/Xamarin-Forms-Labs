using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(IconButton), typeof(IconButtonRenderer))]
namespace XLabs.Forms.Controls
{
	using System.ComponentModel;

	using Foundation;
	using UIKit;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	using XLabs.Enums;

	/// <summary>
	/// Draws a button on the iOS platform with an icon shown to the right or left of the button's text
	/// </summary>
	public class IconButtonRenderer : ButtonRenderer
	{
		/// <summary>
		/// Gets the underlying element typed as an <see cref="IconButton"/>
		/// </summary>
		private IconButton IconButton
		{
			get { return (IconButton) Element; }
		}

		private static void SetText(IconButton iconButton, UIButton targetButton)
		{
			var renderedIcon = iconButton.Icon;

			// if no IconFontName is provided on the IconButton, default to FontAwesome
			var iconFontName = string.IsNullOrEmpty(iconButton.IconFontName)
				? "fontawesome"
				: iconButton.IconFontName;

			var iconSize = iconButton.IconSize == default(float)
				? 17f
				: iconButton.IconSize;

			var faFont = UIFont.FromName(iconFontName, iconSize);

			// set the icon to either be on the left or right of the button's text
			string combinedText = iconButton.Orientation == ImageOrientation.ImageToLeft 
				? string.Format("{0}  {1}", renderedIcon, iconButton.Text) 
				: string.Format("{0}  {1}", iconButton.Text, renderedIcon);


			// string attributes for the icon
			var iconAttributes = new UIStringAttributes
			{
				ForegroundColor = iconButton.IconColor.ToUIColor(),
				BackgroundColor = targetButton.BackgroundColor,
				Font = faFont,
				TextAttachment = new NSTextAttachment()
			};

			// string attributes for the button's text. 
			// TODO: Calculate an appropriate BaselineOffset for the main button text in order to center it vertically relative to the icon
			var btnAttributes = new UIStringAttributes
			{
				BackgroundColor = iconButton.BackgroundColor.ToUIColor(),
				ForegroundColor = iconButton.TextColor.ToUIColor(),
				Font = GetButtonFont(iconButton,targetButton)
			};

			// Give the overall string the attributes of the button's text
			var prettyString = new NSMutableAttributedString(combinedText,btnAttributes);

			// Set the font for only the icon (1 char)
			prettyString.SetAttributes(iconAttributes.Dictionary,
				iconButton.Orientation == ImageOrientation.ImageToLeft
					? new NSRange(0, 1)
					: new NSRange(prettyString.Length - 1, 1));


			// set the final formatted string as the button's text
			targetButton.SetAttributedTitle(prettyString, UIControlState.Normal);

			// center the button's contents
			targetButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
			targetButton.TitleLabel.TextAlignment = UITextAlignment.Center;
		}

		/// <summary>
		/// Gets the font for the button (applied to all button text EXCEPT the icon)
		/// </summary>
		/// <param name="iconButton"></param>
		/// <param name="targetButton"></param>
		/// <returns></returns>
		private static UIFont GetButtonFont(IconButton iconButton, UIButton targetButton)
		{
			UIFont btnTextFont = iconButton.Font.ToUIFont();

			if (iconButton.Font != Font.Default && btnTextFont != null)
				return btnTextFont;
			else if (iconButton.Font == Font.Default)
				return UIFont.SystemFontOfSize(17f);

			return btnTextFont;
		}


		/// <summary>
		/// Handles the initial drawing of the button
		/// </summary>
		/// <param name="e">Information on the <see cref="IconButton"/></param>
		protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged(e);
			var iconButton = IconButton;
			var targetButton = Control;
			
			if (iconButton != null && targetButton != null && !string.IsNullOrEmpty(iconButton.Icon))
				SetText(iconButton, targetButton);
		}

		/// <summary>
		/// Called when the underlying model's properties are changed.
		/// </summary>
		/// <param name="sender">Model sending the change event.</param>
		/// <param name="e">Event arguments.</param>
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			// Only update the text if the icon or button text changes
			if (e.PropertyName == IconButton.IconProperty.PropertyName || e.PropertyName == Controls.IconButton.TextProperty.PropertyName)
			{
				var sourceButton = Element as IconButton;
				if (sourceButton != null && sourceButton.Icon != null)
				{
					var iconButton = IconButton;
					var targetButton = Control;
					if (iconButton != null && targetButton != null && iconButton.Icon != null)
						SetText(iconButton, targetButton);
				}
			}
		}

	}
}