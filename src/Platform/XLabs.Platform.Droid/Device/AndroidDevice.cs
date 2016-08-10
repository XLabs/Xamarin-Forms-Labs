// ***********************************************************************
// Assembly         : XLabs.Platform.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
//
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="AndroidDevice.cs" company="XLabs Team">
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
using System.IO;
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
using Java.IO;
using Java.Util;
using Java.Util.Concurrent;
using XLabs.Enums;
using XLabs.Platform.Services;
using XLabs.Platform.Services.IO;
using XLabs.Platform.Services.Media;
using FileMode = System.IO.FileMode;

namespace XLabs.Platform.Device
{
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
        /// Creates a default instance of <see cref="AndroidDevice"/>.
        /// </summary>
        public AndroidDevice()
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
        public static IDevice CurrentDevice
        {
            get { return currentDevice ?? (currentDevice = new AndroidDevice()); }
            set { currentDevice = value; }
        }

        /// <summary>
        /// Gets Unique Id for the device.
        /// </summary>
        /// <value>
        /// The id for the device.
        /// </value>
        public virtual string Id
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

        /// <summary>
        /// Gets the language code.
        /// </summary>
        /// <value>The language code.</value>
        public string LanguageCode
        {
            get { return Locale.Default.Language; }
        }

        /// <summary>
        /// Gets the time zone offset.
        /// </summary>
        /// <value>The time zone offset.</value>
        public double TimeZoneOffset
        {
            get
            {
                using (var calendar = new GregorianCalendar())
                {
                    return TimeUnit.Hours.Convert(calendar.TimeZone.RawOffset, TimeUnit.Milliseconds) / 3600;
                }
            }
        }

        /// <summary>
        /// Gets the time zone.
        /// </summary>
        /// <value>The time zone.</value>
        public string TimeZone
        {
            get { return Java.Util.TimeZone.Default.ID; }
        }

        /// <summary>
        /// Gets the orientation.
        /// </summary>
        /// <value>The orientation.</value>
        public Orientation Orientation
        {
            get
            {
                using (var wm = Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>())
                using (var dm = new DisplayMetrics())
                {
                    var rotation = wm.DefaultDisplay.Rotation;
                    wm.DefaultDisplay.GetMetrics(dm);

                    var width = dm.WidthPixels;
                    var height = dm.HeightPixels;

                    if (height > width && (rotation == SurfaceOrientation.Rotation0 || rotation == SurfaceOrientation.Rotation180)  ||
                        width > height && (rotation == SurfaceOrientation.Rotation90 || rotation == SurfaceOrientation.Rotation270))
                    {
                        switch (rotation)
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

                    switch (rotation)
                    {
                        case SurfaceOrientation.Rotation0:
                            return Orientation.Landscape & Orientation.LandscapeLeft;
                        case SurfaceOrientation.Rotation90:
                            return Orientation.Portrait & Orientation.PortraitUp;
                        case SurfaceOrientation.Rotation180:
                            return Orientation.Landscape & Orientation.LandscapeRight;
                        case SurfaceOrientation.Rotation270:
                            return Orientation.Portrait & Orientation.PortraitDown;
                        default:
                            return Orientation.None;
                    }
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
            var launchTaskSource = new TaskCompletionSource<bool>();
            try
            {
                this.StartActivity(new Intent("android.intent.action.VIEW", Android.Net.Uri.Parse(uri.ToString())));
                launchTaskSource.SetResult(true);
            }
            catch (Exception ex)
            {
                Log.Error("Device.LaunchUriAsync", ex.Message);
                launchTaskSource.SetException(ex);
            }

            return launchTaskSource.Task;
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
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Log.Debug("Memory", line);
                }
            }

            using (var reader = new RandomAccessFile("/proc/meminfo", "r"))
            {
                var line = reader.ReadLine(); // first line --> MemTotal: xxxxxx kB
                var split = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return Convert.ToInt64(split[1]) * 1024;
            }
        }
    }
}