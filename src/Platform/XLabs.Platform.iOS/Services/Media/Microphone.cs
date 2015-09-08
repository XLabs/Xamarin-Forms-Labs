namespace XLabs.Platform.Services.Media
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.InteropServices;
	using System.Threading.Tasks;

	using AudioToolbox;

	/// <summary>
	/// Class Microphone.
	/// </summary>
	public class Microphone : IAudioStream
	{
		/// <summary>
		/// The _audio queue
		/// </summary>
		private InputAudioQueue _audioQueue;

		/// <summary>
		/// The _buffer size
		/// </summary>
		private readonly int _bufferSize;

		/// <summary>
		/// Initializes a new instance of the <see cref="Microphone"/> class.
		/// </summary>
		/// <param name="bufferSize">Size of the buffer.</param>
		public Microphone(int bufferSize = 4098)
		{
			_bufferSize = bufferSize;
		}

		/// <summary>
		/// Starts the recording.
		/// </summary>
		/// <param name="rate">The rate.</param>
		private void StartRecording(int rate)
		{
			if (Active)
			{
				Clear();
			}

			SampleRate = rate;

			var audioFormat = new AudioStreamBasicDescription
								  {
									  SampleRate = SampleRate,
									  Format = AudioFormatType.LinearPCM,
									  FormatFlags =
										  AudioFormatFlags.LinearPCMIsSignedInteger
										  | AudioFormatFlags.LinearPCMIsPacked,
									  FramesPerPacket = 1,
									  ChannelsPerFrame = 1,
									  BitsPerChannel = BitsPerSample,
									  BytesPerPacket = 2,
									  BytesPerFrame = 2,
									  Reserved = 0
								  };

			_audioQueue = new InputAudioQueue(audioFormat);
			_audioQueue.InputCompleted += QueueInputCompleted;

			var bufferByteSize = _bufferSize * audioFormat.BytesPerPacket;

			IntPtr bufferPtr;
			for (var index = 0; index < 3; index++)
			{
				_audioQueue.AllocateBufferWithPacketDescriptors(bufferByteSize, _bufferSize, out bufferPtr);
				_audioQueue.EnqueueBuffer(bufferPtr, bufferByteSize, null);
			}

			_audioQueue.Start();
		}

		/// <summary>
		/// Clears this instance.
		/// </summary>
		private void Clear()
		{
			if (_audioQueue != null)
			{
				_audioQueue.Stop(true);
				_audioQueue.InputCompleted -= QueueInputCompleted;
				_audioQueue.Dispose();
				_audioQueue = null;
			}
		}

		/// <summary>
		/// Handles iOS audio buffer queue completed message.
		/// </summary>
		/// <param name="sender">Sender object</param>
		/// <param name="e">Input completed parameters.</param>
		private void QueueInputCompleted(object sender, InputCompletedEventArgs e)
		{
			// return if we aren't actively monitoring audio packets
			if (!Active)
			{
				return;
			}

			var buffer = (AudioQueueBuffer)Marshal.PtrToStructure(e.IntPtrBuffer, typeof(AudioQueueBuffer));
			if (OnBroadcast != null)
			{
				var send = new byte[buffer.AudioDataByteSize];
				Marshal.Copy(buffer.AudioData, send, 0, (int)buffer.AudioDataByteSize);

				OnBroadcast(this, new EventArgs<byte[]>(send));
			}

			var status = _audioQueue.EnqueueBuffer(e.IntPtrBuffer, _bufferSize, e.PacketDescriptions);

			if (status != AudioQueueStatus.Ok)
			{
				// todo: 
			}
		}

		#region IAudioStream implementation

		/// <summary>
		/// Occurs when new audio has been streamed.
		/// </summary>
		public event EventHandler<EventArgs<byte[]>> OnBroadcast;

		/// <summary>
		/// Gets the sample rate.
		/// </summary>
		/// <value>The sample rate in hertz.</value>
		public int SampleRate { get; private set; }

		/// <summary>
		/// Gets the channel count.
		/// </summary>
		/// <value>The channel count.</value>
		public int ChannelCount
		{
			get
			{
				return 1;
			}
		}

		/// <summary>
		/// Gets bits per sample.
		/// </summary>
		/// <value>The bits per sample.</value>
		public int BitsPerSample
		{
			get
			{
				return 16;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Microphone"/> is active.
		/// </summary>
		/// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
		public bool Active
		{
			get
			{
				return _audioQueue != null && _audioQueue.IsRunning;
			}
		}

		/// <summary>
		/// Gets the average data transfer rate
		/// </summary>
		/// <value>The average data transfer rate in bytes per second.</value>
		public IEnumerable<int> SupportedSampleRates
		{
			get
			{
				return new[] { 8000, 16000, 22050, 41000, 44100 };
			}
		}

		/// <summary>
		/// Starts the specified sample rate.
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

						StartRecording(sampleRate);

						return Active;
					});
		}

		/// <summary>
		/// Stops this instance.
		/// </summary>
		/// <returns>Task.</returns>
		public Task Stop()
		{
			return Task.Run(() => Clear());
		}

		#endregion
	}
}