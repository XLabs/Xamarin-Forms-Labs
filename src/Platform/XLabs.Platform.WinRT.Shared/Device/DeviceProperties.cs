/*
 * Copyright (c) 2013-2014 Microsoft Mobile. All rights reserved.
 * See the license text file delivered with this project for more information.
 * https://github.com/Microsoft/phone-info/blob/master/PhoneInfoWP8_1/PhoneInfo/DeviceProperties.cs
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Windows.Devices.Sensors;
using Windows.Graphics.Display;
using Windows.Networking.Proximity;

using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Core;
#if WINDOWS_PHONE_APP || NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.System.Profile;
//using Windows.Devices.Background;
using Size = Windows.Foundation.Size;
using Color = Windows.UI.Color;
using DispatcherPriority = Windows.UI.Core.CoreDispatcherPriority;
#else
using Windows.System.Power;
using Windows.Phone.System.Power;
using Microsoft.Phone.Info;
using Windows.Phone.Media.Capture;
using Microsoft.Devices.Radio;
using Microsoft.Phone.Storage;
using System.Windows.Media;
using Size = System.Windows.Size;
#endif

namespace DeviceInfo
{
    /// <summary>
    /// This class implements methods to resolve harware supported by the
    /// phone and details about the phone software. In addition, the dynamic
    /// traits of the phone are resolved. The resolved values are stored in
    /// the class properties enabling fast queries.
    /// Note that you need to make sure that the application has enough
    /// capabilites enabled for the implementation to work properly.
    /// </summary>
    public class DeviceProperties
    {
        #region Constants
        // Constants ->

        /// <summary>
        /// The debug tag
        /// </summary>
        private const string DebugTag = "DeviceProperties: ";
        #endregion Constants

        #region Private Variables
        /* Focus properties available via MediaCapture.VideoDeviceController
         * may return invalid values for older phones, which original were
         * released with Windows Phone OS version 8.0. Thus, we need to check
         * those phone models explicitly.
         */
        /// <summary>
        /// The w P80 phone models with automatic focus
        /// </summary>
        private readonly string[] _wp80PhoneModelsWithAutoFocus =
        {
            "RM-820", // Nokia Lumia 920
            "RM-821", // Nokia Lumia 920
            "RM-822", // Nokia Lumia 920
            "RM-824", // Nokia Lumia 820
            "RM-825", // Nokia Lumia 820
            "RM-826", // Nokia Lumia 820
            "RM-846", // Nokia Lumia 620
            "RM-867", // Nokia Lumia 920
            "RM-875", // Nokia Lumia 1020
            "RM-876", // Nokia Lumia 1020
            "RM-877", // Nokia Lumia 1020
            "RM-885", // Nokia Lumia 720
            "RM-887", // Nokia Lumia 720
            "RM-892", // Nokia Lumia 925
            "RM-893", // Nokia Lumia 925
            "RM-910", // Nokia Lumia 925
            "RM-955"  // Nokia Lumia 925
        };
        #endregion

        #region Data Types
        // Data types ->

        /// <summary>
        /// Enum Resolutions
        /// </summary>
        public enum Resolutions
        {
            // ReSharper disable InconsistentNaming
            /// <summary>
            /// Wide VGA, 480x800
            /// </summary>
            WVGA,
            /// <summary>
            /// qHD, 540x960
            /// </summary>
            qHD,
            /// <summary>
            /// HD, 720x1280
            /// </summary>
            HD720,
            /// <summary>
            /// Wide Extended Graphics Array (WXGA), 768x1280
            /// </summary>
            WXGA,
            /// <summary>
            /// Full HD, 1080x1920
            /// </summary>
            HD1080,
            /// <summary>
            /// The unknown
            /// </summary>
            Unknown
            // ReSharper restore InconsistentNaming
        };

        /// <summary>
        /// Enum UnitPrefixes
        /// </summary>
        public enum UnitPrefixes
        {
            /// <summary>
            /// The kilo
            /// </summary>
            Kilo,
            /// <summary>
            /// The mega
            /// </summary>
            Mega,
            /// <summary>
            /// The giga
            /// </summary>
            Giga
        };
        #endregion

        // Members and properties ->
        #region Static Properties
        /// <summary>
        /// The _instance
        /// </summary>
        private static DeviceProperties _instance;
        /// <summary>
        /// The _sync lock
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private static readonly object _syncLock = new object();
        #endregion Static Properties

        #region Properties
#if !WINDOWS_PHONE
        /// <summary>
        /// The _media capture
        /// </summary>
        private MediaCapture _mediaCapture;

#if !WINDOWS_UWP
        private HardwareToken _hardwareToken;
#endif

#endif
        /// <summary>
        /// The _number of asynchronous operations to complete
        /// </summary>
        private int _numberOfAsyncOperationsToComplete;
        /// <summary>
        /// The _number of asynchronous operations completed
        /// </summary>
        private int _numberOfAsyncOperationsCompleted;

#if DEBUG
        /// <summary>
        /// The _start of resolve time
        /// </summary>
        private DateTime _startOfResolveTime;
#endif
        #endregion Properties

#if WINDOWS_PHONE
        internal static System.Windows.Threading.Dispatcher Dispatcher
        {
            get { return Deployment.Current?.Dispatcher; }
        }
#else
        /// <summary>
        /// Gets the dispatcher.
        /// </summary>
        /// <value>The dispatcher.</value>
        internal static CoreDispatcher Dispatcher
        {
            get
            {
                var dispatcher = CoreWindow.GetForCurrentThread()?.Dispatcher;
                if (dispatcher != null) return dispatcher;

                try
                {
                    dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView?.Dispatcher;
                    if (dispatcher != null) return dispatcher;
                }
                catch
                {
                }

                try
                {
                    dispatcher = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView()?.Dispatcher;
                    if (dispatcher != null) return dispatcher;
                }
                catch
                {
                }

                return null;
            }
        }
