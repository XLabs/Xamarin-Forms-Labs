using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.Toolkit.Services.Geolocation;

namespace XForms.Toolkit.Services.GeoLocation
{
    /// <summary>
    /// Position extensions
    /// </summary>
    public static class PositionExtensions
    {
        /// <summary>
        /// The equator radius.
        /// </summary>
        public const int EquatorRadius = 6378137;

        /// <summary>
        /// Calculates distance between two locations.
        /// </summary>
        /// <returns>The <see cref="System.Double"/>The distance in meters</returns>
        /// <param name="a">Location a</param>
        /// <param name="b">Location b</param>
        public static double DistanceFrom(this Position a, Position b)
        {
            double distance = Math.Acos(
                (Math.Sin(a.Latitude) * Math.Sin(b.Latitude)) +
                (Math.Cos(a.Latitude) * Math.Cos(b.Latitude))
                * Math.Cos(b.Longitude - a.Longitude));

            return EquatorRadius * distance;
        }

        /// <summary>
        /// Calculates bearing between start and stop.
        /// </summary>
        /// <returns>The <see cref="System.Double"/>.</returns>
        /// <param name="start">Start coordinates.</param>
        /// <param name="stop">Stop coordinates.</param>
        public static double BearingFrom(this Position start, Position stop)
        {
            var deltaLon = stop.Longitude - start.Longitude;
            var cosStop = Math.Cos(stop.Latitude);
            return Math.Atan2(
                (Math.Cos(start.Latitude) * Math.Sin(stop.Latitude)) -
                (Math.Sin(start.Latitude) * cosStop * Math.Cos(deltaLon)),
                Math.Sin(deltaLon) * cosStop);
        }
    }
}
