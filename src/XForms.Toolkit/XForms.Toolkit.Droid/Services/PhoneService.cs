using Android.Telephony;
using Android.Content;
using Android.App;
using Android.Net;
using Xamarin.Forms;

[assembly: Dependency(typeof(XForms.Toolkit.Services.PhoneService))]
namespace XForms.Toolkit.Services
{
    /// <summary>
    /// Android Phone service implements <see cref="IPhoneService"/>.
    /// </summary>
    public class PhoneService : IPhoneService
    {
        /// <summary>
        /// Gets the telephony manager for Android.
        /// </summary>
        public static TelephonyManager Manager
        {
            get
            {
                return Application.Context.GetSystemService(Context.TelephonyService) as TelephonyManager;
            }
        }

        #region IPhone implementation
        /// <summary>
        /// Gets the cellular provider.
        /// </summary>
        public string CellularProvider
        {
            get
            {
                return PhoneService.Manager.NetworkOperatorName;
            }
        }

        /// <summary>
        /// Gets the ISO Country Code
        /// </summary>
        public string ICC
        {
            get
            {
                return PhoneService.Manager.SimCountryIso;
            }
        }

        /// <summary>
        /// Gets the Mobile Country Code
        /// </summary>
        public string MCC
        {
            get
            {
                return PhoneService.Manager.NetworkOperator.Remove(3,3);
            }
        }

        /// <summary>
        /// Gets the Mobile Network Code
        /// </summary>
        public string MNC
        {
            get
            {
                return PhoneService.Manager.NetworkOperator.Remove(0,3);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has cellular data enabled.
        /// </summary>
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
        public bool? IsNetworkAvailable
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Opens native dialog to dial the specified number
        /// </summary>
        /// <param name="number">Number to dial.</param>
        public void DialNumber(string number)
        {
            number.StartActivity(new Intent(Intent.ActionDial, Uri.Parse("tel:" + number)));
        }
        #endregion
    }
}

