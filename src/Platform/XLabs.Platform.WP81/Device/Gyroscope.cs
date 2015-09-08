namespace XLabs.Platform.Device
{
    using Windows.Devices.Sensors;

    /// <summary>
    /// Class Gyroscope.
    /// </summary>
    public partial class Gyroscope
    {
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
            var gyroscope = Gyrometer.GetDefault();

            if (gyroscope == null) return;

            gyroscope.ReportInterval = (uint)Interval;

            gyroscope.ReadingChanged += GyroscopeReadingChanged;
        }

        /// <summary>
        /// Gyroscope reading has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The gyroscope event arguments.</param>
        private void GyroscopeReadingChanged(Gyrometer sender, GyrometerReadingChangedEventArgs args)
        {
            LatestReading = args.Reading.AsVector3();
            readingAvailable.Invoke(this, this.LatestReading);
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        partial void Stop()
        {
            var gyroscope = Gyrometer.GetDefault();

            if (gyroscope != null)
            {
                gyroscope.ReadingChanged -= GyroscopeReadingChanged;
            }
        }
    }
}