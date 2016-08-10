// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="IconButtonRenderer.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System.ComponentModel;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Enums;
using XLabs.Forms.Controls;
using XLabs.Forms.Extensions;

[assembly: ExportRenderer(typeof(IconButton), typeof(IconButtonRenderer))]
namespace XLabs.Forms.Controls
{
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
            get { return (IconButton)Element; }
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
            string combinedText = null;
            string separator = " ";
            if (iconButton.ShowIconSeparator)
                separator = " | ";
            switch (iconButton.Orientation)
            {
                case ImageOrientation.ImageToLeft:
                    if (string.IsNullOrEmpty(iconButton.Text))
                        combinedText = renderedIcon;
                    else
                    {

                        combinedText = renderedIcon + separator + iconButton.Text;
                    }
                    break;
                case ImageOrientation.ImageToRight:
                    if (string.IsNullOrEmpty(iconButton.Text))
                        combinedText = renderedIcon;
                    else
                        combinedText = iconButton.Text + separator + renderedIcon;
                    break;
                case ImageOrientation.ImageOnTop:
                    if (string.IsNullOrEmpty(iconButton.Text))
                        combinedText = renderedIcon;
                    else
                        combinedText = renderedIcon + separator + iconButton.Text;
                    break;
                case ImageOrientation.ImageOnBottom:
                    if (string.IsNullOrEmpty(iconButton.Text))
                        combinedText = renderedIcon;
                    else
                        combinedText = renderedIcon + separator + iconButton.Text;
                    break;
            }
          
            // string attributes for the icon
            var iconAttributes = new UIStringAttributes
            {
                    ForegroundColor = iconButton.IconColor.ToUIColorOrDefault(targetButton.TitleColor(targetButton.State)),
                    BackgroundColor = targetButton.BackgroundColor,
                    Font = faFont,
                    TextAttachment = new NSTextAttachment()
            };
           
            // string attributes for the button's text. 
            // TODO: Calculate an appropriate BaselineOffset for the main button text in order to center it vertically relative to the icon
            var btnAttributes = new UIStringAttributes
            {
                    BackgroundColor = iconButton.BackgroundColor.ToUIColor(),
                    ForegroundColor = iconButton.TextColor.ToUIColorOrDefault(targetButton.TitleColor(targetButton.State)),
                    Font = GetButtonFont(iconButton, targetButton),
            };
            if (!string.IsNullOrEmpty(iconButton.Text))
                btnAttributes.BaselineOffset = 3;

            // Give the overall string the attributes of the button's text
            var prettyString = new NSMutableAttributedString(combinedText, btnAttributes);

            // Set the font for only the icon (1 char)
            prettyString.SetAttributes(iconAttributes.Dictionary,
                iconButton.Orientation == ImageOrientation.ImageToLeft
                    ? new NSRange(0, 1)
                    : new NSRange(prettyString.Length - 1, 1));


       

            // set the final formatted string as the button's text
            targetButton.SetAttributedTitle(prettyString, UIControlState.Normal);

            if (iconButton.TextAlignement == TextAlignment.Center)
            {
                // center the button's contents
                targetButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
                targetButton.TitleLabel.TextAlignment = UITextAlignment.Center;
            }
            else if (iconButton.TextAlignement == TextAlignment.End)
            {
                targetButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Right;
                targetButton.TitleLabel.TextAlignment = UITextAlignment.Right;
            }
            else if (iconButton.TextAlignement == TextAlignment.Start)
            {
                targetButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
                targetButton.TitleLabel.TextAlignment = UITextAlignment.Left;
            }
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
            if (e.PropertyName == IconButton.IconProperty.PropertyName ||
                e.PropertyName == IconButton.TextProperty.PropertyName ||
                e.PropertyName == IconButton.IsEnabledProperty.PropertyName)
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