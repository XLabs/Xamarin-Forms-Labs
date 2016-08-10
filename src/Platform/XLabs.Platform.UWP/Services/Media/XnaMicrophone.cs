
//using Windows.Media.Capture;

//namespace XLabs.Platform.Services.Media
//{
//	using System;
//	using System.Collections.Generic;
//	using System.Threading.Tasks;
//	using Windows.UI.Xaml;


//	/// <summary>
//	/// Class XnaMicrophone.
//	/// </summary>
//	public class XnaMicrophone : IAudioStream
//	{
//		/// <summary>
//		/// The _microphone
//		/// </summary>
//		private static readonly MediaCapture _mediaCapture = new MediaCapture();

//		/// <summary>
//		/// Initializes a new instance of the <see cref="XnaMicrophone"/> class.
//		/// </summary>
//		public XnaMicrophone()
//		{
//		}

//		/// <summary>
//		/// Gets a value indicating whether this instance is available.
//		/// </summary>
//		/// <value><c>true</c> if this instance is available; otherwise, <c>false</c>.</value>
//		public static bool IsAvailable
//		{
//			get
//			{
//				return _mediaCapture.MediaCaptureSettings.AudioDeviceId != null;
//			}
//		}

//		/// <summary>
//		/// Gets the sample rate.
//		/// </summary>
//		/// <value>The sample rate in hertz.</value>
//		public int SampleRate
//		{
//			get { return 0; }
//		}

//		/// <summary>
//		/// Gets the channel count.
//		/// </summary>
//		/// <value>The channel count.</value>
//		public int ChannelCount
//		{
//			get
//			{
//				return 1;
//			}
//		}

//		/// <summary>
//		/// Gets bits per sample.
//		/// </summary>
//		/// <value>The bits per sample.</value>
//		public int BitsPerSample
//		{
//			get
//			{
//				return 16;
//			}
//		}

//		/// <summary>
//		/// Gets a value indicating whether this <see cref="XnaMicrophone"/> is active.
//		/// </summary>
//		/// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
//		public bool Active
//		{
//			get
//			{
//				return _microphone != null && _microphone.State == MicrophoneState.Started;
//			}
//		}

//		/// <summary>
//		/// Gets the average data transfer rate
//		/// </summary>
//		/// <value>The average data transfer rate in bytes per second.</value>
//		public IEnumerable<int> SupportedSampleRates
//		{
//			get
//			{
//				return new[] { 16000 };
//			}
//		}

//		/// <summary>
//		/// Occurs when new audio has been streamed.
//		/// </summary>
//		public event EventHandler<EventArgs<byte[]>> OnBroadcast;

//		/// <summary>
//		/// Microphones the buffer ready.
//		/// </summary>
//		/// <param name="sender">The sender.</param>
//		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
//		private void MicrophoneBufferReady(object sender, EventArgs e)
//		{
//			var buffer = new byte[_microphone.GetSampleSizeInBytes(_microphone.BufferDuration)];
//			int read;

//			do
//			{
//				read = _microphone.GetData(buffer, 0, buffer.Length);
//				OnBroadcast.Invoke<byte[]>(this, buffer);
//			}
//			while (read > 0);
//		}

//		#region IAudioStream Members

//		/// <summary>
//		/// Starts the specified sample rate.
//		/// </summary>
//		/// <param name="sampleRate">The sample rate.</param>
//		/// <returns>Task&lt;System.Boolean&gt;.</returns>
//		public Task<bool> Start(int sampleRate)
//		{
//			return Task.Run(
//				() =>
//					{
//						try
//						{
//							_timer.Start();
//							_microphone.BufferReady += MicrophoneBufferReady;
//							_microphone.Start();
//							return true;
//						}
//						catch
//						{
//							return false;
//						}
//					});
//		}

//		/// <summary>
//		/// Stops this instance.
//		/// </summary>
//		/// <returns>Task.</returns>
//		public Task Stop()
//		{
//			return Task.Run(
//				() =>
//					{
//						_microphone.BufferReady -= MicrophoneBufferReady;
//						_microphone.Stop();
//						_timer.Stop();
//					});
//		}

//		#endregion
//	}
//}