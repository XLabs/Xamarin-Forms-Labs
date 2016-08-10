// ***********************************************************************
// Assembly         : XLabs.Platform.WinUniversal
// Author           : XLabs Team
// Created          : 01-01-2016
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Geolocator.cs" company="XLabs Team">
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
using Windows.Devices.Geolocation;

namespace XLabs.Platform.Services.Geolocation
{
	/// <summary>
	/// The geolocator implements <see cref="Windows.Devices.Geolocation.IGeolocator" /> interface for Windows Phone 8.
	/// </summary>
	public class Geolocator : IGeolocator
	{
		/// <summary>
		/// The _locator
		/// </summary>
		private readonly Windows.Devices.Geolocation.Geolocator _locator;

		/// <summary>
		/// Initializes a new instance of the <see cref="Geolocator" /> class.
		/// </summary>
		public Geolocator()
		{
			_locator = new Windows.Devices.Geolocation.Geolocator();
		}

		/// <summary>
		/// Locators the position changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="PositionChangedEventArgs"/> instance containing the event data.</param>
		private void LocatorPositionChanged(Windows.Devices.Geolocation.Geolocator sender, PositionChangedEventArgs args)
		{
			PositionChanged.TryInvoke(sender, new PositionEventArgs(args.Position.Coordinate.GetPosition()));
		}

		/// <summary>
		/// Locators the status changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="StatusChangedEventArgs"/> instance containing the event data.</param>
		private void LocatorStatusChanged(Windows.Devices.Geolocation.Geolocator sender, StatusChangedEventArgs args)
		{
			switch (args.Status)
			{
				case PositionStatus.Disabled:
					PositionError.TryInvoke(sender, new PositionErrorEventArgs(GeolocationError.Unauthorized));
					break;
				case PositionStatus.Initializing:
					break;
				case PositionStatus.NoData:
					PositionError.TryInvoke(sender, new PositionErrorEventArgs(GeolocationError.PositionUnavailable));
					break;
				case PositionStatus.NotInitialized:
					IsListening = false;
					break;
				case PositionStatus.Ready:
					IsListening = true;
					break;
			}
		}

		#region IGeolocator Members

		/// <summary>
		/// Occurs when [position error].
		/// </summary>
		public event EventHandler<PositionErrorEventArgs> PositionError;

		/// <summary>
		/// Occurs when [position changed].
		/// </summary>
		public event EventHandler<PositionEventArgs> PositionChanged;

		/// <summary>
		/// Gets or sets the desired accuracy.
		/// </summary>
		/// <value>The desired accuracy.</value>
		public double DesiredAccuracy
		{
			get
			{
				return (_locator.DesiredAccuracy == PositionAccuracy.Default) ? 100 : 10;
			}

			set
			{
				_locator.DesiredAccuracy = (value > 10) ? PositionAccuracy.Default : PositionAccuracy.High;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is listening.
		/// </summary>
		/// <value><c>true</c> if this instance is listening; otherwise, <c>false</c>.</value>
		public bool IsListening { get; private set; }

		/// <summary>
		/// Gets a value indicating whether [supports heading].
		/// </summary>
		/// <value><c>true</c> if [supports heading]; otherwise, <c>false</c>.</value>
		public bool SupportsHeading
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is geolocation available.
		/// </summary>
		/// <value><c>true</c> if this instance is geolocation available; otherwise, <c>false</c>.</value>
		public bool IsGeolocationAvailable
		{
			get
			{
				return _locator.LocationStatus != PositionStatus.NotAvailable;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is geolocation enabled.
		/// </summary>
		/// <value><c>true</c> if this instance is geolocation enabled; otherwise, <c>false</c>.</value>
		public bool IsGeolocationEnabled
		{
			get
			{
				return _locator.LocationStatus != PositionStatus.Disabled;
			}
		}

		/// <summary>
		/// get position as an asynchronous operation.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		public async Task<Position> GetPositionAsync(int timeout)
		{
			var position = await _locator.GetGeopositionAsync(TimeSpan.MaxValue, TimeSpan.FromMilliseconds(timeout));
			return position.Coordinate.GetPosition();
		}

		/// <summary>
		/// get position as an asynchronous operation.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <param name="includeHeading">if set to <c>true</c> [include heading].</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		public async Task<Position> GetPositionAsync(int timeout, bool includeHeading)
		{
			return await GetPositionAsync(timeout);
		}

		/// <summary>
		/// get position as an asynchronous operation.
		/// </summary>
		/// <param name="cancelToken">The cancel token.</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		public async Task<Position> GetPositionAsync(CancellationToken cancelToken)
		{
			var t = _locator.GetGeopositionAsync().AsTask();

			while (t.Status == TaskStatus.Running)
			{
				cancelToken.ThrowIfCancellationRequested();
			}

			var position = await t;

			return position.Coordinate.GetPosition();
		}

		/// <summary>
		/// Gets the position asynchronous.
		/// </summary>
		/// <param name="cancelToken">The cancel token.</param>
		/// <param name="includeHeading">if set to <c>true</c> [include heading].</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		public Task<Position> GetPositionAsync(CancellationToken cancelToken, bool includeHeading)
		{
			return GetPositionAsync(cancelToken);
		}

		/// <summary>
		/// Gets the position asynchronous.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <param name="cancelToken">The cancel token.</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		public Task<Position> GetPositionAsync(int timeout, CancellationToken cancelToken)
		{
			var t = GetPositionAsync(timeout);

			while (t.Status == TaskStatus.Running)
			{
				cancelToken.ThrowIfCancellationRequested();
			}

			return t;
		}

		/// <summary>
		/// Gets the position asynchronous.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <param name="cancelToken">The cancel token.</param>
		/// <param name="includeHeading">if set to <c>true</c> [include heading].</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		public Task<Position> GetPositionAsync(int timeout, CancellationToken cancelToken, bool includeHeading)
		{
			var t = GetPositionAsync(timeout, includeHeading);

			while (t.Status == TaskStatus.Running)
			{
				cancelToken.ThrowIfCancellationRequested();
			}

			return t;
		}

		/// <summary>
		/// Start listening to location changes
		/// </summary>
		/// <param name="minTime">Minimum interval in milliseconds</param>
		/// <param name="minDistance">Minimum distance in meters</param>
		public void StartListening(uint minTime, double minDistance)
		{
			_locator.MovementThreshold = minDistance;
			_locator.ReportInterval = minTime;
			_locator.PositionChanged += LocatorPositionChanged;
			_locator.StatusChanged += LocatorStatusChanged;
		}

		/// <summary>
		/// Start listening to location changes
		/// </summary>
		/// <param name="minTime">Minimum interval in milliseconds</param>
		/// <param name="minDistance">Minimum distance in meters</param>
		/// <param name="includeHeading">Include heading information</param>
		public void StartListening(uint minTime, double minDistance, bool includeHeading)
		{
			StartListening(minTime, minDistance);
		}

		/// <summary>
		/// Stop listening to location changes
		/// </summary>
		public void StopListening()
		{
			_locator.PositionChanged -= LocatorPositionChanged;
			_locator.StatusChanged -= LocatorStatusChanged;
		}

		#endregion
	}
}