#endif

        /// <summary>
        /// Gets the display information.
        /// </summary>
        /// <value>The display information.</value>
        public DisplayInfo DisplayInfo { get; private set; }

        #region Public Event Handles
        /// <summary>
        /// Gets or sets the is ready changed.
        /// </summary>
        /// <value>The is ready changed.</value>
        public EventHandler<bool> IsReadyChanged { get; set; }
        #endregion Public Event Handles

        #region Public Properties
        /// <summary>
        /// Gets a value indicating whether this instance is ready.
        /// </summary>
        /// <value><c>true</c> if this instance is ready; otherwise, <c>false</c>.</value>
        public bool IsReady { get; private set; }

        // Battery and power
        /// <summary>
        /// Gets the remaining battery charge.
        /// </summary>
        /// <value>The remaining battery charge.</value>
        public int RemainingBatteryCharge { get; private set; }
        /// <summary>
        /// Gets a value indicating whether this instance has battery status information.
        /// </summary>
        /// <value><c>true</c> if this instance has battery status information; otherwise, <c>false</c>.</value>
        public bool HasBatteryStatusInfo { get; private set; }
        /// <summary>
        /// Gets a value indicating whether this instance is connected to external power supply.
        /// </summary>
        /// <value><c>true</c> if this instance is connected to external power supply; otherwise, <c>false</c>.</value>
        public bool IsConnectedToExternalPowerSupply { get; private set; }
        /// <summary>
        /// Gets a value indicating whether [power saving mode enabled].
        /// </summary>
        /// <value><c>true</c> if [power saving mode enabled]; otherwise, <c>false</c>.</value>
        public bool PowerSavingModeEnabled { get; private set; }

        // Cameras and flashes
        /// <summary>
        /// Gets a value indicating whether this instance has back camera.
        /// </summary>
        /// <value><c>true</c> if this instance has back camera; otherwise, <c>false</c>.</value>
        public bool HasBackCamera { get; private set; }
        /// <summary>
        /// Gets a value indicating whether this instance has front camera.
        /// </summary>
        /// <value><c>true</c> if this instance has front camera; otherwise, <c>false</c>.</value>
        public bool HasFrontCamera { get; private set; }
        /// <summary>
        /// Gets a value indicating whether this instance has back camera flash.
        /// </summary>
        /// <value><c>true</c> if this instance has back camera flash; otherwise, <c>false</c>.</value>
        public bool HasBackCameraFlash { get; private set; }
        /// <summary>
        /// Gets a value indicating whether this instance has front camera flash.
        /// </summary>
        /// <value><c>true</c> if this instance has front camera flash; otherwise, <c>false</c>.</value>
        public bool HasFrontCameraFlash { get; private set; }
        /// <summary>
        /// Gets a value indicating whether this instance has back camera automatic focus.
        /// </summary>
        /// <value><c>true</c> if this instance has back camera automatic focus; otherwise, <c>false</c>.</value>
        public bool HasBackCameraAutoFocus { get; private set; }
        /// <summary>
        /// Gets a value indicating whether this instance has front camera automatic focus.
        /// </summary>
        /// <value><c>true</c> if this instance has front camera automatic focus; otherwise, <c>false</c>.</value>
        public bool HasFrontCameraAutoFocus { get; private set; }
        /// <summary>
        /// Gets the back camera photo resolutions.
        /// </summary>
        /// <value>The back camera photo resolutions.</value>
        public List<Size> BackCameraPhotoResolutions { get; private set; }
        /// <summary>
        /// Gets the front camera photo resolutions.
        /// </summary>
        /// <value>The front camera photo resolutions.</value>
        public List<Size> FrontCameraPhotoResolutions { get; private set; }
        /// <summary>
        /// Gets the back camera video resolutions.
        /// </summary>
        /// <value>The back camera video resolutions.</value>
        public List<Size> BackCameraVideoResolutions { get; private set; }
        /// <summary>
        /// Gets the front camera video resolutions.
        /// </summary>
        /// <value>The front camera video resolutions.</value>
        public List<Size> FrontCameraVideoResolutions { get; private set; }

        // Memory
        /// <summary>
        /// Gets the application current memory usage in bytes.
        /// </summary>
        /// <value>The application current memory usage in bytes.</value>
        public ulong ApplicationCurrentMemoryUsageInBytes { get; private set; }
        /// <summary>
        /// Gets the application memory usage limit in bytes.
        /// </summary>
        /// <value>The application memory usage limit in bytes.</value>
        public ulong ApplicationMemoryUsageLimitInBytes { get; private set; }
        /// <summary>
        /// Gets or sets the application peak memory usage in bytes.
        /// </summary>
        /// <value>The application peak memory usage in bytes.</value>
        public ulong ApplicationPeakMemoryUsageInBytes { get; set; }
        /// <summary>
        /// Gets or sets the device total memory in bytes.
        /// </summary>
        /// <value>The device total memory in bytes.</value>
        public ulong DeviceTotalMemoryInBytes { get; set; }

        // Screen
        /// <summary>
        /// Gets the screen resolution.
        /// </summary>
        /// <value>The screen resolution.</value>
        public Resolutions ScreenResolution { get; private set; }
        /// <summary>
        /// Gets the size of the screen resolution.
        /// </summary>
        /// <value>The size of the screen resolution.</value>
        public Size ScreenResolutionSize { get; private set; }
        /// <summary>
        /// Gets the display size in inches.
        /// </summary>
        /// <value>The display size in inches.</value>
        public double DisplaySizeInInches { get; private set; } // E.g. 4.5 for Nokia Lumia 1020

        // Sensors
        /// <summary>
        /// Gets a value indicating whether this instance has accelerometer sensor.
        /// </summary>
        /// <value><c>true</c> if this instance has accelerometer sensor; otherwise, <c>false</c>.</value>
        public bool HasAccelerometerSensor { get; private set; }
        /// <summary>
        /// Gets a value indicating whether this instance has compass.
        /// </summary>
        /// <value><c>true</c> if this instance has compass; otherwise, <c>false</c>.</value>
        public bool HasCompass { get; private set; }
        /// <summary>
        /// Gets a value indicating whether this instance has gyroscope sensor.
        /// </summary>
        /// <value><c>true</c> if this instance has gyroscope sensor; otherwise, <c>false</c>.</value>
        public bool HasGyroscopeSensor { get; private set; }
        /// <summary>
        /// Gets a value indicating whether this instance has inclinometer sensor.
        /// </summary>
        /// <value><c>true</c> if this instance has inclinometer sensor; otherwise, <c>false</c>.</value>
        public bool HasInclinometerSensor { get; private set; }
        /// <summary>
        /// Gets a value indicating whether this instance has orientation sensor.
        /// </summary>
        /// <value><c>true</c> if this instance has orientation sensor; otherwise, <c>false</c>.</value>
        public bool HasOrientationSensor { get; private set; }
        /// <summary>
        /// Gets a value indicating whether this instance has proximity sensor.
        /// </summary>
        /// <value><c>true</c> if this instance has proximity sensor; otherwise, <c>false</c>.</value>
        public bool HasProximitySensor { get; private set; } // NFC

        // SensorCore
        /// <summary>
        /// Gets a value indicating whether [sensor core activity monitor API supported].
        /// </summary>
        /// <value><c>true</c> if [sensor core activity monitor API supported]; otherwise, <c>false</c>.</value>
        public bool SensorCoreActivityMonitorApiSupported { get; private set; }
        /// <summary>
        /// Gets a value indicating whether [sensor core place monitor API supported].
        /// </summary>
        /// <value><c>true</c> if [sensor core place monitor API supported]; otherwise, <c>false</c>.</value>
        public bool SensorCorePlaceMonitorApiSupported { get; private set; }
        /// <summary>
        /// Gets a value indicating whether [sensor core step counter API supported].
        /// </summary>
        /// <value><c>true</c> if [sensor core step counter API supported]; otherwise, <c>false</c>.</value>
        public bool SensorCoreStepCounterApiSupported { get; private set; }
        /// <summary>
        /// Gets a value indicating whether [sensor core track point monitor API supported].
        /// </summary>
        /// <value><c>true</c> if [sensor core track point monitor API supported]; otherwise, <c>false</c>.</value>
        public bool SensorCoreTrackPointMonitorApiSupported { get; private set; }

        // Other hardware properties
        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        /// <value>The name of the device.</value>
        public string DeviceName { get; private set; }
        /// <summary>
        /// Gets the device identifier.
        /// </summary>
        /// <value>The device identifier.</value>
        public string DeviceId { get; private set; }
        /// <summary>
        /// Gets the manufacturer.
        /// </summary>
        /// <value>The manufacturer.</value>
        public string Manufacturer { get; private set; }
        /// <summary>
        /// Gets the hardware version.
        /// </summary>
        /// <value>The hardware version.</value>
        public string HardwareVersion { get; private set; }
        /// <summary>
        /// Gets the firmware version.
        /// </summary>
        /// <value>The firmware version.</value>
        public string FirmwareVersion { get; private set; }
        /// <summary>
        /// Gets a value indicating whether this instance has sd card present.
        /// </summary>
        /// <value><c>true</c> if this instance has sd card present; otherwise, <c>false</c>.</value>
        public bool HasSDCardPresent { get; private set; }
        /// <summary>
        /// Gets a value indicating whether this instance has vibration device.
        /// </summary>
        /// <value><c>true</c> if this instance has vibration device; otherwise, <c>false</c>.</value>
        public bool HasVibrationDevice { get; private set; }
        /// <summary>
        /// Gets the processor core count.
        /// </summary>
        /// <value>The processor core count.</value>
        public int ProcessorCoreCount { get; private set; }

        // Software and other dynamic, non-hardware properties
