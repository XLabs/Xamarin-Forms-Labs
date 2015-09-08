namespace XLabs.Platform.Device
{
	using System;

	/// <summary>
	/// Interface ISensor
	/// </summary>
	public interface ISensor
	{
		/// <summary>
		/// Occurs when [reading available].
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
}
