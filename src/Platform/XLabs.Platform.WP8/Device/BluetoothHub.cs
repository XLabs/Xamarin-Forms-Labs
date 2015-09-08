namespace XLabs.Platform.Device
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows.Input;

	using Windows.Networking.Proximity;

	using Microsoft.Phone.Tasks;

	/// <summary>
	/// Class BluetoothHub.
	/// </summary>
	public class BluetoothHub : IBluetoothHub
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BluetoothHub"/> class.
		/// </summary>
		public BluetoothHub()
		{
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="IBluetoothHub" /> is enabled.
		/// </summary>
		/// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
		public bool Enabled
		{
			get
			{
				return PeerFinder.AllowBluetooth;
			}
		}

		/// <summary>
		/// Gets the open settings.
		/// </summary>
		/// <value>The open settings.</value>
		public Task OpenSettings()
		{
			return Task.Run(() => new ConnectionSettingsTask {ConnectionSettingsType = ConnectionSettingsType.Bluetooth}.Show());
		}

		/// <summary>
		/// Gets the paired devices.
		/// </summary>
		/// <returns>Task&lt;IReadOnlyList&lt;IBluetoothDevice&gt;&gt;.</returns>
		public async Task<IReadOnlyList<IBluetoothDevice>> GetPairedDevices()
		{
			PeerFinder.AlternateIdentities["Bluetooth:PAIRED"] = string.Empty;
			var devices = await PeerFinder.FindAllPeersAsync();
			return devices.Select(a => new BluetoothDevice(a)).ToList();
		}
	}
}