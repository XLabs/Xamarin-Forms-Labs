using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using XForms.Toolkit.Helpers;

namespace XForms.Toolkit
{
    public partial class Accelerometer
    {
        private AccelerometerInterval interval = AccelerometerInterval.Ui;

        public AccelerometerInterval Interval
        {
            get { return this.interval; }
            set
            {
                if (this.interval != value)
                {
                    this.interval = value;
                    UIAccelerometer.SharedAccelerometer.UpdateInterval = ((long)this.interval) / 1000d;
                }
            }
        }

        partial void Start()
        {
            UIAccelerometer.SharedAccelerometer.Acceleration += HandleAcceleration;
        }

        partial void Stop()
        {
            UIAccelerometer.SharedAccelerometer.Acceleration -= HandleAcceleration;
        }

        private void HandleAcceleration(object sender, UIAccelerometerEventArgs e)
        {
            readingAvailable.Invoke(sender, new Vector3(e.Acceleration.X, e.Acceleration.Y, e.Acceleration.Z));
        }
    }
}