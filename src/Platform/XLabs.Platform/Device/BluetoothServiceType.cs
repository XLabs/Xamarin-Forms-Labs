namespace XLabs.Platform.Device
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Bluetooth Service type.
    /// </summary>
    public class BluetoothServiceType
    {
        static BluetoothServiceType()
        {
            StandardServices = new[]
            {
                new BluetoothServiceType
                {
                    Type = ServiceType.SerialPort,
                    Name = "Serial Port",
                    ServiceId = Guid.Parse("00001101-0000-1000-8000-00805F9B34FB")
                },
            };
        }

        /// <summary>
        /// Standard bluetooth services.
        /// </summary>
        public static IEnumerable<BluetoothServiceType> StandardServices { get; private set; }

        /// <summary>
        /// Gets or sets the service type.
        /// </summary>
        public ServiceType Type { get; set; }

        /// <summary>
        /// Gets or sets the service name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the service id.
        /// </summary>
        public Guid ServiceId { get; set; }

        /// <summary>
        /// Bluetooth service type enumeration.
        /// </summary>
        public enum ServiceType
        {
            /// <summary>
            /// Custom service.
            /// </summary>
            Custom,
            /// <summary>
            /// Serial port Bluetooth device.
            /// </summary>
            SerialPort,
        }
    }
}