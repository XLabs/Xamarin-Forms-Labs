// ***********************************************************************
// Assembly         : XLabs.Platform.WinUniversal
// Author           : XLabs Team
// Created          : 01-01-2016
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="CoordinateExtensions.cs" company="XLabs Team">
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

using Windows.Devices.Geolocation;

namespace XLabs.Platform.Services.Geolocation
{
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