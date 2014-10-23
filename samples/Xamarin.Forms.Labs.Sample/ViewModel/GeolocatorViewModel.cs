using Xamarin.Forms.Labs.Services.Geolocation;
using Xamarin.Forms.Labs.Mvvm;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Sample
{
    /// <summary>
    /// The Geo-locator view model.
    /// </summary>
    [ViewType(typeof(GeolocatorPage))]
	public class GeolocatorViewModel : ViewModel
	{
        private readonly TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
		private IGeolocator geolocator;
		private CancellationTokenSource cancelSource;
        private string positionStatus = string.Empty;
		private string positionLatitude = string.Empty;
        private string positionLongitude = string.Empty;
        private Command getPositionCommand;

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
                    (getPositionCommand = new Command(async () => { await GetPosition(); }, () => true)); 
			}
		}

        private void Setup()
        {
		    if (this.geolocator != null)
		    {
		        return;
		    }
		        
		    this.geolocator = DependencyService.Get<IGeolocator>();
		    this.geolocator.PositionError += OnListeningError;
		    this.geolocator.PositionChanged += OnPositionChanged;
		}

        private async Task GetPosition()
        {
            Setup();

			this.cancelSource = new CancellationTokenSource();

			PositionStatus = string.Empty;
			PositionLatitude = string.Empty;
			PositionLongitude = string.Empty;
            IsBusy = true;
            await
                this.geolocator.GetPositionAsync(10000, this.cancelSource.Token, true)
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

