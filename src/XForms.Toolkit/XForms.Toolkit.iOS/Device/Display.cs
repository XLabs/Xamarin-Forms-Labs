using System;

namespace XForms.Toolkit
{
    /// <summary>
    /// Apple Display class.
    /// </summary>
    public class Display : IDisplay
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XForms.Toolkit.Display"/> class.
        /// </summary>
        /// <param name="height">Height in pixels.</param>
        /// <param name="width">Width in pixels.</param>
        /// <param name="xdpi">Pixel density for X.</param>
        /// <param name="ydpi">Pixel density for  Y.</param>
        internal Display (int height, int width, double xdpi, double ydpi)
        {
            this.Height = height;
            this.Width = width;
            this.Xdpi = xdpi;
            this.Ydpi = ydpi;
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

        #endregion
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="XForms.Toolkit.Display"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="XForms.Toolkit.Display"/>.</returns>
        public override string ToString()
        {
            return string.Format("[Screen: Height={0}, Width={1}, Xdpi={2}, Ydpi={3}]", Height, Width, Xdpi, Ydpi);
        }
    }
}

