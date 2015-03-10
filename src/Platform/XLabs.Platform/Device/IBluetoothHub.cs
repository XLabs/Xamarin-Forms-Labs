namespace XLabs.Platform.Device
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface IBluetoothHub
    /// </summary>
    public interface IBluetoothHub
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="IBluetoothHub"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        bool Enabled { get; }

        /// <summary>
        /// Gets the paired devices.
        /// </summary>
        /// <returns>Task&lt;IReadOnlyList&lt;IBluetoothDevice&gt;&gt;.</returns>
        Task<IReadOnlyList<IBluetoothDevice>> GetPairedDevices();

        /// <summary>
        /// Opens the settings.
        /// </summary>
        /// <returns>Task.</returns>
        Task OpenSettings();
    }
}