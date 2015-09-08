using System;
using System.ComponentModel;
using Android.Graphics;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof (CustomRadioButton), typeof (RadioButtonRenderer))]

namespace XLabs.Forms.Controls
{
    //  using NativeRadioButton = RadioButton;

    public class RadioButtonRenderer : ViewRenderer<CustomRadioButton, RadioButton>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<CustomRadioButton> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.PropertyChanged += ElementOnPropertyChanged;
            }

            if (Control == null)
            {
                var radButton = new RadioButton(Context);

                radButton.CheckedChange += radButton_CheckedChange;

                SetNativeControl(radButton);
            }

            Control.Text = e.NewElement.Text;
            //Control.TextSize = 14;
            Control.Checked = e.NewElement.Checked;
            Control.SetTextColor(e.NewElement.TextColor.ToAndroid());

            if (e.NewElement.FontSize > 0)
            {
                Control.TextSize = (float) e.NewElement.FontSize;
            }

            if (!string.IsNullOrEmpty(e.NewElement.FontName))
            {
                Control.Typeface = TrySetFont(e.NewElement.FontName);
            }

            Element.PropertyChanged += ElementOnPropertyChanged;
        }

        private void radButton_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            Element.Checked = e.IsChecked;
        }

        private void ElementOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Checked":
                    Control.Checked = Element.Checked;
                    break;
                case "Text":
                    Control.Text = Element.Text;
                    break;
                case "TextColor":
                    Control.SetTextColor(Element.TextColor.ToAndroid());
                    break;
                case "FontName":
                    if (!string.IsNullOrEmpty(Element.FontName))
                    {
                        Control.Typeface = TrySetFont(Element.FontName);
                    }
                    break;
                case "FontSize":
                    if (Element.FontSize > 0)
                    {
                        Control.TextSize = (float) Element.FontSize;
                    }
                    break;
            }
        }

        /// <summary>
        ///     Tries the set font.
        /// </summary>
        /// <param name="fontName">Name of the font.</param>
        /// <returns>Typeface.</returns>
        private Typeface TrySetFont(string fontName)
        {
            var tf = Typeface.Default;

            try
            {
                tf = Typeface.CreateFromAsset(Context.Assets, fontName);

                return tf;
            }
            catch (Exception ex)
            {
                Console.Write("not found in assets {0}", ex);
                try
                {
                    tf = Typeface.CreateFromFile(fontName);

                    return tf;
                }
                catch (Exception ex1)
                {
                    Console.Write(ex1);

                    return Typeface.Default;
                }
            }
        }
    }
}