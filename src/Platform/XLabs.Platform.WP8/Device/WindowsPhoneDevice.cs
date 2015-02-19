namespace XLabs.Platform.Device
{
    using System;
    using System.IO.IsolatedStorage;
    using System.Threading.Tasks;

    using Windows.System;

    using Microsoft.Phone.Info;

    using Services;
    using Services.IO;
    using Services.Media;
    using XLabs.Enums;

    /// <summary>
    /// Windows phone device.
    /// </summary>
    public class WindowsPhoneDevice : IDevice
    {
        /// <summary>
        /// The current device.
        /// </summary>
        private static WindowsPhoneDevice currentDevice;

        /// <summary>
        /// The _file manager
        /// </summary>
        private IFileManager fileManager;

        /// <summary>
        /// The Id for the device.
        /// </summary>
        private string id;

        /// <summary>
        /// The _media picker
        /// </summary>
        private IMediaPicker mediaPicker;

        /// <summary>
        /// The _network
        /// </summary>
        private INetwork network;

        /// <summary>
        /// Prevents a default instance of the <see cref="WindowsPhoneDevice" /> class from being created.
        /// </summary>
        private WindowsPhoneDevice()
        {
            this.Display = new Display();
            this.PhoneService = new PhoneService();
            this.Battery = new Battery();
            this.BluetoothHub = new BluetoothHub();

            if (DeviceCapabilities.IsEnabled(DeviceCapabilities.Capability.IdCapSensors))
            {
                if (Microsoft.Devices.Sensors.Accelerometer.IsSupported)
                {
                    this.Accelerometer = new Accelerometer();
                }

                if (Microsoft.Devices.Sensors.Gyroscope.IsSupported)
                {
                    this.Gyroscope = new Gyroscope();
                }
            }

            if (DeviceCapabilities.IsEnabled(DeviceCapabilities.Capability.IdCapMicrophone))
            {
                if (XnaMicrophone.IsAvailable)
                {
                    this.Microphone = new XnaMicrophone();
                }
            }

            //if (DeviceCapabilities.IsEnabled(DeviceCapabilities.Capability.ID_CAP_MEDIALIB_PHOTO))
            //{
            //    MediaPicker = new MediaPicker();
            //}
        }

        /// <summary>
        /// Gets the current device.
        /// </summary>
        /// <value>The current device.</value>
        public static IDevice CurrentDevice
        {
            get
            {
                return currentDevice ?? (currentDevice = new WindowsPhoneDevice());
            }
        }

        #region IDevice Members

        /// <summary>
        /// Gets Unique Id for the device.
        /// </summary>
        /// <value>The id for the device.</value>
        /// <exception cref="UnauthorizedAccessException">Application has no access to device identity. To enable access consider enabling ID_CAP_IDENTITY_DEVICE on app manifest.</exception>
        /// <remarks>Requires the application to check ID_CAP_IDENTITY_DEVICE on application permissions.</remarks>
        public string Id
        {
            get
            {
                if (string.IsNullOrEmpty(this.id))
                {
                    object o;
                    if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out o))
                    {
                        this.id = Convert.ToBase64String((byte[])o);
                    }
                    else
                    {
                        throw new UnauthorizedAccessException(
                            "Application has no access to device identity. To enable access consider enabling ID_CAP_IDENTITY_DEVICE on app manifest.");
                    }
                }

                return this.id;
            }
        }

        /// <summary>
        /// Gets the display.
        /// </summary>
        /// <value>The display.</value>
        public IDisplay Display { get; private set; }

        /// <summary>
        /// Gets the phone service.
        /// </summary>
        /// <value>Phone service instance if available, otherwise null.</value>
        public IPhoneService PhoneService { get; private set; }

        /// <summary>
        /// Gets the battery.
        /// </summary>
        /// <value>The battery.</value>
        public IBattery Battery { get; private set; }

        /// <summary>
        /// Gets the accelerometer for the device if available.
        /// </summary>
        /// <value>Instance of IAccelerometer if available, otherwise null.</value>
        public IAccelerometer Accelerometer { get; private set; }

        /// <summary>
        /// Gets the gyroscope.
        /// </summary>
        /// <value>The gyroscope instance if available, otherwise null.</value>
        public IGyroscope Gyroscope { get; private set; }

        /// <summary>
        /// Gets the picture chooser.
        /// </summary>
        /// <value>The picture chooser.</value>
        /// <exception cref="System.UnauthorizedAccessException">Exception is thrown if application manifest does not enable ID_CAP_ISV_CAMERA capability.</exception>
        public IMediaPicker MediaPicker
        {
            get
            {
                return this.mediaPicker ?? (this.mediaPicker = new MediaPicker());
            }
        }

        /// <summary>
        /// Gets the network service.
        /// </summary>
        /// <value>The network service.</value>
        /// <exception cref="System.UnauthorizedAccessException">Exception is thrown if application manifest does not enable ID_CAP_NETWORKING capability.</exception>
        public INetwork Network
        {
            get
            {
                return this.network ?? (this.network = new Network());
            }
        }

        /// <summary>
        /// Gets the bluetooth hub service.
        /// </summary>
        /// <value>The bluetooth hub service if available, otherwise null.</value>
        public IBluetoothHub BluetoothHub { get; private set; }

        /// <summary>
        /// Gets the default microphone for the device
        /// </summary>
        /// <value>The default microphone if available, otherwise null.</value>
        public IAudioStream Microphone { get; private set; }

        /// <summary>
        /// Gets the file manager for the device.
        /// </summary>
        /// <value>Device file manager.</value>
        public IFileManager FileManager
        {
            get
            {
                return this.fileManager ?? (this.fileManager = new FileManager(IsolatedStorageFile.GetUserStoreForApplication()));
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name of the device.</value>
        public string Name
        {
            get
            {
                return DeviceStatus.DeviceName;
            }
        }

        /// <summary>
        /// Gets the firmware version.
        /// </summary>
        /// <value>The firmware version.</value>
        public string FirmwareVersion
        {
            get
            {
                return DeviceStatus.DeviceFirmwareVersion;
            }
        }

        /// <summary>
        /// Gets the hardware version.
        /// </summary>
        /// <value>The hardware version.</value>
        public string HardwareVersion
        {
            get
            {
                return DeviceStatus.DeviceHardwareVersion;
            }
        }

        /// <summary>
        /// Gets the manufacturer.
        /// </summary>
        /// <value>The manufacturer.</value>
        public string Manufacturer
        {
            get
            {
                return DeviceStatus.DeviceManufacturer;
            }
        }

        /// <summary>
        /// Gets the total memory in bytes.
        /// </summary>
        /// <value>The total memory in bytes.</value>
        public long TotalMemory
        {
            get
            {
                return DeviceStatus.DeviceTotalMemory;
            }
        }

        public double TimeZoneOffset
        {
            get
            {
                return TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalMinutes / 60;
            }
        }

        public string TimeZone
        {
            get
            {
                return TimeZoneInfo.Local.DisplayName;
            }
        }

        public string LanguageCode
        {
            get
            {
                return System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            }
        }

        public Orientation Orientation
        {
            get
            {
                switch (Windows.Graphics.Display.DisplayProperties.CurrentOrientation)
                {

                    case Windows.Graphics.Display.DisplayOrientations.Landscape:
                        return Orientation.Landscape & Orientation.LandscapeLeft;
                    case Windows.Graphics.Display.DisplayOrientations.Portrait:
                        return Orientation.Portrait & Orientation.PortraitUp;
                    case Windows.Graphics.Display.DisplayOrientations.PortraitFlipped:
                        return Orientation.Portrait & Orientation.PortraitDown;
                    case Windows.Graphics.Display.DisplayOrientations.LandscapeFlipped:
                        return Orientation.Landscape & Orientation.LandscapeRight;
                    default:
                        return Orientation.None;
                }
            }
        }

        /// <summary>
        /// Starts the default app associated with the URI for the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>The launch operation.</returns>
        public async Task<bool> LaunchUriAsync(Uri uri)
        {
            return await Launcher.LaunchUriAsync(uri);
        }

        #endregion
    }
}