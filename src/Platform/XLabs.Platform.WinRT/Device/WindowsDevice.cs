// ***********************************************************************
// Assembly         : XLabs.Platform.WinUniversal
// Author           : XLabs Team
// Created          : 01-01-2016
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="WindowsDevice.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System;
using System.Threading.Tasks;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System;
using Windows.System.Profile;
using XLabs.Enums;
using XLabs.Platform.Services;
using XLabs.Platform.Services.IO;
using XLabs.Platform.Services.Media;

namespace XLabs.Platform.Device
{
    /// <summary>
    /// Windows phone device.
    /// </summary>
    public class WindowsDevice : IDevice
    {
        /// <summary>
        /// The current device.
        /// </summary>
        private static IDevice _currentDevice;

        //// <summary>
        //// The file manager.
        //// </summary>
        //private IFileManager _fileManager;

        //// <summary>
        //// The media picker.
        //// </summary>
        //private IMediaPicker _mediaPicker;

        /// <summary>
        /// The network.
        /// </summary>
        private INetwork _network;

        /// <summary>
        /// The _hardware token
        /// </summary>
        private HardwareToken _hardwareToken;

        private EasClientDeviceInformation _clientDeviceInformation;

        /// <summary>
        /// Creates an instance of WindowsPhoneDevice
        /// </summary>
        public WindowsDevice()
        {			
            this.Display = new Display();
            this.Battery = new Battery();
            this.BluetoothHub = new BluetoothHub();

            try
            {
                var result = Windows.Devices.Sensors.Accelerometer.GetDefault();

                if (result != null)
                {
                    this.Accelerometer = new Accelerometer();
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
                    Gyroscope = new Gyroscope();
                }
            }
            catch (Exception)
            {
            }
        
            //if (DeviceCapabilities.IsEnabled(DeviceCapabilities.Capability.IdCapMicrophone))
            //{
            //	if (XnaMicrophone.IsAvailable)
            //	{
            //		this.Microphone = new XnaMicrophone();
            //	}
            //}

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
                return _currentDevice ?? (_currentDevice = new WindowsDevice());
            }
            set
            {
                _currentDevice = value;
            }
        }

        #region IDevice Members

        /// <summary>
        /// Gets Unique Id for the device.
        /// </summary>
        /// <value>The id for the device.</value>
        /// <exception cref="UnauthorizedAccessException">Application has no access to device identity. To enable access consider enabling ID_CAP_IDENTITY_DEVICE on app manifest.</exception>
        /// <remarks>Requires the application to check ID_CAP_IDENTITY_DEVICE on application permissions.</remarks>
        public virtual string Id
        {
            get { return _hardwareToken != null ? _hardwareToken.Id.ToString() : (_hardwareToken = HardwareIdentification.GetPackageSpecificToken(null)).Id.ToString(); }
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
                //return this.mediaPicker ?? (this.mediaPicker = new MediaPicker());
                throw new System.NotImplementedException();

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
                return _network ?? (_network = new Network());
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
                //return this.fileManager ?? (this.fileManager = new FileManager(IsolatedStorageFile.GetUserStoreForApplication()));
                throw new System.NotImplementedException();
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
                return (_clientDeviceInformation ?? (_clientDeviceInformation = new EasClientDeviceInformation())).FriendlyName;
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
                return (_clientDeviceInformation ?? (_clientDeviceInformation = new EasClientDeviceInformation())).SystemProductName;
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
                return (_clientDeviceInformation ?? (_clientDeviceInformation = new EasClientDeviceInformation())).SystemSku;
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
                return (_clientDeviceInformation ?? (_clientDeviceInformation = new EasClientDeviceInformation())).SystemManufacturer;
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
                //return DeviceStatus.DeviceTotalMemory;
                throw new System.NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the time zone offset.
        /// </summary>
        /// <value>The time zone offset.</value>
        public double TimeZoneOffset
        {
            get
            {
                return TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalMinutes / 60;
            }
        }

        /// <summary>
        /// Gets the time zone.
        /// </summary>
        /// <value>The time zone.</value>
        public string TimeZone
        {
            get
            {
                return TimeZoneInfo.Local.DisplayName;
            }
        }

        /// <summary>
        /// Gets the language code.
        /// </summary>
        /// <value>The language code.</value>
        public string LanguageCode
        {
            get
            {
                return System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            }
        }

        /// <summary>
        /// Gets the orientation.
        /// </summary>
        /// <value>The orientation.</value>
        public Orientation Orientation
        {
            get
            {
                switch (Windows.Graphics.Display.DisplayInformation.GetForCurrentView().CurrentOrientation)
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

