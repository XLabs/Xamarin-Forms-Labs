using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.Toolkit
{
    public partial class Gyroscope
    {
        Microsoft.Devices.Sensors.Gyroscope gyroscope;

        public AccelerometerInterval Interval { get; set; }

        partial void Start()
        {
            this.gyroscope = new Microsoft.Devices.Sensors.Gyroscope()
            {
                TimeBetweenUpdates = TimeSpan.FromMilliseconds((long)this.Interval)
            };

            this.gyroscope.CurrentValueChanged += gyroscope_CurrentValueChanged;
            this.gyroscope.Start();
        }

        partial void Stop()
        {
            this.gyroscope.CurrentValueChanged -= gyroscope_CurrentValueChanged;
            this.gyroscope.Stop();
            this.gyroscope = null;
        }

        void gyroscope_CurrentValueChanged(object sender, Microsoft.Devices.Sensors.SensorReadingEventArgs<Microsoft.Devices.Sensors.GyroscopeReading> e)
        {
            if (this.gyroscope.IsDataValid)
            {
                this.LatestReading = e.SensorReading.RotationRate.AsVector3();
                this.readingAvailable.Invoke(this, this.LatestReading);
            }
        }
    }
}
