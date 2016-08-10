// ***********************************************************************
// Assembly         : XLabs.Platform.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="PhoneService.cs" company="XLabs Team">
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

using System;
using SystemConfiguration;
using CoreTelephony;
using Foundation;
using MessageUI;
using UIKit;

namespace XLabs.Platform.Services
{
    /// <summary>
    /// Apple Phone service implements <see cref="IPhoneService" />.
    /// </summary>
    public class PhoneService : IPhoneService
    {
        /// <summary>
        /// The tel net
        /// </summary>
        private static readonly Lazy<CTTelephonyNetworkInfo> TelNet = new Lazy<CTTelephonyNetworkInfo>(() => new CTTelephonyNetworkInfo());

        #region IPhone implementation

        /// <summary>
        /// Gets the cellular provider.
        /// </summary>
        /// <value>The cellular provider name.</value>
        public string CellularProvider
        {
            get
            {
                return TelNet.Value.SubscriberCellularProvider.CarrierName;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has cellular data enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is cellular data enabled; otherwise, <c>false</c>.</value>
        public bool? IsCellularDataEnabled
        {
            get
            {
                NetworkReachabilityFlags flags;
                Reachability.IsNetworkAvailable(out flags);
                return (flags & NetworkReachabilityFlags.IsWWAN) != 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has cellular data roaming enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is cellular data roaming enabled; otherwise, <c>false</c>.</value>
        public bool? IsCellularDataRoamingEnabled
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is network available.
        /// </summary>
        /// <value><c>true</c> if this instance is network available; otherwise, <c>false</c>.</value>
        public bool? IsNetworkAvailable
        {
            get
            {
                NetworkReachabilityFlags flags;
                return Reachability.IsNetworkAvailable(out flags);
            }
        }

        /// <summary>
        /// Gets the ISO Country Code.
        /// </summary>
        /// <value>The ISO Country Code.</value>
        public string ICC
        {
            get
            {
                return TelNet.Value.SubscriberCellularProvider.IsoCountryCode;
            }
        }

        /// <summary>
        /// Gets the Mobile Country Code.
        /// </summary>
        /// <value>The Mobile Country Code.</value>
        public string MCC
        {
            get
            {
                return TelNet.Value.SubscriberCellularProvider.MobileCountryCode;
            }
        }

        /// <summary>
        /// Gets the Mobile Network Code.
        /// </summary>
        /// <value>The Mobile Network Code.</value>
        public string MNC
        {
            get
            {
                return TelNet.Value.SubscriberCellularProvider.MobileNetworkCode;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance can send SMS.
        /// </summary>
        /// <value><c>true</c> if this instance can send SMS; otherwise, <c>false</c>.</value>
        public bool CanSendSMS
        {
            get
            {
                return MFMessageComposeViewController.CanSendText;
            }
        }

        /// <summary>
        /// Opens native dialog to dial the specified number.
        /// </summary>
        /// <param name="number">Number to dial.</param>
        public void DialNumber(string number)
        {
            if (string.IsNullOrEmpty (number))
                return;
            UIApplication.SharedApplication.OpenUrl(NSUrl.FromString("tel://" + number.Replace (" ", "")));
        }

        /// <summary>
        /// Sends the SMS.
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="body">The body.</param>
        public void SendSMS(string to, string body)
        {
            if (CanSendSMS)
            {
                var smsController = new MFMessageComposeViewController { Body = body, Recipients = new[] { to } };
                smsController.Finished += (sender, e) => smsController.DismissViewController(true, null);
                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(smsController, true, null);
            }
        }

        #endregion
    }
}