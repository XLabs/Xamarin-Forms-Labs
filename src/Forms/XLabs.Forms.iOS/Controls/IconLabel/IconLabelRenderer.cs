// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="IconLabelRenderer.cs" company="XLabs Team">
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

using System;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Enums;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(IconLabel), typeof(IconLabelRenderer))]
namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Implementation of IconLabelRender.
    /// </summary>
    public class IconLabelRenderer : LabelRenderer
    {
        IconLabel _iconLabel;
        UIKit.UILabel _nativeLabel;

        /// <summary>
        /// Handles the on element changed messages
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            _iconLabel = (IconLabel)Element;
            _nativeLabel = Control;

            if (_iconLabel != null && _nativeLabel != null && !string.IsNullOrEmpty(_iconLabel.Icon))
                SetText(_iconLabel, _nativeLabel);
        }

        /// <summary>
        /// Handles the <see cref="E:ElementPropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
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
                 SetText(_iconLabel, _nativeLabel);
             }
        }

        /// <summary>
        /// Sets the text.
        /// </summary>
        /// <param name="iconLabel">The icon label.</param>
        /// <param name="targetLabel">The target label.</param>
        /// <exception cref="System.NotSupportedException">
        /// Image orientation top and bottom are not supported on iOS
        /// or
        /// Image orientation top and bottom are not supported on iOS
        /// </exception>
        private void SetText(IconLabel iconLabel, UILabel targetLabel)
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

            // set the final formatted string as the button's text
            targetLabel.AttributedText  = prettyString;

            if (iconLabel.TextAlignement == TextAlignment.Center)
            {
                // center the button's contents
                targetLabel.TextAlignment = UITextAlignment.Center;
            
            }
            else if (iconLabel.TextAlignement == TextAlignment.End)
            {
                targetLabel.TextAlignment = UITextAlignment.Right;
               
            }
            else if (iconLabel.TextAlignement == TextAlignment.Start)
            {
                targetLabel.TextAlignment = UITextAlignment.Left;
            
            }
        }

        /// <summary>
        /// Gets the font for the button (applied to all button text EXCEPT the icon)
        /// </summary>
        /// <param name="iconLabel">The icon label.</param>
        /// <param name="targetLabel">The target label.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <returns>UIFont.</returns>
        private UIFont GetButtonFont(IconLabel iconLabel, UILabel targetLabel, nfloat fontSize)
        {
            UIFont btnTextFont = iconLabel.Font.ToUIFont();

            if (iconLabel.Font != Font.Default && btnTextFont != null)
                return btnTextFont;
            
            if (iconLabel.Font == Font.Default)
                return UIFont.SystemFontOfSize(fontSize);

            return btnTextFont;
        }
    }
}
