namespace XLabs.Platform.Device
{
	using CoreMotion;
	using Foundation;

	/// <summary>
	/// Class Gyroscope.
	/// </summary>
	public partial class Gyroscope
	{
		/// <summary>
		/// The _motion manager
		/// </summary>
		private CMMotionManager _motionManager;

		/// <summary>
		/// Gets or sets the interval.
		/// </summary>
		/// <value>The interval.</value>
		public AccelerometerInterval Interval { get; set; }

		/// <summary>
		/// Gets a value indicating whether this instance is supported.
		/// </summary>
		/// <value><c>true</c> if this instance is supported; otherwise, <c>false</c>.</value>
		public static bool IsSupported
		{
			get
			{
				return new CMMotionManager().GyroAvailable;
			}
		}

		/// <summary>
		/// Starts this instance.
		/// </summary>
		partial void Start()
		{
			_motionManager = new CMMotionManager();
			_motionManager.GyroUpdateInterval = (long)Interval / 1000;
			_motionManager.StartGyroUpdates(NSOperationQueue.MainQueue, OnUpdate);
		}

		/// <summary>
		/// Stops this instance.
		/// </summary>
		partial void Stop()
		{
			_motionManager.StopGyroUpdates();
			_motionManager = null;
		}

		/// <summary>
		/// Called when [update].
		/// </summary>
		/// <param name="gyroData">The gyro data.</param>
		/// <param name="error">The error.</param>
		private void OnUpdate(CMGyroData gyroData, NSError error)
		{
			if (error != null)
			{
				this.readingAvailable.Invoke(
					this,
					new Vector3(gyroData.RotationRate.x, gyroData.RotationRate.y, gyroData.RotationRate.z));
			}
		}
	}
}