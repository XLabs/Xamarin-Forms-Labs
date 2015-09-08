namespace XLabs.Platform.Device
{
    using Windows.Devices.Sensors;
    using AccelerometerSensor = Windows.Devices.Sensors.Accelerometer;

    /// <summary>
    /// Class Accelerometer.
    /// </summary>
    public partial class Accelerometer
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
            var accelerometer = AccelerometerSensor.GetDefault();

            if (accelerometer == null) return;

            accelerometer.ReportInterval = (uint) this.Interval;

            accelerometer.ReadingChanged += AccelerometerOnReadingChanged;
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        partial void Stop()
        {
            var accelerometer = AccelerometerSensor.GetDefault();

            if (accelerometer != null)
            {
                accelerometer.ReadingChanged -= AccelerometerOnReadingChanged;
            }
        }

        private void AccelerometerOnReadingChanged(AccelerometerSensor sender, AccelerometerReadingChangedEventArgs args)
        {
            LatestReading = args.Reading.AsVector3();
            readingAvailable.Invoke(sender, LatestReading);
        }
    }
}