using Xamarin.Forms.Labs.Services.Geolocation;
using Xamarin.Forms.Labs.Mvvm;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Labs.Services.Email;
using System.Windows.Input;
using System.Linq;
using Xamarin.Forms.Labs.Services;
using XLabs.Ioc;

namespace Xamarin.Forms.Labs.Sample
{
    /// <summary>
    /// The Geo-locator view model.
    /// </summary>
    [ViewType(typeof(GeolocatorPage))]
    public class GeolocatorViewModel : Xamarin.Forms.Labs.Mvvm.ViewModel
    {
        private readonly TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        private IGeolocator geolocator;
        private IEmailService emailService;
        private IPhoneService phoneService;
        private IDevice device;
        private CancellationTokenSource cancelSource;
        private string positionStatus = string.Empty;
        private string positionLatitude = string.Empty;
        private string positionLongitude = string.Empty;
        private Command getPositionCommand;
        private ICommand sendPositionEmail;
        private ICommand sendPositionSms;
        private ICommand openPositionUri;
        private ICommand getDrivingDirections;

        /// <summary>
        /// Gets or sets the position status.
        /// </summary>
        /// <value>
        /// The position status.
        /// </value>
        public string PositionStatus
        {
            get
            {
                return positionStatus;
            }
            set
            {
                this.SetProperty(ref positionStatus, value);
            }
        }

        /// <summary>
        /// Gets or sets the position latitude.
        /// </summary>
        /// <value>
        /// The position latitude.
        /// </value>
        public string PositionLatitude
        {
            get
            {
                return positionLatitude;
            }
            set
            {
                this.SetProperty(ref positionLatitude, value);
            }
        }

        /// <summary>
        /// Gets or sets the position longitude.
        /// </summary>
        /// <value>
        /// The position longitude.
        /// </value>
        public string PositionLongitude
        {
            get
            {
                return positionLongitude;
            }
            set
            {
                this.SetProperty(ref positionLongitude, value);
            }
        }

        /// <summary>
        /// Gets the get position command.
        /// </summary>
        /// <value>
        /// The get position command.
        /// </value>
        public Command GetPositionCommand 
        {
            get
            { 
                return getPositionCommand ??
                    (getPositionCommand = new Command(async () => await GetPosition(), () => this.Geolocator != null)); 
            }
        }

        public ICommand SendPositionSms
        {
            get
            {
                return sendPositionSms ??
                    (sendPositionSms = new Command(
                        async () => await this.SmsPosition(),
                        () => this.PhoneService != null && this.PhoneService.CanSendSMS));
            }
        }

        public ICommand SendPositionEmail
        {
            get
            {
                return sendPositionEmail ??
                    (sendPositionEmail = new Command(
                        async () => await this.EmailPosition(),
                        () => this.EmailService != null && this.EmailService.CanSend));
            }
        }

        public ICommand OpenPositionUri
        {
            get
            {
                return openPositionUri ??
                    (openPositionUri = new Command(
                        async () => 
                            {
                                var pos = await this.Geolocator.GetPositionAsync(5000);
                                var uri = Device.OnPlatform(
                                    pos.ToAppleMaps(),
                                    pos.ToUri(),
                                    pos.ToBingMaps());
                                Device.OpenUri(uri);
                            },
                        () => this.Geolocator != null));
            }
        }

        public ICommand GetDrivingDirections
        {
            get
            {
                return this.getDrivingDirections ??
                    (this.getDrivingDirections = new Command(
                        async () =>
                        {
                            var pos = await this.Geolocator.GetPositionAsync(5000);
                            var uri = Device.OnPlatform(
                                pos.ToAppleMaps(),
                                pos.ToUri(),
                                pos.DriveToLink());
                            await LabsDevice.LaunchUriAsync(uri);
                        },
                        () => this.Geolocator != null && this.LabsDevice != null));
            }
        }

        private IDevice LabsDevice
        {
            get
            {
                return this.device ?? (this.device = Resolver.Resolve<IDevice>());
            }
        }

        private IEmailService EmailService
        {
            get
            {
                return this.emailService ?? (this.emailService = DependencyService.Get<IEmailService>());
            }
        }

