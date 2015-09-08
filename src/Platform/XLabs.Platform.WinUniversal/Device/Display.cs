namespace XLabs.Platform.Device
{
    using System;
    using Windows.Graphics.Display;

    /// <summary>
    /// Windows Phone 8 Display.
    /// </summary>
    public class Display : IDisplay
    {
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents the current <see cref="Display" />.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents the current <see cref="Display" />.</returns>
        public override string ToString()
        {
            return string.Format("[Screen: Height={0}, Width={1}, Xdpi={2:0.0}, Ydpi={3:0.0}]", Height, Width, Xdpi, Ydpi);
        }

        #region IDisplay Members

        /// <summary>
        /// Gets the screen height in pixels
        /// </summary>
        /// <value>The height.</value>
        public int Height { get { throw new NotImplementedException();} }

        /// <summary>
        /// Gets the screen width in pixels
        /// </summary>
        /// <value>The width.</value>
        public int Width { get { throw new NotImplementedException(); } }

        /// <summary>
        /// Gets the screens X pixel density per inch
        /// </summary>
        /// <value>The xdpi.</value>
        public double Xdpi { get { return Info.RawDpiX; } }

        /// <summary>
        /// Gets the screens Y pixel density per inch
        /// </summary>
        /// <value>The ydpi.</value>
        public double Ydpi { get { return Info.RawDpiY; }}

        /// <summary>
        /// Convert width in inches to runtime pixels
        /// </summary>
        /// <param name="inches">The inches.</param>
        /// <returns>System.Double.</returns>
        public double WidthRequestInInches(double inches)
        {
            return inches * Info.LogicalDpi;
        }

        /// <summary>
        /// Convert height in inches to runtime pixels
        /// </summary>
        /// <param name="inches">The inches.</param>
        /// <returns>System.Double.</returns>
        public double HeightRequestInInches(double inches)
        {
            return inches * Info.LogicalDpi;
        }

        #endregion

        private static DisplayInformation Info
        {
            get { return DisplayInformation.GetForCurrentView(); }
        }
    }
}