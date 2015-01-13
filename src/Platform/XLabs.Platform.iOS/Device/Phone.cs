namespace XLabs.Platform.Device
{
	using System.ComponentModel;

	using XLabs.Platform.Extensions;
	using XLabs.Platform.Services;

	/// <summary>
	/// Apple iPhone.
	/// </summary>
	public class Phone : AppleDevice
	{
		/// <summary>
		/// The phone type.
		/// </summary>
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
			/// The i phone6
			/// </summary>
			[Description("iPhone 6")]
			IPhone6,

			/// <summary>
			/// The i phone6 plus
			/// </summary>
			[Description("iPhone 6 Plus")]
			IPhone6Plus
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Phone" /> class.
		/// </summary>
		/// <param name="majorVersion">Major version.</param>
		/// <param name="minorVersion">Minor version.</param>
		internal Phone(int majorVersion, int minorVersion)
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
				default:
					Version = PhoneType.Unknown;
					break;
			}

			if (Version == PhoneType.IPhone6)
			{
				Display = new Display(1334, 750, 326, 326);
			}
			else if (Version == PhoneType.IPhone6Plus)
			{
				Display = new Display(2208, 1242, 401 * 1242 / 1080, 401 * 2208 / 1920);
			}
			else if (majorVersion > 4)
			{
				Display = new Display(1136, 640, 326, 326);
			}
			else if (majorVersion > 2)
			{
				Display = new Display(960, 640, 326, 326);
			}
			else
			{
				Display = new Display(480, 320, 163, 163);
			}

			Name = HardwareVersion = Version.GetDescription();
		}

		/// <summary>
		/// Gets the version of iPhone.
		/// </summary>
		/// <value>The version.</value>
		public PhoneType Version { get; private set; }
	}
}