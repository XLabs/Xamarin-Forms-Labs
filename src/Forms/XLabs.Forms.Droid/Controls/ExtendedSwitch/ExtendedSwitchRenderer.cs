using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedSwitch), typeof(ExtendedSwitchRenderer))]

namespace XLabs.Forms.Controls
{
	using System;

	using Android.Graphics.Drawables;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.Android;

	using Switch = Android.Widget.Switch;

	/// <summary>
	/// Class ExtendedSwitchRenderer.
	/// </summary>
	public class ExtendedSwitchRenderer : ViewRenderer<ExtendedSwitch, Switch>
	{
		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<ExtendedSwitch> e)
		{
			if (this.Control == null)
			{
				var toggle = new Switch(this.Context);
				toggle.CheckedChange += ControlValueChanged;
				this.SetNativeControl(toggle);
			}

			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				e.OldElement.Toggled -= ElementToggled;
			}

			if (e.NewElement != null)
			{
				this.Control.Checked = e.NewElement.IsToggled;
				this.SetTintColor(this.Element.TintColor);
				this.Element.Toggled += ElementToggled;
			}
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
				this.SetTintColor(this.Element.TintColor);
			}
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Control.CheckedChange -= this.ControlValueChanged;
				this.Element.Toggled -= ElementToggled;
			}

			base.Dispose(disposing);
		}

		/// <summary>
		/// Sets the color of the tint.
		/// </summary>
		/// <param name="color">The color.</param>
		private void SetTintColor(Color color)
		{
			var thumbStates = new StateListDrawable();
			thumbStates.AddState(new int[]{Android.Resource.Attribute.StateChecked}, new ColorDrawable(color.ToAndroid()));
			//thumbStates.AddState(new int[]{-android.R.attr.state_enabled}, new ColorDrawable(colorDisabled));
			//thumbStates.addState(new int[]{}, new ColorDrawable(this.app.colorOff)); // this one has to come last
			this.Control.ThumbDrawable = thumbStates;
		}

		/// <summary>
		/// Handles the Toggled event of the Element control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="ToggledEventArgs"/> instance containing the event data.</param>
		private void ElementToggled(object sender, ToggledEventArgs e)
		{
			this.Control.Checked = this.Element.IsToggled;
		}

		/// <summary>
		/// Handles the ValueChanged event of the Control control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void ControlValueChanged(object sender, EventArgs e)
		{
			this.Element.IsToggled = this.Control.Checked;
		}
	}
}