        private IPhoneService PhoneService
        {
            get
            {
                return this.phoneService ?? (this.phoneService = DependencyService.Get<IPhoneService>());
            }
        }

        private IGeolocator Geolocator
        {
            get
            {
                if (this.geolocator == null)
                {
                    this.geolocator = DependencyService.Get<IGeolocator>();
                    this.geolocator.PositionError += OnListeningError;
                    this.geolocator.PositionChanged += OnPositionChanged;
                }
                return this.geolocator;
            }
        }

        //private void Setup()
        //{
        //    if (this.geolocator != null)
        //    {
        //        return;
        //    }
                
        //    this.geolocator = DependencyService.Get<IGeolocator>();
        //    this.geolocator.PositionError += OnListeningError;
        //    this.geolocator.PositionChanged += OnPositionChanged;
        //}

        private async Task GetPosition()
        {
            this.cancelSource = new CancellationTokenSource();

            PositionStatus = string.Empty;
            PositionLatitude = string.Empty;
            PositionLongitude = string.Empty;
            IsBusy = true;
            await
                this.Geolocator.GetPositionAsync(10000, this.cancelSource.Token, true)
                    .ContinueWith(t =>
                    {
                        IsBusy = false;
                        if (t.IsFaulted)
                        {
                            this.PositionStatus = ((GeolocationException) t.Exception.InnerException).Error.ToString();
                        }
                        else if (t.IsCanceled)
                        {
                            this.PositionStatus = "Canceled";
                        }
                        else
                        {
                            this.PositionStatus = t.Result.Timestamp.ToString("G");
                            PositionLatitude = "La: " + t.Result.Latitude.ToString("N4");
                            PositionLongitude = "Lo: " + t.Result.Longitude.ToString("N4");
                        }
                    }, scheduler);
        }

        private async Task EmailPosition(int timeout = 5000)
        {
            var position = await this.Geolocator.GetPositionAsync(timeout);

            var builder = new System.Text.StringBuilder();
            builder.Append("WP8 link: ");
            builder.AppendLine(position.ToBingMaps().ToString());
            builder.Append("Android link: ");
            builder.AppendLine(position.ToGoogleMaps().ToString());
            builder.Append("iOS link: ");
            builder.AppendLine(position.ToAppleMaps().ToString());
            this.EmailService.ShowDraft(
                "My position",
                //string.Format("<html><head><title>My location during {1}.</title></head><body><a href=\"{0}\"> Timezone: {2}.</a></body></html>", position.ToUri().ToString(), position.Timestamp, System.TimeZoneInfo.Local.DisplayName), 
                builder.ToString(),
                true, 
                string.Empty, 
                Enumerable.Empty<string>());
        }

        private async Task SmsPosition(int timeout = 5000)
        {
            var position = await this.Geolocator.GetPositionAsync(timeout);

            this.PhoneService.SendSMS(string.Empty, position.ToUri().ToString());

            Device.OpenUri(position.ToUri());

        }

        ////private void CancelPosition ()
        ////{
        ////    CancellationTokenSource cancel = this.cancelSource;
        ////    if (cancel != null)
        ////        cancel.Cancel();
        ////}

////		partial void ToggleListening (NSObject sender)
////		{
////			Setup();
////
////			if (!this.geolocator.IsListening)
////			{
////				ToggleListeningButton.SetTitle ("Stop listening", UIControlState.Normal);
////
////				this.geolocator.StartListening (minTime: 30000, minDistance: 0, includeHeading: true);
////			}
////			else
////			{
////				ToggleListeningButton.SetTitle ("Start listening", UIControlState.Normal);
////				this.geolocator.StopListening();
////			}
////		}

        private void OnListeningError(object sender, PositionErrorEventArgs e)
        {
////			BeginInvokeOnMainThread (() => {
////				ListenStatus.Text = e.Error.ToString();
////			});
        }

        private void OnPositionChanged(object sender, PositionEventArgs e)
        {
////			BeginInvokeOnMainThread (() => {
////				ListenStatus.Text = e.Position.Timestamp.ToString("G");
////				ListenLatitude.Text = "La: " + e.Position.Latitude.ToString("N4");
////				ListenLongitude.Text = "Lo: " + e.Position.Longitude.ToString("N4");
////			});
        }
    }
}

