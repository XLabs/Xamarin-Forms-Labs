using System;
using System.ComponentModel;

namespace XForms.Toolkit
{
    /// <summary>
    /// Apple iPad.
    /// </summary>
    public class Pad : AppleDevice
    {
        public enum iPadVersion
        {
            Unknown = 0,
            [Description("iPad 1G")]
            iPad1 = 1,
            [Description("iPad 2G WiFi")]
            iPad2Wifi,
            [Description("iPad 2G GSM")]
            iPad2GSM,
            [Description("iPad 2G CDMA")]
            iPad2CDMA,            
            [Description("iPad 2G WiFi")]
            iPad2WifiEMC2560,
            [Description("iPad Mini WiFi")]
            iPadMiniWifi,
            [Description("iPad Mini GSM")]
            iPadMiniGSM,
            [Description("iPad Mini CDMA")]
            iPadMiniCDMA,
            [Description("iPad 3G WiFi")]
            iPad3Wifi,
            [Description("iPad 3G CDMA")]
            iPad3CDMA,
            [Description("iPad 3G GSM")]
            iPad3GSM,
            [Description("iPad 4G WiFi")]
            iPad4Wifi,
            [Description("iPad 4G GSM")]
            iPad4GSM,
            [Description("iPad 4G CDMA")]
            iPad4CDMA,
            [Description("iPad Air WiFi")]
            iPadAirWifi,
            [Description("iPad Air GSM")]
            iPadAirGSM,
            [Description("iPad Air CDMA")]
            iPadAirCDMA,
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XForms.Toolkit.Pad"/> class.
        /// </summary>
        /// <param name="majorVersion">Major version.</param>
        /// <param name="minorVersion">Minor version.</param>
        internal Pad (int majorVersion, int minorVersion)
            : base()
        {
            this.PhoneService = null;
            double dpi;
            switch (majorVersion)
            {
            case 1:
                this.Version = iPadVersion.iPad1;
                this.Display = new Display (1024, 768, 132, 132);
                break;
            case 2:
                dpi = minorVersion > 4 ? 163 : 132;
                this.Version = iPadVersion.iPad2Wifi + minorVersion - 1;
                this.Display = new Display (1024, 768, dpi, dpi);
                break;
            case 3:
                this.Version = iPadVersion.iPad3Wifi + minorVersion - 1;
                this.Display = new Display (2048, 1536, 264, 264);
                break;
            case 4:
                dpi = minorVersion > 3 ? 326 : 264;
                this.Version = iPadVersion.iPadAirWifi + minorVersion - 1;
                this.Display = new Display (2048, 1536, dpi, dpi);
                break;
            default:
                this.Version = iPadVersion.Unknown;
                break;
            }

            this.Name = this.HardwareVersion = this.Version.GetDescription ();
        }

        /// <summary>
        /// Gets the version of the iPad.
        /// </summary>
        public iPadVersion Version
        {
            get;
            private set;
        }
    }
}

