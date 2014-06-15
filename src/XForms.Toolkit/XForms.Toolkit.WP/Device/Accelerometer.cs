using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.Toolkit
{
    using Microsoft.Devices.Sensors;
    using XForms.Toolkit.Helpers;
    using Meter = Microsoft.Devices.Sensors.Accelerometer;

    public partial class Accelerometer
    {
        private Meter accelerometer;

        public AccelerometerInterval Interval { get; set; }

        partial void Start()
        {
            this.accelerometer = new Meter { TimeBetweenUpdates = TimeSpan.FromMilliseconds((long)this.Interval) };

            this.accelerometer.CurrentValueChanged += this.AccelerometerOnCurrentValueChanged;
            this.accelerometer.Start();
        }

        partial void Stop()
        {
            if (this.accelerometer != null)
            {
                this.accelerometer.CurrentValueChanged -= this.AccelerometerOnCurrentValueChanged;
                this.accelerometer.Stop();
                this.accelerometer = null;
            }
        }

        private void AccelerometerOnCurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> sensorReadingEventArgs)
        {
            this.LatestReading = sensorReadingEventArgs.SensorReading.Acceleration.AsVector3();
            readingAvailable.Invoke(sender, this.LatestReading);
        }
    }
}
