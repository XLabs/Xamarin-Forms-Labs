using System;
using XForms.Toolkit.Services;
using XForms.Toolkit.Services.Media;
using XForms.Toolkit.Services.Geolocation;

namespace XForms.Toolkit
{
	/// <summary>
	/// Abstracted device interface.
	/// </summary>
	public interface IDevice
	{
		/// <summary>
		/// Gets the display information for the device.
		/// </summary>
		IDisplay Display { get; }

		/// <summary>
		/// Gets the phone service for this device.
		/// </summary>
		/// <value>Phone service instance if available, otherwise null.</value>
		IPhoneService PhoneService { get; }

		/// <summary>
		/// Gets the battery for the device.
		/// </summary>
		IBattery Battery { get; }

		/// <summary>
		/// Gets the accelerometer.
		/// </summary>
        /// <value>The accelerometer instance if available, otherwise null.</value>
		IAccelerometer Accelerometer { get; }

        /// <summary>
        /// Gets the gyroscope.
        /// </summary>
        /// <value>The gyroscope instance if available, otherwise null.</value>
        IGyroscope Gyroscope { get; }

		/// <summary>
		/// Gets the picture chooser.
		/// </summary>
		/// <value>The picture chooser.</value>
		IMediaPicker MediaPicker { get; }

		/// <summary>
		/// Gets the name of the device.
		/// </summary>
		/// <value>The name.</value>
		string Name { get; }

		/// <summary>
		/// Gets the firmware version.
		/// </summary>
		string FirmwareVersion { get; }

		/// <summary>
		/// Gets the hardware version.
		/// </summary>
		string HardwareVersion { get; }

		/// <summary>
		/// Gets the manufacturer.
		/// </summary>
		string Manufacturer { get; }
	}
}

