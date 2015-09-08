using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using XLabs.Forms.Controls;
using NativeCheckBox = System.Windows.Controls.RadioButton;

[assembly: ExportRenderer(typeof (CustomRadioButton), typeof (RadioButtonRenderer))]

namespace XLabs.Forms.Controls
{
    public class RadioButtonRenderer : ViewRenderer<CustomRadioButton, NativeCheckBox>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<CustomRadioButton> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.CheckedChanged -= CheckedChanged;
            }

            if (Control == null)
            {
                var checkBox = new NativeCheckBox();

                checkBox.Checked += (s, args) => Element.Checked = true;
                checkBox.Unchecked += (s, args) => Element.Checked = false;

                SetNativeControl(checkBox);
            }

            Control.Content = e.NewElement.Text;
            Control.IsChecked = e.NewElement.Checked;

            UpdateFont();

            Element.CheckedChanged += CheckedChanged;
            Element.PropertyChanged += ElementOnPropertyChanged;
        }

        private void ElementOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case "Checked":
                    Control.IsChecked = Element.Checked;
                    break;
                case "TextColor":
                    Control.Foreground = Element.TextColor.ToBrush();
                    break;
                case "FontName":
                case "FontSize":
                    UpdateFont();
                    break;
                case "Text":
                    Control.Content = Element.Text;
                    break;
                default:
                    Debug.WriteLine("Property change for {0} has not been implemented.",
                        propertyChangedEventArgs.PropertyName);
                    break;
            }
        }

        private void CheckedChanged(object sender, EventArgs<bool> eventArgs)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Control.Content = Element.Text;
                Control.IsChecked = eventArgs.Value;
            });
        }

        /// <summary>
        /// Updates the font.
        /// </summary>
        private void UpdateFont()
        {
            if (!string.IsNullOrEmpty(Element.FontName))
            {
                Control.FontFamily = new FontFamily(Element.FontName);
            }

            Control.FontSize = (Element.FontSize > 0) ? (float)Element.FontSize : 12.0f;
        }
    }
}