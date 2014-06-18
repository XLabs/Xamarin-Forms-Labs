using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.Toolkit.Helpers;

namespace XForms.Toolkit
{
    public partial class Gyroscope : IGyroscope
    {
        private event EventHandler<EventArgs<Vector3>> readingAvailable;

        public Gyroscope()
        {
            this.Interval = AccelerometerInterval.Ui;
        }

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

        public Vector3 LatestReading
        {
            get;
            private set;
        }

        partial void Start();

        partial void Stop();
    }
}
