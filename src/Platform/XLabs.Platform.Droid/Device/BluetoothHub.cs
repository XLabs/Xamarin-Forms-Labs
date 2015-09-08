namespace XLabs.Platform.Device
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Android.Bluetooth;
    using Android.Content;

    /// <summary>
    /// Class BluetoothHub.
    /// </summary>
    public class BluetoothHub : IBluetoothHub
    {
        /// <summary>
        /// The _adapter
        /// </summary>
        private readonly BluetoothAdapter _adapter;

        /// <summary>
        /// Initializes a new instance of the <see cref="BluetoothHub"/> class.
        /// </summary>
        public BluetoothHub()
            : this(BluetoothAdapter.DefaultAdapter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BluetoothHub"/> class.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        public BluetoothHub(BluetoothAdapter adapter)
        {
            _adapter = adapter;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IBluetoothHub" /> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get
            {
                return _adapter.IsEnabled;
            }
        }

        #region IBluetoothHub implementation

        /// <summary>
        /// Gets the paired devices.
        /// </summary>
        /// <returns>Task&lt;IReadOnlyList&lt;IBluetoothDevice&gt;&gt;.</returns>
        public async Task<IReadOnlyList<IBluetoothDevice>> GetPairedDevices()
        {
            return await Task.Factory.StartNew(() => _adapter.BondedDevices.Select(a => new AndroidBluetoothDevice(a)).ToList());
        }

        /// <summary>
        /// Gets the open settings.
        /// </summary>
        /// <value>The open settings.</value>
        public Task OpenSettings()
        {
            return Task.Run(() => this.StartActivity(new Intent(BluetoothAdapter.ActionRequestEnable)));
        }

        #endregion
    }
}