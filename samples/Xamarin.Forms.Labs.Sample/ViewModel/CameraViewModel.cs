using System.Threading.Tasks;
using Xamarin.Forms.Labs.Services;
using Xamarin.Forms.Labs.Services.Media;
using Xamarin.Forms.Labs.Mvvm.Views;

namespace Xamarin.Forms.Labs.Sample
{

  using System.IO;

  /// <summary>
  /// Class CameraViewModel.
  /// </summary>
  public class CameraViewModel : Mvvm.ViewModels.ViewModel
  {
    /// <summary>
    /// The _scheduler.
    /// </summary>
    private readonly TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();

    /// <summary>
    /// The _picture chooser.
    /// </summary>
    private IMediaPicker mediaPicker;

    /// <summary>
    /// The _image source.
    /// </summary>
    private ImageSource imageSource;

    /// <summary>
    /// The _video info.
    /// </summary>
    private string videoInfo;

    /// <summary>
    /// The _take picture command.
    /// </summary>
    private Command takePictureCommand;

    /// <summary>
    /// The _select picture command.
    /// </summary>
    private Command selectPictureCommand;

    /// <summary>
    /// The _select video command.
    /// </summary>
    private Command selectVideoCommand;

    private string status;


    ////private CancellationTokenSource cancelSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="CameraViewModel" /> class.
    /// </summary>
    public CameraViewModel()
    {
      Setup();
    }

    /// <summary>
    /// Gets or sets the image source.
    /// </summary>
    /// <value>The image source.</value>
    public ImageSource ImageSource
    {
      get
      {
        return imageSource;
      }
      set
      {
        this.SetProperty(ref imageSource, value);
      }
    }

    /// <summary>
    /// Gets or sets the video info.
    /// </summary>
    /// <value>The video info.</value>
    public string VideoInfo
    {
      get
      {
        return videoInfo;
      }
      set
      {
        this.SetProperty(ref videoInfo, value);
      }
    }

    /// <summary>
    /// Gets the take picture command.
    /// </summary>
    /// <value>The take picture command.</value>
    public Command TakePictureCommand
    {
      get
      {
        return takePictureCommand ?? (takePictureCommand = new Command(
                                                               async () => await TakePicture(),
                                                               () => true));
      }
    }

    /// <summary>
    /// Gets the select video command.
    /// </summary>
    /// <value>The select picture command.</value>
    public Command SelectVideoCommand
    {
      get
      {
        return selectVideoCommand ?? (selectVideoCommand = new Command(
                                                               async () => await SelectVideo(),
                                                               () => true));
      }
    }

    /// <summary>
    /// Gets the select picture command.
    /// </summary>
    /// <value>The select picture command.</value>
    public Command SelectPictureCommand
    {
      get
      {
        return selectPictureCommand ?? (selectPictureCommand = new Command(
                                                                   async () => await SelectPicture(),
                                                                   () => true));
      }
    }

    /// <summary>
    /// Gets the status.
    /// </summary>
    /// <value>
    /// The status.
    /// </value>
    public string Status
    {
      get { return status; }
      private set { this.SetProperty(ref status, value); }
    }

    /// <summary>
    /// Setups this instance.
    /// </summary>
    private void Setup()
    {
      if (mediaPicker != null)
      {
        return;
      }

      var device = Resolver.Resolve<IDevice>();

      mediaPicker = DependencyService.Get<IMediaPicker>();
      ////RM: hack for working on windows phone? 
      if (mediaPicker == null)
      {
        mediaPicker = device.MediaPicker;
      }
    }

    /// <summary>
    /// Takes the picture.
    /// </summary>
    /// <returns>Take Picture Task.</returns>
    private async Task TakePicture()
    {
      Setup();

      ImageSource = null;

      await this.mediaPicker.TakePhotoAsync(new CameraMediaStorageOptions { DefaultCamera = CameraDevice.Front, MaxPixelDimension = 400 })
        .ContinueWith(t =>
        {
          if (t.IsFaulted)
          {
            this.Status = t.Exception.InnerException.ToString();
          }
          else if (t.IsCanceled)
          {
            this.Status = "Canceled";
          }
          else
          {
            var mediaFile = t.Result;
            var s = mediaFile.Source;
            var buffer = new byte[(int)s.Length];
            s.Seek(0, SeekOrigin.Begin);
            s.Read(buffer, 0, (int)mediaFile.Source.Length);

            ImageSource = ImageSource.FromStream(() => mediaFile.Source);

          }
        }, scheduler);
    }

    /// <summary>
    /// Selects the picture.
    /// </summary>
    /// <returns>Select Picture Task.</returns>
    private async Task SelectPicture()
    {
      Setup();

      ImageSource = null;
      try
      {
        //var mediaFile = await this.mediaPicker.SelectPhotoAsync(new CameraMediaStorageOptions
        //    {
        //      DefaultCamera = CameraDevice.Front,
        //      MaxPixelDimension = 400
        //    });
        await this.mediaPicker.SelectPhotoAsync(new CameraMediaStorageOptions { DefaultCamera = CameraDevice.Front, MaxPixelDimension = 400 })
          .ContinueWith(t =>
          {
            if (t.IsFaulted)
            {
              this.Status = t.Exception.InnerException.ToString();
            }
            else if (t.IsCanceled)
            {
              this.Status = "Canceled";
            }
            else
            {
              var mediaFile = t.Result;

              ImageSource = ImageSource.FromStream(() => mediaFile.Source);
            }
          }, scheduler);
      }
      catch (System.Exception ex)
      {
        this.Status = ex.Message;
      }
    }

    /// <summary>
    /// Selects the video.
    /// </summary>
    /// <returns>Select Video Task.</returns>
    private async Task SelectVideo()
    {
      Setup();

      VideoInfo = "Selecting video";

      try
      {
        var mediaFile = await this.mediaPicker.SelectVideoAsync(new VideoMediaStorageOptions());

        if (mediaFile != null)
        {
          VideoInfo = string.Format("Your video size {0} MB", ConvertBytesToMegabytes(mediaFile.Source.Length));
        }
        else
        {
          VideoInfo = "No video was selected";
        }
      }
      catch (System.Exception ex)
      {
        if (ex is TaskCanceledException)
        {
          VideoInfo = "Selecting video canceled";
        }
        else
        {
          VideoInfo = ex.Message;
        }
      }
    }

    private static double ConvertBytesToMegabytes(long bytes)
    {
      return (bytes / 1024f) / 1024f;
    }
  }
}

