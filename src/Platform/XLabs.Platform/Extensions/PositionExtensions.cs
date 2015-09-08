namespace XLabs.Platform
{
    using System;
    using System.Globalization;

    using Services.Geolocation;

    /// <summary>
    /// Class PositionExtensions.
    /// </summary>
    public static class PositionExtensions
    {
        /// <summary>
        /// To the URI.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>Uri.</returns>
        public static Uri ToUri(this Position position)
        {
            return new Uri(string.Format(new CultureInfo("en-US"), "geo:{0},{1}", position.Latitude, position.Longitude));
        }

        /// <summary>
        /// To the bing maps.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>Uri.</returns>
        public static Uri ToBingMaps(this Position position)
        {
            return new Uri(string.Format(new CultureInfo("en-US"), "http://www.bing.com/maps/?q={0},{1}", position.Latitude, position.Longitude));
            //return new Uri(string.Format(new CultureInfo("en-US"), "maps:{0} {1}", position.Latitude, position.Longitude));
        }

        /// <summary>
        /// To the google maps.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>Uri.</returns>
        public static Uri ToGoogleMaps(this Position position)
        {
            return new Uri(string.Format(new CultureInfo("en-US"), "http://maps.google.com/?q={0},{1}", position.Latitude, position.Longitude));
        }

        /// <summary>
        /// To the apple maps.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>Uri.</returns>
        public static Uri ToAppleMaps(this Position position)
        {
            return new Uri(string.Format(new CultureInfo("en-US"), "http://maps.apple.com/?q={0},{1}", position.Latitude, position.Longitude));
        }

        /// <summary>
        /// Drives to link.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="destination">The destination.</param>
        /// <returns>Uri.</returns>
        public static Uri DriveToLink(this Position position, string destination = "Driving instructions")
        {
            return new Uri(string.Format(
                new CultureInfo("en-US"), 
                "ms-drive-to:?destination.latitude={0}&destination.longitude={1}&destination.name={2}",
                position.Latitude,
                position.Longitude,
                destination
                ));
        }
    }
}
