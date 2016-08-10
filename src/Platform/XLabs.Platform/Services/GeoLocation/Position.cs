// ***********************************************************************
// Assembly         : XLabs.Platform
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Position.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System;

namespace XLabs.Platform.Services.Geolocation
{
	/// <summary>
	/// Class Position.
	/// </summary>
	public class Position
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Position" /> class.
		/// </summary>
		public Position()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Position" /> class.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <exception cref="System.ArgumentNullException">position</exception>
		public Position(Position position)
		{
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}

			Timestamp = position.Timestamp;
			Latitude = position.Latitude;
			Longitude = position.Longitude;
			Altitude = position.Altitude;
			AltitudeAccuracy = position.AltitudeAccuracy;
			Accuracy = position.Accuracy;
			Heading = position.Heading;
			Speed = position.Speed;
		}

		/// <summary>
		/// Gets or sets the timestamp.
		/// </summary>
		/// <value>The timestamp.</value>
		public DateTimeOffset Timestamp { get; set; }

		/// <summary>
		/// Gets or sets the latitude.
		/// </summary>
		/// <value>The latitude.</value>
		public double Latitude { get; set; }

		/// <summary>
		/// Gets or sets the longitude.
		/// </summary>
		/// <value>The longitude.</value>
		public double Longitude { get; set; }

		/// <summary>
		/// Gets or sets the altitude in meters relative to sea level.
		/// </summary>
		/// <value>The altitude.</value>
		public double? Altitude { get; set; }

		/// <summary>
		/// Gets or sets the potential position error radius in meters.
		/// </summary>
		/// <value>The accuracy.</value>
		public double? Accuracy { get; set; }

		/// <summary>
		/// Gets or sets the potential altitude error range in meters.
		/// </summary>
		/// <value>The altitude accuracy.</value>
		/// <remarks>Not supported on Android, will always read 0.</remarks>
		public double? AltitudeAccuracy { get; set; }

		/// <summary>
		/// Gets or sets the heading in degrees relative to true North.
		/// </summary>
		/// <value>The heading.</value>
		public double? Heading { get; set; }

		/// <summary>
		/// Gets or sets the speed in meters per second.
		/// </summary>
		/// <value>The speed.</value>
		public double? Speed { get; set; }
	}

	/// <summary>
	/// Class PositionEventArgs.
	/// </summary>
	public class PositionEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PositionEventArgs"/> class.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <exception cref="System.ArgumentNullException">position</exception>
		public PositionEventArgs(Position position)
		{
			if (position == null)
			{
				throw new ArgumentNullException("position");
			}

			Position = position;
		}

		/// <summary>
		/// Gets the position.
		/// </summary>
		/// <value>The position.</value>
		public Position Position { get; private set; }
	}

	/// <summary>
	/// Class GeolocationException.
	/// </summary>
	public class GeolocationException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GeolocationException"/> class.
		/// </summary>
		/// <param name="error">The error.</param>
		/// <exception cref="System.ArgumentException">error is not a valid GelocationError member;error</exception>
		public GeolocationException(GeolocationError error)
			: base("A geolocation error occured: " + error)
		{
			if (!Enum.IsDefined(typeof(GeolocationError), error))
			{
				throw new ArgumentException("error is not a valid GelocationError member", "error");
			}

			Error = error;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GeolocationException"/> class.
		/// </summary>
		/// <param name="error">The error.</param>
		/// <param name="innerException">The inner exception.</param>
		/// <exception cref="System.ArgumentException">error is not a valid GelocationError member;error</exception>
		public GeolocationException(GeolocationError error, Exception innerException)
			: base("A geolocation error occured: " + error, innerException)
		{
			if (!Enum.IsDefined(typeof(GeolocationError), error))
			{
				throw new ArgumentException("error is not a valid GelocationError member", "error");
			}

			Error = error;
		}

		/// <summary>
		/// Gets the error.
		/// </summary>
		/// <value>The error.</value>
		public GeolocationError Error { get; private set; }
	}

	/// <summary>
	/// Class PositionErrorEventArgs.
	/// </summary>
	public class PositionErrorEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PositionErrorEventArgs"/> class.
		/// </summary>
		/// <param name="error">The error.</param>
		public PositionErrorEventArgs(GeolocationError error)
		{
			Error = error;
		}

		/// <summary>
		/// Gets the error.
		/// </summary>
		/// <value>The error.</value>
		public GeolocationError Error { get; private set; }
	}

	/// <summary>
	/// Enum GeolocationError
	/// </summary>
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