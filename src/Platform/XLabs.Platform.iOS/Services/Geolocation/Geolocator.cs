// ***********************************************************************
// Assembly         : XLabs.Platform.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
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
using CoreLocation;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace XLabs.Platform.Services.Geolocation
{
	/// <summary>
	/// Class Geolocator.
	/// </summary>
	public class Geolocator : IGeolocator
	{
		/// <summary>
		/// The _position
		/// </summary>
		private Position _position;

		/// <summary>
		/// The _manager
		/// </summary>
		private readonly CLLocationManager _manager;

		/// <summary>
		/// Initializes a new instance of the <see cref="Geolocator"/> class.
		/// </summary>
		public Geolocator()
		{
			_manager = GetManager();
			_manager.AuthorizationChanged += OnAuthorizationChanged;
			_manager.Failed += OnFailed;

		
			if (_manager.RespondsToSelector(new Selector("requestWhenInUseAuthorization")))
			{
				_manager.RequestWhenInUseAuthorization();
			}

			if (UIDevice.CurrentDevice.CheckSystemVersion(6, 0))
			{
				_manager.LocationsUpdated += OnLocationsUpdated;
			}
			else
			{
				_manager.UpdatedLocation += OnUpdatedLocation;
			}

			_manager.UpdatedHeading += OnUpdatedHeading;
		}

		/// <summary>
		/// Gets or sets the desired accuracy.
		/// </summary>
		/// <value>The desired accuracy.</value>
		public double DesiredAccuracy { get; set; }

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
				return CLLocationManager.HeadingAvailable;
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
				return true;
			} // all iOS devices support at least wifi geolocation
		}

		/// <summary>
		/// Gets a value indicating whether this instance is geolocation enabled.
		/// </summary>
		/// <value><c>true</c> if this instance is geolocation enabled; otherwise, <c>false</c>.</value>
		public bool IsGeolocationEnabled
		{
			get
			{

				return CLLocationManager.Status >= CLAuthorizationStatus.Authorized;
			}
		}

		/// <summary>
		/// Stop listening to location changes
		/// </summary>
		public void StopListening()
		{
			if (!IsListening)
			{
				return;
			}

			IsListening = false;
			if (CLLocationManager.HeadingAvailable)
			{
				_manager.StopUpdatingHeading();
			}

			_manager.StopUpdatingLocation();
			_position = null;
		}

		/// <summary>
		/// Occurs when [position error].
		/// </summary>
		public event EventHandler<PositionErrorEventArgs> PositionError;

		/// <summary>
		/// Occurs when [position changed].
		/// </summary>
		public event EventHandler<PositionEventArgs> PositionChanged;

		/// <summary>
		/// Gets the position asynchronous.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		public Task<Position> GetPositionAsync(int timeout)
		{
			return GetPositionAsync(timeout, CancellationToken.None, false);
		}

		/// <summary>
		/// Gets the position asynchronous.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <param name="includeHeading">if set to <c>true</c> [include heading].</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		public Task<Position> GetPositionAsync(int timeout, bool includeHeading)
		{
			return GetPositionAsync(timeout, CancellationToken.None, includeHeading);
		}

		/// <summary>
		/// Gets the position asynchronous.
		/// </summary>
		/// <param name="cancelToken">The cancel token.</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		public Task<Position> GetPositionAsync(CancellationToken cancelToken)
		{
			return GetPositionAsync(Timeout.Infinite, cancelToken, false);
		}

		/// <summary>
		/// Gets the position asynchronous.
		/// </summary>
		/// <param name="cancelToken">The cancel token.</param>
		/// <param name="includeHeading">if set to <c>true</c> [include heading].</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		public Task<Position> GetPositionAsync(CancellationToken cancelToken, bool includeHeading)
		{
			return GetPositionAsync(Timeout.Infinite, cancelToken, includeHeading);
		}

		/// <summary>
		/// Gets the position asynchronous.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <param name="cancelToken">The cancel token.</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		public Task<Position> GetPositionAsync(int timeout, CancellationToken cancelToken)
		{
			return GetPositionAsync(timeout, cancelToken, false);
		}

		/// <summary>
		/// Gets the position asynchronous.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <param name="cancelToken">The cancel token.</param>
		/// <param name="includeHeading">if set to <c>true</c> [include heading].</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		/// <exception cref="ArgumentOutOfRangeException">timeout;Timeout must be positive or Timeout.Infinite</exception>
		public Task<Position> GetPositionAsync(int timeout, CancellationToken cancelToken, bool includeHeading)
		{
			if (timeout <= 0 && timeout != Timeout.Infinite)
			{
				throw new ArgumentOutOfRangeException("timeout", "Timeout must be positive or Timeout.Infinite");
			}

			TaskCompletionSource<Position> tcs;
			if (!IsListening)
			{
				var m = GetManager();

				tcs = new TaskCompletionSource<Position>(m);
				var singleListener = new GeolocationSingleUpdateDelegate(m, DesiredAccuracy, includeHeading, timeout, cancelToken);
				m.Delegate = singleListener;

				m.StartUpdatingLocation();
				if (includeHeading && SupportsHeading)
				{
					m.StartUpdatingHeading();
				}

				return singleListener.Task;
			}
			tcs = new TaskCompletionSource<Position>();
			if (_position == null)
			{
				EventHandler<PositionErrorEventArgs> gotError = null;
				gotError = (s, e) =>
					{
						tcs.TrySetException(new GeolocationException(e.Error));
						PositionError -= gotError;
					};

				PositionError += gotError;

				EventHandler<PositionEventArgs> gotPosition = null;
				gotPosition = (s, e) =>
					{
						tcs.TrySetResult(e.Position);
						PositionChanged -= gotPosition;
					};

				PositionChanged += gotPosition;
			}
			else
			{
				tcs.SetResult(_position);
			}

			return tcs.Task;
		}

		/// <summary>
		/// Start listening to location changes
		/// </summary>
		/// <param name="minTime">Minimum interval in milliseconds</param>
		/// <param name="minDistance">Minimum distance in meters</param>
		public void StartListening(uint minTime, double minDistance)
		{
			StartListening(minTime, minDistance, false);
		}

		/// <summary>
		/// Start listening to location changes
		/// </summary>
		/// <param name="minTime">Minimum interval in milliseconds</param>
		/// <param name="minDistance">Minimum distance in meters</param>
		/// <param name="includeHeading">Include heading information</param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// minTime
		/// or
		/// minDistance
		/// </exception>
		/// <exception cref="InvalidOperationException">Already listening</exception>
		public void StartListening(uint minTime, double minDistance, bool includeHeading)
		{
			if (minTime < 0)
			{
				throw new ArgumentOutOfRangeException("minTime");
			}
			if (minDistance < 0)
			{
				throw new ArgumentOutOfRangeException("minDistance");
			}
			if (IsListening)
			{
				throw new InvalidOperationException("Already listening");
			}

			IsListening = true;
			_manager.DesiredAccuracy = DesiredAccuracy;
			_manager.DistanceFilter = minDistance;
			_manager.StartUpdatingLocation();

			if (includeHeading && CLLocationManager.HeadingAvailable)
			{
				_manager.StartUpdatingHeading();
			}
		}

		/// <summary>
		/// Gets the manager.
		/// </summary>
		/// <returns>CLLocationManager.</returns>
		private CLLocationManager GetManager()
		{
			CLLocationManager m = null;
			new NSObject().InvokeOnMainThread(() => m = new CLLocationManager());
			return m;
		}

		/// <summary>
		/// Handles the <see cref="E:UpdatedHeading" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="CLHeadingUpdatedEventArgs"/> instance containing the event data.</param>
		private void OnUpdatedHeading(object sender, CLHeadingUpdatedEventArgs e)
		{
			if (e.NewHeading.TrueHeading == -1)
			{
				return;
			}

			var p = (_position == null) ? new Position() : new Position(_position);

			p.Heading = e.NewHeading.TrueHeading;

			_position = p;

			OnPositionChanged(new PositionEventArgs(p));
		}

		/// <summary>
		/// Handles the <see cref="E:LocationsUpdated" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="CLLocationsUpdatedEventArgs"/> instance containing the event data.</param>
		private void OnLocationsUpdated(object sender, CLLocationsUpdatedEventArgs e)
		{
			foreach (var location in e.Locations)
			{
				UpdatePosition(location);
			}
		}

		/// <summary>
		/// Handles the <see cref="E:UpdatedLocation" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="CLLocationUpdatedEventArgs"/> instance containing the event data.</param>
		private void OnUpdatedLocation(object sender, CLLocationUpdatedEventArgs e)
		{
			UpdatePosition(e.NewLocation);
		}

		/// <summary>
		/// Updates the position.
		/// </summary>
		/// <param name="location">The location.</param>
		private void UpdatePosition(CLLocation location)
		{
			var p = (_position == null) ? new Position() : new Position(_position);

			if (location.HorizontalAccuracy > -1)
			{
				p.Accuracy = location.HorizontalAccuracy;
				p.Latitude = location.Coordinate.Latitude;
				p.Longitude = location.Coordinate.Longitude;
			}

			if (location.VerticalAccuracy > -1)
			{
				p.Altitude = location.Altitude;
				p.AltitudeAccuracy = location.VerticalAccuracy;
			}

			if (location.Speed > -1)
			{
				p.Speed = location.Speed;
			}

			p.Timestamp = new DateTimeOffset((DateTime)location.Timestamp);

			_position = p;

			OnPositionChanged(new PositionEventArgs(p));

			location.Dispose();
		}

		/// <summary>
		/// Handles the <see cref="E:Failed" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="NSErrorEventArgs"/> instance containing the event data.</param>
		private void OnFailed(object sender, NSErrorEventArgs e)
		{
			if ((int)e.Error.Code == (int)CLError.Network)
			{
				OnPositionError(new PositionErrorEventArgs(GeolocationError.PositionUnavailable));
			}
		}

		/// <summary>
		/// Handles the <see cref="E:AuthorizationChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="CLAuthorizationChangedEventArgs"/> instance containing the event data.</param>
		private void OnAuthorizationChanged(object sender, CLAuthorizationChangedEventArgs e)
		{
			if (e.Status == CLAuthorizationStatus.Denied || e.Status == CLAuthorizationStatus.Restricted)
			{
				OnPositionError(new PositionErrorEventArgs(GeolocationError.Unauthorized));
			}
		}

		/// <summary>
		/// Handles the <see cref="E:PositionChanged" /> event.
		/// </summary>
		/// <param name="e">The <see cref="PositionEventArgs"/> instance containing the event data.</param>
		private void OnPositionChanged(PositionEventArgs e)
		{
			var changed = PositionChanged;
			if (changed != null)
			{
				changed(this, e);
			}
		}

		/// <summary>
		/// Handles the <see cref="E:PositionError" /> event.
		/// </summary>
		/// <param name="e">The <see cref="PositionErrorEventArgs"/> instance containing the event data.</param>
		private void OnPositionError(PositionErrorEventArgs e)
		{
			StopListening();

			var error = PositionError;
			if (error != null)
			{
				error(this, e);
			}
		}
	}
}