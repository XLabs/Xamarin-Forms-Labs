//namespace XLabs.Platform.Services.Media
//{
//	using System;
//	using System.IO;
//	using System.IO.IsolatedStorage;
//	using System.Threading;
//	using System.Threading.Tasks;
//	using System.Windows;
//	using System.Windows.Media.Imaging;

//	using Microsoft.Devices;
//	using Microsoft.Phone.Tasks;

//	using XLabs.Platform.Device;

//	/// <summary>
//	/// Class MediaPicker.
//	/// </summary>
//	public class MediaPicker : IMediaPicker
//	{
//		#region Constructors

//		/// <summary>
//		/// Initializes a new instance of the <see cref="MediaPicker" /> class.
//		/// </summary>
//		public MediaPicker()
//		{
//			//if (!DeviceCapabilities.IsEnabled(DeviceCapabilities.Capability.ID_CAP_MEDIALIB_PHOTO))
//			//{
//			//    throw new UnauthorizedAccessException(string.Format("Access to MediaPicker requires {0} to be defined in the manifest.", DeviceCapabilities.Capability.ID_CAP_MEDIALIB_PHOTO));
//			//}

//			_photoChooser.Completed += InternalOnPhotoChosen;
//			_cameraCapture.Completed += InternalOnPhotoChosen;

//			_photoChooser.ShowCamera = false;

//			IsCameraAvailable = //DeviceCapabilities.IsEnabled(DeviceCapabilities.Capability.ID_CAP_ISV_CAMERA) && 
//				(Camera.IsCameraTypeSupported(CameraType.Primary) || Camera.IsCameraTypeSupported(CameraType.FrontFacing));

//			IsPhotosSupported = DeviceCapabilities.IsEnabled(DeviceCapabilities.Capability.IdCapMedialibPhoto);
//			IsVideosSupported = IsCameraAvailable;
//		}

//		#endregion Constructors

//		#region Private Member Variables

//		/// <summary>
//		/// The _camera capture
//		/// </summary>
//		private readonly CameraCaptureTask _cameraCapture = new CameraCaptureTask();

//		/// <summary>
//		/// The _photo chooser
//		/// </summary>
//		private readonly PhotoChooserTask _photoChooser = new PhotoChooserTask();

//		/// <summary>
//		/// The _completion source
//		/// </summary>
//		private TaskCompletionSource<MediaFile> _completionSource;

//		#endregion Private Member Variables

//		#region Public Properties

//		/// <summary>
//		/// Gets a value indicating whether this instance is camera available.
//		/// </summary>
//		/// <value><c>true</c> if this instance is camera available; otherwise, <c>false</c>.</value>
//		public bool IsCameraAvailable { get; private set; }

//		/// <summary>
//		/// Gets a value indicating whether this instance is photos supported.
//		/// </summary>
//		/// <value><c>true</c> if this instance is photos supported; otherwise, <c>false</c>.</value>
//		public bool IsPhotosSupported { get; private set; }

//		/// <summary>
//		/// Gets a value indicating whether this instance is videos supported.
//		/// </summary>
//		/// <value><c>true</c> if this instance is videos supported; otherwise, <c>false</c>.</value>
//		public bool IsVideosSupported { get; private set; }

//		#endregion Public Properties

//		#region Public Methods

//		/// <summary>
//		/// Select a picture from library.
//		/// </summary>
//		/// <param name="options">The storage options.</param>
//		/// <returns>Task&lt;IMediaFile&gt;.</returns>
//		/// <exception cref="InvalidOperationException">Only one operation can be active at at time</exception>
//		/// <exception cref="System.NotImplementedException"></exception>
//		public Task<MediaFile> SelectPhotoAsync(CameraMediaStorageOptions options)
//		{
//			options.VerifyOptions();

//			var ntcs = new TaskCompletionSource<MediaFile>(options);
//			if (Interlocked.CompareExchange(ref _completionSource, ntcs, null) != null)
//			{
//				throw new InvalidOperationException("Only one operation can be active at at time");
//			}

//			_photoChooser.Show();

//			return ntcs.Task;
//		}

//		/// <summary>
//		/// Takes the picture.
//		/// </summary>
//		/// <param name="options">The storage options.</param>
//		/// <returns>Task&lt;IMediaFile&gt;.</returns>
//		/// <exception cref="InvalidOperationException">Only one operation can be active at a time</exception>
//		/// <exception cref="System.NotImplementedException"></exception>
//		public Task<MediaFile> TakePhotoAsync(CameraMediaStorageOptions options)
//		{
//			options.VerifyOptions();

