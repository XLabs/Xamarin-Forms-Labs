// ***********************************************************************
// Assembly         : XLabs.Platform
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="IGeolocator.cs" company="XLabs Team">
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
using System.Threading;
using System.Threading.Tasks;

namespace XLabs.Platform.Services.Geolocation
{
	/// <summary>
	///     Interface IGeolocator
	/// </summary>
	public interface IGeolocator
	{
		/// <summary>
		///     Gets or sets the desired accuracy.
		/// </summary>
		/// <value>The desired accuracy.</value>
		double DesiredAccuracy { get; set; }

		/// <summary>
		///     Gets a value indicating whether this instance is listening.
		/// </summary>
		/// <value><c>true</c> if this instance is listening; otherwise, <c>false</c>.</value>
		bool IsListening { get; }

		/// <summary>
		///     Gets a value indicating whether [supports heading].
		/// </summary>
		/// <value><c>true</c> if [supports heading]; otherwise, <c>false</c>.</value>
		bool SupportsHeading { get; }

		/// <summary>
		///     Gets a value indicating whether this instance is geolocation available.
		/// </summary>
		/// <value><c>true</c> if this instance is geolocation available; otherwise, <c>false</c>.</value>
		bool IsGeolocationAvailable { get; }

		/// <summary>
		///     Gets a value indicating whether this instance is geolocation enabled.
		/// </summary>
		/// <value><c>true</c> if this instance is geolocation enabled; otherwise, <c>false</c>.</value>
		bool IsGeolocationEnabled { get; }

		/// <summary>
		///     Occurs when [position error].
		/// </summary>
		event EventHandler<PositionErrorEventArgs> PositionError;

		/// <summary>
		///     Occurs when [position changed].
		/// </summary>
		event EventHandler<PositionEventArgs> PositionChanged;

		/// <summary>
		///     Gets the position asynchronous.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		Task<Position> GetPositionAsync(int timeout);

		/// <summary>
		///     Gets the position asynchronous.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <param name="includeHeading">if set to <c>true</c> [include heading].</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		Task<Position> GetPositionAsync(int timeout, bool includeHeading);

		/// <summary>
		///     Gets the position asynchronous.
		/// </summary>
		/// <param name="cancelToken">The cancel token.</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		Task<Position> GetPositionAsync(CancellationToken cancelToken);

		/// <summary>
		///     Gets the position asynchronous.
		/// </summary>
		/// <param name="cancelToken">The cancel token.</param>
		/// <param name="includeHeading">if set to <c>true</c> [include heading].</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		Task<Position> GetPositionAsync(CancellationToken cancelToken, bool includeHeading);

		/// <summary>
		///     Gets the position asynchronous.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <param name="cancelToken">The cancel token.</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		Task<Position> GetPositionAsync(int timeout, CancellationToken cancelToken);

		/// <summary>
		///     Gets the position asynchronous.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <param name="cancelToken">The cancel token.</param>
		/// <param name="includeHeading">if set to <c>true</c> [include heading].</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		Task<Position> GetPositionAsync(int timeout, CancellationToken cancelToken, bool includeHeading);

		/// <summary>
		///     Start listening to location changes
		/// </summary>
		/// <param name="minTime">Minimum interval in milliseconds</param>
		/// <param name="minDistance">Minimum distance in meters</param>
		void StartListening(uint minTime, double minDistance);

		/// <summary>
		///     Start listening to location changes
		/// </summary>
		/// <param name="minTime">Minimum interval in milliseconds</param>
		/// <param name="minDistance">Minimum distance in meters</param>
		/// <param name="includeHeading">Include heading information</param>
		void StartListening(uint minTime, double minDistance, bool includeHeading);

		/// <summary>
		///     Stop listening to location changes
		/// </summary>
		void StopListening();
	}
}