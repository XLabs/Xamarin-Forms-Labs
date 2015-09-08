namespace XLabs.Platform.Device
{
	using System;
	using System.IO;
	using System.Threading.Tasks;

	using Windows.Networking.Proximity;
	using Windows.Networking.Sockets;

	/// <summary>
	/// Class BluetoothDevice.
	/// </summary>
	public class BluetoothDevice : IBluetoothDevice
	{
		/// <summary>
		/// The _socket
		/// </summary>
		private StreamSocket _socket;

		/// <summary>
		/// The _device
		/// </summary>
		private readonly PeerInformation _device;

		/// <summary>
		/// Initializes a new instance of the <see cref="BluetoothDevice"/> class.
		/// </summary>
		/// <param name="peerInfo">The peer information.</param>
		public BluetoothDevice(PeerInformation peerInfo)
		{
			_device = peerInfo;
		}

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get
			{
				return _device.DisplayName;
			}
		}

		/// <summary>
		/// Gets the address.
		/// </summary>
		/// <value>The address.</value>
		public string Address
		{
			get
			{
				return _device.HostName.DisplayName;
			}
		}

		/// <summary>
		/// Gets the input stream.
		/// </summary>
		/// <value>The input stream.</value>
		public Stream InputStream
		{
			get
			{
				return _socket == null ? null : _socket.InputStream.AsStreamForRead();
			}
		}

		/// <summary>
		/// Gets the output stream.
		/// </summary>
		/// <value>The output stream.</value>
		public Stream OutputStream
		{
			get
			{
				return _socket == null ? null : _socket.OutputStream.AsStreamForWrite();
			}
		}

		/// <summary>
		/// Connects this instance.
		/// </summary>
		/// <returns>Task.</returns>
		public async Task Connect()
		{
			if (_socket != null)
			{
				_socket.Dispose();
			}

			try
			{
				_socket = new StreamSocket();

				await _socket.ConnectAsync(_device.HostName, _device.ServiceName);

				//return true;
			}
			catch //(Exception ex)
			{
				if (_socket != null)
				{
					_socket.Dispose();
					_socket = null;
				}

				throw;
			}
		}

		/// <summary>
		/// Disconnects this instance.
		/// </summary>
		public void Disconnect()
		{
			if (_socket != null)
			{
				_socket.Dispose();
				_socket = null;
			}
		}
	}
}