namespace XLabs.Platform.Device
{
	using System.ComponentModel;

	using XLabs.Platform.Extensions;

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
			IPadMini2GCellular
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Pad" /> class.
		/// </summary>
		/// <param name="majorVersion">Major version.</param>
		/// <param name="minorVersion">Minor version.</param>
		internal Pad(int majorVersion, int minorVersion)
		{
			PhoneService = null;
			double dpi;
			switch (majorVersion)
			{
				case 1:
					Version = IPadVersion.IPad1;
					Display = new Display(1024, 768, 132, 132);
					break;
				case 2:
					dpi = minorVersion > 4 ? 163 : 132;
					Version = IPadVersion.IPad2Wifi + minorVersion - 1;
					Display = new Display(1024, 768, dpi, dpi);
					break;
				case 3:
					Version = IPadVersion.IPad3Wifi + minorVersion - 1;
					Display = new Display(2048, 1536, 264, 264);
					break;
				case 4:
					dpi = minorVersion > 3 ? 326 : 264;
					Version = IPadVersion.IPadAirWifi + minorVersion - 1;
					Display = new Display(2048, 1536, dpi, dpi);
					break;
				default:
					Version = IPadVersion.Unknown;
					break;
			}

			Name = HardwareVersion = Version.GetDescription();
		}

		/// <summary>
		/// Gets the version of the iPad.
		/// </summary>
		/// <value>The version.</value>
		public IPadVersion Version { get; private set; }
	}
}