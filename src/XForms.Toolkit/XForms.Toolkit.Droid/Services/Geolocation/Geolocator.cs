//
//  Copyright 2011-2013, Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//

using System;
using System.Threading.Tasks;
using Android.Locations;
using System.Threading;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using System.Linq;
using Android.Content;
using XForms.Toolkit.Services.Geolocation;
using Xamarin.Forms;

[assembly: Dependency(typeof(XForms.Toolkit.Droid.Services.Geolocation.Geolocator))]
namespace XForms.Toolkit.Droid.Services.Geolocation
{
	public class Geolocator : IGeolocator
	{

		public Geolocator ()
		{
			this.manager = (LocationManager)Application.Context.GetSystemService (Context.LocationService);
			this.providers = manager.GetProviders (enabledOnly: false).Where (s => s != LocationManager.PassiveProvider).ToArray ();
		}

	public event EventHandler<PositionErrorEventArgs> PositionError;
	public event EventHandler<PositionEventArgs> PositionChanged;

	public bool IsListening
	{
		get { return this.listener != null; }
	}

	public double DesiredAccuracy
	{
		get;
		set;
	}

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

	public bool IsGeolocationAvailable
	{
		get { return this.providers.Length > 0; }
	}

	public bool IsGeolocationEnabled
	{
		get { return this.providers.Any (this.manager.IsProviderEnabled); }
	}

	public Task<Position> GetPositionAsync (CancellationToken cancelToken)
	{
		return GetPositionAsync (cancelToken, false);
	}

	public Task<Position> GetPositionAsync (CancellationToken cancelToken, bool includeHeading)
	{
		return GetPositionAsync (Timeout.Infinite, cancelToken);
	}

	public Task<Position> GetPositionAsync (int timeout)
	{
		return GetPositionAsync (timeout, false);
	}

	public Task<Position> GetPositionAsync (int timeout, bool includeHeading)
	{
		return GetPositionAsync (timeout, CancellationToken.None);
	}

	public Task<Position> GetPositionAsync (int timeout, CancellationToken cancelToken)
	{
		return GetPositionAsync (timeout, cancelToken, false);
	}

	public Task<Position> GetPositionAsync (int timeout, CancellationToken cancelToken, bool includeHeading)
	{
		if (timeout <= 0 && timeout != Timeout.Infinite)
			throw new ArgumentOutOfRangeException ("timeout", "timeout must be greater than or equal to 0");

		var tcs = new TaskCompletionSource<Position>();

		if (!IsListening)
		{
			GeolocationSingleListener singleListener = null;
			singleListener = new GeolocationSingleListener ((float)DesiredAccuracy, timeout, this.providers.Where (this.manager.IsProviderEnabled),
				finishedCallback: () =>
				{
					for (int i = 0; i < this.providers.Length; ++i)
						this.manager.RemoveUpdates (singleListener);
				});

			if (cancelToken != CancellationToken.None)
			{
				cancelToken.Register (() =>
					{
						singleListener.Cancel();

						for (int i = 0; i < this.providers.Length; ++i)
							this.manager.RemoveUpdates (singleListener);
					}, true);
			}

			try
			{
				Looper looper = Looper.MyLooper() ?? Looper.MainLooper;

				int enabled = 0;
				for (int i = 0; i < this.providers.Length; ++i)
				{
					if (this.manager.IsProviderEnabled (this.providers[i]))
						enabled++;

					this.manager.RequestLocationUpdates (this.providers[i], 0, 0, singleListener, looper);
				}

				if (enabled == 0)
				{
					for (int i = 0; i < this.providers.Length; ++i)
						this.manager.RemoveUpdates (singleListener);

					tcs.SetException (new GeolocationException (GeolocationError.PositionUnavailable));
					return tcs.Task;
				}
			}
			catch (Java.Lang.SecurityException ex)
			{
				tcs.SetException (new GeolocationException (GeolocationError.Unauthorized, ex));
				return tcs.Task;
			}

			return singleListener.Task;
		}

		// If we're already listening, just use the current listener
		lock (this.positionSync)
		{
			if (this.lastPosition == null)
			{
				if (cancelToken != CancellationToken.None)
				{
					cancelToken.Register (() => tcs.TrySetCanceled());
				}

				EventHandler<PositionEventArgs> gotPosition = null;
				gotPosition = (s, e) =>
				{
					tcs.TrySetResult (e.Position);
					PositionChanged -= gotPosition;
				};

				PositionChanged += gotPosition;
			}
			else
			{
				tcs.SetResult (this.lastPosition);
			}
		}

		return tcs.Task;
	}

	public void StartListening (uint minTime, double minDistance)
	{
		StartListening (minTime, minDistance, false);
	}

	public void StartListening (uint minTime, double minDistance, bool includeHeading)
	{
		if (minTime < 0)
			throw new ArgumentOutOfRangeException ("minTime");
		if (minDistance < 0)
			throw new ArgumentOutOfRangeException ("minDistance");
		if (IsListening)
			throw new InvalidOperationException ("This Geolocator is already listening");

		this.listener = new GeolocationContinuousListener (this.manager, TimeSpan.FromMilliseconds (minTime), this.providers);
		this.listener.PositionChanged += OnListenerPositionChanged;
		this.listener.PositionError += OnListenerPositionError;

		Looper looper = Looper.MyLooper() ?? Looper.MainLooper;
		for (int i = 0; i < this.providers.Length; ++i)
			this.manager.RequestLocationUpdates (providers[i], minTime, (float)minDistance, listener, looper);
	}

	public void StopListening()
	{
		if (this.listener == null)
			return;

		this.listener.PositionChanged -= OnListenerPositionChanged;
		this.listener.PositionError -= OnListenerPositionError;

		for (int i = 0; i < this.providers.Length; ++i)
			this.manager.RemoveUpdates (this.listener);

		this.listener = null;
	}

	private readonly string[] providers;
	private readonly LocationManager manager;
	private string headingProvider;

	private GeolocationContinuousListener listener;

	private readonly object positionSync = new object();
	private Position lastPosition;

	private void OnListenerPositionChanged (object sender, PositionEventArgs e)
	{
		if (!IsListening) // ignore anything that might come in afterwards
			return;

		lock (this.positionSync)
		{
			this.lastPosition = e.Position;

			var changed = PositionChanged;
			if (changed != null)
				changed (this, e);
		}
	}

	private void OnListenerPositionError (object sender, PositionErrorEventArgs e)
	{
		StopListening();

		var error = PositionError;
		if (error != null)
			error (this, e);
	}

	private static readonly DateTime Epoch = new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
	internal static DateTimeOffset GetTimestamp (Location location)
	{
		return new DateTimeOffset (Epoch.AddMilliseconds (location.Time));
	}
	}
}

