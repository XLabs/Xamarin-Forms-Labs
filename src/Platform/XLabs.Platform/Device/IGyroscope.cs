namespace XLabs.Platform.Device
{
	using System;

	/// <summary>
	/// Interface IGyroscope
	/// </summary>
	public interface IGyroscope
	{
		/// <summary>
		/// Occurs when [reading available].
		/// </summary>
		event EventHandler<EventArgs<Vector3>> ReadingAvailable;

		/// <summary>
		/// Gets the latest reading vector
		/// </summary>
		/// <value>Rotation values in radians per second</value>
		Vector3 LatestReading { get; }

		/// <summary>
		/// Gets or sets the interval.
		/// </summary>
		/// <value>The interval.</value>
		AccelerometerInterval Interval { get; set; }
	}
}
