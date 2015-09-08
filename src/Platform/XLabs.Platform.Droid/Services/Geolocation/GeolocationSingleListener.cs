namespace XLabs.Platform.Services.Geolocation
{
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;

	using Android.Locations;
	using Android.OS;

	using Object = Java.Lang.Object;

	/// <summary>
	///     Class GeolocationSingleListener.
	/// </summary>
	internal class GeolocationSingleListener : Object, ILocationListener
	{
		/// <summary>
		///     The _best location
		/// </summary>
		private Location _bestLocation;

		/// <summary>
		///     The _active providers
		/// </summary>
		private readonly HashSet<string> _activeProviders;

		/// <summary>
		///     The _completion source
		/// </summary>
		private readonly TaskCompletionSource<Position> _completionSource = new TaskCompletionSource<Position>();

		/// <summary>
		///     The _desired accuracy
		/// </summary>
		private readonly float _desiredAccuracy;

		/// <summary>
		///     The _finished callback
		/// </summary>
		private readonly Action _finishedCallback;

		/// <summary>
		///     The _location synchronize
		/// </summary>
		private readonly object _locationSync = new object();

		/// <summary>
		///     The _timer
		/// </summary>
		private readonly Timer _timer;

		/// <summary>
		///     Initializes a new instance of the <see cref="GeolocationSingleListener" /> class.
		/// </summary>
		/// <param name="desiredAccuracy">The desired accuracy.</param>
		/// <param name="timeout">The timeout.</param>
		/// <param name="activeProviders">The active providers.</param>
		/// <param name="finishedCallback">The finished callback.</param>
		public GeolocationSingleListener(
			float desiredAccuracy,
			int timeout,
			IEnumerable<string> activeProviders,
			Action finishedCallback)
		{
			_desiredAccuracy = desiredAccuracy;
			_finishedCallback = finishedCallback;

			_activeProviders = new HashSet<string>(activeProviders);

			if (timeout != Timeout.Infinite)
			{
				_timer = new Timer(TimesUp, null, timeout, 0);
			}
		}

		/// <summary>
		///     Gets the task.
		/// </summary>
		/// <value>The task.</value>
		public Task<Position> Task
		{
			get
			{
				return _completionSource.Task;
			}
		}

		/// <summary>
		///     Called when the location has changed.
		/// </summary>
		/// <param name="location">The new location, as a Location object.</param>
		/// <since version="Added in API level 1" />
		/// <remarks>
		///     <para tool="javadoc-to-mdoc">
		///         Called when the location has changed.
		///     </para>
		///     <para tool="javadoc-to-mdoc"> There are no restrictions on the use of the supplied Location object.</para>
		///     <para tool="javadoc-to-mdoc">
		///         <format type="text/html">
		///             <a
		///                 href="http://developer.android.com/reference/android/location/LocationListener.html#onLocationChanged(android.location.Location)"
		///                 target="_blank">
		///                 [Android Documentation]
		///             </a>
		///         </format>
		///     </para>
		/// </remarks>
		public void OnLocationChanged(Location location)
		{
			if (location.Accuracy <= _desiredAccuracy)
			{
				Finish(location);
				return;
			}

			lock (_locationSync)
			{
				if (_bestLocation == null || location.Accuracy <= _bestLocation.Accuracy)
				{
					_bestLocation = location;
				}
			}
		}

		/// <summary>
		///     Called when the provider is disabled by the user.
		/// </summary>
		/// <param name="provider">
		///     the name of the location provider associated with this
		///     update.
		/// </param>
		/// <since version="Added in API level 1" />
		/// <remarks>
		///     <para tool="javadoc-to-mdoc">
		///         Called when the provider is disabled by the user. If requestLocationUpdates
		///         is called on an already disabled provider, this method is called
		///         immediately.
		///     </para>
		///     <para tool="javadoc-to-mdoc">
		///         <format type="text/html">
		///             <a
		///                 href="http://developer.android.com/reference/android/location/LocationListener.html#onProviderDisabled(java.lang.String)"
		///                 target="_blank">
		///                 [Android Documentation]
		///             </a>
		///         </format>
		///     </para>
		/// </remarks>
		public void OnProviderDisabled(string provider)
		{
			lock (_activeProviders)
			{
				if (_activeProviders.Remove(provider) && _activeProviders.Count == 0)
				{
					_completionSource.TrySetException(new GeolocationException(GeolocationError.PositionUnavailable));
				}
			}
		}

		/// <summary>
		///     Called when the provider is enabled by the user.
		/// </summary>
		/// <param name="provider">
		///     the name of the location provider associated with this
		///     update.
		/// </param>
		/// <since version="Added in API level 1" />
		/// <remarks>
		///     <para tool="javadoc-to-mdoc">Called when the provider is enabled by the user.</para>
		///     <para tool="javadoc-to-mdoc">
		///         <format type="text/html">
		///             <a
		///                 href="http://developer.android.com/reference/android/location/LocationListener.html#onProviderEnabled(java.lang.String)"
		///                 target="_blank">
		///                 [Android Documentation]
		///             </a>
		///         </format>
		///     </para>
		/// </remarks>
		public void OnProviderEnabled(string provider)
		{
			lock (_activeProviders) _activeProviders.Add(provider);
		}

		/// <summary>
		///     Called when the provider status changes.
		/// </summary>
		/// <param name="provider">
		///     the name of the location provider associated with this
		///     update.
		/// </param>
		/// <param name="status">
		///     <c>
		///         <see cref="F:Android.Locations.Availability.OutOfService" />
		///     </c>
		///     if the
		///     provider is out of service, and this is not expected to change in the
		///     near future;
		///     <c>
		///         <see cref="F:Android.Locations.Availability.TemporarilyUnavailable" />
		///     </c>
		///     if
		///     the provider is temporarily unavailable but is expected to be available
		///     shortly; and
		///     <c>
		///         <see cref="F:Android.Locations.Availability.Available" />
		///     </c>
		///     if the
		///     provider is currently available.
		/// </param>
		/// <param name="extras">
		///     an optional Bundle which will contain provider specific
		///     status variables.
		///     <para tool="javadoc-to-mdoc" />
		///     A number of common key/value pairs for the extras Bundle are listed
		///     below. Providers that use any of the keys on this list must
		///     provide the corresponding value as described below.
		///     <list type="bullet">
		///         <item>
		///             <term>
		///                 satellites - the number of satellites used to derive the fix
		///             </term>
		///         </item>
		///     </list>
		/// </param>
		/// <since version="Added in API level 1" />
		/// <remarks>
		///     <para tool="javadoc-to-mdoc">
		///         Called when the provider status changes. This method is called when
		///         a provider is unable to fetch a location or if the provider has recently
		///         become available after a period of unavailability.
		///     </para>
		///     <para tool="javadoc-to-mdoc">
		///         <format type="text/html">
		///             <a
		///                 href="http://developer.android.com/reference/android/location/LocationListener.html#onStatusChanged(java.lang.String, int, android.os.Bundle)"
		///                 target="_blank">
		///                 [Android Documentation]
		///             </a>
		///         </format>
		///     </para>
		/// </remarks>
		public void OnStatusChanged(string provider, Availability status, Bundle extras)
		{
			switch (status)
			{
				case Availability.Available:
					OnProviderEnabled(provider);
					break;

				case Availability.OutOfService:
					OnProviderDisabled(provider);
					break;
			}
		}

		/// <summary>
		///     Cancels this instance.
		/// </summary>
		public void Cancel()
		{
			_completionSource.TrySetCanceled();
		}

		/// <summary>
		///     Timeses up.
		/// </summary>
		/// <param name="state">The state.</param>
		private void TimesUp(object state)
		{
			lock (_locationSync)
			{
				if (_bestLocation == null)
				{
					if (_completionSource.TrySetCanceled() && _finishedCallback != null)
					{
						_finishedCallback();
					}
				}
				else
				{
					Finish(_bestLocation);
				}
			}
		}

		/// <summary>
		///     Finishes the specified location.
		/// </summary>
		/// <param name="location">The location.</param>
		private void Finish(Location location)
		{
			var p = new Position();
			if (location.HasAccuracy)
			{
				p.Accuracy = location.Accuracy;
			}
			if (location.HasAltitude)
			{
				p.Altitude = location.Altitude;
			}
			if (location.HasBearing)
			{
				p.Heading = location.Bearing;
			}
			if (location.HasSpeed)
			{
				p.Speed = location.Speed;
			}

			p.Longitude = location.Longitude;
			p.Latitude = location.Latitude;
			p.Timestamp = Geolocator.GetTimestamp(location);

			if (_finishedCallback != null)
			{
				_finishedCallback();
			}

			_completionSource.TrySetResult(p);
		}
	}
}