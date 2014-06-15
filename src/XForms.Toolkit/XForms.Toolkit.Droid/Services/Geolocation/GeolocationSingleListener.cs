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
using Android.OS;
using System.Threading;
using System.Collections.Generic;
using XForms.Toolkit.Services.Geolocation;

namespace XForms.Toolkit.Droid.Services.Geolocation
{
	internal class GeolocationSingleListener
		: Java.Lang.Object, ILocationListener
	{
		public GeolocationSingleListener (float desiredAccuracy, int timeout, IEnumerable<string> activeProviders, Action finishedCallback)
		{
			this.desiredAccuracy = desiredAccuracy;
			this.finishedCallback = finishedCallback;

			this.activeProviders = new HashSet<string> (activeProviders);

			if (timeout != Timeout.Infinite)
				this.timer = new Timer (TimesUp, null, timeout, 0);
		}

		public Task<Position> Task
		{
			get { return this.completionSource.Task; }
		}

		public void OnLocationChanged (Location location)
		{
			if (location.Accuracy <= this.desiredAccuracy)
			{
				Finish (location);
				return;
			}

			lock (this.locationSync)
			{
				if (this.bestLocation == null || location.Accuracy <= this.bestLocation.Accuracy)
					this.bestLocation = location;
			}
		}

		public void OnProviderDisabled (string provider)
		{
			lock (this.activeProviders)
			{
				if (this.activeProviders.Remove (provider) && this.activeProviders.Count == 0)
					this.completionSource.TrySetException (new GeolocationException (GeolocationError.PositionUnavailable));
			}
		}

		public void OnProviderEnabled (string provider)
		{
			lock (this.activeProviders)
				this.activeProviders.Add (provider);	
		}

		public void OnStatusChanged (string provider, Availability status, Bundle extras)
		{
			switch (status)
			{
			case Availability.Available:
				OnProviderEnabled (provider);
				break;

			case Availability.OutOfService:
				OnProviderDisabled (provider);
				break;
			}
		}

		public void Cancel()
		{
			this.completionSource.TrySetCanceled();
		}

		private readonly object locationSync = new object();
		private Location bestLocation;

		private readonly Action finishedCallback;
		private readonly float desiredAccuracy;
		private readonly Timer timer;
		private readonly TaskCompletionSource<Position> completionSource = new TaskCompletionSource<Position>();
		private HashSet<string> activeProviders = new HashSet<string>();

		private void TimesUp (object state)
		{
			lock (this.locationSync)
			{
				if (this.bestLocation == null)
				{
					if (this.completionSource.TrySetCanceled() && this.finishedCallback != null)
						this.finishedCallback();
				}
				else
					Finish (this.bestLocation);
			}
		}

		private void Finish (Location location)
		{
			var p = new Position();
			if (location.HasAccuracy)
				p.Accuracy = location.Accuracy;
			if (location.HasAltitude)
				p.Altitude = location.Altitude;
			if (location.HasBearing)
				p.Heading = location.Bearing;
			if (location.HasSpeed)
				p.Speed = location.Speed;

			p.Longitude = location.Longitude;
			p.Latitude = location.Latitude;
			p.Timestamp = Geolocator.GetTimestamp (location);

			if (this.finishedCallback != null)
				this.finishedCallback();

			this.completionSource.TrySetResult (p);
		}
	}
}