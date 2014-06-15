using System;
using System.ComponentModel;

namespace XForms.Toolkit
{
    /// <summary>
    /// Apple iPod.
    /// </summary>
    public class Pod: AppleDevice
    {
        public enum PodVersion
        {
            [Description("iPod Touch 1G")]
            FirstGeneration = 1,
            [Description("iPod Touch 2G")]
            SecondGeneration,
            [Description("iPod Touch 3G")]
            ThirdGeneration,
            [Description("iPod Touch 4G")]
            FourthGeneration,
            [Description("iPod Touch 5G")]
            FifthGeneration
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XForms.Toolkit.Pod"/> class.
        /// </summary>
        /// <param name="majorVersion">Major version.</param>
        /// <param name="minorVersion">Minor version.</param>
        internal Pod (int majorVersion, int minorVersion)
            : base()
        {
            this.Version = (PodVersion)majorVersion;
            this.PhoneService = null;

            this.Name = this.HardwareVersion = this.Version.GetDescription ();

            if (majorVersion > 4)
            {
                this.Display = new Display(1136, 640, 326, 326);
            }
            else if (majorVersion > 3)
            {
                this.Display = new Display(960, 640, 326, 326);
            }
            else
            {
                this.Display = new Display(480, 320, 163, 163);
            }
        }

        /// <summary>
        /// Gets the version of iPod.
        /// </summary>
        /// <value>The version.</value>
        public PodVersion Version
        {
            get;
            private set;
        }
    }
}

