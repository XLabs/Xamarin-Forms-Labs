namespace XLabs.Sample.ViewModel
{
	using System;
	using System.Threading.Tasks;
	using Platform.Services.Media;
	using Xamarin.Forms;

    /// <summary>
	/// Class SoundServiceViewModel.
	/// </summary>
	public class SoundServiceViewModel : XLabs.Forms.Mvvm.ViewModel
	{
		/// <summary>
		/// The _sound service
		/// </summary>
		private readonly ISoundService _soundService;
		/// <summary>
		/// The _play command
		/// </summary>
		private readonly Command _playCommand;

		/// <summary>
		/// The _duration
		/// </summary>
		private double _duration = 0.1;

		/// <summary>
		/// Initializes a new instance of the <see cref="SoundServiceViewModel"/> class.
		/// </summary>
		/// <param name="playCommand">The play command.</param>
		/// <exception cref="System.ArgumentNullException">musicservice;new Exception (Didn't find any music service implementation for current platform)</exception>
		/// <exception cref="System.Exception">Didn't find any music service implementation for current platform</exception>
		public SoundServiceViewModel (Command playCommand)
		{
			_playCommand = playCommand;
			_soundService = DependencyService.Get<ISoundService> ();
			if (_soundService == null)
				throw new ArgumentNullException ("musicservice", new Exception ("Didn't find any music service implementation for current platform"));

		}

		public SoundServiceViewModel () : this (null) { }

		/// <summary>
		/// Gets the play command.
		/// </summary>
		/// <value>The play command.</value>
		public Command PlayCommand {
			get {
				return _playCommand ??  new Command( async ()=>{
					await SetandPlayMp3();	
				});
			}
		}

		/// <summary>
		/// Gets or sets the duration.
		/// </summary>
		/// <value>The duration.</value>
		public double Duration {
			get{
				return _duration;
			}
			set {
				SetProperty (ref _duration, value);
				NotifyPropertyChanged ("DurationText");
			}
		}

		/// <summary>
		/// Gets the duration text.
		/// </summary>
		/// <value>The duration text.</value>
		public string DurationText {
			get{
				return TimeSpan.FromSeconds(_duration).ToString(@"mm\:ss");
			}
		}

		/// <summary>
		/// Setands the play MP3.
		/// </summary>
		/// <returns>Task.</returns>
		private async Task SetandPlayMp3(){
			var mediafile = await _soundService.PlayAsync ("BusyEarnin.mp3");
			Duration = mediafile.Duration.TotalSeconds;
		}
	}
}

