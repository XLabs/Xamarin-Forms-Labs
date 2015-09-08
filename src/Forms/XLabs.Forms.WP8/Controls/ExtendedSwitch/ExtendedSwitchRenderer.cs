using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedSwitch), typeof(ExtendedSwitchRenderer))]

namespace XLabs.Forms.Controls
{
	using System;
	using System.Windows.Controls;

	using Microsoft.Phone.Controls.Primitives;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.WinPhone;

	/// <summary>
	/// Class ExtendedSwitchRenderer.
	/// </summary>
	public class ExtendedSwitchRenderer : ViewRenderer<ExtendedSwitch, Border>
	{
		/// <summary>
		/// The _toggle switch
		/// </summary>
		private ToggleSwitchButton _toggleSwitch;

		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<ExtendedSwitch> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
			{
				SetNativeControl(new Border {
					Child = _toggleSwitch = new ToggleSwitchButton { HorizontalAlignment = System.Windows.HorizontalAlignment.Right }
				});

				_toggleSwitch.Checked += (s, a) => Element.IsToggled = true;
				_toggleSwitch.Unchecked += (s, a) => Element.IsToggled = false;
				SetTintColor(Element.TintColor);
			}

			if (e.OldElement != null)
			{
				e.OldElement.Toggled -= ElementToggled;
			}

			if (e.NewElement != null)
			{
				_toggleSwitch.IsChecked = e.NewElement.IsToggled;
				Element.Toggled += ElementToggled;
			}
		}

		/// <summary>
		/// Updates the native widget.
		/// </summary>
		protected override void UpdateNativeWidget()
		{
			base.UpdateNativeWidget();
			_toggleSwitch.IsChecked = Element.IsToggled;
		}

		/// <summary>
		/// Handles the <see cref="E:ElementPropertyChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == "TintColor")
			{
				SetTintColor(Element.TintColor);
			}
		}


		/// <summary>
		/// Sets the color of the tint.
		/// </summary>
		/// <param name="color">The color.</param>
		private void SetTintColor(Color color)
		{
			_toggleSwitch.SwitchForeground = color.ToBrush();
			//this.toggleSwitch.Background = color.ToBrush();
			//this.toggleSwitch.BorderBrush = color.ToBrush();
			//this.toggleSwitch.Foreground = color.ToBrush();
		}

		/// <summary>
		/// Elements the toggled.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ToggledEventArgs"/> instance containing the event data.</param>
		private void ElementToggled(object sender, ToggledEventArgs e)
		{
			_toggleSwitch.IsChecked = Element.IsToggled;
		}

		/// <summary>
		/// Controls the value changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void ControlValueChanged(object sender, EventArgs e)
		{
			Element.IsToggled = _toggleSwitch.IsChecked.Value;
		}
	}
}
