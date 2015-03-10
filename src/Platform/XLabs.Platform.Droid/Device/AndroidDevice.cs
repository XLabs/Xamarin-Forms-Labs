namespace XLabs.Platform.Device
{
    using System;
    using System.IO.IsolatedStorage;
    using System.Threading.Tasks;
    using Android.App;
    using Android.Bluetooth;
    using Android.Content;
    using Android.OS;
    using Android.Runtime;
    using Android.Telephony;
    using Android.Util;
    using Android.Views;
    using Enums;
    using Java.IO;
    using Java.Util;
    using Java.Util.Concurrent;
    using Services;
    using Services.IO;
    using Services.Media;

    /// <summary>
    /// Android device implements <see cref="IDevice"/>.
    /// </summary>
    public class AndroidDevice : IDevice
    {
        private static readonly long DeviceTotalMemory = GetTotalMemory();
        private static IDevice currentDevice;

        private IBluetoothHub btHub;
        private IFileManager fileManager;
        private INetwork network;

        /// <summary>
        /// Prevents a default instance of the <see cref="AndroidDevice"/> class from being created. 
        /// </summary>
        private AndroidDevice()
        {
            var manager = Services.PhoneService.Manager;

            if (manager != null && manager.PhoneType != PhoneType.None)
            {
                this.PhoneService = new PhoneService();
            }

            if (Device.Accelerometer.IsSupported)
            {
                this.Accelerometer = new Accelerometer();
            }

            if (Device.Gyroscope.IsSupported)
            {
                this.Gyroscope = new Gyroscope();
            }

            if (Services.Media.Microphone.IsEnabled)
            {
                this.Microphone = new Microphone();
            }

            this.Display = new Display();

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
        /// <value>
        /// The current device.
        /// </value>
        public static IDevice CurrentDevice { get { return currentDevice ?? (currentDevice = new AndroidDevice()); } }

        /// <summary>
        /// Gets Unique Id for the device.
        /// </summary>
        /// <value>
        /// The id for the device.
        /// </value>
        public string Id
        {
            // TODO: Verify what is the best combination of Unique Id for Android
            get
            {
                return Build.Serial;
            }
        }

        /// <summary>
        /// Gets the phone service for this device.
        /// </summary>
        /// <value>Phone service instance if available, otherwise null.</value>
        public IPhoneService PhoneService { get; private set; }

        /// <summary>
        /// Gets the display information for the device.
        /// </summary>
        /// <value>
        /// The display.
        /// </value>
        public IDisplay Display { get; private set; }

        /// <summary>
        /// Gets the battery.
        /// </summary>
        /// <value>
        /// The battery.
        /// </value>
        public IBattery Battery { get; private set; }

        /// <summary>
        /// Gets the picture chooser.
        /// </summary>
        /// <value>The picture chooser.</value>
        public IMediaPicker MediaPicker { get; private set; }

        /// <summary>
        /// Gets the network service.
        /// </summary>
        /// <value>The network service.</value>
        public INetwork Network { get { return this.network ?? (this.network = new Network()); } }

        /// <summary>
        /// Gets the accelerometer for the device if available.
        /// </summary>
        /// <value>Instance of IAccelerometer if available, otherwise null.</value>
        public IAccelerometer Accelerometer { get; private set; }

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
        /// Gets the bluetooth hub service.
        /// </summary>
        /// <value>The bluetooth hub service if available, otherwise null.</value>
        public IBluetoothHub BluetoothHub
        {
            get
            {
                if (this.btHub == null && BluetoothAdapter.DefaultAdapter != null)
                {
                    this.btHub = new BluetoothHub(BluetoothAdapter.DefaultAdapter);
                }

                return this.btHub;
            }
        }

        /// <summary>
        /// Gets the default microphone for the device
        /// </summary>
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
        /// Gets the name of the device.
        /// </summary>
        /// <value>
        /// The name of the device.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the firmware version.
        /// </summary>
        /// <value>
        /// The firmware version.
        /// </value>
        public string FirmwareVersion { get; private set; }

        /// <summary>
        /// Gets the hardware version.
        /// </summary>
        /// <value>
        /// The hardware version.
        /// </value>
        public string HardwareVersion { get; private set; }

        /// <summary>
        /// Gets the manufacturer.
        /// </summary>
        /// <value>
        /// The manufacturer.
        /// </value>
        public string Manufacturer { get; private set; }

        public string LanguageCode
        {
            get { return Locale.Default.Language; }
        }

        public double TimeZoneOffset
        {
            get
            {
                var calendar = new GregorianCalendar();
                var timezone = calendar.TimeZone;
                return TimeUnit.Hours.Convert(timezone.RawOffset, TimeUnit.Milliseconds) / 3600;
            }
        }

        public string TimeZone
        {
            get { return Java.Util.TimeZone.Default.ID; }
        }

        public Orientation Orientation
        {
            get
            {
                var wm = Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();

                switch (wm.DefaultDisplay.Rotation)
                {
                    case SurfaceOrientation.Rotation0:
                        return Orientation.Portrait & Orientation.PortraitUp;
                    case SurfaceOrientation.Rotation90:
                        return Orientation.Landscape & Orientation.LandscapeLeft;
                    case SurfaceOrientation.Rotation180:
                        return Orientation.Portrait & Orientation.PortraitDown;
                    case SurfaceOrientation.Rotation270:
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
        public Task<bool> LaunchUriAsync(Uri uri)
        {
            return Task.Run(() =>
                {
                    try
                    {
                        this.StartActivity(new Intent("android.intent.action.VIEW", Android.Net.Uri.Parse(uri.ToString())));
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Device.LaunchUriAsync", ex.Message);
                        return false;
                    }
                });
        }

        /// <summary>
        /// Gets the total memory in bytes.
        /// </summary>
        /// <value>The total memory in bytes.</value>
        public long TotalMemory 
        {
            get 
            {
                return DeviceTotalMemory;
            }
        }

        private static long GetTotalMemory() 
        {
            using (var reader = new RandomAccessFile("/proc/meminfo", "r")) 
            {
                var line = reader.ReadLine(); // first line --> MemTotal: xxxxxx kB
                var split = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return Convert.ToInt64(split[1]) * 1024;
            }
        }
    }
}