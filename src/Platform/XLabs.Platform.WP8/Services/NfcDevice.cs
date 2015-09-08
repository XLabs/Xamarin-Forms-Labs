namespace XLabs.Platform.Services
{
	using System;
	using System.Collections.Generic;

	using Windows.Networking.Proximity;

	/// <summary>
	/// Class NfcDevice.
	/// </summary>
	public class NfcDevice : INfcDevice
	{
		/// <summary>
		/// The _publish identifier
		/// </summary>
		private long? _publishId;

		/// <summary>
		/// The _device
		/// </summary>
		private readonly ProximityDevice _device;

		/// <summary>
		/// The _published
		/// </summary>
		private readonly Dictionary<Guid, long> _published = new Dictionary<Guid, long>();

		/// <summary>
		/// Initializes a new instance of the <see cref="NfcDevice"/> class.
		/// </summary>
		public NfcDevice()
			: this(ProximityDevice.GetDefault())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NfcDevice"/> class.
		/// </summary>
		/// <param name="proximityDevice">The proximity device.</param>
		public NfcDevice(ProximityDevice proximityDevice)
		{
			_device = proximityDevice;
		}

		/// <summary>
		/// Occurs when [in range].
		/// </summary>
		private event EventHandler<EventArgs<INfcDevice>> InRange;

		/// <summary>
		/// Occurs when [out of range].
		/// </summary>
		private event EventHandler<EventArgs<INfcDevice>> OutOfRange;

		/// <summary>
		/// Devices the arrived.
		/// </summary>
		/// <param name="sender">The sender.</param>
		private void DeviceArrived(ProximityDevice sender)
		{
			//if (sender == this.device)
			//{
			//    System.Diagnostics.Debug.WriteLine("Sender is the same NFC device.");
			//}

			//this.inRange.Invoke<INfcDevice>(this, new NfcDevice(sender));
			InRange.Invoke<INfcDevice>(this, this);
		}

		/// <summary>
		/// Devices the departed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		private void DeviceDeparted(ProximityDevice sender)
		{
			OutOfRange.Invoke<INfcDevice>(this, this);
		}

		#region INfcDevice Members

		/// <summary>
		/// Gets the device identifier.
		/// </summary>
		/// <value>The device identifier.</value>
		public string DeviceId
		{
			get
			{
				return _device == null ? "Unknown" : _device.DeviceId;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is enabled.
		/// </summary>
		/// <value><c>true</c> if this instance is enabled; otherwise, <c>false</c>.</value>
		public bool IsEnabled
		{
			get
			{
				return _device != null;
			}
		}

		/// <summary>
		/// Occurs when [device in range].
		/// </summary>
		public event EventHandler<EventArgs<INfcDevice>> DeviceInRange
		{
			add
			{
				if (InRange == null)
				{
					_device.DeviceArrived += DeviceArrived;
				}

				InRange += value;
			}
			remove
			{
				InRange -= value;

				if (InRange == null)
				{
					_device.DeviceArrived -= DeviceArrived;
				}
			}
		}

		/// <summary>
		/// Occurs when [device out of range].
		/// </summary>
		public event EventHandler<EventArgs<INfcDevice>> DeviceOutOfRange
		{
			add
			{
				if (OutOfRange == null)
				{
					_device.DeviceDeparted += DeviceDeparted;
				}

				OutOfRange += value;
			}

			remove
			{
				OutOfRange -= value;

				if (OutOfRange == null)
				{
					_device.DeviceDeparted -= DeviceDeparted;
				}
			}
		}

		/// <summary>
		/// Publishes the URI.
		/// </summary>
		/// <param name="uri">The URI.</param>
		/// <returns>Guid.</returns>
		public Guid PublishUri(Uri uri)
		{
			var id = _device.PublishUriMessage(uri);
			var key = Guid.NewGuid();

			_published.Add(key, id);

			return key;
		}

		/// <summary>
		/// Unpublishes the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		public void Unpublish(Guid id)
		{
			if (_published.ContainsKey(id))
			{
				_device.StopPublishingMessage(_published[id]);
				_published.Remove(id);
			}
		}

		#endregion
	}
}