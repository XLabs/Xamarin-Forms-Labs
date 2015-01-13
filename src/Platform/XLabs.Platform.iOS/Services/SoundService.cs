namespace XLabs.Platform.Services
{
	using System;
	using System.Threading.Tasks;

	using AVFoundation;
	using Foundation;

	using XLabs.Platform.Services.Sound;

	/// <summary>
	/// Class SoundService.
	/// </summary>
	public class SoundService : ISoundService
	{
		/// <summary>
		/// The _is scrubbing
		/// </summary>
		private bool _isScrubbing;

		/// <summary>
		/// The _player
		/// </summary>
		private AVAudioPlayer _player;

		/// <summary>
		/// Initializes a new instance of the <see cref="SoundService"/> class.
		/// </summary>
		public SoundService()
		{
			IsPlaying = false;
			CurrentFile = null;
		}

		/// <summary>
		/// Gets or sets the volume.
		/// </summary>
		/// <value>The volume.</value>
		public double Volume
		{
			get
			{
				if (_player == null)
				{
					return 0.5;
				}
				return _player.Volume;
			}
			set
			{
				if (_player != null)
				{
					_player.Volume = (float)value;
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is playing.
		/// </summary>
		/// <value><c>true</c> if this instance is playing; otherwise, <c>false</c>.</value>
		public bool IsPlaying { get; private set; }

		/// <summary>
		/// Gets the current time.
		/// </summary>
		/// <value>The current time.</value>
		public double CurrentTime
		{
			get
			{
				if (_player == null)
				{
					return 0;
				}
				return _player.CurrentTime;
			}
		}

		/// <summary>
		/// Gets the current file.
		/// </summary>
		/// <value>The current file.</value>
		public SoundFile CurrentFile { get; private set; }

		/// <summary>
		/// Plays this instance.
		/// </summary>
		public void Play()
		{
			if (_player != null && !IsPlaying)
			{
				_player.Play();
				IsPlaying = true;
			}
		}

		/// <summary>
		/// Stops this instance.
		/// </summary>
		public void Stop()
		{
			if (_player != null)
			{
				_player.Stop();
				IsPlaying = false;
				_player.CurrentTime = 0.0;
			}
		}

		/// <summary>
		/// Pauses this instance.
		/// </summary>
		public void Pause()
		{
			if (_player != null && IsPlaying)
			{
				_player.Pause();
				IsPlaying = false;
			}
		}

		/// <summary>
		/// Occurs when [sound file finished].
		/// </summary>
		public event EventHandler SoundFileFinished;

		/// <summary>
		/// Handles the <see cref="E:FileFinished" /> event.
		/// </summary>
		/// <param name="e">The <see cref="SoundFinishedEventArgs"/> instance containing the event data.</param>
		protected virtual void OnFileFinished(SoundFinishedEventArgs e)
		{
			if (SoundFileFinished != null)
			{
				SoundFileFinished(this, e);
			}
		}

		/// <summary>
		/// Sets the media asynchronous.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <returns>Task&lt;SoundFile&gt;.</returns>
		public Task<SoundFile> SetMediaAsync(string filename)
		{
			return Task.Run(
				() =>
					{
						CurrentFile = new SoundFile();
						CurrentFile.Filename = filename;
						var url = NSUrl.FromFilename(CurrentFile.Filename);
						_player = AVAudioPlayer.FromUrl(url);
						_player.FinishedPlaying += (object sender, AVStatusEventArgs e) =>
							{
								if (e.Status)
								{
									OnFileFinished(new SoundFinishedEventArgs(CurrentFile));
								}
							};
						CurrentFile.Duration = TimeSpan.FromSeconds(_player.Duration);
						return CurrentFile;
					});
		}

		/// <summary>
		/// Plays the asynchronous.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="extension">The extension.</param>
		/// <returns>Task&lt;SoundFile&gt;.</returns>
		public Task<SoundFile> PlayAsync(string filename, string extension = null)
		{
			return Task.Run<SoundFile>(
				async () =>
					{
						if (_player == null || string.Compare(filename, CurrentFile.Filename) > 0)
						{
							await SetMediaAsync(filename);
						}
						_player.Play();
						IsPlaying = true;
						return CurrentFile;
					});
		}

		/// <summary>
		/// Goes to asynchronous.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <returns>Task.</returns>
		public Task GoToAsync(double position)
		{
			return Task.Run(
				() =>
					{
						if (!_isScrubbing)
						{
							_isScrubbing = true;
							_player.CurrentTime = position;
							_isScrubbing = false;
						}
					});
		}
	}
}