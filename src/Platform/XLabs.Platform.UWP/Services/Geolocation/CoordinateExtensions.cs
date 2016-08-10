namespace XLabs.Platform.Services.Geolocation
{
	using Windows.Devices.Geolocation;

	/// <summary>
	/// The coordinate extensions for Windows Phone.
	/// </summary>
	public static class CoordinateExtensions
	{
		/// <summary>
		/// Converts <see cref="Geocoordinate" /> class into <see cref="Position" />.
		/// </summary>
		/// <param name="geocoordinate">The Geocoordinate.</param>
		/// <returns>The <see cref="Position" />.</returns>
		public static Position GetPosition(this Geocoordinate geocoordinate)
		{
			return new Position
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