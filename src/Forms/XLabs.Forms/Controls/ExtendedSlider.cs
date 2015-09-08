using System;
using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class ExtendedSlider.
	/// </summary>
	public class ExtendedSlider : Slider
	{
		/// <summary>
		/// The current step value property
		/// </summary>
		public static readonly BindableProperty CurrentStepValueProperty =
								BindableProperty.Create<ExtendedSlider, double>(p => p.StepValue, 1.0f);

		/// <summary>
		/// Gets or sets the step value.
		/// </summary>
		/// <value>The step value.</value>
		public double StepValue
		{
			get { return (double)GetValue(CurrentStepValueProperty); }

			set { SetValue(CurrentStepValueProperty, value); }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExtendedSlider"/> class.
		/// </summary>
		public ExtendedSlider()
		{
			ValueChanged += OnSliderValueChanged;
		}

		/// <summary>
		/// Handles the <see cref="E:SliderValueChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ValueChangedEventArgs"/> instance containing the event data.</param>
		private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
		{
			var newStep = Math.Round(e.NewValue / StepValue);

			Value = newStep * StepValue;
		}
	}
}
