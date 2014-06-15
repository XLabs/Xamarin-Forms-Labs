using System;

namespace XForms.Toolkit.Services.Geolocation
{
	public class Position
	{
		public Position()
		{
		}

		public Position (Position position)
		{
			if (position == null)
				throw new ArgumentNullException ("position");

			Timestamp = position.Timestamp;
			Latitude = position.Latitude;
			Longitude = position.Longitude;
			Altitude = position.Altitude;
			AltitudeAccuracy = position.AltitudeAccuracy;
			Accuracy = position.Accuracy;
			Heading = position.Heading;
			Speed = position.Speed;
		}

		public DateTimeOffset Timestamp
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the latitude.
		/// </summary>
		public double Latitude
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the longitude.
		/// </summary>
		public double Longitude
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the altitude in meters relative to sea level.
		/// </summary>
		public double? Altitude
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the potential position error radius in meters.
		/// </summary>
		public double? Accuracy
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the potential altitude error range in meters.
		/// </summary>
		/// <remarks>
		/// Not supported on Android, will always read 0.
		/// </remarks>
		public double? AltitudeAccuracy
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the heading in degrees relative to true North.
		/// </summary>
		public double? Heading
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the speed in meters per second.
		/// </summary>
		public double? Speed
		{
			get;
			set;
		}
	}

	public class PositionEventArgs
		: EventArgs
	{
		public PositionEventArgs (Position position)
		{
			if (position == null)
				throw new ArgumentNullException ("position");

			Position = position;
		}

		public Position Position
		{
			get;
			private set;
		}
	}

	public class GeolocationException
		: Exception
	{
		public GeolocationException (GeolocationError error)
			: base ("A geolocation error occured: " + error)
		{
			if (!Enum.IsDefined (typeof (GeolocationError), error))
				throw new ArgumentException ("error is not a valid GelocationError member", "error");

			Error = error;
		}

		public GeolocationException (GeolocationError error, Exception innerException)
			: base ("A geolocation error occured: " + error, innerException)
		{
			if (!Enum.IsDefined (typeof (GeolocationError), error))
				throw new ArgumentException ("error is not a valid GelocationError member", "error");

			Error = error;
		}

		public GeolocationError Error
		{
			get;
			private set;
		}
	}

	public class PositionErrorEventArgs
		: EventArgs
	{
		public PositionErrorEventArgs (GeolocationError error)
		{
			Error = error;
		}

		public GeolocationError Error
		{
			get;
			private set;
		}
	}

	public enum GeolocationError
	{
		/// <summary>
		/// The provider was unable to retrieve any position data.
		/// </summary>
		PositionUnavailable,

		/// <summary>
		/// The app is not, or no longer, authorized to receive location data.
		/// </summary>
		Unauthorized
	}
}

