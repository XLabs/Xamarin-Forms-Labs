using System;
using System.ComponentModel;
using System.Diagnostics;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Forms.Controls;
using XLabs.Platform.Extensions;

[assembly: ExportRenderer(typeof (CustomRadioButton), typeof (RadioButtonRenderer))]

namespace XLabs.Forms.Controls
{
    /// <summary>
    ///     The Radio button renderer for iOS.
    /// </summary>
    public class RadioButtonRenderer : ViewRenderer<CustomRadioButton, RadioButtonView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<CustomRadioButton> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var checkBox = new RadioButtonView(Bounds);

                checkBox.TouchUpInside += (s, args) => Element.Checked = Control.Checked;

                SetNativeControl(checkBox);
            }

            if (this.Element == null) return;

            BackgroundColor = this.Element.BackgroundColor.ToUIColor();
            UpdateFont();

            Control.LineBreakMode = UILineBreakMode.CharacterWrap;
            Control.VerticalAlignment = UIControlContentVerticalAlignment.Center;
            Control.Text = this.Element.Text;
            Control.Checked = this.Element.Checked;
            Control.SetTitleColor(this.Element.TextColor.ToUIColor(), UIControlState.Normal);
            Control.SetTitleColor(this.Element.TextColor.ToUIColor(), UIControlState.Selected);
        }

        private void ResizeText()
        {
            var text = Element.Text;

            var bounds = Control.Bounds;

            var width = Control.TitleLabel.Bounds.Width;

            var height = text.StringHeight(Control.Font, width);

            var minHeight = string.Empty.StringHeight(Control.Font, width);

            var requiredLines = Math.Round(height/minHeight, MidpointRounding.AwayFromZero);

            var supportedLines = Math.Round(bounds.Height/minHeight, MidpointRounding.ToEven);

            if (supportedLines == requiredLines)
            {
                return;
            }

            bounds.Height += (float) (minHeight*(requiredLines - supportedLines));

            Control.Bounds = bounds;
            Element.HeightRequest = bounds.Height;
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            ResizeText();
        }

        /// <summary>
        ///     Updates the font.
        /// </summary>
        private void UpdateFont()
        {
            if (string.IsNullOrEmpty(Element.FontName))
            {
                return;
            }

            var font = UIFont.FromName(Element.FontName, (Element.FontSize > 0) ? (float) Element.FontSize : 12.0f);

            if (font != null)
            {
                Control.Font = font;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            switch (e.PropertyName)
            {
                case "Checked":
                    Control.Checked = Element.Checked;
                    break;
                case "Text":
                    Control.Text = Element.Text;
                    break;
                case "TextColor":
                    Control.SetTitleColor(Element.TextColor.ToUIColor(), UIControlState.Normal);
                    Control.SetTitleColor(Element.TextColor.ToUIColor(), UIControlState.Selected);
                    break;
                case "Element":
                    break;
                case "FontSize":
                    UpdateFont();
                    break;
                case "FontName":
                    UpdateFont();
                    break;
                default:
//                    Debug.WriteLine("Property change for {0} has not been implemented.", e.PropertyName);
                    return;
            }
        }
    }
}