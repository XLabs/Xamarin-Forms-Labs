// ***********************************************************************
// Assembly         : XLabs.Platform.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Phone.cs" company="XLabs Team">
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
using XLabs.Platform.Services;

namespace XLabs.Platform.Device
{
    /// <summary>
    /// Apple iPhone.
    /// </summary>
    public class Phone : AppleDevice
    {
        /// <summary>
        /// The phone type.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum PhoneType
        {
            /// <summary>
            /// Unknown phone type.
            /// </summary>
            [Description("Unknown device")]
            Unknown = 0,

            /// <summary>
            /// The iPhone 1G.
            /// </summary>
            [Description("iPhone 1G")]
            IPhone1G = 1,

            /// <summary>
            /// The i phone3 g
            /// </summary>
            [Description("iPhone 3G")]
            IPhone3G,

            /// <summary>
            /// The i phone3 gs
            /// </summary>
            [Description("iPhone 3GS")]
            IPhone3Gs,

            /// <summary>
            /// The i phone4 GSM
            /// </summary>
            [Description("iPhone 4 GSM")]
            IPhone4Gsm,

            /// <summary>
            /// The i phone4 cdma
            /// </summary>
            [Description("iPhone 4 CDMA")]
            IPhone4Cdma,

            /// <summary>
            /// The i phone4 s
            /// </summary>
            [Description("iPhone 4S")]
            IPhone4S,

            /// <summary>
            /// The i phone5 GSM
            /// </summary>
            [Description("iPhone 5 GSM")]
            IPhone5Gsm,

            /// <summary>
            /// The i phone5 cdma
            /// </summary>
            [Description("iPhone 5 CDMA")]
            IPhone5Cdma,

            /// <summary>
            /// The i phone5 c cdma
            /// </summary>
            [Description("iPhone 5C CDMA")]
            IPhone5CCdma,

            /// <summary>
            /// The i phone5 c GSM
            /// </summary>
            [Description("iPhone 5C GSM")]
            IPhone5CGsm,

            /// <summary>
            /// The i phone5 s cdma
            /// </summary>
            [Description("iPhone 5S CDMA")]
            IPhone5SCdma,

            /// <summary>
            /// The i phone5 s GSM
            /// </summary>
            [Description("iPhone 5S GSM")]
            IPhone5SGsm,

            /// <summary>
            /// The iPhone6.
            /// </summary>
            [Description("iPhone 6")]
            IPhone6,

            /// <summary>
            /// The iPhone 6+.
            /// </summary>
            [Description("iPhone 6 Plus")]
            IPhone6Plus,

            /// <summary>
            /// The iPhone 6S.
            /// </summary>
            [Description("iPhone 6S")]
            IPhone6S,

            /// <summary>
            /// The iPhone 6S+.
            /// </summary>
            [Description("iPhone 6S Plus")]
            IPhone6SPlus,

            /// <sumary>
            /// The iPhone SE
            /// </sumary>
            [Description("iPhone SE")]
            IPhoneSE
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Phone" /> class.
        /// </summary>
        /// <param name="majorVersion">Major version.</param>
        /// <param name="minorVersion">Minor version.</param>
        public Phone(int majorVersion, int minorVersion)
        {
            PhoneService = new PhoneService();

            switch (majorVersion)
            {
                case 1:
                    Version = minorVersion == 1 ? PhoneType.IPhone1G : PhoneType.IPhone3G;
                    break;
                case 2:
                    Version = PhoneType.IPhone3Gs;
                    break;
                case 3:
                    Version = minorVersion == 1 ? PhoneType.IPhone4Gsm : PhoneType.IPhone4Cdma;
                    break;
                case 4:
                    Version = PhoneType.IPhone4S;
                    break;
                case 5:
                    Version = PhoneType.IPhone5Gsm + minorVersion - 1;
                    break;
                case 6:
                    Version = minorVersion == 1 ? PhoneType.IPhone5SCdma : PhoneType.IPhone5SGsm;
                    break;
                case 7:
                    Version = minorVersion == 1 ? PhoneType.IPhone6Plus : PhoneType.IPhone6;
                    break;
                case 8:
                    if (minorVersion == 1)
                        Version = PhoneType.IPhone6S;
                    else if (minorVersion == 2)
                        Version = PhoneType.IPhone6SPlus;
                    else if (minorVersion == 4)
                        Version = PhoneType.IPhoneSE;
                    break;
                default:
                    Version = PhoneType.Unknown;
                    break;
            }

            int width;
            int height;
            if(UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
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

            double baseDPI = 163; //dpi from 1st Gen iPhone devices
            double dpi = baseDPI * UIKit.UIScreen.MainScreen.Scale;
            if(Version == PhoneType.IPhone6Plus || Version == PhoneType.IPhone6SPlus)
            {
                dpi = 401;
            }

            Display = new Display(height, width, dpi, dpi);

            Name = HardwareVersion = Version.GetDescription();
        }

        /// <summary>
        /// Gets the version of iPhone.
        /// </summary>
        /// <value>The version.</value>
        public PhoneType Version { get; private set; }
    }
}