namespace XLabs.Platform.Services
{
	using Microsoft.Phone.Net.NetworkInformation;
	using Microsoft.Phone.Tasks;

	/// <summary>
	///     Phone service for Windows Phone devices.
	/// </summary>
	public class PhoneService : IPhoneService
	{
		/// <summary>
		///     Gets the cellular provider.
		/// </summary>
		/// <value>The cellular provider.</value>
		public string CellularProvider
		{
			get
			{
				return DeviceNetworkInformation.CellularMobileOperator;
			}
		}

		/// <summary>
		///     Gets a value indicating whether this instance has cellular data enabled.
		/// </summary>
		/// <value>
		///     <c>null</c> if [is cellular data enabled] contains no value, <c>true</c> if [is cellular data enabled];
		///     otherwise, <c>false</c>.
		/// </value>
		public bool? IsCellularDataEnabled
		{
			get
			{
				return DeviceNetworkInformation.IsCellularDataEnabled;
			}
		}

		/// <summary>
		///     Gets a value indicating whether this instance has cellular data roaming enabled.
		/// </summary>
		/// <value>
		///     <c>null</c> if [is cellular data roaming enabled] contains no value, <c>true</c> if [is cellular data roaming
		///     enabled]; otherwise, <c>false</c>.
		/// </value>
		public bool? IsCellularDataRoamingEnabled
		{
			get
			{
				return DeviceNetworkInformation.IsCellularDataRoamingEnabled;
			}
		}

		/// <summary>
		///     Gets a value indicating whether this instance is network available.
		/// </summary>
		/// <value>
		///     <c>null</c> if [is network available] contains no value, <c>true</c> if [is network available]; otherwise,
		///     <c>false</c>.
		/// </value>
		public bool? IsNetworkAvailable
		{
			get
			{
				return DeviceNetworkInformation.IsNetworkAvailable;
			}
		}

		/// <summary>
		///     Gets the ISO Country Code
		/// </summary>
		/// <value>The icc.</value>
		public string ICC
		{
			get
			{
				return string.Empty;
			}
		}

		/// <summary>
		///     Gets the Mobile Country Code
		/// </summary>
		/// <value>The MCC.</value>
		public string MCC
		{
			get
			{
				return string.Empty;
			}
		}

		/// <summary>
		///     Gets the Mobile Network Code
		/// </summary>
		/// <value>The MNC.</value>
		public string MNC
		{
			get
			{
				return string.Empty;
			}
		}

		/// <summary>
		///     Gets a value indicating whether this instance can send SMS.
		/// </summary>
		/// <value><c>true</c> if this instance can send SMS; otherwise, <c>false</c>.</value>
		public bool CanSendSMS
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		///     Opens native dialog to dial the specified number
		/// </summary>
		/// <param name="number">Number to dial.</param>
		public void DialNumber(string number)
		{
			new PhoneCallTask { PhoneNumber = number }.Show();
		}

		/// <summary>
		///     Sends the SMS.
		/// </summary>
		/// <param name="to">To.</param>
		/// <param name="body">The body.</param>
		public void SendSMS(string to, string body)
		{
			new SmsComposeTask { To = to, Body = body }.Show();
		}
	}
}