//			var ntcs = new TaskCompletionSource<MediaFile>(options);
//			if (Interlocked.CompareExchange(ref _completionSource, ntcs, null) != null)
//			{
//				throw new InvalidOperationException("Only one operation can be active at a time");
//			}

//			_cameraCapture.Show();

//			return ntcs.Task;
//		}

//		/// <summary>
//		/// Selects the video asynchronous.
//		/// </summary>
//		/// <param name="options">The options.</param>
//		/// <returns>Task&lt;IMediaFile&gt;.</returns>
//		/// <exception cref="NotImplementedException"></exception>
//		/// <exception cref="System.NotImplementedException"></exception>
//		public Task<MediaFile> SelectVideoAsync(VideoMediaStorageOptions options)
//		{
//			throw new NotImplementedException();
//		}

//		/// <summary>
//		/// Takes the video asynchronous.
//		/// </summary>
//		/// <param name="options">The options.</param>
//		/// <returns>Task&lt;IMediaFile&gt;.</returns>
//		/// <exception cref="NotImplementedException"></exception>
//		/// <exception cref="System.NotImplementedException"></exception>
//		public Task<MediaFile> TakeVideoAsync(VideoMediaStorageOptions options)
//		{
//			throw new NotImplementedException();
//		}

//		#endregion Public Methods

//		#region Event Handlers

//		/// <summary>
//		/// Event the fires when media has been selected
//		/// </summary>
//		/// <value>The on photo selected.</value>
//		public EventHandler<MediaPickerArgs> OnMediaSelected { get; set; }

//		/// <summary>
//		/// Gets or sets the on error.
//		/// </summary>
//		/// <value>The on error.</value>
//		public EventHandler<MediaPickerErrorArgs> OnError { get; set; }

//		#endregion Event Handlers

//		#region Private Methods

//		/// <summary>
//		/// Internals the on photo chosen.
//		/// </summary>
//		/// <param name="sender">The sender.</param>
//		/// <param name="photoResult">The photo result.</param>
//		private void InternalOnPhotoChosen(object sender, PhotoResult photoResult)
//		{
//			var tcs = Interlocked.Exchange(ref _completionSource, null);

//			if (photoResult.TaskResult == TaskResult.Cancel)
//			{
//				tcs.SetCanceled();
//				return;
//			}

//			var path = string.Empty;

//			var pos = photoResult.ChosenPhoto.Position;
//			var options = tcs.Task.AsyncState as CameraMediaStorageOptions;
//			var streamImage = photoResult.ChosenPhoto;
//			var saveImage = true;
//			Action<bool> dispose = null;

//			if (options != null)
//			{
//				ResizeImageStream(
//					options.MaxPixelDimension,
//					options.PercentQuality,
//					streamImage,
//					stream => SafeAsyncCall(stream, o => streamImage = o));
//				saveImage = options.SaveMediaOnCapture;
//			}

//			if (saveImage)
//			{
//				using (var store = IsolatedStorageFile.GetUserStoreForApplication())
//				{
//					path = options.GetUniqueMediaFileWithPath((options == null) ? "temp" : options.Directory, p => store.FileExists(p));

//					var dir = Path.GetDirectoryName(path);
//					if (!String.IsNullOrWhiteSpace(dir))
//					{
//						store.CreateDirectory(dir);
//					}

//					using (var fs = store.CreateFile(path))
//					{
//						var buffer = new byte[20480];
//						int len;
//						//while ((len = photoResult.ChosenPhoto.Read(buffer, 0, buffer.Length)) > 0)
//						while ((len = streamImage.Read(buffer, 0, buffer.Length)) > 0)
//						{
//							fs.Write(buffer, 0, len);
//						}

//						fs.Flush(true);
//					}
//				}

//				if (options == null)
//				{
//					dispose = d =>
//						{
//							using (var store = IsolatedStorageFile.GetUserStoreForApplication())
//							{
//								store.DeleteFile(path);
//							}
//						};
//				}
//			}

//			switch (photoResult.TaskResult)
//			{
//				case TaskResult.OK:
//					photoResult.ChosenPhoto.Position = pos;
//					var mf = new MediaFile(path, () => streamImage, dispose);

