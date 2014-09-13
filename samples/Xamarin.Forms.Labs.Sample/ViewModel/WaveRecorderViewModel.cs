using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms.Labs.Mvvm;
using Xamarin.Forms.Labs.Services;
using Xamarin.Forms.Labs.Services.Media;

namespace Xamarin.Forms.Labs.Sample
{
  public class WaveRecorderViewModel : Mvvm.ViewModels.ViewModel
  {
    private string fileName;
    private int sampleRate;
    private bool isRecording;
    private IAudioStream audioStream;
    private WaveRecorder recorder;

    public WaveRecorderViewModel()
    {
      this.SampleRate = 16000;

      var app = Resolver.Resolve<IXFormsApp>();

      //this.FileName = System.IO.Path.Combine(app.AppDataDirectory, "audiosample.wav");
      this.FileName = Device.OnPlatform(
          System.IO.Path.Combine(app.AppDataDirectory, "audiosample.wav"),
          "audiosample.wav",
          System.IO.Path.Combine(app.AppDataDirectory, "audiosample.wav")
          );

      var device = Resolver.Resolve<IDevice>();

      if (device != null)
      {
        this.audioStream = device.Microphone;
        this.recorder = new WaveRecorder();
      }

      this.Record = new Command(
          () =>
          {
            this.audioStream.OnBroadcast += audioStream_OnBroadcast;
            //this.audioStream.Start.Execute(this.SampleRate);
            this.recorder.StartRecorder(
                this.audioStream,
                device.FileManager.OpenFile(this.FileName, Services.IO.FileMode.Create, Services.IO.FileAccess.Write),
                this.SampleRate).ContinueWith(t =>
                    {
                      if (t.IsCompleted)
                      {
                        this.IsRecording = t.Result;
                        System.Diagnostics.Debug.WriteLine("Microphone recorder {0}.", this.IsRecording ? "was started" : "failed to start.");
                        this.Record.ChangeCanExecute();
                        this.Stop.ChangeCanExecute();
                      }
                      else if (t.IsFaulted)
                      {
                        this.audioStream.OnBroadcast -= audioStream_OnBroadcast;
                      }
                    });

          },
          () => this.RecordingEnabled &&
              this.audioStream.SupportedSampleRates.Contains(this.SampleRate) &&
              !this.IsRecording &&
              device.FileManager != null
          );

      this.Stop = new Command(
          async () =>
          {
            this.audioStream.OnBroadcast -= audioStream_OnBroadcast;
            await this.recorder.StopRecorder();
            //this.audioStream.Stop.Execute(this);
            System.Diagnostics.Debug.WriteLine("Microphone recorder was stopped.");
            this.Record.ChangeCanExecute();
            this.Stop.ChangeCanExecute();
          },
          () =>
          {
            return this.IsRecording;
          }
          );
    }

    void audioStream_OnBroadcast(object sender, EventArgs<byte[]> e)
    {
      System.Diagnostics.Debug.WriteLine("Microphone recorded {0} bytes.", e.Value.Length);
    }

    public bool RecordingEnabled
    {
      get
      {
        return (this.audioStream != null);
      }
    }

    public bool IsRecording
    {
      get { return isRecording; }
      set { this.SetProperty(ref isRecording, value); }
    }

    public int SampleRate
    {
      get { return sampleRate; }
      set { this.SetProperty(ref sampleRate, value); }
    }

    public string FileName
    {
      get { return fileName; }
      set { this.SetProperty(ref fileName, value); }
    }

    public Command Record
    {
      get;
      private set;
    }

    public Command Stop
    {
      get;
      private set;
    }
  }
}
