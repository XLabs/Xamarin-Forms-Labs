using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace XForms.Toolkit.Services.Geolocation
{
    public static class CoordinateExtensions
    {
        public static Position GetPosition(this Geocoordinate geocoordinate)
        {
            return new Position()
            {
                Accuracy = geocoordinate.Accuracy,
                Altitude = geocoordinate.Altitude,
                Heading = geocoordinate.Heading,
                Latitude = geocoordinate.Latitude,
                Longitude = geocoordinate.Longitude,
                Speed = geocoordinate.Speed,
                Timestamp = geocoordinate.Timestamp
            };
        }
    }
}
