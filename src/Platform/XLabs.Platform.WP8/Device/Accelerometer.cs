namespace XLabs.Platform.Device
{
	using System;

	using Microsoft.Devices.Sensors;

	/// <summary>
	/// Class Accelerometer.
	/// </summary>
	public partial class Accelerometer
	{
		/// <summary>
		/// The accelerometer
		/// </summary>
		private Microsoft.Devices.Sensors.Accelerometer accelerometer;

		/// <summary>
		/// Gets or sets the interval.
		/// </summary>
		/// <value>The interval.</value>
		public AccelerometerInterval Interval { get; set; }

		/// <summary>
		/// Starts this instance.
		/// </summary>
		partial void Start()
		{
			this.accelerometer = new Microsoft.Devices.Sensors.Accelerometer
				                 {
					                 TimeBetweenUpdates =
						                 TimeSpan.FromMilliseconds((long)Interval)
				                 };

			this.accelerometer.CurrentValueChanged += AccelerometerOnCurrentValueChanged;
			this.accelerometer.Start();
		}

		/// <summary>
		/// Stops this instance.
		/// </summary>
		partial void Stop()
		{
			if (this.accelerometer != null)
			{
				this.accelerometer.CurrentValueChanged -= AccelerometerOnCurrentValueChanged;
				this.accelerometer.Stop();
				this.accelerometer = null;
			}
		}

		/// <summary>
		/// Accelerometers the on current value changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="sensorReadingEventArgs">The sensor reading event arguments.</param>
		private void AccelerometerOnCurrentValueChanged(
			object sender,
			SensorReadingEventArgs<AccelerometerReading> sensorReadingEventArgs)
		{
			LatestReading = sensorReadingEventArgs.SensorReading.Acceleration.AsVector3();
			readingAvailable.Invoke(sender, LatestReading);
		}
	}
}