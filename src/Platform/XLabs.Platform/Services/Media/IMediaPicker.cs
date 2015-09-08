namespace XLabs.Platform.Services.Media
{
	using System;
	using System.Threading.Tasks;

	/// <summary>
	/// Interface IMediaPicker
	/// </summary>
	public interface IMediaPicker
	{
		/// <summary>
		/// Gets a value indicating whether this instance is camera available.
		/// </summary>
		/// <value><c>true</c> if this instance is camera available; otherwise, <c>false</c>.</value>
		bool IsCameraAvailable { get; }
		/// <summary>
		/// Gets a value indicating whether this instance is photos supported.
		/// </summary>
		/// <value><c>true</c> if this instance is photos supported; otherwise, <c>false</c>.</value>
		bool IsPhotosSupported { get; }
		/// <summary>
		/// Gets a value indicating whether this instance is videos supported.
		/// </summary>
		/// <value><c>true</c> if this instance is videos supported; otherwise, <c>false</c>.</value>
		bool IsVideosSupported { get; }

		/// <summary>
		/// Select a picture from library.
		/// </summary>
		/// <param name="options">The storage options.</param>
		/// <returns>Task&lt;IMediaFile&gt;.</returns>
		Task<MediaFile> SelectPhotoAsync(CameraMediaStorageOptions options);

		/// <summary>
		/// Takes the picture.
		/// </summary>
		/// <param name="options">The storage options.</param>
		/// <returns>Task&lt;IMediaFile&gt;.</returns>
		Task<MediaFile> TakePhotoAsync(CameraMediaStorageOptions options);

		/// <summary>
		/// Selects the video asynchronous.
		/// </summary>
		/// <param name="options">The options.</param>
		/// <returns>Task&lt;IMediaFile&gt;.</returns>
		Task<MediaFile> SelectVideoAsync(VideoMediaStorageOptions options);

		/// <summary>
		/// Takes the video asynchronous.
		/// </summary>
		/// <param name="options">The options.</param>
		/// <returns>Task&lt;IMediaFile&gt;.</returns>
		Task<MediaFile> TakeVideoAsync(VideoMediaStorageOptions options);

		/// <summary>
		/// Event the fires when media has been selected
		/// </summary>
		/// <value>The on photo selected.</value>
		EventHandler<MediaPickerArgs> OnMediaSelected { get; set; }

		/// <summary>
		/// Gets or sets the on error.
		/// </summary>
		/// <value>The on error.</value>
		EventHandler<MediaPickerErrorArgs> OnError { get; set; }
	}

	/// <summary>
	/// Class MediaPickerArgs.
	/// </summary>
	public class MediaPickerArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MediaPickerArgs" /> class.
		/// </summary>
		/// <param name="mf">The mf.</param>
		public MediaPickerArgs(MediaFile mf)
		{
			MediaFile = mf;
		}

		/// <summary>
		/// Gets the media file.
		/// </summary>
		/// <value>The media file.</value>
		public MediaFile MediaFile { get; private set; }
	}

	/// <summary>
	/// Class MediaPickerErrorArgs.
	/// </summary>
	public class MediaPickerErrorArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MediaPickerErrorArgs" /> class.
		/// </summary>
		/// <param name="ex">The ex.</param>
		public MediaPickerErrorArgs(Exception ex)
		{
			Error = ex;
		}

		/// <summary>
		/// Gets the error.
		/// </summary>
		/// <value>The error.</value>
		public Exception Error { get; private set; }
	}
}