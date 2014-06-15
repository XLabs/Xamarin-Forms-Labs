using System;
using MonoTouch.CoreTelephony;
using MonoTouch.UIKit;

namespace XForms.Toolkit.Services
{
    /// <summary>
    /// Apple Phone service implements <see cref="IPhoneService"/>.
    /// </summary>
    public class PhoneService : IPhoneService
    {
        private static Lazy<CTTelephonyNetworkInfo> TelNet = new Lazy<CTTelephonyNetworkInfo> ();

        #region IPhone implementation
        /// <summary>
        /// Gets the cellular provider.
        /// </summary>
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
                return null;
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
                return null;
            }
        }

        /// <summary>
        /// Gets the ISO Country Code
        /// </summary>
        public string ICC
        {
            get
            {
                return TelNet.Value.SubscriberCellularProvider.IsoCountryCode;
            }
        }

        /// <summary>
        /// Gets the Mobile Country Code
        /// </summary>
        public string MCC
        {
            get
            {
                return TelNet.Value.SubscriberCellularProvider.MobileCountryCode;
            }
        }

        /// <summary>
        /// Gets the Mobile Network Code
        /// </summary>
        public string MNC
        {
            get
            {
                return TelNet.Value.SubscriberCellularProvider.MobileNetworkCode;
            }
        }

        /// <summary>
        /// Opens native dialog to dial the specified number
        /// </summary>
        /// <param name="number">Number to dial.</param>
        public void DialNumber(string number)
        {
            UIApplication.SharedApplication.OpenUrl(new MonoTouch.Foundation.NSUrl("tel:" + number));
        }
        #endregion
    }
}

