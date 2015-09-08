namespace XLabs.Platform.Services.Geolocation
{
	using System;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	using Android.App;
	using Android.Content;
	using Android.Locations;
	using Android.OS;

	using Java.Lang;

	/// <summary>
	///     Class Geolocator.
	/// </summary>
	public class Geolocator : IGeolocator
	{
		/// <summary>
		///     The epoch
		/// </summary>
		private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		/// <summary>
		///     The _heading provider
		/// </summary>
		private string _headingProvider;

		/// <summary>
		///     The _last position
		/// </summary>
		private Position _lastPosition;

		/// <summary>
		///     The _listener
		/// </summary>
		private GeolocationContinuousListener _listener;

		/// <summary>
		///     The _manager
		/// </summary>
		private readonly LocationManager _manager;

		/// <summary>
		///     The _position synchronize
		/// </summary>
		private readonly object _positionSync = new object();

		/// <summary>
		///     The _providers
		/// </summary>
		private readonly string[] _providers;

		/// <summary>
		///     Initializes a new instance of the <see cref="Geolocator" /> class.
		/// </summary>
		public Geolocator()
		{
			_manager = (LocationManager)Application.Context.GetSystemService(Context.LocationService);
			_providers = _manager.GetProviders(false).Where(s => s != LocationManager.PassiveProvider).ToArray();
		}

		/// <summary>
		///     Gets a value indicating whether this instance is listening.
		/// </summary>
		/// <value><c>true</c> if this instance is listening; otherwise, <c>false</c>.</value>
		public bool IsListening
		{
			get
			{
				return _listener != null;
			}
		}

		/// <summary>
		///     Gets or sets the desired accuracy.
		/// </summary>
		/// <value>The desired accuracy.</value>
		public double DesiredAccuracy { get; set; }

		/// <summary>
		///     Gets a value indicating whether [supports heading].
		/// </summary>
		/// <value><c>true</c> if [supports heading]; otherwise, <c>false</c>.</value>
		public bool SupportsHeading
		{
			get
			{
				return false;
				//				if (this.headingProvider == null || !this.manager.IsProviderEnabled (this.headingProvider))
				//				{
				//					Criteria c = new Criteria { BearingRequired = true };
				//					string providerName = this.manager.GetBestProvider (c, enabledOnly: false);
				//
				//					LocationProvider provider = this.manager.GetProvider (providerName);
				//
				//					if (provider.SupportsBearing())
				//					{
				//						this.headingProvider = providerName;
				//						return true;
				//					}
				//					else
				//					{
				//						this.headingProvider = null;
				//						return false;
				//					}
				//				}
				//				else
				//					return true;
			}
		}

		/// <summary>
		///     Gets a value indicating whether this instance is geolocation available.
		/// </summary>
		/// <value><c>true</c> if this instance is geolocation available; otherwise, <c>false</c>.</value>
		public bool IsGeolocationAvailable
		{
			get
			{
				return _providers.Length > 0;
			}
		}

		/// <summary>
		///     Gets a value indicating whether this instance is geolocation enabled.
		/// </summary>
		/// <value><c>true</c> if this instance is geolocation enabled; otherwise, <c>false</c>.</value>
		public bool IsGeolocationEnabled
		{
			get
			{
				return _providers.Any(_manager.IsProviderEnabled);
			}
		}

		/// <summary>
		///     Stop listening to location changes
		/// </summary>
		public void StopListening()
		{
			if (_listener == null)
			{
				return;
			}

			_listener.PositionChanged -= OnListenerPositionChanged;
			_listener.PositionError -= OnListenerPositionError;

			for (var i = 0; i < _providers.Length; ++i)
			{
				_manager.RemoveUpdates(_listener);
			}

			_listener = null;
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
		///     Gets the position asynchronous.
		/// </summary>
		/// <param name="cancelToken">The cancel token.</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		public Task<Position> GetPositionAsync(CancellationToken cancelToken)
		{
			return GetPositionAsync(cancelToken, false);
		}

		/// <summary>
		///     Gets the position asynchronous.
		/// </summary>
		/// <param name="cancelToken">The cancel token.</param>
		/// <param name="includeHeading">if set to <c>true</c> [include heading].</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		public Task<Position> GetPositionAsync(CancellationToken cancelToken, bool includeHeading)
		{
			return GetPositionAsync(Timeout.Infinite, cancelToken);
		}

		/// <summary>
		///     Gets the position asynchronous.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		public Task<Position> GetPositionAsync(int timeout)
		{
			return GetPositionAsync(timeout, false);
		}

		/// <summary>
		///     Gets the position asynchronous.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <param name="includeHeading">if set to <c>true</c> [include heading].</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		public Task<Position> GetPositionAsync(int timeout, bool includeHeading)
		{
			return GetPositionAsync(timeout, CancellationToken.None);
		}

		/// <summary>
		///     Gets the position asynchronous.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <param name="cancelToken">The cancel token.</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		public Task<Position> GetPositionAsync(int timeout, CancellationToken cancelToken)
		{
			return GetPositionAsync(timeout, cancelToken, false);
		}

		/// <summary>
		///     Gets the position asynchronous.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <param name="cancelToken">The cancel token.</param>
		/// <param name="includeHeading">if set to <c>true</c> [include heading].</param>
		/// <returns>Task&lt;Position&gt;.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">timeout;timeout must be greater than or equal to 0</exception>
		public Task<Position> GetPositionAsync(int timeout, CancellationToken cancelToken, bool includeHeading)
		{
			if (timeout <= 0 && timeout != Timeout.Infinite)
			{
				throw new ArgumentOutOfRangeException("timeout", "timeout must be greater than or equal to 0");
			}

			var tcs = new TaskCompletionSource<Position>();

			if (!IsListening)
			{
				GeolocationSingleListener singleListener = null;
				singleListener = new GeolocationSingleListener(
					(float)DesiredAccuracy,
					timeout,
					_providers.Where(_manager.IsProviderEnabled),
					() =>
						{
							for (var i = 0; i < _providers.Length; ++i)
							{
								_manager.RemoveUpdates(singleListener);
							}
						});

				if (cancelToken != CancellationToken.None)
				{
					cancelToken.Register(
						() =>
							{
								singleListener.Cancel();

								for (var i = 0; i < _providers.Length; ++i)
								{
									_manager.RemoveUpdates(singleListener);
								}
							},
						true);
				}

				try
				{
					var looper = Looper.MyLooper() ?? Looper.MainLooper;

					var enabled = 0;
					for (var i = 0; i < _providers.Length; ++i)
					{
						if (_manager.IsProviderEnabled(_providers[i]))
						{
							enabled++;
						}

						_manager.RequestLocationUpdates(_providers[i], 0, 0, singleListener, looper);
					}

					if (enabled == 0)
					{
						for (var i = 0; i < _providers.Length; ++i)
						{
							_manager.RemoveUpdates(singleListener);
						}

						tcs.SetException(new GeolocationException(GeolocationError.PositionUnavailable));
						return tcs.Task;
					}
				}
				catch (SecurityException ex)
				{
					tcs.SetException(new GeolocationException(GeolocationError.Unauthorized, ex));
					return tcs.Task;
				}

				return singleListener.Task;
			}

			// If we're already listening, just use the current listener
			lock (_positionSync)
			{
				if (_lastPosition == null)
				{
					if (cancelToken != CancellationToken.None)
					{
						cancelToken.Register(() => tcs.TrySetCanceled());
					}

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
					tcs.SetResult(_lastPosition);
				}
			}

			return tcs.Task;
		}

		/// <summary>
		///     Start listening to location changes
		/// </summary>
		/// <param name="minTime">Minimum interval in milliseconds</param>
		/// <param name="minDistance">Minimum distance in meters</param>
		public void StartListening(uint minTime, double minDistance)
		{
			StartListening(minTime, minDistance, false);
		}

		/// <summary>
		///     Start listening to location changes
		/// </summary>
		/// <param name="minTime">Minimum interval in milliseconds</param>
		/// <param name="minDistance">Minimum distance in meters</param>
		/// <param name="includeHeading">Include heading information</param>
		/// <exception cref="System.ArgumentOutOfRangeException">
		///     minTime
		///     or
		///     minDistance
		/// </exception>
		/// <exception cref="System.InvalidOperationException">This Geolocator is already listening</exception>
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
				throw new InvalidOperationException("This Geolocator is already listening");
			}

			_listener = new GeolocationContinuousListener(_manager, TimeSpan.FromMilliseconds(minTime), _providers);
			_listener.PositionChanged += OnListenerPositionChanged;
			_listener.PositionError += OnListenerPositionError;

			var looper = Looper.MyLooper() ?? Looper.MainLooper;
			for (var i = 0; i < _providers.Length; ++i)
			{
				_manager.RequestLocationUpdates(_providers[i], minTime, (float)minDistance, _listener, looper);
			}
		}

		/// <summary>
		///     Handles the <see cref="E:ListenerPositionChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="PositionEventArgs" /> instance containing the event data.</param>
		private void OnListenerPositionChanged(object sender, PositionEventArgs e)
		{
			if (!IsListening) // ignore anything that might come in afterwards
			{
				return;
			}

			lock (_positionSync)
			{
				_lastPosition = e.Position;

				var changed = PositionChanged;
				if (changed != null)
				{
					changed(this, e);
				}
			}
		}

		/// <summary>
		///     Handles the <see cref="E:ListenerPositionError" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="PositionErrorEventArgs" /> instance containing the event data.</param>
		private void OnListenerPositionError(object sender, PositionErrorEventArgs e)
		{
			StopListening();

			var error = PositionError;
			if (error != null)
			{
				error(this, e);
			}
		}

		/// <summary>
		///     Gets the timestamp.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <returns>DateTimeOffset.</returns>
		internal static DateTimeOffset GetTimestamp(Location location)
		{
			return new DateTimeOffset(Epoch.AddMilliseconds(location.Time));
		}
	}
}