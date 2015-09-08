namespace XLabs.Platform.Device
{
	using System;

	/// <summary>
	/// The Accelerometer interface.
	/// </summary>
	public interface IAccelerometer
	{
		/// <summary>
		/// The reading available event handler.
		/// </summary>
		event EventHandler<EventArgs<Vector3>> ReadingAvailable;

		/// <summary>
		/// Gets the latest reading.
		/// </summary>
		/// <value>The latest reading.</value>
		Vector3 LatestReading { get; }

		/// <summary>
		/// Gets or sets the interval.
		/// </summary>
		/// <value>The interval.</value>
		AccelerometerInterval Interval { get; set; }
	}

	/// <summary>
	/// The accelerometer interval.
	/// </summary>
	public enum AccelerometerInterval
	{
		/// <summary>
		/// The fastest interval.
		/// </summary>
		Fastest = 1,

		/// <summary>
		/// The game interval, approximately 20ms.
		/// </summary>
		Game = 20,

		/// <summary>
		/// The UI interval, approximately 70ms.
		/// </summary>
		Ui = 70,

		/// <summary>
		/// The normal interval, approximately 200ms.
		/// </summary>
		Normal = 200
	}
}
