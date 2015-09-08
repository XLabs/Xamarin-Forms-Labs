namespace XLabs.Platform.Services.Media
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using Android.App;
	using Android.Content.PM;
	using Android.Media;

	/// <summary>
	///     Class Microphone.
	/// </summary>
	public class Microphone : IAudioStream
	{
		/// <summary>
		///     The audio source.
		/// </summary>
		private AudioRecord _audioSource;

		/// <summary>
		///     The _buffer size
		/// </summary>
		private int _bufferSize;

		/// <summary>
		///     Initializes a new instance of the <see cref="Microphone" /> class.
		/// </summary>
		public Microphone()
		{
			SupportedSampleRates =
				(new[] { 8000, 11025, 16000, 22050, 44100 }).Where(
					rate => AudioRecord.GetMinBufferSize(rate, ChannelIn.Mono, Encoding.Pcm16bit) > 0).ToList();
		}

		/// <summary>
		///     Gets a value indicating whether this instance is enabled.
		/// </summary>
		/// <value><c>true</c> if this instance is enabled; otherwise, <c>false</c>.</value>
		public static bool IsEnabled
		{
			get
			{
				return Application.Context.PackageManager.HasSystemFeature(PackageManager.FeatureMicrophone)
				       && Application.Context.PackageManager.CheckPermission(
					       "android.permission.RECORD_AUDIO",
					       Application.Context.PackageName) == Permission.Granted;
			}
		}

		/// <summary>
		///     Gets the sample rate.
		/// </summary>
		/// <value>The sample rate.</value>
		public int SampleRate
		{
			get
			{
				return _audioSource == null ? -1 : _audioSource.SampleRate;
			}
		}

		/// <summary>
		///     Gets bits per sample.
		/// </summary>
		/// <value>The bits per sample.</value>
		public int BitsPerSample
		{
			get
			{
				return _audioSource == null ? -1 : (_audioSource.AudioFormat == Encoding.Pcm16bit) ? 16 : 8;
			}
		}

		/// <summary>
		///     Gets the channel count.
		/// </summary>
		/// <value>The channel count.</value>
		public int ChannelCount
		{
			get
			{
				return _audioSource == null ? -1 : _audioSource.ChannelCount;
			}
		}

		/// <summary>
		///     Gets the average data transfer rate
		/// </summary>
		/// <value>The average data transfer rate in bytes per second.</value>
		public int AverageBytesPerSecond
		{
			get
			{
				return _audioSource == null ? -1 : SampleRate * BitsPerSample / 8 * ChannelCount;
			}
		}

		/// <summary>
		///     Gets a value indicating whether this <see cref="Microphone" /> is active.
		/// </summary>
		/// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
		public bool Active
		{
			get
			{
				return (_audioSource != null && _audioSource.RecordingState == RecordState.Recording);
			}
		}

		/// <summary>
		///     Gets the average data transfer rate
		/// </summary>
		/// <value>The average data transfer rate in bytes per second.</value>
		public IEnumerable<int> SupportedSampleRates { get; private set; }

		/// <summary>
		///     Occurs when new audio has been streamed.
		/// </summary>
		public event EventHandler<EventArgs<byte[]>> OnBroadcast;

		/// <summary>
		///     Starts the specified sample rate.
		/// </summary>
		/// <param name="sampleRate">The sample rate.</param>
		/// <returns>Task&lt;System.Boolean&gt;.</returns>
		public Task<bool> Start(int sampleRate)
		{
			return Task.Run(
				() =>
					{
						if (!SupportedSampleRates.Contains(sampleRate))
						{
							return false;
						}

						_bufferSize = AudioRecord.GetMinBufferSize(sampleRate, ChannelIn.Mono, Encoding.Pcm16bit);

						_audioSource = new AudioRecord(AudioSource.Mic, sampleRate, ChannelIn.Mono, Encoding.Pcm16bit, _bufferSize);

						StartRecording();

						return true;
					});
		}

		/// <summary>
		///     Stops this instance.
		/// </summary>
		/// <returns>Task.</returns>
		public Task Stop()
		{
			return Task.Run(
				() =>
					{
						_audioSource.Stop();
						_audioSource = null;
					});
		}

		/// <summary>
		///     Start recording from the hardware audio source.
		/// </summary>
		private void StartRecording()
		{
			_audioSource.StartRecording();

			Task.Run(
				async () =>
					{
						do
						{
							await Record();
						}
						while (Active);
					});
		}

		/// <summary>
		///     Record from the microphone and broadcast the buffer.
		/// </summary>
		/// <returns>Task.</returns>
		private async Task Record()
		{
			var buffer = new byte[_bufferSize];

			var readCount = await _audioSource.ReadAsync(buffer, 0, _bufferSize);

			OnBroadcast.Invoke<byte[]>(this, buffer);
		}
	}
}