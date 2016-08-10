// ***********************************************************************
// Assembly       : XLabs.Platform.UWP
// Author           : XLabs Team
// Created          : 01-02-2016
//
// Last Modified By : XLabs Team
// Last Modified On : 01-20-2016
// ***********************************************************************
// <copyright file="WindowsDevice.cs" company="XLabs Team">
//        Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//        This project is licensed under the Apache 2.0 license
//        https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//
//        XLabs is a open source project that aims to provide a powerfull and cross
//        platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
//

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.System;
using PropertyChanged;
using XLabs.Enums;
using XLabs.Platform.Services;
using XLabs.Platform.Services.IO;
using XLabs.Platform.Services.Media;
using XLabs.Ioc;

#if !WINDOWS_PHONE
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System.Profile;
#endif

namespace XLabs.Platform.Device
{
    /// <summary>
    /// Windows phone device.
    /// </summary>
    [ImplementPropertyChanged]
    public class WindowsDevice : IDevice
    {
        #region Private Variables
        private static IDevice _currentDevice;

        private IBluetoothHub _bluetoothHub;

        private IFileManager _fileManager;

        private IMediaPicker _mediaPicker;

        private INetwork _network;

        private IAudioStream _microphone;

        private IPhoneService _phoneService;

        private IBattery _battery;

        private IDisplay _display;

        private IAccelerometer _accelerometer;

        private IGyroscope _gyroscope;

        private Orientation _orientation = Orientation.Portrait;
        #endregion Private Variables

