// ***********************************************************************
// Assembly         : XLabs.Platform.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Pad.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System.ComponentModel;
using XLabs.Platform.Extensions;

namespace XLabs.Platform.Device
{
    /// <summary>
    /// Apple iPad.
    /// </summary>
    public class Pad : AppleDevice
    {
        /// <summary>
        /// Enum IPadVersion
        /// </summary>
        public enum IPadVersion
        {
            /// <summary>
            /// The unknown
            /// </summary>
            Unknown = 0,

            /// <summary>
            /// The i pad1
            /// </summary>
            [Description("iPad 1G")]
            IPad1 = 1,

            /// <summary>
            /// The i pad2 wifi
            /// </summary>
            [Description("iPad 2G WiFi")]
            IPad2Wifi,

            /// <summary>
            /// The i pad2 GSM
            /// </summary>
            [Description("iPad 2G GSM")]
            IPad2Gsm,

            /// <summary>
            /// The i pad2 cdma
            /// </summary>
            [Description("iPad 2G CDMA")]
            IPad2Cdma,

            /// <summary>
            /// The i pad2 wifi emc2560
            /// </summary>
            [Description("iPad 2G WiFi")]
            IPad2WifiEmc2560,

            /// <summary>
            /// The i pad mini wifi
            /// </summary>
            [Description("iPad Mini WiFi")]
            IPadMiniWifi,

            /// <summary>
            /// The i pad mini GSM
            /// </summary>
            [Description("iPad Mini GSM")]
            IPadMiniGsm,

            /// <summary>
            /// The i pad mini cdma
            /// </summary>
            [Description("iPad Mini CDMA")]
            IPadMiniCdma,

            /// <summary>
            /// The i pad3 wifi
            /// </summary>
            [Description("iPad 3G WiFi")]
            IPad3Wifi,

            /// <summary>
            /// The i pad3 cdma
            /// </summary>
            [Description("iPad 3G CDMA")]
            IPad3Cdma,

            /// <summary>
            /// The i pad3 GSM
            /// </summary>
            [Description("iPad 3G GSM")]
            IPad3Gsm,

            /// <summary>
            /// The i pad4 wifi
            /// </summary>
            [Description("iPad 4G WiFi")]
            IPad4Wifi,

            /// <summary>
            /// The i pad4 GSM
            /// </summary>
            [Description("iPad 4G GSM")]
            IPad4Gsm,

            /// <summary>
            /// The i pad4 cdma
            /// </summary>
            [Description("iPad 4G CDMA")]
            IPad4Cdma,

            /// <summary>
            /// The i pad air wifi
            /// </summary>
            [Description("iPad Air WiFi")]
            IPadAirWifi,

            /// <summary>
            /// The i pad air GSM
            /// </summary>
            [Description("iPad Air GSM")]
            IPadAirGsm,

            /// <summary>
            /// The i pad air cdma
            /// </summary>
            [Description("iPad Air CDMA")]
            IPadAirCdma,

            /// <summary>
            /// The i pad mini2 g wi fi
            /// </summary>
            [Description("iPad Mini 2G WiFi")]
            IPadMini2GWiFi,

            /// <summary>
            /// The i pad mini2 g cellular
            /// </summary>
            [Description("iPad Mini 2G Cellular")]
            IPadMini2GCellular,

            /// <summary>
            /// The i pad mini3
            /// </summary>
            [Description("iPad Mini 3")]
            IPadMini3,

            /// <summary>
            /// The i pad mini3 Wifi
            /// </summary>
            [Description("iPad Mini 3 Wifi")]
            IPadMini3Wifi,

            /// <summary>
            /// The i pad mini3 Wifi
            /// </summary>
            [Description("iPad Mini 3 Wifi & LTE")]
            IPadMini3Lte,

            /// <summary>
            /// The i pad mini4
            /// </summary>
            [Description("iPad Mini 4")]
            IPadMini4,

