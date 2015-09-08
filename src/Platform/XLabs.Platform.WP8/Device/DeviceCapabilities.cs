namespace XLabs.Platform.Device
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Xml.Linq;

	using Microsoft.Xna.Framework;

	/// <summary>
	/// Class DeviceCapabilities.
	/// </summary>
	public static class DeviceCapabilities
	{
		/// <summary>
		/// Enum Capability
		/// </summary>
		public enum Capability
		{
			/// <summary>
			/// The identifier cap networking
			/// </summary>
			IdCapNetworking,

			/// <summary>
			/// The identifier cap medialib audio
			/// </summary>
			IdCapMedialibAudio,

			/// <summary>
			/// The identifier cap medialib playback
			/// </summary>
			IdCapMedialibPlayback,

			/// <summary>
			/// The identifier cap webbrowsercomponent
			/// </summary>
			IdCapWebbrowsercomponent,

			/// <summary>
			/// The identifier cap appointments
			/// </summary>
			IdCapAppointments,

			/// <summary>
			/// The identifier cap contacts
			/// </summary>
			IdCapContacts,

			/// <summary>
			/// The identifier cap gamerservices
			/// </summary>
			IdCapGamerservices,

			/// <summary>
			/// The identifier cap identity device
			/// </summary>
			IdCapIdentityDevice,

			/// <summary>
			/// The identifier cap identity user
			/// </summary>
			IdCapIdentityUser,

			/// <summary>
			/// The identifier cap isv camera
			/// </summary>
			IdCapIsvCamera,

			/// <summary>
			/// The identifier cap location
			/// </summary>
			IdCapLocation,

			/// <summary>
			/// The identifier cap map
			/// </summary>
			IdCapMap,

			/// <summary>
			/// The identifier cap medialib photo
			/// </summary>
			IdCapMedialibPhoto,

			/// <summary>
			/// The identifier cap microphone
			/// </summary>
			IdCapMicrophone,

			/// <summary>
			/// The identifier cap phonedialer
			/// </summary>
			IdCapPhonedialer,

			/// <summary>
			/// The identifier cap proximity
			/// </summary>
			IdCapProximity,

			/// <summary>
			/// The identifier cap push notification
			/// </summary>
			IdCapPushNotification,

			/// <summary>
			/// The identifier cap removable storage
			/// </summary>
			IdCapRemovableStorage,

			/// <summary>
			/// The identifier cap sensors
			/// </summary>
			IdCapSensors,

			/// <summary>
			/// The identifier cap speech recognition
			/// </summary>
			IdCapSpeechRecognition,

			/// <summary>
			/// The identifier cap voip
			/// </summary>
			IdCapVoip,

			/// <summary>
			/// The identifier cap wallet
			/// </summary>
			IdCapWallet,

			/// <summary>
			/// The identifier cap wallet paymentinstruments
			/// </summary>
			IdCapWalletPaymentinstruments,

			/// <summary>
			/// The identifier cap wallet secureelement
			/// </summary>
			IdCapWalletSecureelement
		}

		/// <summary>
		/// The w m_ ap p_ manifest
		/// </summary>
		private const string WM_APP_MANIFEST = "WMAppManifest.xml";

		/// <summary>
		/// The capabilities
		/// </summary>
		private const string CAPABILITIES = "Capabilities";

		/// <summary>
		/// The name
		/// </summary>
		private const string NAME = "Name";

		/// <summary>
		/// The capabilities
		/// </summary>
		private static readonly Dictionary<Capability, bool> capabilities;

		/// <summary>
		/// Initializes static members of the <see cref="DeviceCapabilities"/> class.
		/// </summary>
		static DeviceCapabilities()
		{
			using (var strm = TitleContainer.OpenStream(WM_APP_MANIFEST))
			{
				var xml = XElement.Load(strm);

				capabilities = new Dictionary<Capability, bool>();

				var permissions = xml.Descendants(CAPABILITIES).Elements();

				foreach (var e in Enum.GetValues(typeof(Capability)))
				{
					capabilities.Add(
						(Capability)e,
						permissions.FirstOrDefault(n => n.Attribute(NAME).Value.Equals(e.ToString())) != null);
				}
			}
		}

		/// <summary>
		/// Determines whether the specified capability is enabled.
		/// </summary>
		/// <param name="capability">The capability.</param>
		/// <returns><c>true</c> if the specified capability is enabled; otherwise, <c>false</c>.</returns>
		public static bool IsEnabled(Capability capability)
		{
			return capabilities[capability];
		}

		/// <summary>
		/// Checks the capability.
		/// </summary>
		/// <param name="capabilities">The capabilities.</param>
		/// <param name="capability">The capability.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		private static bool CheckCapability(IEnumerable<XElement> capabilities, Capability capability)
		{
			return capabilities.FirstOrDefault(n => n.Attribute(NAME).Value.Equals(capability.ToString())) != null;
		}
	}
}