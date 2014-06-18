using System;
using Android.App;

namespace XForms.Toolkit
{
    /// <summary>
    /// Android Display implements <see cref="IDisplay"/>.
    /// </summary>
    public class Display : IDisplay
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XForms.Toolkit.Display"/> class.
        /// </summary>
        public Display()
        {
            var dm = Metrics;
            this.Height = dm.HeightPixels;
            this.Width = dm.WidthPixels;
            this.Xdpi = dm.Xdpi;
            this.Ydpi = dm.Ydpi;
        }

        public static Android.Util.DisplayMetrics Metrics
        {
            get
            {
                return Application.Context.Resources.DisplayMetrics;
            }
        }

        #region IScreen implementation
        /// <summary>
        /// Gets the screen height in pixels
        /// </summary>
        public int Height
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the screen width in pixels
        /// </summary>
        public int Width
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the screens X pixel density per inch
        /// </summary>
        public double Xdpi
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the screens Y pixel density per inch
        /// </summary>
        public double Ydpi
        {
            get;
            private set;
        }

        /// <summary>
        /// Convert width in inches to runtime pixels
        /// </summary>
        public double WidthRequestInInches(double inches)
        {
            return inches * this.Xdpi / Metrics.Density;
        }

        /// <summary>
        /// Convert height in inches to runtime pixels
        /// </summary>
        public double HeightRequestInInches(double inches)
        {
            return inches * this.Ydpi / Metrics.Density;
        }
        #endregion

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="XForms.Toolkit.Display"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="XForms.Toolkit.Display"/>.</returns>
        public override string ToString()
        {
            return string.Format("[Screen: Height={0}, Width={1}, Xdpi={2:0.0}, Ydpi={3:0.0}]", Height, Width, Xdpi, Ydpi);
        }
    }
}

