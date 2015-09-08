namespace XLabs.Platform.Services.Geolocation
{
	using System;

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
		/// <param name="a">Location a</param>
		/// <param name="b">Location b</param>
		/// <returns>The <see cref="System.Double" />The distance in meters</returns>
		public static double DistanceFrom(this Position a, Position b)
		{
			/*
			double distance = Math.Acos(
				(Math.Sin(a.Latitude) * Math.Sin(b.Latitude)) +
				(Math.Cos(a.Latitude) * Math.Cos(b.Latitude))
				* Math.Cos(b.Longitude - a.Longitude));
			 * */

			var dLat = b.Latitude.DegreesToRadians() - a.Latitude.DegreesToRadians();
			var dLon = b.Longitude.DegreesToRadians() - a.Longitude.DegreesToRadians();

			var a1 = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(a.Latitude.DegreesToRadians()) * Math.Cos(b.Latitude.DegreesToRadians()) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
			var distance = 2 * Math.Atan2(Math.Sqrt(a1), Math.Sqrt(1 - a1));

			return EquatorRadius * distance;
		}

		/// <summary>
		/// Calculates bearing between start and stop.
		/// </summary>
		/// <param name="start">Start coordinates.</param>
		/// <param name="stop">Stop coordinates.</param>
		/// <returns>The <see cref="System.Double" />.</returns>
		public static double BearingFrom(this Position start, Position stop)
		{
			var deltaLon = stop.Longitude - start.Longitude;
			var cosStop = Math.Cos(stop.Latitude);
			return Math.Atan2(
				(Math.Cos(start.Latitude) * Math.Sin(stop.Latitude)) -
				(Math.Sin(start.Latitude) * cosStop * Math.Cos(deltaLon)),
				Math.Sin(deltaLon) * cosStop);
		}

		/// <summary>
		/// Radianses to degrees.
		/// </summary>
		/// <param name="rad">The RAD.</param>
		/// <returns>System.Double.</returns>
		public static double RadiansToDegrees(this double rad)
		{
			return 180.0 * rad / Math.PI;
		}

		/// <summary>
		/// Degreeses to radians.
		/// </summary>
		/// <param name="deg">The deg.</param>
		/// <returns>System.Double.</returns>
		public static double DegreesToRadians(this double deg)
		{
			return Math.PI * deg / 180.0;
		}
	}
}