#if WINDOWS_PHONE_APP
        /// <summary>
        /// Gets the application theme.
        /// </summary>
        /// <value>The application theme.</value>
        public ApplicationTheme AppTheme { get; private set; }
#endif
        /// <summary>
        /// Gets the color of the theme accent.
        /// </summary>
        /// <value>The color of the theme accent.</value>
        public Color ThemeAccentColor { get; private set; }
        #endregion Public Properties

        #region Construction, initialisation and refreshing

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns>The singleton instance of this class.</returns>
        public static DeviceProperties GetInstance()
        {
            lock (_syncLock)
            {
                if (_instance == null)
                {
                    _instance = new DeviceProperties();
                }
            }

            return _instance;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        private DeviceProperties()
        {
        }

        /// <summary>
        /// Resolves all the properties.
        /// Note that this method is synchronous, but some method calls within
        /// are asynchronous. Thus, this method will be executed while some of
        /// the asynchronous methods may still be running. If you remove or add
        /// asynchronous method calls, make sure to update the value of
        /// _numberOfAsyncOperationsToComplete so that the IsReadyChanged event
        /// is properly fired.
        /// </summary>
        public void Resolve()
        {
#if (DEBUG)
            Debug.WriteLine(DebugTag + "Resolve() ->");
            _startOfResolveTime = DateTime.Now;
#endif
            if (!IsReady)
            {
                _numberOfAsyncOperationsToComplete = 4; // This must match the number of async method calls!
                _numberOfAsyncOperationsCompleted = 0;

                ResolveDeviceId();
                ResolveDeviceInformation(); // ResolveCameraInfoAsync() depends on this to be run first!
                ResolveCameraInfoAsync();
                ResolveMemoryInfo();
                ResolvePowerInfo();
                ResolveProcessorCoreCount();
                ResolveScreenResolutionAsync();
                ResolveSDCardInfoAsync();
                //ResolveSensorCoreAvailabilityAsync();
                ResolveSensorInfo();
                ResolveUiThemeAsync();
                ResolveVibrationDeviceInfo();
            }
            else
            {
                // Refreshing dynamic properties
                Debug.WriteLine(DebugTag + "Resolve(): Already resolved once, refreshing dynamic properties...");
                IsReady = false;

                if (IsReadyChanged != null)
                {
                    IsReadyChanged(this, IsReady);
                }

                _numberOfAsyncOperationsToComplete = 2; // This must match the number of async method calls!
                _numberOfAsyncOperationsCompleted = 0;

                ResolveMemoryInfo();
                ResolvePowerInfo();
                ResolveSDCardInfoAsync();
                ResolveUiThemeAsync();
            }

            if (_numberOfAsyncOperationsToComplete == 0)
            {
                /* There was no async method calls, so all the properties
                 * have been resolved. AsyncOperationComplete() will change
                 * IsReady property and notify listeners.
                 */
                AsyncOperationComplete();
            }
        }

        /// <summary>
        /// For convenience. Runs Resolve() asynchronously.
        /// </summary>
        public async Task ResolveAsync()
        {
            await Task.Run(() => Resolve()).ConfigureAwait(true);
        }

        /// <summary>
        /// Asynchronouses the operation complete.
        /// </summary>
        private void AsyncOperationComplete()
        {
            lock (_syncLock)
            {
                _numberOfAsyncOperationsCompleted++;

                if (_numberOfAsyncOperationsCompleted >= _numberOfAsyncOperationsToComplete - 1)
                {
                    Debug.WriteLine(DebugTag + "AsyncOperationComplete(): All operations complete!");
                    IsReady = true;
                    NotifyIsReadyChangedAsync();
#if (DEBUG)
                    Debug.WriteLine(DebugTag + "AsyncOperationComplete(): Time elapsed: " + (DateTime.Now - _startOfResolveTime));
#endif
                }
            }
        }

        /// <summary>
        /// notify is ready changed as an asynchronous operation.
        /// </summary>
        private async void NotifyIsReadyChangedAsync()
        {
            if (Dispatcher == null) return;

            await Dispatcher.RunAsync(
                DispatcherPriority.Normal, () =>
                {
                    if (IsReadyChanged != null)
                    {
                        IsReadyChanged(this, IsReady);
                    }
                });
        }

        #endregion // Construction, initialisation and refreshing

        #region Battery and power supply

        /// <summary>
        /// Resolves the power information.
        /// </summary>
        private void ResolvePowerInfo()
        {
            HasBatteryStatusInfo = false;

#if WINDOWS_PHONE || WINDOWS_PHONE_APP
            var battery = Windows.Phone.Devices.Power.Battery.GetDefault();
            if (battery != null)
            {
                HasBatteryStatusInfo = true;
                RemainingBatteryCharge = battery.RemainingChargePercent;
#if WINDOWS_PHONE
                PowerSavingModeEnabled = Windows.Phone.System.Power.PowerManager.PowerSavingMode == PowerSavingMode.On;
#else
                PowerSavingModeEnabled = Windows.Phone.System.Power.PowerManager.PowerSavingModeEnabled;
#endif
            }
#elif WINDOWS_UWP
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.System.Power.PowerManager"))
            {
                HasBatteryStatusInfo = true;
                RemainingBatteryCharge = Windows.System.Power.PowerManager.RemainingChargePercent;

#if WINDOWS_PHONE_UWP
                PowerSavingModeEnabled = Windows.Phone.System.Power.PowerManager.PowerSavingModeEnabled;
#else
                PowerSavingModeEnabled = Windows.System.Power.PowerManager.EnergySaverStatus == Windows.System.Power.EnergySaverStatus.On;
#endif
            }
#endif

                if (!HasBatteryStatusInfo)
            {
                Debug.WriteLine(DebugTag
                                + "ResolvePowerInfo(): No battery status info available.");
            }
            else
            {
                Debug.WriteLine(DebugTag + "ResolvePowerInfo(): " + RemainingBatteryCharge + ", " + PowerSavingModeEnabled);
            }
        }

        #endregion // Battery and power supply

        #region Cameras and flashes

        /// <summary>
        /// Resolves the following properties for both back and front camera:
        /// Flash and (auto) focus support and both photo and video capture
        /// resolutions.
        /// </summary>
        private async Task ResolveCameraInfoAsync()
        {
            HasBackCamera = false;
            HasFrontCamera = false;
            HasBackCameraFlash = false;
            HasFrontCameraFlash = false;
            HasBackCameraAutoFocus = false;
            HasFrontCameraAutoFocus = false;
            BackCameraPhotoResolutions = new List<Size>();
            BackCameraVideoResolutions = new List<Size>();
            FrontCameraPhotoResolutions = new List<Size>();
            FrontCameraVideoResolutions = new List<Size>();

#if WINDOWS_PHONE

            try
            {
                HasBackCamera = PhotoCaptureDevice.AvailableSensorLocations.Contains<CameraSensorLocation>(CameraSensorLocation.Back);
                HasFrontCamera = PhotoCaptureDevice.AvailableSensorLocations.Contains<CameraSensorLocation>(CameraSensorLocation.Front);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(DebugTag
                    + ".ResolveCameraAndFlashInfo(): Failed to resolve the camera availability: "
                    + e.ToString());
            }

            if (HasBackCamera)
            {
                var cam = new Microsoft.Devices.PhotoCamera(Microsoft.Devices.CameraType.Primary);
                HasBackCameraFlash = cam.IsFlashModeSupported(Microsoft.Devices.FlashMode.On);
            }

            if (HasFrontCamera)
            {
                var cam = new Microsoft.Devices.PhotoCamera(Microsoft.Devices.CameraType.FrontFacing);
                HasFrontCameraFlash = cam.IsFlashModeSupported(Microsoft.Devices.FlashMode.On);
            }

#else
            var devices = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(
                Windows.Devices.Enumeration.DeviceClass.VideoCapture);
            DeviceInformation backCameraDeviceInformation = devices.FirstOrDefault(x => x.EnclosureLocation != null
                && x.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Back);
            DeviceInformation frontCameraDeviceInformation = devices.FirstOrDefault(x => x.EnclosureLocation != null
                && x.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Front);

            string backCameraId = null;
            string frontCameraId = null;
            var cameraIds = new List<string>();

            if (backCameraDeviceInformation != null)
            {
                backCameraId = backCameraDeviceInformation.Id;
                HasBackCamera = true;
                cameraIds.Add(backCameraId);
            }

            if (frontCameraDeviceInformation != null)
            {
                frontCameraId = frontCameraDeviceInformation.Id;
                HasFrontCamera = true;
                cameraIds.Add(frontCameraId);
            }

            foreach (string cameraId in cameraIds)
            {
                if (_mediaCapture == null)
                {
                    _mediaCapture = new MediaCapture();

                    try
                    {
                        await _mediaCapture.InitializeAsync(
                            new MediaCaptureInitializationSettings
                            {
                                VideoDeviceId = cameraId
                            });
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(DebugTag + "ResolveCameraInfoAsync(): Failed to initialize camera: " + e.ToString());
                        _mediaCapture = null;
                        continue;
                    }
                }

                if (_mediaCapture.VideoDeviceController == null)
                {
                    Debug.WriteLine(DebugTag + "ResolveCameraInfoAsync(): No video device controller!");
                    continue;
                }

                bool hasFlash = _mediaCapture.VideoDeviceController.FlashControl.Supported;

                Windows.Media.Devices.MediaDeviceControlCapabilities focusCaps = _mediaCapture.VideoDeviceController.Focus.Capabilities;
#if !NETFX_CORE
                bool focusChangedSupported = _mediaCapture.VideoDeviceController.FocusControl.FocusChangedSupported;
#endif
                bool autoAdjustmentEnabled = false;
                _mediaCapture.VideoDeviceController.Focus.TryGetAuto(out autoAdjustmentEnabled);

                Debug.WriteLine(DebugTag + "ResolveCameraInfoAsync(): Focus details of the "
                    + (cameraId.Equals(backCameraId) ? "back camera:" : "front camera:")
                    + "\n\t- Focus.Capabilities.AutoModeSupported: " + focusCaps.AutoModeSupported
                    + "\n\t- Focus.Capabilities.Max: " + focusCaps.Max
                    + "\n\t- Focus.Capabilities.Min: " + focusCaps.Min
                    + "\n\t- Focus.Capabilities.Step: " + focusCaps.Step
                    + "\n\t- Focus.Capabilities.Supported: " + focusCaps.Supported
                    + "\n\t- Focus.TryGetAuto() (automatic adjustment enabled): " + autoAdjustmentEnabled
#if !NETFX_CORE
                    + "\n\t- FocusControl.FocusChangedSupported: " + focusChangedSupported
#endif
                    );

                if (cameraId.Equals(backCameraId))
                {
                    HasBackCameraFlash = hasFlash;
#if !NETFX_CORE
                    HasBackCameraAutoFocus = focusChangedSupported;
#endif
                    BackCameraPhotoResolutions = ResolveCameraResolutions(_mediaCapture, MediaStreamType.Photo);
                    BackCameraVideoResolutions = ResolveCameraResolutions(_mediaCapture, MediaStreamType.VideoRecord);
                }
                else if (cameraId.Equals(frontCameraId))
                {
                    HasFrontCameraFlash = hasFlash;
#if !NETFX_CORE
                    HasFrontCameraAutoFocus = focusChangedSupported;
#endif
                    FrontCameraPhotoResolutions = ResolveCameraResolutions(_mediaCapture, MediaStreamType.Photo);
                    FrontCameraVideoResolutions = ResolveCameraResolutions(_mediaCapture, MediaStreamType.VideoRecord);
                }

                _mediaCapture.Dispose();
                _mediaCapture = null;
            }

            // Auto focus fix for older phones
            foreach (string model in _wp80PhoneModelsWithAutoFocus)
            {
                if (DeviceName.Contains(model))
                {
                    Debug.WriteLine(DebugTag + "ResolveCameraInfoAsync(): Auto focus fix applied");
                    HasBackCameraAutoFocus = true;
                    break;
                }
            }

            // Sort resolutions from highest to lowest
            SortSizesFromHighestToLowest(BackCameraPhotoResolutions);
            SortSizesFromHighestToLowest(FrontCameraPhotoResolutions);
            SortSizesFromHighestToLowest(BackCameraVideoResolutions);
            SortSizesFromHighestToLowest(FrontCameraVideoResolutions);

            Debug.WriteLine(DebugTag + "ResolveCameraInfoAsync(): "
                + "\n\t- Back camera ID: " + backCameraId
                + "\n\t- Front camera ID: " + frontCameraId
                + "\n\t- Back camera flash supported: " + HasBackCameraFlash
                + "\n\t- Front camera flash supported: " + HasFrontCameraFlash);
#endif
            AsyncOperationComplete();
        }

#if !WINDOWS_PHONE
        /// <summary>
        /// Resolves the available resolutions for the device defined by the given
        /// media capture instance.
        /// </summary>
        /// <param name="mediaCapture">An initialised media capture instance.</param>
        /// <param name="mediaStreamType">The type of the media stream (e.g. video or photo).</param>
        /// <returns>The list of available resolutions or an empty list, if not available.</returns>
        private List<Size> ResolveCameraResolutions(MediaCapture mediaCapture, MediaStreamType mediaStreamType)
        {
            List<Size> resolutions = new List<Size>();
            IReadOnlyList<IMediaEncodingProperties> mediaStreamPropertiesList;

            try
            {
                mediaStreamPropertiesList = mediaCapture.VideoDeviceController.GetAvailableMediaStreamProperties(mediaStreamType);
            }
            catch (Exception e)
            {
                Debug.WriteLine(DebugTag + "ResolveCameraResolutions(): " + e);
                return resolutions;
            }

            foreach (var mediaStreamProperties in mediaStreamPropertiesList)
            {
                Size size = new Size(0, 0);
                bool sizeSet = false;

                var streamProperties = mediaStreamProperties as VideoEncodingProperties;
                if (streamProperties != null)
                {
                    VideoEncodingProperties properties = streamProperties;
                    size = new Size(properties.Width, properties.Height);
                    sizeSet = true;
                }
                else
                {
                    var encodingProperties = mediaStreamProperties as ImageEncodingProperties;
                    if (encodingProperties != null)
                    {
                        ImageEncodingProperties properties = encodingProperties;
                        size = new Size(properties.Width, properties.Height);
                        sizeSet = true;
                    }
                }

                if (sizeSet)
                {
                    if (!resolutions.Contains(size))
                    {
                        resolutions.Add(size);
                    }
                }
            }

            return resolutions;
        }
#endif

        #endregion // Cameras and flashes

        #region Memory

        /// <summary>
        /// Resolves the memory information.
        /// </summary>
        private void ResolveMemoryInfo()
        {
#if WINDOWS_PHONE
            ApplicationCurrentMemoryUsageInBytes = (ulong) DeviceStatus.ApplicationCurrentMemoryUsage;
            ApplicationMemoryUsageLimitInBytes = (ulong)DeviceStatus.ApplicationMemoryUsageLimit;
            ApplicationPeakMemoryUsageInBytes = (ulong)DeviceStatus.ApplicationPeakMemoryUsage;
            DeviceTotalMemoryInBytes = (ulong)DeviceStatus.DeviceTotalMemory;

            // The following properties also work:
            //ApplicationCurrentMemoryUsageInBytes = (long)Windows.Phone.System.Memory.MemoryManager.ProcessCommittedBytes;
            //ApplicationMemoryUsageLimitInBytes = (long)Windows.Phone.System.Memory.MemoryManager.ProcessCommittedLimit;

#elif WINDOWS_PHONE_APP || WINDOWS_UWP
            ApplicationCurrentMemoryUsageInBytes = (ulong)Windows.System.MemoryManager.AppMemoryUsage;
            ApplicationMemoryUsageLimitInBytes = (ulong)Windows.System.MemoryManager.AppMemoryUsageLimit;
            //#elif WINDOWS_PHONE_UWP
#else
            //TODO Fille this out
            ApplicationCurrentMemoryUsageInBytes = 0;
            ApplicationMemoryUsageLimitInBytes = 0;
#endif

            System.Diagnostics.Debug.WriteLine(
                "From DeviceStatus class:"
                + "\n - ApplicationCurrentMemoryUsage: " + TransformBytes(ApplicationCurrentMemoryUsageInBytes, UnitPrefixes.Mega, 1) + " MB"
                + "\n - ApplicationMemoryUsageLimit: " + TransformBytes(ApplicationMemoryUsageLimitInBytes, UnitPrefixes.Mega, 1) + " MB"
                + "\n - ApplicationPeakMemoryUsage: " + TransformBytes(ApplicationPeakMemoryUsageInBytes, UnitPrefixes.Mega, 1) + " MB"
                + "\n - DeviceTotalMemory: " + TransformBytes(DeviceTotalMemoryInBytes, UnitPrefixes.Mega, 1) + " MB"
                );
        }

        #endregion // Memory

        #region Screen

        /// <summary>
        /// Resolves the screen resolution and display size.
        /// </summary>
        private async Task ResolveScreenResolutionAsync()
        {
            // Initialise the values
            ScreenResolution = Resolutions.Unknown;
            ScreenResolutionSize = new Size(0, 0);

            DisplayInfo = new DisplayInfo();

            double screenResolutionX = 0;
            double screenResolutionY = 0;

            if (Dispatcher == null) return;

            await Dispatcher.RunAsync(
                DispatcherPriority.High, () =>
                {
                    System.Diagnostics.Debug.WriteLine("Querying Display Information");
#if WINDOWS_PHONE_APP || WINDOWS_UWP || NETFX_CORE
                    var info = DisplayInformation.GetForCurrentView();

                    DisplayInfo.CurrentOrientation = info.CurrentOrientation;
                    DisplayInfo.LogicalDpi = info.LogicalDpi;
                    DisplayInfo.NativeOrientation = info.NativeOrientation;
                    DisplayInfo.RawDpiX = info.RawDpiX;
                    DisplayInfo.RawDpiY = info.RawDpiY;
                    DisplayInfo.ResolutionScale = info.ResolutionScale;
                    DisplayInfo.StereoEnabled = info.StereoEnabled;

#if NETFX_CORE
                    DisplayInfo.RawPixelsPerViewPixel = 1;
#else
                    DisplayInfo.RawPixelsPerViewPixel = info.RawPixelsPerViewPixel;
#endif

                    screenResolutionX = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Bounds.Width * DisplayInfo.RawPixelsPerViewPixel;
                    screenResolutionY = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Bounds.Height * DisplayInfo.RawPixelsPerViewPixel;
#elif WINDOWS_PHONE || SILVERLIGHT
                    DisplayInfo.LogicalDpi = DisplayProperties.LogicalDpi;
                    DisplayInfo.LogicalDpi = DisplayProperties.LogicalDpi;
                    DisplayInfo.StereoEnabled = DisplayProperties.StereoEnabled;

                    DisplayInfo.CurrentOrientation = DisplayProperties.CurrentOrientation;
                    DisplayInfo.NativeOrientation = DisplayProperties.NativeOrientation;
                    DisplayInfo.ResolutionScale = DisplayProperties.ResolutionScale;

                    object objExtendedProperty;

                    if (DeviceExtendedProperties.TryGetValue("PhysicalScreenResolution", out objExtendedProperty))
                    {
                        var physicalScreenResolution = (Size)objExtendedProperty;
                        screenResolutionY = (int)physicalScreenResolution.Height;
                        screenResolutionX = (int)physicalScreenResolution.Width;

                        //	var scaleFactor = Application.Current.Host.Content.ScaleFactor;

                        //	var screenResolution = (Size)objExtendedProperty;
                        //	var width = Application.Current.Host.Content.ActualWidth;
                        //	var physicalSize = new Size(screenResolution.Width / DisplayInfo.RawDpiX, screenResolution.Height / rawDpiY);
                        //	var scale = Math.Max(1, physicalSize.Width / DisplayConstants.BaselineWidthInInches);
                        //	var idealViewWidth = Math.Min(DisplayConstants.BaselineWidthInViewPixels * scale, screenResolution.Width);
                        //	var idealScale = screenResolution.Width / idealViewWidth;
                        //	rawPixelsPerViewPixel = idealScale.NudgeToClosestPoint(1); //bucketizedScale
                        //	var viewResolution = new Size(screenResolution.Width / rawPixelsPerViewPixel, screenResolution.Height / rawPixelsPerViewPixel);
                    }
                    else
                    {
                        var scaleFactor = Application.Current.Host.Content.ScaleFactor;
                        screenResolutionY = (int)(Application.Current.Host.Content.ActualHeight * scaleFactor);
                        screenResolutionX = (int)(Application.Current.Host.Content.ActualWidth * scaleFactor);

                    }

                    objExtendedProperty = null;

                    if (DeviceExtendedProperties.TryGetValue("RawDpiX", out objExtendedProperty))
                    {
#if WINDOWS_PHONE
                        DisplayInfo.RawDpiX = (double)objExtendedProperty;
#else
                        DisplayInfo.RawDpiX = (float)objExtendedProperty;
#endif
                    }

                    if (DeviceExtendedProperties.TryGetValue("RawDpiY", out objExtendedProperty))
                    {
#if WINDOWS_PHONE
                        DisplayInfo.RawDpiY = (double)objExtendedProperty;
#else
                        DisplayInfo.RawDpiY = (float)objExtendedProperty;
#endif
                    }
#endif
                });

            ScreenResolutionSize = new Size(Math.Round(screenResolutionX), Math.Round(screenResolutionY));

            if (screenResolutionY < 960)
            {
                ScreenResolution = Resolutions.WVGA;
            }
            else if (screenResolutionY < 1280)
            {
                ScreenResolution = Resolutions.qHD;
            }
            else if (screenResolutionY < 1920)
            {
                if (screenResolutionX < 768)
                {
                    ScreenResolution = Resolutions.HD720;
                }
                else
                {
                    ScreenResolution = Resolutions.WXGA;
                }
            }
            else if (screenResolutionY > 1280)
            {
                ScreenResolution = Resolutions.HD1080;
            }

            if (DisplayInfo.RawDpiX > 0 && DisplayInfo.RawDpiY > 0)
            {
                // Calculate screen diagonal in inches.
                DisplaySizeInInches =
                    Math.Sqrt(Math.Pow(ScreenResolutionSize.Width / DisplayInfo.RawDpiX, 2) +
                              Math.Pow(ScreenResolutionSize.Height / DisplayInfo.RawDpiY, 2));
                DisplaySizeInInches = Math.Round(DisplaySizeInInches, 1); // One decimal is enough
            }

            Debug.WriteLine(DebugTag + "ResolveScreenResolutionAsync(): Screen properties:"
                //+ "\n - Raw pixels per view pixel: " + rawPixelsPerViewPixel
                + "\n - Raw DPI: " + DisplayInfo.RawDpiX + ", " + DisplayInfo.RawDpiY
                + "\n . Logical DPI: " + DisplayInfo.LogicalDpi
                + "\n - Resolution: " + ScreenResolution
                + "\n - Resolution in pixels: " + ScreenResolutionSize
                + "\n - Screen size in inches: " + DisplaySizeInInches);

            AsyncOperationComplete();
        }

        #endregion // Screen

        #region Sensors

        /// <summary>
        /// Resolves the sensor information.
        /// </summary>
        private void ResolveSensorInfo()
        {
            if (Windows.Devices.Sensors.Accelerometer.GetDefault() != null)
            {
                HasAccelerometerSensor = true;
            }

            if (Compass.GetDefault() != null)
            {
                HasCompass = true;
            }

            try
            {
                if (Gyrometer.GetDefault() != null)
                {
                    HasGyroscopeSensor = true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

            if (Windows.Devices.Sensors.Inclinometer.GetDefault() != null)
            {
                HasInclinometerSensor = true;
            }

            if (Windows.Devices.Sensors.OrientationSensor.GetDefault() != null)
            {
                HasOrientationSensor = true;
            }

            // ProximityDevice is NFC
            if (ProximityDevice.GetDefault() != null)
            {
                HasProximitySensor = true;
            }
        }

        //private async void ResolveSensorCoreAvailabilityAsync()
        //{
        //	SensorCoreActivityMonitorApiSupported = await Lumia.Sense.ActivityMonitor.IsSupportedAsync();
        //	SensorCorePlaceMonitorApiSupported = await Lumia.Sense.PlaceMonitor.IsSupportedAsync();
        //	SensorCoreStepCounterApiSupported = await Lumia.Sense.StepCounter.IsSupportedAsync();
        //	SensorCoreTrackPointMonitorApiSupported = await Lumia.Sense.TrackPointMonitor.IsSupportedAsync();

        //	AsyncOperationComplete();
        //}

        #endregion

        #region Other hardware properties

        /// <summary>
        /// Resolves the device information.
        /// </summary>
        private void ResolveDeviceInformation()
        {
#if WINDOWS_PHONE_APP || NETFX_CORE || WINDOWS_PHONE_UWP
            Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation deviceInformation =
                new Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation();

            DeviceName = deviceInformation.SystemProductName;
            Manufacturer = deviceInformation.SystemManufacturer;

#if WINDOWS_PHONE_UWP && !NETFX_CORE
            HardwareVersion = deviceInformation.SystemHardwareVersion;
            FirmwareVersion = deviceInformation.SystemFirmwareVersion;
#endif
#else
            DeviceName = DeviceStatus.DeviceName;
            Manufacturer = DeviceStatus.DeviceManufacturer;
            HardwareVersion = DeviceStatus.DeviceHardwareVersion;
            FirmwareVersion = DeviceStatus.DeviceFirmwareVersion;
#endif
        }

        private void ResolveDeviceId()
        {
#if WINDOWS_PHONE
            object o;
            if (Microsoft.Phone.Info.DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out o))
            {
                DeviceId = BitConverter.ToString((byte[])o);
            }
            else
            {
                throw new UnauthorizedAccessException(
                    "Application has no access to device identity. To enable access consider enabling ID_CAP_IDENTITY_DEVICE on app manifest.");
            }
#elif (WINDOWS_PHONE_APP || NETFX_CORE || WINDOWS_PHONE_UWP) && !WINDOWS_UWP
            var value = _hardwareToken != null ? _hardwareToken.Id.ToArray() : (_hardwareToken = HardwareIdentification.GetPackageSpecificToken(null))?.Id.ToArray();
            DeviceId = BitConverter.ToString(value);
#elif !NETFX_CORE || !WINDOWS_UWP
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.System.Profile.HardwareIdentification"))
            {
                var value = _hardwareToken != null ? _hardwareToken.Id.ToArray() : (_hardwareToken = HardwareIdentification.GetPackageSpecificToken(null)).Id.ToArray();
                DeviceId = BitConverter.ToString(value);
            }
#else
#endif
        }

        /// <summary>
        /// Resolves the SD card information. Note that the result false if the
        /// card is not installed even if the device supports one.
        /// "You can't simply check the presence of SD card without first
        /// registering in Package.appxmanifest Declarations page a File type
        /// Association. After you have done that, then you can check the
        /// presence of SD card with this code."
        /// </summary>
        private async Task ResolveSDCardInfoAsync()
        {
            try
            {
                var removableDevices = Windows.Storage.KnownFolders.RemovableDevices;
                var sdCards = await removableDevices.GetFoldersAsync();
                HasSDCardPresent = (sdCards.Count > 0);
            }
            catch (UnauthorizedAccessException e)
            {
                Debug.WriteLine(DebugTag + "ResolveSDCardInfoAsync(): " + e.ToString());
            }

            AsyncOperationComplete();
        }

        /// <summary>
        /// Resolves the vibration device information.
        /// </summary>
        private void ResolveVibrationDeviceInfo()
        {
#if !NETFX_CORE || WINDOWS_PHONE_UWP || WINDOWS_PHONE_APP || WINDOWS_PHONE
            if (Windows.Phone.Devices.Notification.VibrationDevice.GetDefault() != null)
            {
                HasVibrationDevice = true;
            }
#endif
        }

        /// <summary>
        /// Resolves the processor core count.
        /// </summary>
        private void ResolveProcessorCoreCount()
        {
            ProcessorCoreCount = System.Environment.ProcessorCount;
            Debug.WriteLine(DebugTag + "ResolveProcessorCoreCount(): " + ProcessorCoreCount);
        }

        #endregion // Other hardware properties

        #region Software, themes and non-hardware dependent

        private async Task ResolveUiThemeAsync()
        {
            if (Dispatcher == null) return;

            await Dispatcher.RunAsync(
                DispatcherPriority.Normal, () =>
                {
#if WINDOWS_PHONE
                    ThemeAccentColor = (Color)Application.Current.Resources["PhoneAccentColor"];

#elif WINDOWS_PHONE_APP
                    AppTheme = Application.Current.RequestedTheme;

                    try
                    {
                        var brush = (SolidColorBrush)Application.Current.Resources["PhoneAccentBrush"];
                        ThemeAccentColor = brush.Color;
                    }
                    catch (System.Runtime.InteropServices.COMException e)
                    {
                        // Thrown if no resources with the given key is found
                        Debug.WriteLine(DebugTag + "ResolveUiThemeAsync(): " + e.ToString());
                    }
#endif
                });

            AsyncOperationComplete();
        }

        #endregion // Software, themes and non-hardware dependent

        #region Utility methods

        /// <summary>
        /// Transforms the given bytes based on the given desired unit.
        /// </summary>
        /// <param name="bytes">The number of bytes to transform.</param>
        /// <param name="toUnit">The unit into which to transform, e.g. gigabytes.</param>
        /// <param name="numberOfDecimals">The number of decimals desired.</param>
        /// <returns>System.Double.</returns>
        public static double TransformBytes(ulong bytes, UnitPrefixes toUnit, int numberOfDecimals = 0)
        {
            double retval = 0;
            double denominator = 0;

            switch (toUnit)
            {
                case UnitPrefixes.Kilo:
                    denominator = 1024;
                    break;
                case UnitPrefixes.Mega:
                    denominator = 1024 * 1024;
                    break;
                case UnitPrefixes.Giga:
                    denominator = Math.Pow(1024, 3);
                    break;
                default:
                    break;
            }

            if (denominator != 0)
            {
                retval = bytes / denominator;
            }

            if (numberOfDecimals >= 0)
            {
                retval = Math.Round(retval, numberOfDecimals);
            }

            return retval;
        }

        /// <summary>
        /// Dumps the details of every device to the output.
        /// </summary>
        public async static Task DumpDeviceInformation()
        {
#if !WINDOWS_PHONE
            DeviceInformationCollection devices = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync();

            foreach (DeviceInformation device in devices)
            {
                Debug.WriteLine("Found device: " /*+ device.Id + ": "*/ + device.Name + (device.IsEnabled ? " (enabled) " : " (disabled)"));
                IReadOnlyDictionary<string, object> properties = device.Properties;

                foreach (string key in properties.Keys)
                {
                    object value = null;
                    properties.TryGetValue(key, out value);

                    if (value != null)
                    {
                        Debug.WriteLine("\t" + key + ": " + value.ToString());
                    }
                }
            }
#endif
        }

        /// <summary>
        /// Sorts the sizes from highest to lowest.
        /// </summary>
        /// <param name="sizes">The sizes.</param>
        private void SortSizesFromHighestToLowest(List<Size> sizes)
        {
            if (sizes != null && sizes.Count > 1)
            {
                sizes.Sort(delegate (Size x, Size y)
                {
                    if (x.Width * x.Height < y.Width * y.Height)
                    {
                        return 1;
                    }

                    return -1;
                });
            }
        }

        #endregion // Utility methods
    }

    /// <summary>
    /// Class DisplayInfo.
    /// </summary>
    public class DisplayInfo
    {
        /// <summary>
        /// Gets the current orientation.
        /// </summary>
        /// <value>The current orientation.</value>
        public DisplayOrientations CurrentOrientation { get; internal set; }
        /// <summary>
        /// Gets the logical dpi.
        /// </summary>
        /// <value>The logical dpi.</value>
        public float LogicalDpi { get; internal set; }
        /// <summary>
        /// Gets the native orientation.
        /// </summary>
        /// <value>The native orientation.</value>
        public DisplayOrientations NativeOrientation { get; internal set; }
        /// <summary>
        /// Gets the raw dpi x.
        /// </summary>
        /// <value>The raw dpi x.</value>
#if WINDOWS_PHONE
        public double RawDpiX { get; internal set; }
#else
        public float RawDpiX { get; internal set; }
#endif
        /// <summary>
        /// Gets the raw dpi y.
        /// </summary>
        /// <value>The raw dpi y.</value>
#if WINDOWS_PHONE
        public double RawDpiY { get; internal set; }
#else
        public float RawDpiY { get; internal set; }
#endif
        /// <summary>
        /// Gets the resolution scale.
        /// </summary>
        /// <value>The resolution scale.</value>
        public ResolutionScale ResolutionScale { get; internal set; }
        /// <summary>
        /// Gets a value indicating whether [stereo enabled].
        /// </summary>
        /// <value><c>true</c> if [stereo enabled]; otherwise, <c>false</c>.</value>
        public bool StereoEnabled { get; internal set; }
        /// <summary>
        /// Gets or sets the raw pixels per view pixel.
        /// </summary>
        /// <value>The raw pixels per view pixel.</value>
        public double RawPixelsPerViewPixel { get; set; }
    }

    //#if WINDOWS_PHONE1
    //    /// <summary>
    //    /// Enum DisplayOrientations
    //    /// </summary>
    //    public enum DisplayOrientations : uint
    //    {
    //        /// <summary>
    //        /// The none
    //        /// </summary>
    //        None = 0U,
    //        /// <summary>
    //        /// The landscape
    //        /// </summary>
    //        Landscape = 1U,
    //        /// <summary>
    //        /// The portrait
    //        /// </summary>
    //        Portrait = 2U,
    //        /// <summary>
    //        /// The landscape flipped
    //        /// </summary>
    //        LandscapeFlipped = 4U,
    //        /// <summary>
    //        /// The portrait flipped
    //        /// </summary>
    //        PortraitFlipped = 8U,
    //    }

    //    /// <summary>
    //    /// Enum ResolutionScale
    //    /// </summary>
    //    public enum ResolutionScale
    //    {
    //        /// <summary>
    //        /// The invalid
    //        /// </summary>
    //        Invalid = 0,
    //        /// <summary>
    //        /// The scale100 percent
    //        /// </summary>
    //        Scale100Percent = 100,
    //        /// <summary>
    //        /// The scale120 percent
    //        /// </summary>
    //        Scale120Percent = 120,
    //        /// <summary>
    //        /// The scale125 percent
    //        /// </summary>
    //        Scale125Percent = 125,
    //        /// <summary>
    //        /// The scale140 percent
    //        /// </summary>
    //        Scale140Percent = 140,
    //        /// <summary>
    //        /// The scale150 percent
    //        /// </summary>
    //        Scale150Percent = 150,
    //        /// <summary>
    //        /// The scale160 percent
    //        /// </summary>
    //        Scale160Percent = 160,
    //        /// <summary>
    //        /// The scale175 percent
    //        /// </summary>
    //        Scale175Percent = 175,
    //        /// <summary>
    //        /// The scale180 percent
    //        /// </summary>
    //        Scale180Percent = 180,
    //        /// <summary>
    //        /// The scale200 percent
    //        /// </summary>
    //        Scale200Percent = 200,
    //        /// <summary>
    //        /// The scale225 percent
    //        /// </summary>
    //        Scale225Percent = 225,
    //        /// <summary>
    //        /// The scale250 percent
    //        /// </summary>
    //        Scale250Percent = 250,
    //        /// <summary>
    //        /// The scale300 percent
    //        /// </summary>
    //        Scale300Percent = 300,
    //        /// <summary>
    //        /// The scale350 percent
    //        /// </summary>
    //        Scale350Percent = 350,
    //        /// <summary>
    //        /// The scale400 percent
    //        /// </summary>
    //        Scale400Percent = 400,
    //        /// <summary>
    //        /// The scale450 percent
    //        /// </summary>
    //        Scale450Percent = 450,
    //        /// <summary>
    //        /// The scale500 percent
    //        /// </summary>
    //        Scale500Percent = 500,
    //    }
    //#endif

#if WINDOWS_PHONE
    internal static class DispatcherExtensions
    {
        public static Task RunAsync(this System.Windows.Threading.Dispatcher dispatcher, DispatcherPriority priority, Action callback)
        {
            return Task.Factory.StartNew(() => dispatcher.BeginInvoke(callback));
        }
    }

    internal enum DispatcherPriority
    {
        Normal,
        High
    }

#endif
}
