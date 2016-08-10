using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using XLabs.Platform.Services.Media;

namespace XLabs.Platform.Device
{
    public class AudioStream : IAudioStream
    {
        private MediaCapture _capture;
        private InMemoryRandomAccessStream _buffer;

        public event EventHandler<EventArgs<byte[]>> OnBroadcast;
        public int SampleRate { get; }
        public int ChannelCount { get; }
        public int BitsPerSample { get; }
        public IEnumerable<int> SupportedSampleRates { get; }

        public static bool Recording;

        public async Task<bool> Init(int sampleRate)
        {
            if (_buffer != null)
            {
                _buffer.Dispose();
            }

            _buffer = new InMemoryRandomAccessStream();
            if (_capture != null)
            {
                _capture.Dispose();
            }
            try
            {
                MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings
                {
                    StreamingCaptureMode = StreamingCaptureMode.Audio
                };
                _capture = new MediaCapture();

                await _capture.InitializeAsync(settings);

                _capture.RecordLimitationExceeded += async (MediaCapture sender) =>
                {
                    await Stop();
                    throw new Exception("Exceeded Record Limitation");
                };

                _capture.Failed += (MediaCapture sender, MediaCaptureFailedEventArgs errorEventArgs) =>
                {
                    Recording = false;
                    throw new Exception(string.Format("Code: {0}. {1}", errorEventArgs.Code, errorEventArgs.Message));
                };
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.GetType() == typeof(UnauthorizedAccessException))
                {
                    throw ex.InnerException;
                }
                throw;
            }
            return true;
        }

        public async Task<bool> Start(int sampleRate)
        {
            if (!await Init(sampleRate))
            {
                return false;
            }

            await _capture.StartRecordToStreamAsync(MediaEncodingProfile.CreateM4a(AudioEncodingQuality.Auto), _buffer);
            if (Recording) throw new InvalidOperationException("cannot excute two records at the same time");

            Recording = true;

            return Recording;
        }

        public async Task Stop()
        {
            await _capture.StopRecordAsync();
            Recording = false;
        }
    }
}
