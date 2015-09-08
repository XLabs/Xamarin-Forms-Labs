using Xamarin.Forms;

using XLabs.Forms.Controls;
using XLabs.Forms.Controls.SensorBar;

[assembly: ExportRenderer(typeof(SensorBarView), typeof(SensorBarViewRenderer))]
namespace XLabs.Forms.Controls
{
	using System.ComponentModel;

	using Xamarin.Forms.Platform.Android;

	using XLabs.Forms.Controls.SensorBar;

	/// <summary>
	/// Class SensorBarViewRenderer.
	/// </summary>
	public class SensorBarViewRenderer : ViewRenderer<SensorBarView, SensorBarDroidView>
	{
		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<SensorBarView> e)
		{
			base.OnElementChanged (e);

			if (e.NewElement == null)
			{
				return;
			}

			if (this.Control == null)
			{
				this.SetNativeControl(new SensorBarDroidView(this.Context));
			}

			this.SetProperties();
		}

		/// <summary>
		/// Handles the <see cref="E:ElementPropertyChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			this.SetProperties();
		}

		/// <summary>
		/// Sets the properties.
		/// </summary>
		private void SetProperties()
		{
			this.Control.CurrentValue = this.Element.CurrentValue;
			this.Control.Limit = this.Element.Limit;
			this.Control.NegativeColor = this.Element.NegativeColor.ToAndroid();
			this.Control.PositiveColor = this.Element.PositiveColor.ToAndroid();

			this.Control.Invalidate();
		}
	}
}

