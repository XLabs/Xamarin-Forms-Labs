// ***********************************************************************
// Assembly       : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 01-06-2016
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-06-2016
// ***********************************************************************
// <copyright file="CheckBoxRenderer.cs" company="XLabs Team">
//        Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//        This project is licensed under the Apache 2.0 license
//        https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
// 
//        XLabs is a open source project that aims to provide a powerfull and cross
//        platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Forms.Controls;
using XLabs.Forms.Extensions;
using XLabs.Platform.Extensions;

[assembly: ExportRenderer(typeof(CheckBox), typeof(CheckBoxRenderer))]

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// The check box renderer for iOS.
    /// </summary>
    public class CheckBoxRenderer : ViewRenderer<CheckBox, CheckBoxView>
    {
        private UIColor defaultTextColor;

        /// <summary>
        /// Handles the Element Changed event
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<CheckBox> e)
        {
            base.OnElementChanged(e);

            if (Element == null) return;

            BackgroundColor = Element.BackgroundColor.ToUIColor();
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var checkBox = new CheckBoxView (Bounds);
                    checkBox.TouchUpInside += (s, args) => Element.Checked = Control.Checked;
                    defaultTextColor = checkBox.TitleColor(UIControlState.Normal);
                    SetNativeControl (checkBox);
                }
                Control.LineBreakMode = UILineBreakMode.CharacterWrap;
                Control.VerticalAlignment = UIControlContentVerticalAlignment.Top;
                Control.CheckedTitle = string.IsNullOrEmpty (e.NewElement.CheckedText) ? e.NewElement.DefaultText : e.NewElement.CheckedText;
                Control.UncheckedTitle = string.IsNullOrEmpty (e.NewElement.UncheckedText) ? e.NewElement.DefaultText : e.NewElement.UncheckedText;
                Control.Checked = e.NewElement.Checked;
                UpdateTextColor();
            }

            Control.Frame = Frame;
            Control.Bounds = Bounds;

            UpdateFont();
        }

        /// <summary>
        /// Resizes the text.
        /// </summary>
        private void ResizeText()
        {
            if (Element == null)
                return;

            var text = Element.Checked ? string.IsNullOrEmpty(Element.CheckedText) ? Element.DefaultText : Element.CheckedText :
                string.IsNullOrEmpty(Element.UncheckedText) ? Element.DefaultText : Element.UncheckedText;

            var bounds = Control.Bounds;

            var width = Control.TitleLabel.Bounds.Width;

            var height = text.StringHeight(Control.Font, width);

            var minHeight = string.Empty.StringHeight(Control.Font, width);

            var requiredLines = Math.Round(height / minHeight, MidpointRounding.AwayFromZero);

            var supportedLines = Math.Round(bounds.Height / minHeight, MidpointRounding.ToEven);

            if (supportedLines != requiredLines)
            {
                bounds.Height += (float)(minHeight * (requiredLines - supportedLines));
                Control.Bounds = bounds;
                Element.HeightRequest = bounds.Height;
            }
        }

        /// <summary>
        /// Draws the specified rect.
        /// </summary>
        /// <param name="rect">The rect.</param>
        public override void Draw(CoreGraphics.CGRect rect)
        {
            base.Draw(rect);
            ResizeText();
        }

        /// <summary>
        /// Updates the font.
        /// </summary>
        private void UpdateFont()
        {
            if (!string.IsNullOrEmpty (Element.FontName)) {
                var font = UIFont.FromName (Element.FontName, (Element.FontSize > 0) ? (float)Element.FontSize : 12.0f);
                if (font != null) {
                    Control.Font = font;
                }
            } else if (Element.FontSize > 0) {
                var font = UIFont.FromName (Control.Font.Name, (float)Element.FontSize);
                if (font != null) {
                    Control.Font = font;
                }
            }
        }

        private void UpdateTextColor()
        {
            Control.SetTitleColor (Element.TextColor.ToUIColorOrDefault(defaultTextColor), UIControlState.Normal);
            Control.SetTitleColor (Element.TextColor.ToUIColorOrDefault(defaultTextColor), UIControlState.Selected);
        }

        /// <summary>
        /// Handles the <see cref="E:ElementPropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            switch (e.PropertyName)
            {
                case "Checked":
                    Control.Checked = Element.Checked;
                    break;
                case "TextColor":
                    UpdateTextColor();
                    break;
                case "CheckedText":
                    Control.CheckedTitle = string.IsNullOrEmpty(Element.CheckedText) ? Element.DefaultText : Element.CheckedText;
                    break;
                case "UncheckedText":
                    Control.UncheckedTitle = string.IsNullOrEmpty(Element.UncheckedText) ? Element.DefaultText : Element.UncheckedText;
                    break;
                case "FontSize":
                    UpdateFont();
                    break;
                case "FontName":
                    UpdateFont();
                    break;
                case "Element":
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("Property change for {0} has not been implemented.", e.PropertyName);
                    return;
            }
        }
    }
}