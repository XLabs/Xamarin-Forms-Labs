using Java.Lang;
using Java.Util.Regex;

namespace XLabs.Platform.Services.Media
{
	using System;
	using System.IO;
	using System.Threading;
	using System.Threading.Tasks;

	using Android.App;
	using Android.Content;
	using Android.Database;
	using Android.OS;
	using Android.Provider;

	using Environment = Android.OS.Environment;
	using Uri = Android.Net.Uri;

	/// <summary>
	/// Class MediaPickerActivity.
	/// </summary>
	[Activity]
	internal class MediaPickerActivity
		: Activity
	{
		#region Constants
		/// <summary>
		/// The extra path
		/// </summary>
		internal const string ExtraPath = "path";
		/// <summary>
		/// The extra location
		/// </summary>
		internal const string ExtraLocation = "location";
		/// <summary>
		/// The extra type
		/// </summary>
		internal const string ExtraType = "type";
		/// <summary>
		/// The extra identifier
		/// </summary>
		internal const string ExtraId = "id";
		/// <summary>
		/// The extra action
		/// </summary>
		internal const string ExtraAction = "action";
		/// <summary>
		/// The extra tasked
		/// </summary>
		internal const string ExtraTasked = "tasked";

		/// <summary>
		/// The medi a_ fil e_ extr a_ name
		/// </summary>
		internal const string MediaFileExtraName = "MediaFile";
		#endregion Constants

		#region Private Member Variables
		/// <summary>
		/// The action
		/// </summary>
		private string _action;

		/// <summary>
		/// The description
		/// </summary>
		private string _description;
		/// <summary>
		/// The identifier
		/// </summary>
		private int _id;

		/// <summary>
		/// The is photo
		/// </summary>
		private bool _isPhoto;

		/// <summary>
		/// The user's destination path.
		/// </summary>
		private Uri _path;

		/// <summary>
		/// The quality
		/// </summary>
		private VideoQuality _quality;
		/// <summary>
		/// The seconds
		/// </summary>
		private int _seconds;

		/// <summary>
		/// The tasked
		/// </summary>
		private bool _tasked;
		/// <summary>
		/// The title
		/// </summary>
		private string _title;
		/// <summary>
		/// The type
		/// </summary>
		private string _type;
		#endregion Private Member Variables

		#region Event Handlers
		/// <summary>
		/// Occurs when [media picked].
		/// </summary>
		internal static event EventHandler<MediaPickedEventArgs> MediaPicked;
		#endregion Event Handlers

		#region Methods
		#region Overides
		/// <summary>
		/// Called to retrieve per-instance state from an activity before being killed
		/// so that the state can be restored in <c><see cref="M:Android.App.Activity.OnCreate(Android.OS.Bundle)" /></c> or
		/// <c><see cref="M:Android.App.Activity.OnRestoreInstanceState(Android.OS.Bundle)" /></c> (the <c><see cref="T:Android.OS.Bundle" /></c> populated by this method
		/// will be passed to both).
		/// </summary>
		/// <param name="outState">Bundle in which to place your saved state.</param>
		/// <since version="Added in API level 1" />
		/// <altmember cref="M:Android.App.Activity.OnCreate(Android.OS.Bundle)" />
		/// <altmember cref="M:Android.App.Activity.OnRestoreInstanceState(Android.OS.Bundle)" />
		/// <altmember cref="M:Android.App.Activity.OnPause" />
		/// <remarks><para tool="javadoc-to-mdoc">Called to retrieve per-instance state from an activity before being killed
		/// so that the state can be restored in <c><see cref="M:Android.App.Activity.OnCreate(Android.OS.Bundle)" /></c> or
		/// <c><see cref="M:Android.App.Activity.OnRestoreInstanceState(Android.OS.Bundle)" /></c> (the <c><see cref="T:Android.OS.Bundle" /></c> populated by this method
		/// will be passed to both).
		/// </para>
		/// <para tool="javadoc-to-mdoc">This method is called before an activity may be killed so that when it
		/// comes back some time in the future it can restore its state.  For example,
		/// if activity B is launched in front of activity A, and at some point activity
		/// A is killed to reclaim resources, activity A will have a chance to save the
		/// current state of its user interface via this method so that when the user
		/// returns to activity A, the state of the user interface can be restored
		/// via <c><see cref="M:Android.App.Activity.OnCreate(Android.OS.Bundle)" /></c> or <c><see cref="M:Android.App.Activity.OnRestoreInstanceState(Android.OS.Bundle)" /></c>.
		/// </para>
		/// <para tool="javadoc-to-mdoc">Do not confuse this method with activity lifecycle callbacks such as
		/// <c><see cref="M:Android.App.Activity.OnPause" /></c>, which is always called when an activity is being placed
		/// in the background or on its way to destruction, or <c><see cref="M:Android.App.Activity.OnStop" /></c> which
		/// is called before destruction.  One example of when <c><see cref="M:Android.App.Activity.OnPause" /></c> and
		/// <c><see cref="M:Android.App.Activity.OnStop" /></c> is called and not this method is when a user navigates back
		/// from activity B to activity A: there is no need to call <c><see cref="M:Android.App.Activity.OnSaveInstanceState(Android.OS.Bundle)" /></c>
		/// on B because that particular instance will never be restored, so the
		/// system avoids calling it.  An example when <c><see cref="M:Android.App.Activity.OnPause" /></c> is called and
		/// not <c><see cref="M:Android.App.Activity.OnSaveInstanceState(Android.OS.Bundle)" /></c> is when activity B is launched in front of activity A:
		/// the system may avoid calling <c><see cref="M:Android.App.Activity.OnSaveInstanceState(Android.OS.Bundle)" /></c> on activity A if it isn't
		/// killed during the lifetime of B since the state of the user interface of
		/// A will stay intact.
		/// </para>
		/// <para tool="javadoc-to-mdoc">The default implementation takes care of most of the UI per-instance
		/// state for you by calling <c><see cref="M:Android.Views.View.OnSaveInstanceState" /></c> on each
		/// view in the hierarchy that has an id, and by saving the id of the currently
		/// focused view (all of which is restored by the default implementation of
		/// <c><see cref="M:Android.App.Activity.OnRestoreInstanceState(Android.OS.Bundle)" /></c>).  If you override this method to save additional
		/// information not captured by each individual view, you will likely want to
		/// call through to the default implementation, otherwise be prepared to save
		/// all of the state of each view yourself.
		/// </para>
		/// <para tool="javadoc-to-mdoc">If called, this method will occur before <c><see cref="M:Android.App.Activity.OnStop" /></c>.  There are
		/// no guarantees about whether it will occur before or after <c><see cref="M:Android.App.Activity.OnPause" /></c>.</para>
		/// <para tool="javadoc-to-mdoc">
		///   <format type="text/html">
		///     <a href="http://developer.android.com/reference/android/app/Activity.html#onSaveInstanceState(android.os.Bundle)" target="_blank">[Android Documentation]</a>
		///   </format>
		/// </para></remarks>
		protected override void OnSaveInstanceState(Bundle outState)
		{
			outState.PutBoolean("ran", true);
			outState.PutString(MediaStore.MediaColumns.Title, _title);
			outState.PutString(MediaStore.Images.ImageColumns.Description, _description);
			outState.PutInt(ExtraId, _id);
			outState.PutString(ExtraType, _type);
			outState.PutString(ExtraAction, _action);
			outState.PutInt(MediaStore.ExtraDurationLimit, _seconds);
			outState.PutInt(MediaStore.ExtraVideoQuality, (int) _quality);
			outState.PutBoolean(ExtraTasked, _tasked);

			if (_path != null)
				outState.PutString(ExtraPath, _path.Path);

			base.OnSaveInstanceState(outState);
		}

		/// <summary>
		/// Called when the activity is starting.
		/// </summary>
		/// <param name="savedInstanceState">If the activity is being re-initialized after
		/// previously being shut down then this Bundle contains the data it most
		/// recently supplied in <c><see cref="M:Android.App.Activity.OnSaveInstanceState(Android.OS.Bundle)" /></c>.  <format type="text/html"><b><i>Note: Otherwise it is null.</i></b></format></param>
		/// <since version="Added in API level 1" />
		/// <altmember cref="M:Android.App.Activity.OnStart" />
		/// <altmember cref="M:Android.App.Activity.OnSaveInstanceState(Android.OS.Bundle)" />
		/// <altmember cref="M:Android.App.Activity.OnRestoreInstanceState(Android.OS.Bundle)" />
		/// <altmember cref="M:Android.App.Activity.OnPostCreate(Android.OS.Bundle)" />
		/// <remarks><para tool="javadoc-to-mdoc">Called when the activity is starting.  This is where most initialization
		/// should go: calling <c><see cref="M:Android.App.Activity.SetContentView(System.Int32)" /></c> to inflate the
		/// activity's UI, using <c><see cref="M:Android.App.Activity.FindViewById(System.Int32)" /></c> to programmatically interact
		/// with widgets in the UI, calling
		/// <c><see cref="M:Android.App.Activity.ManagedQuery(Android.Net.Uri, System.String[], System.String[], System.String[], System.String[])" /></c> to retrieve
		/// cursors for data being displayed, etc.
		/// </para>
		/// <para tool="javadoc-to-mdoc">You can call <c><see cref="M:Android.App.Activity.Finish" /></c> from within this function, in
		/// which case onDestroy() will be immediately called without any of the rest
		/// of the activity lifecycle (<c><see cref="M:Android.App.Activity.OnStart" /></c>, <c><see cref="M:Android.App.Activity.OnResume" /></c>,
		/// <c><see cref="M:Android.App.Activity.OnPause" /></c>, etc) executing.
		/// </para>
		/// <para tool="javadoc-to-mdoc">
		///   <i>Derived classes must call through to the super class's
		/// implementation of this method.  If they do not, an exception will be
		/// thrown.</i>
		/// </para>
		/// <para tool="javadoc-to-mdoc">
		///   <format type="text/html">
		///     <a href="http://developer.android.com/reference/android/app/Activity.html#onCreate(android.os.Bundle)" target="_blank">[Android Documentation]</a>
		///   </format>
		/// </para></remarks>
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			var b = (savedInstanceState ?? Intent.Extras);

			var ran = b.GetBoolean("ran", false);

			_title = b.GetString(MediaStore.MediaColumns.Title);
			_description = b.GetString(MediaStore.Images.ImageColumns.Description);

			_tasked = b.GetBoolean(ExtraTasked);
			_id = b.GetInt(ExtraId, 0);
			_type = b.GetString(ExtraType);
			
			if (_type == "image/*")
			{
				_isPhoto = true;
			}

			_action = b.GetString(ExtraAction);
			Intent pickIntent = null;

			try
			{
				pickIntent = new Intent(_action);
				if (_action == Intent.ActionPick)
					pickIntent.SetType(_type);
				else
				{
					if (!_isPhoto)
					{
						_seconds = b.GetInt(MediaStore.ExtraDurationLimit, 0);
						if (_seconds != 0)
						{
							pickIntent.PutExtra(MediaStore.ExtraDurationLimit, _seconds);
						}
					}

					_quality = (VideoQuality) b.GetInt(MediaStore.ExtraVideoQuality, (int) VideoQuality.High);
					pickIntent.PutExtra(MediaStore.ExtraVideoQuality, GetVideoQuality(_quality));

					if (!ran)
					{
						_path = GetOutputMediaFile(this, b.GetString(ExtraPath), _title, _isPhoto);

						Touch();
						pickIntent.PutExtra(MediaStore.ExtraOutput, _path);
					}
					else
						_path = Uri.Parse(b.GetString(ExtraPath));
				}

				if (!ran)
				{
					StartActivityForResult(pickIntent, _id);
				}
			}
			catch (Exception ex)
			{
				RaiseOnMediaPicked(new MediaPickedEventArgs(_id, ex));
			}
			finally
			{
				if (pickIntent != null)
					pickIntent.Dispose();
			}
		}

