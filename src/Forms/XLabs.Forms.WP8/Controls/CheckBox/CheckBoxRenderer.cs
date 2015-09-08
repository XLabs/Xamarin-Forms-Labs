using Xamarin.Forms;

using XLabs.Forms.Controls;
//using System.Windows.Controls;

[assembly: ExportRenderer(typeof(CheckBox), typeof(CheckBoxRenderer))]

namespace XLabs.Forms.Controls
{
	using System.ComponentModel;
	using System.Windows.Media;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.WinPhone;

	using XLabs;

	using NativeCheckBox = System.Windows.Controls.CheckBox;

	/// <summary>
	/// Class CheckBoxRenderer.
	/// </summary>
	public class CheckBoxRenderer : ViewRenderer<CheckBox, NativeCheckBox>
	{
		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<CheckBox> e)
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
			Control.Foreground = e.NewElement.TextColor.ToBrush();
			
			UpdateFont();

			Element.CheckedChanged += CheckedChanged;
			Element.PropertyChanged += ElementOnPropertyChanged;
		}

		/// <summary>
		/// Elements the on property changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="propertyChangedEventArgs">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
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
				case "CheckedText":
				case "UncheckedText":
					Control.Content = Element.Text;
					break;
				default:
					System.Diagnostics.Debug.WriteLine("Property change for {0} has not been implemented.", propertyChangedEventArgs.PropertyName);
					break;
			}
		}

		/// <summary>
		/// Checkeds the changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="eventArgs">The event arguments.</param>
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