        /// <summary>
        /// Creates an instance of <see cref="WindowsDevice" />.
        /// </summary>
        public WindowsDevice()
        {

            _display = new Display();
            _battery = new Battery();

            Query();

            try
            {
                var result = Windows.Devices.Sensors.Accelerometer.GetDefault();

                if (result != null)
                {
                    _accelerometer = new Accelerometer();
                }
            }
            catch (Exception)
            {
            }

            try
            {
                var result = Windows.Devices.Sensors.Gyrometer.GetDefault();

                if (result != null)
                {
                    _gyroscope = new Gyroscope();
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Gets the current device.
        /// </summary>
        /// <value>The current device.</value>
        public static IDevice CurrentDevice
        {
            get
            {
                return _currentDevice ?? (_currentDevice = new WindowsDevice());
            }
            set
            {
                _currentDevice = value;
            }
        }

        #region IDevice Members

        #region Properties
        /// <summary>
        /// Gets the file manager for the device.
        /// </summary>
        /// <value>Device file manager.</value>
        public IFileManager FileManager
        {
#if WINDOWS_PHONE
            get { return _fileManager ?? (_fileManager = Resolver.Resolve<IFileManager>() ?? new FileManager(System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication())); }
#else
            get { return _fileManager ?? (_fileManager = Resolver.Resolve<IFileManager>()); }
#endif
            set { _fileManager = value; }
        }

        /// <summary>
        /// Gets the picture chooser.
        /// </summary>
        /// <value>The picture chooser.</value>
        /// <exception cref="System.UnauthorizedAccessException">Exception is thrown if application manifest does not enable ID_CAP_ISV_CAMERA capability.</exception>
        public IMediaPicker MediaPicker
        {
#if WINDOWS_PHONE
            get { return _mediaPicker ?? (_mediaPicker = Resolver.Resolve<IMediaPicker>() ?? new MediaPicker()); }
#else
            get { return _mediaPicker ?? (_mediaPicker = Resolver.Resolve<IMediaPicker>()); }
#endif
            set { _mediaPicker = value; }
        }

        /// <summary>
        /// Gets the display.
        /// </summary>
        /// <value>The display.</value>
        public IDisplay Display { get { return _display ?? (_display = Resolver.Resolve<IDisplay>()); } set { _display = value; } }

        /// <summary>
        /// Gets the phone service.
        /// </summary>
        /// <value>Phone service instance if available, otherwise null.</value>

        public IPhoneService PhoneService { get { return _phoneService ?? (_phoneService = Resolver.Resolve<IPhoneService>()); } set { _phoneService = value; } }

        /// <summary>
        /// Gets the battery.
        /// </summary>
        /// <value>The battery.</value>
        public IBattery Battery { get { return _battery ?? (_battery = Resolver.Resolve<IBattery>()); } set { _battery = value; } }

        /// <summary>
        /// Gets the accelerometer for the device if available.
        /// </summary>
        /// <value>Instance of IAccelerometer if available, otherwise null.</value>
        public IAccelerometer Accelerometer { get { return _accelerometer ?? (_accelerometer = Resolver.Resolve<IAccelerometer>()); } set { _accelerometer = value; } }

        /// <summary>
        /// Gets the gyroscope.
        /// </summary>
        /// <value>The gyroscope instance if available, otherwise null.</value>
        public IGyroscope Gyroscope { get { return _gyroscope ?? (_gyroscope = Resolver.Resolve<IGyroscope>()); } set { _gyroscope = value; } }

        /// <summary>
        /// Gets the network service.
        /// </summary>
        /// <value>The network service.</value>
        /// <exception cref="System.UnauthorizedAccessException">Exception is thrown if application manifest does not enable ID_CAP_NETWORKING capability.</exception>
        public INetwork Network { get { return _network ?? (_network = Resolver.Resolve<INetwork>()) ?? new Network(); } set { _network = value; } }

        /// <summary>
        /// Gets the bluetooth hub service.
        /// </summary>
        /// <value>The bluetooth hub service if available, otherwise null.</value>
        public IBluetoothHub BluetoothHub { get { return _bluetoothHub ?? (_bluetoothHub = Resolver.Resolve<IBluetoothHub>()) ?? new BluetoothHub(); } set { _bluetoothHub = value; } }

        /// <summary>
        /// Gets the default microphone for the device
        /// </summary>
        /// <value>The default microphone if available, otherwise null.</value>
        public IAudioStream Microphone { get { return _microphone ?? (_microphone = Resolver.Resolve<IAudioStream>()) ?? new AudioStream(); } set { _microphone = value; } }

        /// <summary>
        /// Gets Unique Id for the device.
        /// </summary>
        /// <value>The id for the device.</value>
        /// <exception cref="UnauthorizedAccessException">Application has no access to device identity. To enable access consider enabling ID_CAP_IDENTITY_DEVICE on app manifest.</exception>
        /// <remarks>Requires the application to check ID_CAP_IDENTITY_DEVICE on application permissions.</remarks>
        public virtual string Id { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name of the device.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the firmware version.
        /// </summary>
        /// <value>The firmware version.</value>
        public string FirmwareVersion { get; private set; }

        /// <summary>
        /// Gets the hardware version.
        /// </summary>
        /// <value>The hardware version.</value>
        public string HardwareVersion { get; private set; }

        /// <summary>
        /// Gets the manufacturer.
        /// </summary>
        /// <value>The manufacturer.</value>
        public string Manufacturer { get; private set; }

        /// <summary>
        /// Gets the total memory in bytes.
        /// </summary>
        /// <value>The total memory in bytes.</value>
        public long TotalMemory { get; private set; }

        /// <summary>
        /// Gets the time zone offset.
        /// </summary>
        /// <value>The time zone offset.</value>
        public double TimeZoneOffset { get; private set; }

        /// <summary>
        /// Gets the time zone.
        /// </summary>
        /// <value>The time zone.</value>
        public string TimeZone { get; private set; }

        /// <summary>
        /// Gets the language code.
        /// </summary>
        /// <value>The language code.</value>
        public string LanguageCode { get; private set; }

        /// <summary>
        /// Gets the orientation.
        /// </summary>
        /// <value>The orientation.</value>
        public Orientation Orientation
        {
            get { QueryOrientation(); return _orientation; }
            private set { _orientation = value; }
        }

        #endregion Properties

        #region Methods
        /// <summary>
        /// Starts the default app associated with the URI for the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>The launch operation.</returns>
        public async Task<bool> LaunchUriAsync(Uri uri)
        {
            return await Launcher.LaunchUriAsync(uri);
        }

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task Refresh()
        {
            var deviceProperties = DeviceInfo.DeviceProperties.GetInstance();

            deviceProperties.IsReadyChanged += (sender, b) => Query();

            deviceProperties.Resolve();
        }
        #endregion Methods

        #endregion

        private void Query()
        {
            QueryDeviceDetails();
            QueryDeviceMemory();
            QueryLanguageCode();
            QueryOrientation();
            QueryTimeZoneName();
            QueryTimeZoneOffset();
        }

        private void QueryOrientation()
        {
            switch (DeviceInfo.DeviceProperties.GetInstance().DisplayInfo.CurrentOrientation)
            {

                case Windows.Graphics.Display.DisplayOrientations.Landscape:
                    Orientation = Orientation.Landscape & Orientation.LandscapeLeft;
                    break;
                case Windows.Graphics.Display.DisplayOrientations.Portrait:
                    Orientation = Orientation.Portrait & Orientation.PortraitUp;
                    break;
                case Windows.Graphics.Display.DisplayOrientations.PortraitFlipped:
                    Orientation = Orientation.Portrait & Orientation.PortraitDown;
                    break;
                case Windows.Graphics.Display.DisplayOrientations.LandscapeFlipped:
                    Orientation = Orientation.Landscape & Orientation.LandscapeRight;
                    break;
                default:
                    Orientation = Orientation.None;
                    break;
            }
        }

        private void QueryLanguageCode()
        {
            LanguageCode = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        }

        private void QueryTimeZoneName()
        {
            TimeZone = TimeZoneInfo.Local.DisplayName;
        }

        private void QueryTimeZoneOffset()
        {
            TimeZoneOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalMinutes / 60;
        }

        private void QueryDeviceMemory()
        {
#if WINDOWS_PHONE
            TotalMemory = Microsoft.Phone.Info.DeviceStatus.DeviceTotalMemory;
#elif WINDOWS_PHONE_APP || WINDOWS_UWP
            TotalMemory = (long)(Windows.System.MemoryManager.AppMemoryUsageLimit);
#else
            TotalMemory = 0;
#endif
        }

        private void QueryDeviceDetails()
        {
            var deviceProperties = DeviceInfo.DeviceProperties.GetInstance();

            if (deviceProperties.IsReady)
            {
                Id = deviceProperties.DeviceId;
                Name = deviceProperties.DeviceName;
                Manufacturer = deviceProperties.Manufacturer;
                HardwareVersion = deviceProperties.HardwareVersion;
                FirmwareVersion = deviceProperties.FirmwareVersion;
            }

            deviceProperties.IsReadyChanged += (sender, o) =>
            {
                Id = deviceProperties.DeviceId;
                Name = deviceProperties.DeviceName;
                Manufacturer = deviceProperties.Manufacturer;
                HardwareVersion = deviceProperties.HardwareVersion;
                FirmwareVersion = deviceProperties.FirmwareVersion;

            };
        }
    }
}
