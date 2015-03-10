namespace XLabs.Platform.Device
{
    using System;
    using System.IO.IsolatedStorage;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Enums;
    using Foundation;
    using ObjCRuntime;
    using Services;
    using Services.IO;
    using Services.Media;
    using UIKit;

    /// <summary>
    /// Apple device base class.
    /// </summary>
    public abstract class AppleDevice : IDevice
    {
        /// <summary>
        /// The iPhone expression.
        /// </summary>
        private const string IPHONE_EXPRESSION = "iPhone([1-7]),([1-4])";

        /// <summary>
        /// The iPod expression.
        /// </summary>
        private const string IPOD_EXPRESSION = "iPod([1-5]),([1])";

        /// <summary>
        /// The iPad expression.
        /// </summary>
        private const string IPAD_EXPRESSION = "iPad([1-4]),([1-8])";

        /// <summary>
        /// Generic CPU/IO.
        /// </summary>
        private const int CTL_HW = 6;

        /// <summary>
        /// Total memory.
        /// </summary>
        private const int HW_PHYSMEM = 5;

        /// <summary>
        /// The device.
        /// </summary>
        private static IDevice device;

        private static readonly long totalMemory = GetTotalMemory();

        /// <summary>
        /// The file manager
        /// </summary>
        private IFileManager fileManager;

        /// <summary>
        /// Reference to the Bluetooth hub singleton.
        /// </summary>
        private IBluetoothHub bluetoothHub;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppleDevice" /> class.
        /// </summary>
        protected AppleDevice()
        {
            this.Battery = new Battery();
            this.Accelerometer = new Accelerometer();
            this.FirmwareVersion = UIDevice.CurrentDevice.SystemVersion;

            if (Device.Gyroscope.IsSupported)
            {
                this.Gyroscope = new Gyroscope();
            }

            this.MediaPicker = new MediaPicker();

            this.Network = new Network();
        }

        /// <summary>
        /// Gets the runtime device for Apple's devices.
        /// </summary>
        /// <value>The current device.</value>
        public static IDevice CurrentDevice
        {
            get
            {
                if (device != null)
                {
                    return device;
                }

                var hardwareVersion = GetSystemProperty("hw.machine");

                var regex = new Regex(IPHONE_EXPRESSION).Match(hardwareVersion);
                if (regex.Success)
                {
                    return device = new Phone(int.Parse(regex.Groups[1].Value), int.Parse(regex.Groups[2].Value));
                }

                regex = new Regex(IPOD_EXPRESSION).Match(hardwareVersion);
                if (regex.Success)
                {
                    return device = new Pod(int.Parse(regex.Groups[1].Value), int.Parse(regex.Groups[2].Value));
                }

                regex = new Regex(IPAD_EXPRESSION).Match(hardwareVersion);
                if (regex.Success)
                {
                    return device = new Pad(int.Parse(regex.Groups[1].Value), int.Parse(regex.Groups[2].Value));
                }

                return device = new Simulator();
            }
        }

        /// <summary>
        /// Sysctlbynames the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="output">The output.</param>
        /// <param name="oldLen">The old length.</param>
        /// <param name="newp">The newp.</param>
        /// <param name="newlen">The newlen.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(Constants.SystemLibrary)]
        internal static extern int sysctlbyname(
            [MarshalAs(UnmanagedType.LPStr)] string property,
            IntPtr output,
            IntPtr oldLen,
            IntPtr newp,
            uint newlen);

        [DllImport(Constants.SystemLibrary)]
        static internal extern int sysctl(
            [MarshalAs(UnmanagedType.LPArray)] int[] name, 
            uint namelen, 
            out uint oldp, 
            ref int oldlenp, 
            IntPtr newp, 
            uint newlen);


        /// <summary>
        /// Gets the system property.
        /// </summary>
        /// <param name="property">Property to get.</param>
        /// <returns>The system property value.</returns>
        public static string GetSystemProperty(string property)
        {
            var pLen = Marshal.AllocHGlobal(sizeof(int));
            sysctlbyname(property, IntPtr.Zero, pLen, IntPtr.Zero, 0);
            var length = Marshal.ReadInt32(pLen);
            var pStr = Marshal.AllocHGlobal(length);
            sysctlbyname(property, pStr, pLen, IntPtr.Zero, 0);
            return Marshal.PtrToStringAnsi(pStr);
        }

        #region IDevice implementation

        /// <summary>
        /// Gets Unique Id for the device.
        /// </summary>
        /// <value>The id for the device.</value>
        public string Id
        {
            get
            {
                return UIDevice.CurrentDevice.IdentifierForVendor.AsString();
            }
        }

        /// <summary>
        /// Gets or sets the display information for the device.
        /// </summary>
        /// <value>The display.</value>
        public IDisplay Display { get; protected set; }

        /// <summary>
        /// Gets or sets the phone service for this device.
        /// </summary>
        /// <value>Phone service instance if available, otherwise null.</value>
        public IPhoneService PhoneService { get; protected set; }

        /// <summary>
        /// Gets or sets the battery for the device.
        /// </summary>
        /// <value>The battery.</value>
        public IBattery Battery { get; protected set; }

        /// <summary>
        /// Gets the picture chooser.
        /// </summary>
        /// <value>The picture chooser.</value>
        public IMediaPicker MediaPicker { get; private set; }

        /// <summary>
        /// Gets the network service.
        /// </summary>
        /// <value>The network service.</value>
        public INetwork Network { get; private set; }

        /// <summary>
        /// Gets or sets the accelerometer for the device if available
        /// </summary>
        /// <value>Instance of IAccelerometer if available, otherwise null.</value>
        public IAccelerometer Accelerometer { get; protected set; }

        /// <summary>
        /// Gets the gyroscope.
        /// </summary>
        /// <value>The gyroscope instance if available, otherwise null.</value>
        public IGyroscope Gyroscope { get; private set; }

        /// <summary>
        /// Gets the audio stream from the device's microphone.
        /// </summary>
        public IAudioStream Microphone
        {
            get
            {
                return new Microphone();
            }
        }

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
        /// Gets or sets the name of the device.
        /// </summary>
        /// <value>The name of the device.</value>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets or sets the firmware version.
        /// </summary>
        /// <value>The firmware version.</value>
        public string FirmwareVersion { get; protected set; }

        /// <summary>
        /// Gets or sets the hardware version.
        /// </summary>
        /// <value>The hardware version.</value>
        public string HardwareVersion { get; protected set; }

        /// <summary>
        /// Gets the manufacturer.
        /// </summary>
        /// <value>The manufacturer.</value>
        public string Manufacturer
        {
            get
            {
                return "Apple";
            }
        }

        /// <summary>
        /// Gets the bluetooth hub service.
        /// </summary>
        /// <value>The bluetooth hub service if available, otherwise null.</value>
        public IBluetoothHub BluetoothHub
        {
            get
            {
                return this.bluetoothHub ?? (this.bluetoothHub = new BluetoothHub());
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
                return totalMemory; 
            }
        }

        public string LanguageCode
        {
            get { return NSLocale.PreferredLanguages[0]; }
        }

        public double TimeZoneOffset
        {
            get { return NSTimeZone.LocalTimeZone.GetSecondsFromGMT / 3600.0; }
        }

        public string TimeZone
        {
            get { return NSTimeZone.LocalTimeZone.Name; }
        }

        public Orientation Orientation
        {
            get
            {
                switch (UIApplication.SharedApplication.StatusBarOrientation)
                {
                    case UIInterfaceOrientation.LandscapeLeft:
                        return Orientation.Landscape & Orientation.LandscapeLeft;
                    case UIInterfaceOrientation.Portrait:
                        return Orientation.Portrait & Orientation.PortraitUp;
                    case UIInterfaceOrientation.PortraitUpsideDown:
                        return Orientation.Portrait & Orientation.PortraitDown;
                    case UIInterfaceOrientation.LandscapeRight:
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
            return Task.Run(() => UIApplication.SharedApplication.OpenUrl(new NSUrl(uri.ToString())));
        }
        #endregion

        private static uint GetTotalMemory()
        {
            var oldlenp = sizeof(int);
            var mib = new int[2] { CTL_HW, HW_PHYSMEM };

            uint mem;
            sysctl(mib, 2, out mem, ref oldlenp, IntPtr.Zero, 0);

            return mem;
        }
    }
}
