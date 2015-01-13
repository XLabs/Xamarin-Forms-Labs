namespace XLabs.Platform.Device
{
	using UIKit;

	/// <summary>
	/// The accelerometer.
	/// </summary>
	public partial class Accelerometer
	{
		/// <summary>
		/// The _interval
		/// </summary>
		private AccelerometerInterval _interval = AccelerometerInterval.Ui;

		/// <summary>
		/// Gets or sets the interval.
		/// </summary>
		/// <value>The interval.</value>
		public AccelerometerInterval Interval
		{
			get
			{
				return _interval;
			}
			set
			{
				if (_interval != value)
				{
					_interval = value;
					UIAccelerometer.SharedAccelerometer.UpdateInterval = ((long)_interval) / 1000d;
				}
			}
		}

		/// <summary>
		/// Starts this instance.
		/// </summary>
		partial void Start()
		{
			UIAccelerometer.SharedAccelerometer.Acceleration += HandleAcceleration;
		}

		/// <summary>
		/// Stops this instance.
		/// </summary>
		partial void Stop()
		{
			UIAccelerometer.SharedAccelerometer.Acceleration -= HandleAcceleration;
		}

		/// <summary>
		/// Handles the acceleration.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="UIAccelerometerEventArgs"/> instance containing the event data.</param>
		private void HandleAcceleration(object sender, UIAccelerometerEventArgs e)
		{
			readingAvailable.Invoke(sender, new Vector3(e.Acceleration.X, e.Acceleration.Y, e.Acceleration.Z));
		}
	}
}