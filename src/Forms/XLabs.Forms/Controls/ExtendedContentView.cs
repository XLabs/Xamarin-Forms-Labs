using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Extended content view.
    /// </summary>
    public class ExtendedContentView : ContentView
    {
        /// <summary>
        /// The width request in inches property.
        /// </summary>
        public static readonly BindableProperty WidthRequestInInchesProperty =
            BindableProperty.Create<ExtendedContentView, double>(
                p => p.WidthRequestInInches, default(double));

        /// <summary>
        /// Gets or sets the width request in inches.
        /// </summary>
        /// <value>The width request in inches.</value>
        public double WidthRequestInInches
        {
            get
            {
                return this.GetHeightRequestInInches();
            }

            set
            {
                this.SetHeightRequestInInches(value);
                this.SetValue(WidthRequestInInchesProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the height request in inches.
        /// </summary>
        /// <value>The height request in inches.</value>
        public double HeightRequestInInches
        {
            get
            {
                return this.GetHeightRequestInInches();
            }

            set
            {
                this.SetHeightRequestInInches(value);
            }
        }
    }
}
