namespace XLabs.Platform.Device
{
	using System.IO;
	using System.Threading.Tasks;

	using Android.Bluetooth;
	using Android.Util;

	using Java.Util;

	using IOException = Java.IO.IOException;

	/// <summary>
	///     Class AndroidBluetoothDevice.
	/// </summary>
	public class AndroidBluetoothDevice : IBluetoothDevice
	{
		/// <summary>
		///     The bt UUID
		/// </summary>
		private const string BT_UUID = "00001101-0000-1000-8000-00805F9B34FB";

		/// <summary>
		///     The socket
		/// </summary>
		private BluetoothSocket _socket;

		/// <summary>
		///     The device
		/// </summary>
		private readonly BluetoothDevice _device;

		/// <summary>
		///     The UUID
		/// </summary>
		private readonly UUID _uuid;

		/// <summary>
		///     Initializes a new instance of the <see cref="AndroidBluetoothDevice" /> class.
		/// </summary>
		/// <param name="device">The device.</param>
		public AndroidBluetoothDevice(BluetoothDevice device)
		{
			_device = device;
			_uuid = UUID.RandomUUID();
		}

		#region IBluetoothDevice implementation

		/// <summary>
		///     Connects this instance.
		/// </summary>
		/// <returns>Task.</returns>
		public async Task Connect()
		{
			if (_socket == null)
			{
				_socket = _device.CreateRfcommSocketToServiceRecord(_uuid);
			}

			await _socket.ConnectAsync();
		}

		/// <summary>
		///     Disconnects this instance.
		/// </summary>
		public void Disconnect()
		{
			if (_socket != null)
			{
				try
				{
					_socket.Close();
					_socket = null;
				}
				catch (IOException ex)
				{
					Log.Error("BluetoothSocket.Close()", ex.Message);
				}
			}
		}

		/// <summary>
		///     Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get
			{
				return _device.Name;
			}
		}

		/// <summary>
		///     Gets the address.
		/// </summary>
		/// <value>The address.</value>
		public string Address
		{
			get
			{
				return _device.Address;
			}
		}

		/// <summary>
		///     Gets the input stream.
		/// </summary>
		/// <value>The input stream.</value>
		public Stream InputStream
		{
			get
			{
				return (_socket == null) ? null : _socket.InputStream;
			}
		}

		/// <summary>
		///     Gets the output stream.
		/// </summary>
		/// <value>The output stream.</value>
		public Stream OutputStream
		{
			get
			{
				return (_socket == null) ? null : _socket.OutputStream;
			}
		}

		#endregion
	}
}