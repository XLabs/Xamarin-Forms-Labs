using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.Toolkit.Helpers;

namespace XForms.Toolkit
{
    public interface IGyroscope
    {
        event EventHandler<EventArgs<Vector3>> ReadingAvailable;

        /// <summary>
        /// Gets the latest reading vector
        /// </summary>
        /// <value>Rotation values in radians per second</value>
        Vector3 LatestReading { get; }

        AccelerometerInterval Interval { get; set; }
    }
}
