using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Enums;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(IconLabel), typeof(IconLabelRenderer))]
namespace XLabs.Forms.Controls
{
    public class IconLabelRenderer : LabelRenderer
    {
        IconLabel iconLabel;
        UIKit.UILabel nativeLabel;

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            iconLabel = (IconLabel)Element;
            nativeLabel = Control;

            if (iconLabel != null && nativeLabel != null && !string.IsNullOrEmpty(iconLabel.Icon))
                SetText(iconLabel, nativeLabel);
        }


        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
             if(e.PropertyName == IconLabel.IconProperty.PropertyName ||
                e.PropertyName == IconLabel.TextProperty.PropertyName ||
                e.PropertyName == IconLabel.IconColorProperty.PropertyName ||
                e.PropertyName == IconLabel.TextColorProperty.PropertyName ||
                e.PropertyName == IconLabel.IsVisibleProperty.PropertyName ||
                e.PropertyName == IconLabel.IconSizeProperty.PropertyName ||
                e.PropertyName == IconLabel.FontSizeProperty.PropertyName ||
                e.PropertyName == IconLabel.OrientationProperty.PropertyName)
             {
                 SetText(iconLabel, nativeLabel);
             }
        }


        private  void SetText(IconLabel iconLabel, UILabel targetLabel)
        {
            var renderedIcon = iconLabel.Icon;

            
            var iconFontName = string.IsNullOrEmpty(iconLabel.IconFontName)
                ? "fontawesome"
                : iconLabel.IconFontName;

            var iconSize = (iconLabel.IconSize == 0) ? iconLabel.FontSize : iconLabel.IconSize;
          



            var faFont = UIFont.FromName(iconFontName, (float)iconSize);
            string combinedText = null;
            string separator = " ";
            if (iconLabel.ShowIconSeparator)
                separator = " | ";
            switch (iconLabel.Orientation)
            {
                case ImageOrientation.ImageToLeft:
                    if (string.IsNullOrEmpty(iconLabel.Text))
                        combinedText = renderedIcon;
                    else
                    {

                        combinedText = renderedIcon + separator + iconLabel.Text;
                    }
                    break;
                case ImageOrientation.ImageToRight:
                    if (string.IsNullOrEmpty(iconLabel.Text))
                        combinedText = renderedIcon;
                    else
                        combinedText = iconLabel.Text + separator + renderedIcon;
                    break;
                case ImageOrientation.ImageOnTop:
                    throw new NotSupportedException("Image orientation top and bottom are not supported on iOS");
                    
                case ImageOrientation.ImageOnBottom:
                    throw new NotSupportedException("Image orientation top and bottom are not supported on iOS");
            }
          

            // string attributes for the icon
            var iconAttributes = new UIStringAttributes
            {
                ForegroundColor = iconLabel.IconColor.ToUIColor(),
                BackgroundColor = targetLabel.BackgroundColor,
                Font = faFont,
                TextAttachment = new NSTextAttachment()
            };

          
            // TODO: Calculate an appropriate BaselineOffset for the main button text in order to center it vertically relative to the icon
            var btnAttributes = new UIStringAttributes
            {
                BackgroundColor = iconLabel.BackgroundColor.ToUIColor(),
                ForegroundColor = iconLabel.TextColor.ToUIColor(),
                Font = GetButtonFont(iconLabel, targetLabel, 17f),

            };
            if (!string.IsNullOrEmpty(iconLabel.Text))
                btnAttributes.BaselineOffset = 3;

            // Give the overall string the attributes of the button's text
            var prettyString = new NSMutableAttributedString(combinedText, btnAttributes);

            // Set the font for only the icon (1 char)
            prettyString.SetAttributes(iconAttributes.Dictionary,
                iconLabel.Orientation == ImageOrientation.ImageToLeft
                    ? new NSRange(0, 1)
                    : new NSRange(prettyString.Length - 1, 1));


            //prettyString.SetAttributes(separationBarAttributes.Dictionary,
            //    iconButton.Orientation == ImageOrientation.ImageToLeft
            //        ? new NSRange(2, 1)
            //        : new NSRange(prettyString.Length - 2, 1));

            // set the final formatted string as the button's text
            targetLabel.AttributedText  = prettyString;

            if (iconLabel.TextAlignement == TextAlignment.Center)
            {
                // center the button's contents
                targetLabel.TextAlignment = UITextAlignment.Center;
                //targetLabel.TitleLabel.TextAlignment = UITextAlignment.Center;
            }
            else if (iconLabel.TextAlignement == TextAlignment.End)
            {
                targetLabel.TextAlignment = UITextAlignment.Right;
                //targetLabel.TitleLabel.TextAlignment = UITextAlignment.Right;
            }
            else if (iconLabel.TextAlignement == TextAlignment.Start)
            {
                targetLabel.TextAlignment = UITextAlignment.Left;
                //targetLabel.TitleLabel.TextAlignment = UITextAlignment.Left;
            }
        }

        /// <summary>
        /// Gets the font for the button (applied to all button text EXCEPT the icon)
        /// </summary>
        /// <param name="iconLabel"></param>
        /// <param name="targetLabel"></param>
        /// <returns></returns>
        private  UIFont GetButtonFont(IconLabel iconLabel, UILabel targetLabel, nfloat fontSize)
        {
            UIFont btnTextFont = iconLabel.Font.ToUIFont();

            if (iconLabel.Font != Font.Default && btnTextFont != null)
                return btnTextFont;
            else if (iconLabel.Font == Font.Default)
                return UIFont.SystemFontOfSize(fontSize);

            return btnTextFont;
        }
    }
}
