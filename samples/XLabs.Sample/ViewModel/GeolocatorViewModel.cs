namespace XLabs.Sample.ViewModel
{
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Windows.Input;

	using Xamarin.Forms;

	using XLabs.Forms.Mvvm;
	using XLabs.Ioc;
	using XLabs.Platform;
	using XLabs.Platform.Device;
	using XLabs.Platform.Services;
	using XLabs.Platform.Services.Email;
	using XLabs.Platform.Services.Geolocation;
	using XLabs.Sample.Pages.Services;

	/// <summary>
	/// The Geo-locator view model.
	/// </summary>
	[ViewType(typeof(GeolocatorPage))]
	public class GeolocatorViewModel : ViewModel
	{
		/// <summary>
		/// The scheduler
		/// </summary>
		private readonly TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();
		/// <summary>
		/// The geolocator
		/// </summary>
		private IGeolocator _geolocator;
		/// <summary>
		/// The email service
		/// </summary>
		private IEmailService _emailService;
		/// <summary>
		/// The phone service
		/// </summary>
		private IPhoneService _phoneService;
		/// <summary>
		/// The device
		/// </summary>
		private IDevice _device;
		/// <summary>
		/// The cancel source
		/// </summary>
		private CancellationTokenSource _cancelSource;
		/// <summary>
		/// The position status
		/// </summary>
		private string _positionStatus = string.Empty;
		/// <summary>
		/// The position latitude
		/// </summary>
		private string _positionLatitude = string.Empty;
		/// <summary>
		/// The position longitude
		/// </summary>
		private string _positionLongitude = string.Empty;
		/// <summary>
		/// The get position command
		/// </summary>
		private Command _getPositionCommand;
		/// <summary>
		/// The send position email
		/// </summary>
		private ICommand _sendPositionEmail;
		/// <summary>
		/// The send position SMS
		/// </summary>
		private ICommand _sendPositionSms;
		/// <summary>
		/// The open position URI
		/// </summary>
		private ICommand _openPositionUri;
		/// <summary>
		/// The get driving directions
		/// </summary>
		private ICommand _getDrivingDirections;

		/// <summary>
		/// Gets or sets the position status.
		/// </summary>
		/// <value>The position status.</value>
		public string PositionStatus
		{
			get
			{
				return _positionStatus;
			}
			set
			{
				SetProperty(ref _positionStatus, value);
			}
		}

		/// <summary>
		/// Gets or sets the position latitude.
		/// </summary>
		/// <value>The position latitude.</value>
		public string PositionLatitude
		{
			get
			{
				return _positionLatitude;
			}
			set
			{
				SetProperty(ref _positionLatitude, value);
			}
		}

		/// <summary>
		/// Gets or sets the position longitude.
		/// </summary>
		/// <value>The position longitude.</value>
		public string PositionLongitude
		{
			get
			{
				return _positionLongitude;
			}
			set
			{
				SetProperty(ref _positionLongitude, value);
			}
		}

		/// <summary>
		/// Gets the get position command.
		/// </summary>
		/// <value>The get position command.</value>
		public Command GetPositionCommand 
		{
			get
			{ 
				return _getPositionCommand ??
					(_getPositionCommand = new Command(async () => await GetPosition(), () => Geolocator != null)); 
			}
		}

		/// <summary>
		/// Gets the send position SMS.
		/// </summary>
		/// <value>The send position SMS.</value>
		public ICommand SendPositionSms
		{
			get
			{
				return _sendPositionSms ??
					(_sendPositionSms = new Command(
						async () => await SmsPosition(),
						() => PhoneService != null && PhoneService.CanSendSMS));
			}
		}

		/// <summary>
		/// Gets the send position email.
		/// </summary>
		/// <value>The send position email.</value>
		public ICommand SendPositionEmail
		{
			get
			{
				return _sendPositionEmail ??
					(_sendPositionEmail = new Command(
						async () => await EmailPosition(),
						() => EmailService != null && EmailService.CanSend));
			}
		}

		/// <summary>
		/// Gets the open position URI.
		/// </summary>
		/// <value>The open position URI.</value>
		public ICommand OpenPositionUri
		{
			get
			{
				return _openPositionUri ??
					(_openPositionUri = new Command(
						async () => 
							{
								var pos = await Geolocator.GetPositionAsync(5000);
								var uri = Device.OnPlatform(
									pos.ToAppleMaps(),
									pos.ToUri(),
									pos.ToBingMaps());
								Device.OpenUri(uri);
							},
						() => Geolocator != null));
			}
		}

		/// <summary>
		/// Gets the get driving directions.
		/// </summary>
		/// <value>The get driving directions.</value>
		public ICommand GetDrivingDirections
		{
			get
			{
				return _getDrivingDirections ??
					(_getDrivingDirections = new Command(
						async () =>
						{
							var pos = await Geolocator.GetPositionAsync(5000);
							var uri = Device.OnPlatform(
								pos.ToAppleMaps(),
								pos.ToUri(),
								pos.DriveToLink());
							await LabsDevice.LaunchUriAsync(uri);
						},
						() => Geolocator != null && LabsDevice != null));
			}
		}

		/// <summary>
		/// Gets the labs device.
		/// </summary>
		/// <value>The labs device.</value>
		private IDevice LabsDevice
		{
			get
			{
				return _device ?? (_device = Resolver.Resolve<IDevice>());
			}
		}

		/// <summary>
		/// Gets the email service.
		/// </summary>
		/// <value>The email service.</value>
		private IEmailService EmailService
		{
			get
			{
				return _emailService ?? (_emailService = DependencyService.Get<IEmailService>());
			}
		}

		/// <summary>
		/// Gets the phone service.
		/// </summary>
		/// <value>The phone service.</value>
		private IPhoneService PhoneService
		{
			get
			{
				return _phoneService ?? (_phoneService = DependencyService.Get<IPhoneService>());
			}
		}

		/// <summary>
		/// Gets the geolocator.
		/// </summary>
		/// <value>The geolocator.</value>
		private IGeolocator Geolocator
		{
			get
			{
				if (_geolocator == null)
				{
					_geolocator = DependencyService.Get<IGeolocator>();
					_geolocator.PositionError += OnListeningError;
					_geolocator.PositionChanged += OnPositionChanged;
				}
				return _geolocator;
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

		/// <summary>
		/// Gets the position.
		/// </summary>
		/// <returns>Task.</returns>
		private async Task GetPosition()
		{
			_cancelSource = new CancellationTokenSource();

			PositionStatus = string.Empty;
			PositionLatitude = string.Empty;
			PositionLongitude = string.Empty;
			IsBusy = true;
			await
				Geolocator.GetPositionAsync(10000, _cancelSource.Token, true)
					.ContinueWith(t =>
					{
						IsBusy = false;
						if (t.IsFaulted)
						{
							PositionStatus = ((GeolocationException) t.Exception.InnerException).Error.ToString();
						}
						else if (t.IsCanceled)
						{
							PositionStatus = "Canceled";
						}
						else
						{
							PositionStatus = t.Result.Timestamp.ToString("G");
							PositionLatitude = "La: " + t.Result.Latitude.ToString("N4");
							PositionLongitude = "Lo: " + t.Result.Longitude.ToString("N4");
						}
					}, _scheduler);
		}

		/// <summary>
		/// Emails the position.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <returns>Task.</returns>
		private async Task EmailPosition(int timeout = 5000)
		{
			var position = await Geolocator.GetPositionAsync(timeout);

			var builder = new System.Text.StringBuilder();
			builder.Append("WP8 link: ");
			builder.AppendLine(position.ToBingMaps().ToString());
			builder.Append("Android link: ");
			builder.AppendLine(position.ToGoogleMaps().ToString());
			builder.Append("iOS link: ");
			builder.AppendLine(position.ToAppleMaps().ToString());
			EmailService.ShowDraft(
				"My position",
				//string.Format("<html><head><title>My location during {1}.</title></head><body><a href=\"{0}\"> Timezone: {2}.</a></body></html>", position.ToUri().ToString(), position.Timestamp, System.TimeZoneInfo.Local.DisplayName), 
				builder.ToString(),
				true, 
				string.Empty, 
				Enumerable.Empty<string>());
		}

		/// <summary>
		/// SMSs the position.
		/// </summary>
		/// <param name="timeout">The timeout.</param>
		/// <returns>Task.</returns>
		private async Task SmsPosition(int timeout = 5000)
		{
			var position = await Geolocator.GetPositionAsync(timeout);

			PhoneService.SendSMS(string.Empty, position.ToUri().ToString());

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

		/// <summary>
		/// Handles the <see cref="E:ListeningError" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="PositionErrorEventArgs"/> instance containing the event data.</param>
		private void OnListeningError(object sender, PositionErrorEventArgs e)
		{
////			BeginInvokeOnMainThread (() => {
////				ListenStatus.Text = e.Error.ToString();
////			});
		}

		/// <summary>
		/// Handles the <see cref="E:PositionChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="PositionEventArgs"/> instance containing the event data.</param>
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

