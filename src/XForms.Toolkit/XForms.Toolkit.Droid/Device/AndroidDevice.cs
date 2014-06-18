using Android.Telephony;
using Android.OS;
using XForms.Toolkit.Droid.Services.Media;
using XForms.Toolkit.Services;
using XForms.Toolkit.Services.Media;

namespace XForms.Toolkit
{
    /// <summary>
    /// Android device implements <see cref=""/>.
    /// </summary>
    public class AndroidDevice: IDevice
    {
        private static IDevice currentDevice;

        /// <summary>
        /// Initializes a new instance of the <see cref="XForms.Toolkit.AndroidDevice"/> class.
        /// </summary>
        private AndroidDevice ()
        {
            var manager = XForms.Toolkit.Services.PhoneService.Manager;

            if (manager != null && manager.PhoneType != PhoneType.None)
            {
                this.PhoneService = new PhoneService();
            }

            if (XForms.Toolkit.Accelerometer.IsSupported)
            {
                this.Accelerometer = new Accelerometer();
            }

            if (XForms.Toolkit.Gyroscope.IsSupported)
            {
                this.Gyroscope = new Gyroscope();
            }

//            if (BluetoothAdapter.DefaultAdapter != null)
//            {
//                this.BluetoothHub = new BluetoothHub(BluetoothAdapter.DefaultAdapter);
//            }

            this.Display = new Display ();

            this.Manufacturer = Build.Manufacturer;
            this.Name = Build.Model;
            this.HardwareVersion = Build.Hardware;
            this.FirmwareVersion = Build.VERSION.Release;

            this.Battery = new Battery();

	        this.MediaPicker = new MediaPicker();
        }

        /// <summary>
        /// Gets the current device.
        /// </summary>
        public static IDevice CurrentDevice { get { return currentDevice ?? (currentDevice = new AndroidDevice()); } }

        #region IDevice implementation
        /// <summary>
        /// Gets the phone service for this device.
        /// </summary>
        /// <value>Phone service instance if available, otherwise null.</value>
        public IPhoneService PhoneService 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// Gets the display information for the device.
        /// </summary>
        public IDisplay Display
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
		/// Gets the picture chooser.
		/// </summary>
		/// <value>The picture chooser.</value>
	    public IMediaPicker MediaPicker
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
        /// Gets the gyroscope.
        /// </summary>
        /// <value>The gyroscope instance if available, otherwise null.</value>
        public IGyroscope Gyroscope
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the firmware version.
        /// </summary>
        public string FirmwareVersion
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the hardware version.
        /// </summary>
        public string HardwareVersion
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the manufacturer.
        /// </summary>
        public string Manufacturer
        {
            get;
            private set;
        }
        #endregion
    }
}

