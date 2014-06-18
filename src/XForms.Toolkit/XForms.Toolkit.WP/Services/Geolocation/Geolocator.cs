using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XForms.Toolkit;

[assembly: Dependency(typeof(XForms.Toolkit.Services.Geolocation.Geolocator))]
namespace XForms.Toolkit.Services.Geolocation
{
    using Windows.Devices.Geolocation;
    using Locator = Windows.Devices.Geolocation.Geolocator;

    public class Geolocator : IGeolocator
    {
        private Locator locator;

        public Geolocator()
        {
            locator = new Locator();
        }

        #region IGeolocator Members

        public event EventHandler<PositionErrorEventArgs> PositionError;

        public event EventHandler<PositionEventArgs> PositionChanged;

        public double DesiredAccuracy
        {
            get
            {
                return (locator.DesiredAccuracy == PositionAccuracy.Default)
                           ? 100
                           : 10;
            }

            set
            {
                locator.DesiredAccuracy = (value > 10) ?
                        PositionAccuracy.Default :
                        PositionAccuracy.High;
            }
        }

        public bool IsListening
        {
            get;
            private set;
        }

        public bool SupportsHeading
        {
            get { return true; }
        }

        public bool IsGeolocationAvailable
        {
            get { return locator.LocationStatus != PositionStatus.NotAvailable; }
        }

        public bool IsGeolocationEnabled
        {
            get { return locator.LocationStatus != PositionStatus.Disabled; }
        }

        public async Task<Position> GetPositionAsync(int timeout)
        {
            var position = await locator.GetGeopositionAsync(TimeSpan.MaxValue, TimeSpan.FromMilliseconds(timeout));
            return position.Coordinate.GetPosition();
        }

        public async Task<Position> GetPositionAsync(int timeout, bool includeHeading)
        {
            return await GetPositionAsync(timeout);
        }

        public async Task<Position> GetPositionAsync(System.Threading.CancellationToken cancelToken)
        {
            var t = locator.GetGeopositionAsync().AsTask();

            while (t.Status == TaskStatus.Running)
            {
                cancelToken.ThrowIfCancellationRequested();
            }

            var position = await t;
            
            return position.Coordinate.GetPosition();
        }

        public Task<Position> GetPositionAsync(System.Threading.CancellationToken cancelToken, bool includeHeading)
        {
            return this.GetPositionAsync(cancelToken);
        }

        public Task<Position> GetPositionAsync(int timeout, System.Threading.CancellationToken cancelToken)
        {
            var t = GetPositionAsync(timeout);

            while (t.Status == TaskStatus.Running)
            {
                cancelToken.ThrowIfCancellationRequested();
            }

            return t;
        }

        public Task<Position> GetPositionAsync(int timeout, System.Threading.CancellationToken cancelToken, bool includeHeading)
        {
            var t = GetPositionAsync(timeout, includeHeading);

            while (t.Status == TaskStatus.Running)
            {
                cancelToken.ThrowIfCancellationRequested();
            }

            return t;
        }

        public void StartListening(uint minTime, double minDistance)
        {
            locator.MovementThreshold = minDistance;
            locator.ReportInterval = minTime;
            locator.PositionChanged += locator_PositionChanged;
            locator.StatusChanged += locator_StatusChanged;
        }

        public void StartListening(uint minTime, double minDistance, bool includeHeading)
        {
            this.StartListening(minTime, minDistance);
        }

        public void StopListening()
        {
            locator.PositionChanged -= locator_PositionChanged;
            locator.StatusChanged -= locator_StatusChanged;
        }

        #endregion

        void locator_PositionChanged(Locator sender, PositionChangedEventArgs args)
        {
            this.PositionChanged.TryInvoke(
                sender, 
                new PositionEventArgs(args.Position.Coordinate.GetPosition()));
        }

        void locator_StatusChanged(Locator sender, StatusChangedEventArgs args)
        {
            switch (args.Status)
            {
                case PositionStatus.Disabled:
                    this.PositionError.TryInvoke(
                        sender, 
                        new PositionErrorEventArgs(GeolocationError.Unauthorized));
                    break;
                case PositionStatus.Initializing:
                    break;
                case PositionStatus.NoData:
                    this.PositionError.TryInvoke(
                        sender, 
                        new PositionErrorEventArgs(GeolocationError.PositionUnavailable));
                    break;
                case PositionStatus.NotInitialized:
                    this.IsListening = false;
                    break;
                case PositionStatus.Ready:
                    this.IsListening = true;
                    break;
            }
        }
    }
}
