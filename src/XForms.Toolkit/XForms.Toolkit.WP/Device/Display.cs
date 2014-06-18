using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Phone.Info;

namespace XForms.Toolkit
{
    /// <summary>
    /// Windows Phone 8 Display.
    /// </summary>
    public class Display : IDisplay
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XForms.Toolkit.Display"/> class.
        /// </summary>
        public Display()
        {
            object physicalScreenResolutionObject;

            if (DeviceExtendedProperties.TryGetValue("PhysicalScreenResolution", out physicalScreenResolutionObject))
            {
                var physicalScreenResolution = (Size)physicalScreenResolutionObject;
                this.Height = (int)physicalScreenResolution.Height;
                this.Width = (int)physicalScreenResolution.Width;
            }
            else
            {
                var scaleFactor = Application.Current.Host.Content.ScaleFactor;
                this.Height = (int)(Application.Current.Host.Content.ActualHeight * scaleFactor);
                this.Width = (int)(Application.Current.Host.Content.ActualWidth * scaleFactor);
            }

            object rawDpiX, rawDpiY;

            if (DeviceExtendedProperties.TryGetValue("RawDpiX", out rawDpiX))
            {
                this.Xdpi = (double)rawDpiX;
            }

            if (DeviceExtendedProperties.TryGetValue("RawDpiY", out rawDpiY))
            {
                this.Ydpi = (double)rawDpiY;
            }
        }

        #region IDisplay Members

        public int Height
        {
            get;
            private set;
        }

        public int Width
        {
            get;
            private set;
        }

        public double Xdpi
        {
            get;
            private set;
        }

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
            return inches * this.Xdpi * 100 / Application.Current.Host.Content.ScaleFactor;
        }

        /// <summary>
        /// Convert height in inches to runtime pixels
        /// </summary>
        public double HeightRequestInInches(double inches)
        {
            return inches * this.Ydpi * 100 / Application.Current.Host.Content.ScaleFactor;
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
