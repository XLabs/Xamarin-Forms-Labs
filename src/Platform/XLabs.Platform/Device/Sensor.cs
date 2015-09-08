namespace XLabs.Platform.Device
{
	using System;

	/// <summary>
	/// Class DeviceSensor.
	/// </summary>
	public abstract class DeviceSensor : ISensor
	{
		/// <summary>
		/// Occurs when [reading available].
		/// </summary>
		protected event EventHandler<EventArgs<Vector3>> readingAvailable;

		/// <summary>
		/// Occurs when [reading available].
		/// </summary>
		public event EventHandler<EventArgs<Vector3>> ReadingAvailable
		{
			add
			{
				if (readingAvailable == null)
				{
					Start();
				}
				readingAvailable += value;
			}
			remove
			{
				readingAvailable -= value;
				if (readingAvailable == null)
				{
					Stop();
				}
			}
		}

		/// <summary>
		/// Gets the latest reading.
		/// </summary>
		/// <value>The latest reading.</value>
		public Vector3 LatestReading
		{
			get;
			protected set;
		}

		/// <summary>
		/// Starts this instance.
		/// </summary>
		protected abstract void Start();

		/// <summary>
		/// Stops this instance.
		/// </summary>
		protected abstract void Stop();

		/// <summary>
		/// Gets or sets the interval.
		/// </summary>
		/// <value>The interval.</value>
		public abstract AccelerometerInterval Interval { get; set;}
	}
}
