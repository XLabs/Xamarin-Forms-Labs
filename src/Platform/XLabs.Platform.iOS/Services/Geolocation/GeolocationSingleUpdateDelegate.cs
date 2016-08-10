// ***********************************************************************
// Assembly         : XLabs.Platform.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="GeolocationSingleUpdateDelegate.cs" company="XLabs Team">
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

namespace XLabs.Platform.Services.Geolocation
{
	/// <summary>
	/// Class GeolocationSingleUpdateDelegate.
	/// </summary>
	internal class GeolocationSingleUpdateDelegate : CLLocationManagerDelegate
	{
		/// <summary>
		/// The _best heading
		/// </summary>
		private CLHeading _bestHeading;

		/// <summary>
		/// The _have heading
		/// </summary>
		private bool _haveHeading;

		/// <summary>
		/// The _have location
		/// </summary>
		private bool _haveLocation;

		/// <summary>
		/// The _desired accuracy
		/// </summary>
		private readonly double _desiredAccuracy;

		/// <summary>
		/// The _include heading
		/// </summary>
		private readonly bool _includeHeading;

		/// <summary>
		/// The _manager
		/// </summary>
		private readonly CLLocationManager _manager;

		/// <summary>
		/// The _position
		/// </summary>
		private readonly Position _position = new Position();

		/// <summary>
		/// The _TCS
		/// </summary>
		private readonly TaskCompletionSource<Position> _tcs;

		/// <summary>
		/// Initializes a new instance of the <see cref="GeolocationSingleUpdateDelegate"/> class.
		/// </summary>
		/// <param name="manager">The manager.</param>
		/// <param name="desiredAccuracy">The desired accuracy.</param>
		/// <param name="includeHeading">if set to <c>true</c> [include heading].</param>
		/// <param name="timeout">The timeout.</param>
		/// <param name="cancelToken">The cancel token.</param>
		public GeolocationSingleUpdateDelegate(
			CLLocationManager manager,
			double desiredAccuracy,
			bool includeHeading,
			int timeout,
			CancellationToken cancelToken)
		{
			_manager = manager;
			_tcs = new TaskCompletionSource<Position>(manager);
			_desiredAccuracy = desiredAccuracy;
			_includeHeading = includeHeading;

			if (timeout != Timeout.Infinite)
			{
				Timer t = null;
				t = new Timer(
					s =>
						{
							if (_haveLocation)
							{
								_tcs.TrySetResult(new Position(_position));
							}
							else
							{
								_tcs.TrySetCanceled();
							}

							StopListening();
							t.Dispose();
						},
					null,
					timeout,
					0);
			}

			cancelToken.Register(
				() =>
					{
						StopListening();
						_tcs.TrySetCanceled();
					});
		}

		/// <summary>
		/// Gets the task.
		/// </summary>
		/// <value>The task.</value>
		public Task<Position> Task
		{
			get
			{
				return _tcs.Task;
			}
		}

		/// <summary>
		/// Authorizations the changed.
		/// </summary>
		/// <param name="manager">The manager.</param>
		/// <param name="status">The status.</param>
		public override void AuthorizationChanged(CLLocationManager manager, CLAuthorizationStatus status)
		{
			// If user has services disabled, we're just going to throw an exception for consistency.
			if (status == CLAuthorizationStatus.Denied || status == CLAuthorizationStatus.Restricted)
			{
				StopListening();
				_tcs.TrySetException(new GeolocationException(GeolocationError.Unauthorized));
			}
		}

		/// <summary>
		/// Faileds the specified manager.
		/// </summary>
		/// <param name="manager">The manager.</param>
		/// <param name="error">The error.</param>
		public override void Failed(CLLocationManager manager, NSError error)
		{
			switch ((CLError)(int)error.Code)
			{
				case CLError.Network:
					StopListening();
					_tcs.SetException(new GeolocationException(GeolocationError.PositionUnavailable));
					break;
			}
		}

		/// <summary>
		/// Shoulds the display heading calibration.
		/// </summary>
		/// <param name="manager">The manager.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public override bool ShouldDisplayHeadingCalibration(CLLocationManager manager)
		{
			return true;
		}

		/// <summary>
		/// Updateds the location.
		/// </summary>
		/// <param name="manager">The manager.</param>
		/// <param name="newLocation">The new location.</param>
		/// <param name="oldLocation">The old location.</param>
		public override void UpdatedLocation(CLLocationManager manager, CLLocation newLocation, CLLocation oldLocation)
		{
			if (newLocation.HorizontalAccuracy < 0)
			{
				return;
			}

			if (_haveLocation && newLocation.HorizontalAccuracy > _position.Accuracy)
			{
				return;
			}

			_position.Accuracy = newLocation.HorizontalAccuracy;
			_position.Altitude = newLocation.Altitude;
			_position.AltitudeAccuracy = newLocation.VerticalAccuracy;
			_position.Latitude = newLocation.Coordinate.Latitude;
			_position.Longitude = newLocation.Coordinate.Longitude;
			_position.Speed = newLocation.Speed;
			_position.Timestamp = new DateTimeOffset((DateTime)newLocation.Timestamp);

			_haveLocation = true;

			if ((!_includeHeading || _haveHeading) && _position.Accuracy <= _desiredAccuracy)
			{
				_tcs.TrySetResult(new Position(_position));
				StopListening();
			}
		}

		/// <summary>
		/// Updateds the heading.
		/// </summary>
		/// <param name="manager">The manager.</param>
		/// <param name="newHeading">The new heading.</param>
		public override void UpdatedHeading(CLLocationManager manager, CLHeading newHeading)
		{
			if (newHeading.HeadingAccuracy < 0)
			{
				return;
			}
			if (_bestHeading != null && newHeading.HeadingAccuracy >= _bestHeading.HeadingAccuracy)
			{
				return;
			}

			_bestHeading = newHeading;
			_position.Heading = newHeading.TrueHeading;
			_haveHeading = true;

			if (_haveLocation && _position.Accuracy <= _desiredAccuracy)
			{
				_tcs.TrySetResult(new Position(_position));
				StopListening();
			}
		}

		/// <summary>
		/// Stops the listening.
		/// </summary>
		private void StopListening()
		{
			if (CLLocationManager.HeadingAvailable)
			{
				_manager.StopUpdatingHeading();
			}

			_manager.StopUpdatingLocation();
		}
	}
}