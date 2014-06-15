using System;
using MonoTouch.UIKit;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using XForms.Toolkit.Services;

namespace XForms.Toolkit
{
    /// <summary>
    /// Apple device base class.
    /// </summary>
    public abstract class AppleDevice : IDevice
    {
        private const string iPhoneExpression = "iPhone([1-6]),([1-4])";
        private const string iPodExpression = "iPod([1-5]),([1])";
        private const string iPadExpression = "iPad([1-4]),([1-6])";

        private static IDevice device;

        [DllImport(MonoTouch.Constants.SystemLibrary)]
        static internal extern int sysctlbyname([MarshalAs(UnmanagedType.LPStr)] string property, IntPtr output, IntPtr oldLen, IntPtr newp, uint newlen);

        /// <summary>
        /// Initializes a new instance of the <see cref="XForms.Toolkit.AppleDevice"/> class.
        /// </summary>
        protected AppleDevice()
        {
            this.Battery = new Battery();
            this.Accelerometer = new Accelerometer();
            this.FirmwareVersion = UIDevice.CurrentDevice.SystemVersion;
        }

        /// <summary>
        /// Gets the runtime device for Apple's devices
        /// </summary>
        public static IDevice CurrentDevice
        {
            get
            {
                if (device != null)
                {
                    return device;
                }

                var hardwareVersion = GetSystemProperty("hw.machine");

                var regex = new Regex(iPhoneExpression).Match(hardwareVersion);
                if (regex.Success)
                {
                    return device = new Phone(int.Parse(regex.Groups[1].Value), int.Parse(regex.Groups[2].Value));
                }

                regex = new Regex(iPodExpression).Match(hardwareVersion);
                if (regex.Success)
                {
                    return device = new Pod(int.Parse(regex.Groups[1].Value), int.Parse(regex.Groups[2].Value));
                }

                regex = new Regex(iPadExpression).Match(hardwareVersion);
                if (regex.Success)
                {
                    return device = new Pad(int.Parse(regex.Groups[1].Value), int.Parse(regex.Groups[2].Value));
                }

                return device = new Simulator();
            }
        }

        #region IDevice implementation
        /// <summary>
        /// Gets the display information for the device.
        /// </summary>
        public IDisplay Display
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the phone service for this device.
        /// </summary>
        /// <value>Phone service instance if available, otherwise null.</value>
        public IPhoneService PhoneService
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the battery for the device.
        /// </summary>
        public IBattery Battery
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the accelerometer for the device if available
        /// </summary>
        /// <value>Instance of IAccelerometer if available, otherwise null.</value>
        public IAccelerometer Accelerometer
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        public string Name
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the firmware version.
        /// </summary>
        public string FirmwareVersion
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the hardware version.
        /// </summary>
        public string HardwareVersion
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the manufacturer.
        /// </summary>
        public string Manufacturer
        {
            get
            {
                return "Apple";
            }
        }
        #endregion

        /// <summary>
        /// Gets the system property.
        /// </summary>
        /// <returns>The system property value.</returns>
        /// <param name="property">Property to get.</param>
        public static string GetSystemProperty(string property)
        {
            var pLen = Marshal.AllocHGlobal(sizeof(int));
            sysctlbyname(property, IntPtr.Zero, pLen, IntPtr.Zero, 0);
            var length = Marshal.ReadInt32(pLen);
            var pStr = Marshal.AllocHGlobal(length);
            sysctlbyname(property, pStr, pLen, IntPtr.Zero, 0);
            return Marshal.PtrToStringAnsi(pStr);
        }
    }
}

