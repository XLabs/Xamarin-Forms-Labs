using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Enum GradientOrientation
    /// </summary>
    public enum GradientOrientation
    {
        /// <summary>
        /// The vertical
        /// </summary>
        Vertical,
        /// <summary>
        /// The horizontal
        /// </summary>
        Horizontal
    }

    /// <summary>
    /// ContentView that allows you to have a Gradient for
    /// the background. Let there be Gradients!
    /// </summary>
    public class GradientContentView : ContentView
    {
        /// <summary>
        /// Start color of the gradient
        /// Defaults to White
        /// </summary>
        public GradientOrientation Orientation
        {
            get { return (GradientOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a BindableProperty as the backing store for StartColor.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty OrientationProperty =
            BindableProperty.Create<GradientContentView, GradientOrientation>(x => x.Orientation, GradientOrientation.Vertical, BindingMode.OneWay);

        /// <summary>
        /// Start color of the gradient
        /// Defaults to White
        /// </summary>
        public Color StartColor
        {
            get { return (Color)GetValue(StartColorProperty); }
            set { SetValue(StartColorProperty, value); }
        }

        // Using a BindableProperty as the backing store for StartColor.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty StartColorProperty =
            BindableProperty.Create<GradientContentView, Color>(x => x.StartColor, Color.White, BindingMode.OneWay);

        /// <summary>
        /// End color of the gradient
        /// Defaults to Black
        /// </summary>
        public Color EndColor
        {
            get { return (Color)GetValue(EndColorProperty); }
            set { SetValue(EndColorProperty, value); }
        }

        // Using a BindableProperty as the backing store for StartColor.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty EndColorProperty =
            BindableProperty.Create<GradientContentView, Color>(x => x.EndColor, Color.Black, BindingMode.OneWay);
    }
}