            /// <summary>
            /// The i pad mini4 Cellular
            /// </summary>
            [Description("iPad Mini 4 Cellular")]
            IPadMini4Cellular,

            /// <summary>
            /// The i pad air 2 wifi
            /// </summary>
            [Description("iPad Air 2 WiFi")]
            IPadAir2Wifi,

            /// <summary>
            /// The i pad air 2 GSM
            /// </summary>
            [Description("iPad Air 2 Cellular")]
            IPadAir2Cellular,

            /// <summary>
            /// The i pad pro 9.7"
            /// </summary>
            [Description("iPad Pro 9.7\"")]
            IPadPro1_97,

            /// <summary>
            /// The i pad pro 9.7" Cellular
            /// </summary>
            [Description("iPad Pro 9.7\" Cellular")]
            IPadPro1_97Cellular,

            /// <summary>
            /// The i pad pro 12.9"
            /// </summary>
            [Description("iPad Pro 12.9\"")]
            IPadPro1_129,

            /// <summary>
            /// The i pad pro 12.9" Cellular
            /// </summary>
            [Description("iPad Pro 12.9\" Cellular")]
            IPadPro1_129Cellular
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pad" /> class.
        /// </summary>
        /// <param name="majorVersion">Major version.</param>
        /// <param name="minorVersion">Minor version.</param>
        public Pad(int majorVersion, int minorVersion)
        {
            PhoneService = null;
            double baseDPI = 132;
            switch (majorVersion)
            {
                case 1:
                    baseDPI = 132;
                    Version = IPadVersion.IPad1;
                    break;
                case 2:
                    baseDPI = minorVersion > 4 ? 163 : 132;
                    Version = IPadVersion.IPad2Wifi + minorVersion - 1;
                    break;
                case 3:
                    baseDPI = minorVersion > 3 ? 163 : 132;
                    Version = IPadVersion.IPad3Wifi + minorVersion - 1;
                    break;
                case 4:
                    baseDPI = minorVersion > 3 ? 163 : 132;
                    Version = IPadVersion.IPadAirWifi + minorVersion - 1;
                    break;
                case 5:
                    baseDPI = minorVersion < 3 ? 163 : 132;
                    Version = IPadVersion.IPadMini4 + minorVersion - 1;
                    break;
                case 6:
                    baseDPI = 132;
                    switch (minorVersion)
                    {
                        case 3:
                            Version = IPadVersion.IPadPro1_97;
                            break;
                        case 4:
                            Version = IPadVersion.IPadPro1_97Cellular;
                            break;
                        case 7:
                            Version = IPadVersion.IPadPro1_129;
                            break;
                        case 8:
                            Version = IPadVersion.IPadPro1_129Cellular;
                            break;
                    }
                    break;
                default:
                    Version = IPadVersion.Unknown;
                    break;
            }

            int width;
            int height;
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                CoreGraphics.CGRect bounds = UIKit.UIScreen.MainScreen.NativeBounds;
                width = (int)bounds.Width;
                height = (int)bounds.Height;
            }
            else
            {
                //All older devices are portrait by design so treat the default bounds as such
                CoreGraphics.CGRect bounds = UIKit.UIScreen.MainScreen.Bounds;
                width = Math.Min((int)bounds.Width, (int)bounds.Height);
                height = Math.Max((int)bounds.Width, (int)bounds.Height);
            }

            width *= (int)UIKit.UIScreen.MainScreen.Scale;
            height *= (int)UIKit.UIScreen.MainScreen.Scale;
            
            double dpi = baseDPI * UIKit.UIScreen.MainScreen.Scale;

            Display = new Display(height, width, dpi, dpi);

            Name = HardwareVersion = Version.GetDescription();
        }

        /// <summary>
        /// Gets the version of the iPad.
        /// </summary>
        /// <value>The version.</value>
        public IPadVersion Version { get; private set; }
    }
}