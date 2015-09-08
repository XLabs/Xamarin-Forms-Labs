namespace XLabs.Platform.Services.Media
{
	using System;
	using CoreGraphics;
	using System.IO;
	using System.Threading.Tasks;

	using Foundation;
	using UIKit;

	using XLabs.Platform.Extensions;

	/// <summary>
	/// Class MediaPickerDelegate.
	/// </summary>
	internal class MediaPickerDelegate : UIImagePickerControllerDelegate
	{
		/// <summary>
		/// The _orientation
		/// </summary>
		private UIDeviceOrientation? _orientation;

		/// <summary>
		/// The _observer
		/// </summary>
		private readonly NSObject _observer;

		/// <summary>
		/// The _options
		/// </summary>
		private readonly MediaStorageOptions _options;

		/// <summary>
		/// The _source
		/// </summary>
		private readonly UIImagePickerControllerSourceType _source;

		/// <summary>
		/// The _TCS
		/// </summary>
		private readonly TaskCompletionSource<MediaFile> _tcs = new TaskCompletionSource<MediaFile>();

		/// <summary>
		/// The _view controller
		/// </summary>
		private readonly UIViewController _viewController;

		/// <summary>
		/// Initializes a new instance of the <see cref="MediaPickerDelegate"/> class.
		/// </summary>
		/// <param name="viewController">The view controller.</param>
		/// <param name="sourceType">Type of the source.</param>
		/// <param name="options">The options.</param>
		internal MediaPickerDelegate(
			UIViewController viewController,
			UIImagePickerControllerSourceType sourceType,
			MediaStorageOptions options)
		{
			_viewController = viewController;
			_source = sourceType;
			_options = options ?? new CameraMediaStorageOptions();

			if (viewController != null)
			{
				UIDevice.CurrentDevice.BeginGeneratingDeviceOrientationNotifications();
				_observer = NSNotificationCenter.DefaultCenter.AddObserver(UIDevice.OrientationDidChangeNotification, DidRotate);
			}
		}

		/// <summary>
		/// Gets or sets the popover.
		/// </summary>
		/// <value>The popover.</value>
		public UIPopoverController Popover { get; set; }

		/// <summary>
		/// Gets the view.
		/// </summary>
		/// <value>The view.</value>
		public UIView View
		{
			get
			{
				return _viewController.View;
			}
		}

		/// <summary>
		/// Gets the task.
		/// </summary>
		/// <value>The task.</value>
		public Task<MediaFile> Task
		{
			get
			{
				return _tcs.Task;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is captured.
		/// </summary>
		/// <value><c>true</c> if this instance is captured; otherwise, <c>false</c>.</value>
		private bool IsCaptured
		{
			get
			{
				return _source == UIImagePickerControllerSourceType.Camera;
			}
		}

		/// <summary>
		/// Finisheds the picking media.
		/// </summary>
		/// <param name="picker">The picker.</param>
		/// <param name="info">The information.</param>
		/// <exception cref="NotSupportedException"></exception>
		public override void FinishedPickingMedia(UIImagePickerController picker, NSDictionary info)
		{
			MediaFile mediaFile;
			switch ((NSString)info[UIImagePickerController.MediaType])
			{
				case MediaPicker.TypeImage:
					mediaFile = GetPictureMediaFile(info);
					break;

				case MediaPicker.TypeMovie:
					mediaFile = GetMovieMediaFile(info);
					break;

				default:
					throw new NotSupportedException();
			}

			Dismiss(picker, () => _tcs.TrySetResult(mediaFile));
		}

		/// <summary>
		/// Canceleds the specified picker.
		/// </summary>
		/// <param name="picker">The picker.</param>
		public override void Canceled(UIImagePickerController picker)
		{
			Dismiss(picker, () => _tcs.TrySetCanceled());
		}

		/// <summary>
		/// Displays the popover.
		/// </summary>
		/// <param name="hideFirst">if set to <c>true</c> [hide first].</param>
		public void DisplayPopover(bool hideFirst = false)
		{
			if (Popover == null)
			{
				return;
			}

			var swidth = UIScreen.MainScreen.Bounds.Width;
			var sheight = UIScreen.MainScreen.Bounds.Height;

			float width = 400;
			float height = 300;

			if (_orientation == null)
			{
				if (IsValidInterfaceOrientation(UIDevice.CurrentDevice.Orientation))
				{
					_orientation = UIDevice.CurrentDevice.Orientation;
				}
				else
				{
					_orientation = GetDeviceOrientation(_viewController.InterfaceOrientation);
				}
			}

			float x, y;
			if (_orientation == UIDeviceOrientation.LandscapeLeft || _orientation == UIDeviceOrientation.LandscapeRight)
			{
				y = (float)(swidth / 2) - (height / 2);
				x = (float)(sheight / 2) - (width / 2);
			}
			else
			{
				x = (float)(swidth / 2) - (width / 2);
				y = (float)(sheight / 2) - (height / 2);
			}

			if (hideFirst && Popover.PopoverVisible)
			{
				Popover.Dismiss(false);
			}

			Popover.PresentFromRect(new CGRect(x, y, width, height), View, 0, true);
		}

		/// <summary>
		/// Dismisses the specified picker.
		/// </summary>
		/// <param name="picker">The picker.</param>
		/// <param name="onDismiss">The on dismiss.</param>
		private void Dismiss(UIImagePickerController picker, Action onDismiss)
		{
			if (_viewController == null)
			{
				onDismiss();
			}
			else
			{
				NSNotificationCenter.DefaultCenter.RemoveObserver(_observer);
				UIDevice.CurrentDevice.EndGeneratingDeviceOrientationNotifications();

				_observer.Dispose();

				if (Popover != null)
				{
					Popover.Dismiss(true);
					Popover.Dispose();
					Popover = null;

					onDismiss();
				}
				else
				{
					picker.DismissViewController(true, onDismiss);
					picker.Dispose();
				}
			}
		}

		/// <summary>
		/// Dids the rotate.
		/// </summary>
		/// <param name="notice">The notice.</param>
		private void DidRotate(NSNotification notice)
		{
			var device = (UIDevice)notice.Object;
			if (!IsValidInterfaceOrientation(device.Orientation) || Popover == null)
			{
				return;
			}
			if (_orientation.HasValue && IsSameOrientationKind(_orientation.Value, device.Orientation))
			{
				return;
			}

			if (UIDevice.CurrentDevice.CheckSystemVersion(6, 0))
			{
				if (!GetShouldRotate6(device.Orientation))
				{
					return;
				}
			}
			else if (!GetShouldRotate(device.Orientation))
			{
				return;
			}

			var co = _orientation;
			_orientation = device.Orientation;

			if (co == null)
			{
				return;
			}

			DisplayPopover(true);
		}

		/// <summary>
		/// Gets the should rotate.
		/// </summary>
		/// <param name="orientation">The orientation.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		private bool GetShouldRotate(UIDeviceOrientation orientation)
		{
			var iorientation = UIInterfaceOrientation.Portrait;
			switch (orientation)
			{
				case UIDeviceOrientation.LandscapeLeft:
					iorientation = UIInterfaceOrientation.LandscapeLeft;
					break;

				case UIDeviceOrientation.LandscapeRight:
					iorientation = UIInterfaceOrientation.LandscapeRight;
					break;

				case UIDeviceOrientation.Portrait:
					iorientation = UIInterfaceOrientation.Portrait;
					break;

				case UIDeviceOrientation.PortraitUpsideDown:
					iorientation = UIInterfaceOrientation.PortraitUpsideDown;
					break;

				default:
					return false;
			}

			return _viewController.ShouldAutorotateToInterfaceOrientation(iorientation);
		}

		/// <summary>
		/// Gets the should rotate6.
		/// </summary>
		/// <param name="orientation">The orientation.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		private bool GetShouldRotate6(UIDeviceOrientation orientation)
		{
			if (!_viewController.ShouldAutorotate())
			{
				return false;
			}

			var mask = UIInterfaceOrientationMask.Portrait;
			switch (orientation)
			{
				case UIDeviceOrientation.LandscapeLeft:
					mask = UIInterfaceOrientationMask.LandscapeLeft;
					break;

				case UIDeviceOrientation.LandscapeRight:
					mask = UIInterfaceOrientationMask.LandscapeRight;
					break;

				case UIDeviceOrientation.Portrait:
					mask = UIInterfaceOrientationMask.Portrait;
					break;

				case UIDeviceOrientation.PortraitUpsideDown:
					mask = UIInterfaceOrientationMask.PortraitUpsideDown;
					break;

				default:
					return false;
			}

			return _viewController.GetSupportedInterfaceOrientations().HasFlag(mask);
		}

		/// <summary>
		/// Gets the picture media file.
		/// </summary>
		/// <param name="info">The information.</param>
		/// <returns>MediaFile.</returns>
		private MediaFile GetPictureMediaFile(NSDictionary info)
		{
			var image = (UIImage)info[UIImagePickerController.EditedImage];
			if (image == null)
			{
				image = (UIImage)info[UIImagePickerController.OriginalImage];
			}

			var path = GetOutputPath(
				MediaPicker.TypeImage,
				_options.Directory ?? ((IsCaptured) ? String.Empty : "temp"),
				_options.Name);

			using (var fs = File.OpenWrite(path))
			using (Stream s = new NsDataStream(image.AsJPEG()))
			{
				s.CopyTo(fs);
				fs.Flush();
			}

			Action<bool> dispose = null;
			if (_source != UIImagePickerControllerSourceType.Camera)
			{
				dispose = d => File.Delete(path);
			}

			return new MediaFile(path, () => File.OpenRead(path), dispose);
		}

		/// <summary>
		/// Gets the movie media file.
		/// </summary>
		/// <param name="info">The information.</param>
		/// <returns>MediaFile.</returns>
		private MediaFile GetMovieMediaFile(NSDictionary info)
		{
			var url = (NSUrl)info[UIImagePickerController.MediaURL];

			var path = GetOutputPath(
				MediaPicker.TypeMovie,
				_options.Directory ?? ((IsCaptured) ? String.Empty : "temp"),
				_options.Name ?? Path.GetFileName(url.Path));

			File.Move(url.Path, path);

			Action<bool> dispose = null;
			if (_source != UIImagePickerControllerSourceType.Camera)
			{
				dispose = d => File.Delete(path);
			}

			return new MediaFile(path, () => File.OpenRead(path), dispose);
		}

		/// <summary>
		/// Gets the unique path.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="path">The path.</param>
		/// <param name="name">The name.</param>
		/// <returns>System.String.</returns>
		private static string GetUniquePath(string type, string path, string name)
		{
			var isPhoto = (type == MediaPicker.TypeImage);
			var ext = Path.GetExtension(name);
			if (ext == String.Empty)
			{
				ext = ((isPhoto) ? ".jpg" : ".mp4");
			}

			name = Path.GetFileNameWithoutExtension(name);

			var nname = name + ext;
			var i = 1;
			while (File.Exists(Path.Combine(path, nname)))
			{
				nname = name + "_" + (i++) + ext;
			}

			return Path.Combine(path, nname);
		}

		/// <summary>
		/// Gets the output path.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="path">The path.</param>
		/// <param name="name">The name.</param>
		/// <returns>System.String.</returns>
		private static string GetOutputPath(string type, string path, string name)
		{
			path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), path);
			Directory.CreateDirectory(path);

			if (String.IsNullOrWhiteSpace(name))
			{
				var timestamp = DateTime.Now.ToString("yyyMMdd_HHmmss");
				if (type == MediaPicker.TypeImage)
				{
					name = "IMG_" + timestamp + ".jpg";
				}
				else
				{
					name = "VID_" + timestamp + ".mp4";
				}
			}

			return Path.Combine(path, GetUniquePath(type, path, name));
		}

		/// <summary>
		/// Determines whether [is valid interface orientation] [the specified self].
		/// </summary>
		/// <param name="self">The self.</param>
		/// <returns><c>true</c> if [is valid interface orientation] [the specified self]; otherwise, <c>false</c>.</returns>
		private static bool IsValidInterfaceOrientation(UIDeviceOrientation self)
		{
			return (self != UIDeviceOrientation.FaceUp && self != UIDeviceOrientation.FaceDown
			        && self != UIDeviceOrientation.Unknown);
		}

		/// <summary>
		/// Determines whether [is same orientation kind] [the specified o1].
		/// </summary>
		/// <param name="o1">The o1.</param>
		/// <param name="o2">The o2.</param>
		/// <returns><c>true</c> if [is same orientation kind] [the specified o1]; otherwise, <c>false</c>.</returns>
		private static bool IsSameOrientationKind(UIDeviceOrientation o1, UIDeviceOrientation o2)
		{
			if (o1 == UIDeviceOrientation.FaceDown || o1 == UIDeviceOrientation.FaceUp)
			{
				return (o2 == UIDeviceOrientation.FaceDown || o2 == UIDeviceOrientation.FaceUp);
			}
			if (o1 == UIDeviceOrientation.LandscapeLeft || o1 == UIDeviceOrientation.LandscapeRight)
			{
				return (o2 == UIDeviceOrientation.LandscapeLeft || o2 == UIDeviceOrientation.LandscapeRight);
			}
			if (o1 == UIDeviceOrientation.Portrait || o1 == UIDeviceOrientation.PortraitUpsideDown)
			{
				return (o2 == UIDeviceOrientation.Portrait || o2 == UIDeviceOrientation.PortraitUpsideDown);
			}

			return false;
		}

		/// <summary>
		/// Gets the device orientation.
		/// </summary>
		/// <param name="self">The self.</param>
		/// <returns>UIDeviceOrientation.</returns>
		/// <exception cref="InvalidOperationException"></exception>
		private static UIDeviceOrientation GetDeviceOrientation(UIInterfaceOrientation self)
		{
			switch (self)
			{
				case UIInterfaceOrientation.LandscapeLeft:
					return UIDeviceOrientation.LandscapeLeft;
				case UIInterfaceOrientation.LandscapeRight:
					return UIDeviceOrientation.LandscapeRight;
				case UIInterfaceOrientation.Portrait:
					return UIDeviceOrientation.Portrait;
				case UIInterfaceOrientation.PortraitUpsideDown:
					return UIDeviceOrientation.PortraitUpsideDown;
				default:
					throw new InvalidOperationException();
			}
		}
	}
}