using System;
using XForms.Toolkit.Services.Geolocation;
using XForms.Toolkit.Mvvm;
using Xamarin.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace XForms.Toolkit.Sample
{
	[ViewType(typeof(GeolocatorPage))]
	public class GeolocatorViewModel : ViewModel
	{
		private IGeolocator geolocator;
		private TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
		private CancellationTokenSource cancelSource;

		public GeolocatorViewModel ()
		{

		}
		private string _positionStatus = string.Empty;
		public string PositionStatus
		{
			get
			{
				return _positionStatus;
			}
			set
			{
				this.ChangeAndNotify(ref _positionStatus, value);
			}
		}
		private string _positionLatitude = string.Empty;
		public string PositionLatitude
		{
			get
			{
				return _positionLatitude;
			}
			set
			{
				this.ChangeAndNotify(ref _positionLatitude, value);
			}
		}
		private string _positionLongitude = string.Empty;
		public string PositionLongitude
		{
			get
			{
				return _positionLongitude;
			}
			set
			{
				this.ChangeAndNotify(ref _positionLongitude, value);
			}
		}

		private RelayCommand _getPositionCommand;
		public RelayCommand GetPositionCommand 
		{
			get
			{ 
				return _getPositionCommand ?? new RelayCommand (
					async () => { await GetPosition(); }, 
					() => true); 
			}
		}

		void Setup(){
			if (this.geolocator != null)
				return;

			this.geolocator = DependencyService.Get<IGeolocator> ();
			this.geolocator.PositionError += OnListeningError;
			this.geolocator.PositionChanged += OnPositionChanged;
		}
		async Task GetPosition ()
		{
			Setup();

			this.cancelSource = new CancellationTokenSource();

			PositionStatus = String.Empty;
			PositionLatitude = String.Empty;
			PositionLongitude = String.Empty;

			await this.geolocator.GetPositionAsync (timeout: 10000, cancelToken: this.cancelSource.Token, includeHeading: true)
				.ContinueWith (t =>
					{
						if (t.IsFaulted)
							PositionStatus = ((GeolocationException)t.Exception.InnerException).Error.ToString();
						else if (t.IsCanceled)
							PositionStatus = "Canceled";
						else
						{
							PositionStatus = t.Result.Timestamp.ToString("G");
							PositionLatitude = "La: " + t.Result.Latitude.ToString("N4");
							PositionLongitude = "Lo: " + t.Result.Longitude.ToString("N4");
						}

					}, scheduler);
		}

		void CancelPosition ()
		{
			CancellationTokenSource cancel = this.cancelSource;
			if (cancel != null)
				cancel.Cancel();
		}

//		partial void ToggleListening (NSObject sender)
//		{
//			Setup();
//
//			if (!this.geolocator.IsListening)
//			{
//				ToggleListeningButton.SetTitle ("Stop listening", UIControlState.Normal);
//
//				this.geolocator.StartListening (minTime: 30000, minDistance: 0, includeHeading: true);
//			}
//			else
//			{
//				ToggleListeningButton.SetTitle ("Start listening", UIControlState.Normal);
//				this.geolocator.StopListening();
//			}
//		}

		private void OnListeningError (object sender, PositionErrorEventArgs e)
		{
//			BeginInvokeOnMainThread (() => {
//				ListenStatus.Text = e.Error.ToString();
//			});
		}

		private void OnPositionChanged (object sender, PositionEventArgs e)
		{
//			BeginInvokeOnMainThread (() => {
//				ListenStatus.Text = e.Position.Timestamp.ToString("G");
//				ListenLatitude.Text = "La: " + e.Position.Latitude.ToString("N4");
//				ListenLongitude.Text = "Lo: " + e.Position.Longitude.ToString("N4");
//			});
		}
	}
}

