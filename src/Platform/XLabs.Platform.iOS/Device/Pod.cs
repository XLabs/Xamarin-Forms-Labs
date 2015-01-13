namespace XLabs.Platform.Device
{
	using System.ComponentModel;

	using XLabs.Platform.Extensions;

	/// <summary>
	/// Apple iPod.
	/// </summary>
	public class Pod : AppleDevice
	{
		/// <summary>
		/// Enum PodVersion
		/// </summary>
		public enum PodVersion
		{
			/// <summary>
			/// The first generation
			/// </summary>
			[Description("iPod Touch 1G")]
			FirstGeneration = 1,

			/// <summary>
			/// The second generation
			/// </summary>
			[Description("iPod Touch 2G")]
			SecondGeneration,

			/// <summary>
			/// The third generation
			/// </summary>
			[Description("iPod Touch 3G")]
			ThirdGeneration,

			/// <summary>
			/// The fourth generation
			/// </summary>
			[Description("iPod Touch 4G")]
			FourthGeneration,

			/// <summary>
			/// The fifth generation
			/// </summary>
			[Description("iPod Touch 5G")]
			FifthGeneration
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Pod" /> class.
		/// </summary>
		/// <param name="majorVersion">Major version.</param>
		/// <param name="minorVersion">Minor version.</param>
		internal Pod(int majorVersion, int minorVersion)
		{
			Version = (PodVersion)majorVersion;
			PhoneService = null;

			Name = HardwareVersion = Version.GetDescription();

			if (majorVersion > 4)
			{
				Display = new Display(1136, 640, 326, 326);
			}
			else if (majorVersion > 3)
			{
				Display = new Display(960, 640, 326, 326);
			}
			else
			{
				Display = new Display(480, 320, 163, 163);
			}
		}

		/// <summary>
		/// Gets the version of iPod.
		/// </summary>
		/// <value>The version.</value>
		public PodVersion Version { get; private set; }
	}
}