		/// <summary>
		/// Called when an activity you launched exits, giving you the requestCode
		/// you started it with, the resultCode it returned, and any additional
		/// data from it.
		/// </summary>
		/// <param name="requestCode">The integer request code originally supplied to
		/// startActivityForResult(), allowing you to identify who this
		/// result came from.</param>
		/// <param name="resultCode">The integer result code returned by the child activity
		/// through its setResult().</param>
		/// <param name="data">An Intent, which can return result data to the caller
		/// (various data can be attached to Intent "extras").</param>
		/// <since version="Added in API level 1" />
		/// <altmember cref="M:Android.App.Activity.StartActivityForResult(Android.Content.Intent, System.Int32)" />
		/// <altmember cref="M:Android.App.Activity.CreatePendingResult(System.Int32, Android.Content.Intent, Android.Content.Intent)" />
		/// <altmember cref="M:Android.App.Activity.SetResult(Android.App.Result)" />
		/// <remarks><para tool="javadoc-to-mdoc">Called when an activity you launched exits, giving you the requestCode
		/// you started it with, the resultCode it returned, and any additional
		/// data from it.  The <format type="text/html"><var>resultCode</var></format> will be
		/// <c><see cref="F:Android.App.Result.Canceled" /></c> if the activity explicitly returned that,
		/// didn't return any result, or crashed during its operation.
		/// </para>
		/// <para tool="javadoc-to-mdoc">You will receive this call immediately before onResume() when your
		/// activity is re-starting.</para>
		/// <para tool="javadoc-to-mdoc">
		///   <format type="text/html">
		///     <a href="http://developer.android.com/reference/android/app/Activity.html#onActivityResult(int, int, android.content.Intent)" target="_blank">[Android Documentation]</a>
		///   </format>
		/// </para></remarks>
		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			if (_tasked)
			{
				var future = resultCode == Result.Canceled
					? TaskUtils.TaskFromResult(new MediaPickedEventArgs(requestCode, true))
					: GetMediaFileAsync(this, requestCode, _action, _isPhoto, ref _path, (data != null) ? data.Data : null);

				Finish();

				future.ContinueWith(t => RaiseOnMediaPicked(t.Result));
			}
			else
			{
				if (resultCode == Result.Canceled)
				{
					SetResult(Result.Canceled);
				}
				else
				{
					var resultData = new Intent();
					resultData.PutExtra(MediaFileExtraName, (data != null) ? data.Data : null);
					resultData.PutExtra(ExtraPath, _path);
					resultData.PutExtra("isPhoto", _isPhoto);
					resultData.PutExtra(ExtraAction, _action);

					SetResult(Result.Ok, resultData);
				}

				Finish();
			}
		}
		#endregion Overides

		#region Private Static Methods
		/// <summary>
		/// Gets the media file asynchronous.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="requestCode">The request code.</param>
		/// <param name="action">The action.</param>
		/// <param name="isPhoto">if set to <c>true</c> [is photo].</param>
		/// <param name="path">The path.</param>
		/// <param name="data">The data.</param>
		/// <returns>Task&lt;MediaPickedEventArgs&gt;.</returns>
		internal static Task<MediaPickedEventArgs> GetMediaFileAsync(Context context, int requestCode, string action,
			bool isPhoto, ref Uri path, Uri data)
		{
			Task<Tuple<string, bool>> pathFuture;
			Action<bool> dispose = null;
			string originalPath = null;

			if (action != Intent.ActionPick)
			{
				originalPath = path.Path;

				// Not all camera apps respect EXTRA_OUTPUT, some will instead
				// return a content or file uri from data.
				if (data != null && data.Path != originalPath)
				{
					originalPath = data.ToString();
					var currentPath = path.Path;

					pathFuture = TryMoveFileAsync(context, data, path, isPhoto).ContinueWith(t =>
						new Tuple<string, bool>(t.Result ? currentPath : null, false));
				}
				else
					pathFuture = TaskUtils.TaskFromResult(new Tuple<string, bool>(path.Path, false));
			}
			else if (data != null)
			{
				originalPath = data.ToString();
				path = data;
				pathFuture = GetFileForUriAsync(context, path, isPhoto);
			}
			else
			{
				pathFuture = TaskUtils.TaskFromResult<Tuple<string, bool>>(null);
			}

			return pathFuture.ContinueWith(t =>
			{
				string resultPath = t.Result.Item1;
				if (resultPath != null && File.Exists(t.Result.Item1))
				{
					if (t.Result.Item2)
					{
						dispose = d => File.Delete(resultPath);
					}

					var mf = new MediaFile(resultPath, () => File.OpenRead(t.Result.Item1) , dispose);

					return new MediaPickedEventArgs(requestCode, false, mf);
				}
				return new MediaPickedEventArgs(requestCode, new MediaFileNotFoundException(originalPath));
			});
		}

		/// <summary>
		/// Tries the move file asynchronous.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="url">The URL.</param>
		/// <param name="path">The path.</param>
		/// <param name="isPhoto">if set to <c>true</c> [is photo].</param>
		/// <returns>Task&lt;System.Boolean&gt;.</returns>
		private static Task<bool> TryMoveFileAsync(Context context, Uri url, Uri path, bool isPhoto)
		{
			string moveTo = GetLocalPath(path);
			return GetFileForUriAsync(context, url, isPhoto).ContinueWith(t =>
			{
				if (t.Result.Item1 == null)
					return false;

				File.Delete(moveTo);
				File.Move(t.Result.Item1, moveTo);

				if (url.Scheme == "content")
					context.ContentResolver.Delete(url, null, null);

				return true;
			}, TaskScheduler.Default);
		}

		/// <summary>
		/// Gets the video quality.
		/// </summary>
		/// <param name="videoQuality">The video quality.</param>
		/// <returns>System.Int32.</returns>
		private static int GetVideoQuality(VideoQuality videoQuality)
		{
			switch (videoQuality)
			{
				case VideoQuality.Medium:
				case VideoQuality.High:
					return 1;

				default:
					return 0;
			}
		}

		/// <summary>
		/// Gets the output media file.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="subdir">The subdir.</param>
		/// <param name="name">The name.</param>
		/// <param name="isPhoto">if set to <c>true</c> [is photo].</param>
		/// <returns>Uri.</returns>
		/// <exception cref="System.IO.IOException">Couldn't create directory, have you added the WRITE_EXTERNAL_STORAGE permission?</exception>
		private static Uri GetOutputMediaFile(Context context, string subdir, string name, bool isPhoto)
		{
			subdir = subdir ?? String.Empty;

			if (String.IsNullOrWhiteSpace(name))
			{
				name = MediaFileHelpers.GetMediaFileWithPath(isPhoto, subdir, string.Empty, name);
			}

			var mediaType = (isPhoto) ? Environment.DirectoryPictures : Environment.DirectoryMovies;
			using (var mediaStorageDir = new Java.IO.File(context.GetExternalFilesDir(mediaType), subdir))
			{
				if (!mediaStorageDir.Exists())
				{
					if (!mediaStorageDir.Mkdirs())
						throw new IOException("Couldn't create directory, have you added the WRITE_EXTERNAL_STORAGE permission?");

					// Ensure this media doesn't show up in gallery apps
					using (var nomedia = new Java.IO.File(mediaStorageDir, ".nomedia"))
						nomedia.CreateNewFile();
				}

				return Uri.FromFile(new Java.IO.File(MediaFileHelpers.GetUniqueMediaFileWithPath(isPhoto, mediaStorageDir.Path, name, File.Exists)));
			}
		}

        /// <summary>
        /// FIx uripath for 5.0.2 new Gallery Picker Error 
        /// see https://forums.xamarin.com/discussion/43908/issue-with-xlabs-mediapicker-selectpicture-and-selectvideo
        /// </summary>
        /// <param name="uriPath"></param>
        /// <returns>if result != null uri is fixed</returns>
        private static Uri FixUri(string uriPath)
        {
            //remove /ACTUAL
            if (uriPath.Contains("/ACTUAL"))
                uriPath = uriPath.Substring(0, uriPath.IndexOf("/ACTUAL", StringComparison.Ordinal));

            Java.Util.Regex.Pattern pattern = Java.Util.Regex.Pattern.Compile("(content://media/.*\\d)");

            if (uriPath.Contains("content"))
            {
                Matcher matcher = pattern.Matcher(uriPath);

                if (matcher.Find())
                    return Uri.Parse(matcher.Group(1));
                else
                    throw new IllegalArgumentException("Cannot handle this URI");
            }
            else
                return null;
        }

        /// <summary>
        /// Gets the file for URI asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="isPhoto">if set to <c>true</c> [is photo].</param>
        /// <returns>Task&lt;Tuple&lt;System.String, System.Boolean&gt;&gt;.</returns>
        internal static Task<Tuple<string, bool>> GetFileForUriAsync(Context context, Uri uri, bool isPhoto)
		{
			var tcs = new TaskCompletionSource<Tuple<string, bool>>();

            var fixedUri = FixUri(uri.Path);

            if (fixedUri != null)
                uri = fixedUri;

            if (uri.Scheme == "file")
				tcs.SetResult(new Tuple<string, bool>(new System.Uri(uri.ToString()).LocalPath, false));
			else if (uri.Scheme == "content")
			{
				Task.Factory.StartNew(() =>
				{
					ICursor cursor = null;
					try
					{
						cursor = context.ContentResolver.Query(uri, null, null, null, null);
						if (cursor == null || !cursor.MoveToNext())
							tcs.SetResult(new Tuple<string, bool>(null, false));
						else
						{
							int column = cursor.GetColumnIndex(MediaStore.MediaColumns.Data);
							string contentPath = null;

							if (column != -1)
								contentPath = cursor.GetString(column);

							bool copied = false;

							// If they don't follow the "rules", try to copy the file locally
//							if (contentPath == null || !contentPath.StartsWith("file"))
//							{
//								copied = true;
//								Uri outputPath = GetOutputMediaFile(context, "temp", null, isPhoto);
//
//								try
//								{
//									using (Stream input = context.ContentResolver.OpenInputStream(uri))
//									using (Stream output = File.Create(outputPath.Path))
//										input.CopyTo(output);
//
//									contentPath = outputPath.Path;
//								}
//								catch (FileNotFoundException)
//								{
//									// If there's no data associated with the uri, we don't know
//									// how to open this. contentPath will be null which will trigger
//									// MediaFileNotFoundException.
//								}
//							}

							tcs.SetResult(new Tuple<string, bool>(contentPath, copied));
						}
					}
					finally
					{
						if (cursor != null)
						{
							cursor.Close();
							cursor.Dispose();
						}
					}
				}, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
			}
			else
				tcs.SetResult(new Tuple<string, bool>(null, false));

			return tcs.Task;
		}

		/// <summary>
		/// Gets the local path.
		/// </summary>
		/// <param name="uri">The URI.</param>
		/// <returns>System.String.</returns>
		private static string GetLocalPath(Uri uri)
		{
			return new System.Uri(uri.ToString()).LocalPath;
		}
		#endregion Private Static Methods

		#region Private Methods
		/// <summary>
		/// Touches this instance.
		/// </summary>
		private void Touch()
		{
			if (_path.Scheme != "file")
				return;

			File.Create(GetLocalPath(_path)).Close();
		}
		#endregion Private Methods

		#region Raise Event Handlers
		/// <summary>
		/// Handles the <see cref="E:MediaPicked" /> event.
		/// </summary>
		/// <param name="e">The <see cref="MediaPickedEventArgs"/> instance containing the event data.</param>
		private static void RaiseOnMediaPicked(MediaPickedEventArgs e)
		{
			var picked = MediaPicked;
			if (picked != null)
			{
				picked(null, e);
			}
		}
		#endregion Raise Event Handlers
		#endregion Methods
	}

	/// <summary>
	/// Class MediaPickedEventArgs.
	/// </summary>
	internal class MediaPickedEventArgs
		: EventArgs
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="MediaPickedEventArgs"/> class.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="error">The error.</param>
		/// <exception cref="System.ArgumentNullException">error</exception>
		public MediaPickedEventArgs(int id, Exception error)
		{
			if (error == null)
				throw new ArgumentNullException("error");

			RequestId = id;
			Error = error;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MediaPickedEventArgs"/> class.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="isCanceled">if set to <c>true</c> [is canceled].</param>
		/// <param name="media">The media.</param>
		/// <exception cref="System.ArgumentNullException">media</exception>
		public MediaPickedEventArgs(int id, bool isCanceled, MediaFile media = null)
		{
			RequestId = id;
			IsCanceled = isCanceled;
			if (!IsCanceled && media == null)
				throw new ArgumentNullException("media");

			Media = media;
		}
		#endregion Constructors

		#region Public Properties
		/// <summary>
		/// Gets the request identifier.
		/// </summary>
		/// <value>The request identifier.</value>
		public int RequestId { get; private set; }

		/// <summary>
		/// Gets a value indicating whether this instance is canceled.
		/// </summary>
		/// <value><c>true</c> if this instance is canceled; otherwise, <c>false</c>.</value>
		public bool IsCanceled { get; private set; }

		/// <summary>
		/// Gets the error.
		/// </summary>
		/// <value>The error.</value>
		public Exception Error { get; private set; }

		/// <summary>
		/// Gets the media.
		/// </summary>
		/// <value>The media.</value>
		public MediaFile Media { get; private set; }
		#endregion Public Properties

		#region Public Methods
		/// <summary>
		/// To the task.
		/// </summary>
		/// <returns>Task&lt;MediaFile&gt;.</returns>
		public Task<MediaFile> ToTask()
		{
			var tcs = new TaskCompletionSource<MediaFile>();

			if (IsCanceled)
				tcs.SetCanceled();
			else if (Error != null)
				tcs.SetException(Error);
			else
				tcs.SetResult(Media);

			return tcs.Task;
		}
		#endregion Public Methods
	}
}