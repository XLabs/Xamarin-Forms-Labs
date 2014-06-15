using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Info;
using XForms.Toolkit.Services;

namespace XForms.Toolkit
{
    /// <summary>
    /// Windows phone device.
    /// </summary>
    public class WindowsPhoneDevice : IDevice
    {
        private static WindowsPhoneDevice currentDevice;

        /// <summary>
        /// Initializes a new instance of the <see cref="XForms.Toolkit.WindowsPhoneDevice"/> class.
        /// </summary>
        private WindowsPhoneDevice()
        {
            this.Display = new Display();
            this.PhoneService = new PhoneService();
            this.Battery = new Battery();

            if (Microsoft.Devices.Sensors.Accelerometer.IsSupported)
            {
                this.Accelerometer = new Accelerometer();
            }
        }

        /// <summary>
        /// Gets the current device.
        /// </summary>
        public static IDevice CurrentDevice { get { return currentDevice ?? (currentDevice = new WindowsPhoneDevice()); } }

        #region IDevice Members
        /// <summary>
        /// Gets the display.
        /// </summary>
        public IDisplay Display
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the phone service.
        /// </summary>
        public IPhoneService PhoneService
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the battery.
        /// </summary>
        public IBattery Battery
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the accelerometer for the device if available
        /// </summary>
        /// <value>Instance of IAccelerometer if available, otherwise null.</value>
        public IAccelerometer Accelerometer
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get { return DeviceStatus.DeviceName; }
        }

        /// <summary>
        /// Gets the firmware version.
        /// </summary>
        public string FirmwareVersion
        {
            get { return DeviceStatus.DeviceFirmwareVersion; }
        }

        /// <summary>
        /// Gets the hardware version.
        /// </summary>
        public string HardwareVersion
        {
            get { return DeviceStatus.DeviceHardwareVersion; }
        }

        /// <summary>
        /// Gets the manufacturer.
        /// </summary>
        public string Manufacturer
        {
            get { return DeviceStatus.DeviceManufacturer; }
        }

        #endregion
    }
}
