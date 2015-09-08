namespace XLabs.Platform.Services.Geolocation
{
	using System;
	using System.Collections.Generic;
	using System.Threading;

	using Android.Locations;
	using Android.OS;

	using Object = Java.Lang.Object;

	/// <summary>
	///     Class GeolocationContinuousListener.
	/// </summary>
	internal class GeolocationContinuousListener : Object, ILocationListener
	{
		/// <summary>
		///     The active provider
		/// </summary>
		private string _activeProvider;

		/// <summary>
		///     The last location
		/// </summary>
		private Location _lastLocation;

		/// <summary>
		///     The providers
		/// </summary>
		private IList<string> _providers;

		/// <summary>
		///     The time period
		/// </summary>
		private TimeSpan _timePeriod;

		/// <summary>
		///     The active providers
		/// </summary>
		private readonly HashSet<string> _activeProviders = new HashSet<string>();

		/// <summary>
		///     The manager
		/// </summary>
		private readonly LocationManager _manager;

		/// <summary>
		///     Initializes a new instance of the <see cref="GeolocationContinuousListener" /> class.
		/// </summary>
		/// <param name="manager">The manager.</param>
		/// <param name="timePeriod">The time period.</param>
		/// <param name="providers">The providers.</param>
		public GeolocationContinuousListener(LocationManager manager, TimeSpan timePeriod, IList<string> providers)
		{
			_manager = manager;
			_timePeriod = timePeriod;
			_providers = providers;

			foreach (var p in providers)
			{
				if (manager.IsProviderEnabled(p))
				{
					_activeProviders.Add(p);
				}
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
			if (location.Provider != _activeProvider)
			{
				if (_activeProvider != null && _manager.IsProviderEnabled(_activeProvider))
				{
					var pr = _manager.GetProvider(location.Provider);
					var lapsed = GetTimeSpan(location.Time) - GetTimeSpan(_lastLocation.Time);

					if (pr.Accuracy > _manager.GetProvider(_activeProvider).Accuracy && lapsed < _timePeriod.Add(_timePeriod))
					{
						location.Dispose();
						return;
					}
				}

				_activeProvider = location.Provider;
			}

			var previous = Interlocked.Exchange(ref _lastLocation, location);
			if (previous != null)
			{
				previous.Dispose();
			}

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

			var changed = PositionChanged;
			if (changed != null)
			{
				changed(this, new PositionEventArgs(p));
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
			if (provider == LocationManager.PassiveProvider)
			{
				return;
			}

			lock (_activeProviders)
			{
				if (_activeProviders.Remove(provider) && _activeProviders.Count == 0)
				{
					OnPositionError(new PositionErrorEventArgs(GeolocationError.PositionUnavailable));
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
			if (provider == LocationManager.PassiveProvider)
			{
				return;
			}

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
		///     Occurs when [position error].
		/// </summary>
		public event EventHandler<PositionErrorEventArgs> PositionError;

		/// <summary>
		///     Occurs when [position changed].
		/// </summary>
		public event EventHandler<PositionEventArgs> PositionChanged;

		/// <summary>
		///     Gets the time span.
		/// </summary>
		/// <param name="time">The time.</param>
		/// <returns>TimeSpan.</returns>
		private TimeSpan GetTimeSpan(long time)
		{
			return new TimeSpan(TimeSpan.TicksPerMillisecond * time);
		}

		/// <summary>
		///     Handles the <see cref="E:PositionError" /> event.
		/// </summary>
		/// <param name="e">The <see cref="PositionErrorEventArgs" /> instance containing the event data.</param>
		private void OnPositionError(PositionErrorEventArgs e)
		{
			var error = PositionError;
			if (error != null)
			{
				error(this, e);
			}
		}
	}
}