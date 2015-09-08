using Xamarin.Forms;

using XLabs.Forms.Controls;
using XLabs.Forms.Controls.SensorBar;

[assembly: ExportRenderer(typeof(SensorBarView), typeof(SensorBarViewRenderer))]

namespace XLabs.Forms.Controls
{
    using System.ComponentModel;

    using Xamarin.Forms.Platform.iOS;

    using XLabs.Forms.Controls.SensorBar;

    /// <summary>
    /// The sensor bar view renderer.
    /// </summary>
    public class SensorBarViewRenderer : ViewRenderer<SensorBarView, UISensorBar>
    {
        private UISensorBar _sensorBar;

        /// <summary>
        /// The on element changed callback.
        /// </summary>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected override void OnElementChanged(ElementChangedEventArgs<SensorBarView> e)
        {
            base.OnElementChanged(e);

            _sensorBar = new UISensorBar(Bounds);
            SetProperties ();
            Element.PropertyChanged += OnPropertyChanged;

            SetNativeControl(_sensorBar);
        }

        /// <summary>
        /// Handles the <see cref="E:PropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SetProperties ();
        }

        /// <summary>
        /// Sets the properties.
        /// </summary>
        private void SetProperties()
        {
            _sensorBar.CurrentValue = Element.CurrentValue;
            _sensorBar.Limit = Element.Limit;
            _sensorBar.NegativeColor = Element.NegativeColor.ToUIColor();
            _sensorBar.PositiveColor = Element.PositiveColor.ToUIColor();
        }
    }
}