//					if (OnMediaSelected != null)
//					{
//						OnMediaSelected(this, new MediaPickerArgs(mf));
//					}

//					tcs.SetResult(mf);
//					break;

//				case TaskResult.None:
//					photoResult.ChosenPhoto.Dispose();
//					if (photoResult.Error != null)
//					{
//						if (OnError != null)
//						{
//							OnError(this, new MediaPickerErrorArgs(photoResult.Error));
//						}

//						tcs.SetException(photoResult.Error);
//					}

//					break;
//			}
//		}

//		/// <summary>
//		/// Resizes the JPEG stream.
//		/// </summary>
//		/// <param name="maxPixelDimension">The maximum pixel dimension ratio (used to resize the image).</param>
//		/// <param name="percentQuality">The percent quality.</param>
//		/// <param name="input">The stream that contains the image.</param>
//		/// <param name="success">The action to execute on actionSuccess of resizing the image.</param>
//		private static void ResizeImageStream(
//			int? maxPixelDimension,
//			int? percentQuality,
//			Stream input,
//			Action<Stream> success)
//		{
//			int targetHeight;
//			int targetWidth;

//			if (!percentQuality.HasValue)
//			{
//				percentQuality = 100;
//			}

//			var bitmap = new BitmapImage();
//			bitmap.SetSource(input);
//			var writeable = new WriteableBitmap(bitmap);

//			ResizeBasedOnPixelDimension(
//				maxPixelDimension,
//				writeable.PixelWidth,
//				writeable.PixelHeight,
//				out targetWidth,
//				out targetHeight);

//			// Note: We are NOT using a "using" statement here on purpose. It is the callers responsibility to handle the dispose of the stream correctly
//			var memoryStream = new MemoryStream();
//			writeable.SaveJpeg(memoryStream, targetWidth, targetHeight, 0, percentQuality.Value);
//			memoryStream.Seek(0L, SeekOrigin.Begin);

//			// Execute the call back with the valid stream
//			success(memoryStream);
//		}

//		/// <summary>
//		/// Calls the asynchronous.
//		/// </summary>
//		/// <param name="input">The stream that contains the image.</param>
//		/// <param name="success">The action to execute on actionSuccess of resizing the image.</param>
//		private static void SafeAsyncCall(Stream input, Action<Stream> success)
//		{
//			if (Deployment.Current.Dispatcher.CheckAccess())
//			{
//				try
//				{
//					success(input);
//				}
//				catch
//				{
//				}
//			}
//			else
//			{
//				Deployment.Current.Dispatcher.BeginInvoke(() => success(input));
//			}
//		}

//		/// <summary>
//		/// Does the with invalid operation protection.
//		/// </summary>
//		/// <param name="action">The action.</param>
//		private void SafeAsyncCall(Action action)
//		{
//			if (Deployment.Current.Dispatcher.CheckAccess())
//			{
//				try
//				{
//					action();
//				}
//				catch
//				{
//				}
//			}
//			else
//			{
//				Deployment.Current.Dispatcher.BeginInvoke(action);
//			}
//		}

//		/// <summary>
//		/// Calcualtes the target height and width of the image based on the max pixel dimension.
//		/// </summary>
//		/// <param name="maxPixelDimension">The maximum pixel dimension ratio (used to resize the image).</param>
//		/// <param name="currentWidth">Current Width of the image.</param>
//		/// <param name="currentHeight">Current Height of the image.</param>
//		/// <param name="targetWidth">Target Width of the image.</param>
//		/// <param name="targetHeight">Target Height of the image.</param>
//		public static void ResizeBasedOnPixelDimension(
//			int? maxPixelDimension,
//			int currentWidth,
//			int currentHeight,
//			out int targetWidth,
//			out int targetHeight)
//		{
//			if (!maxPixelDimension.HasValue)
//			{
//				targetWidth = currentWidth;
//				targetHeight = currentHeight;
//				return;
//			}

//			double ratio;
//			if (currentWidth > currentHeight)
//			{
//				ratio = (maxPixelDimension.Value) / ((double)currentWidth);
//			}
//			else
//			{
//				ratio = (maxPixelDimension.Value) / ((double)currentHeight);
//			}

//			targetWidth = (int)Math.Round(ratio * currentWidth);
//			targetHeight = (int)Math.Round(ratio * currentHeight);
//		}

//		#endregion Private Methods
//	}
//}