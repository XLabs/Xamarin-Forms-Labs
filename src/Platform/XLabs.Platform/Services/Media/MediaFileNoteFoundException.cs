
namespace XLabs.Platform.Services.Media
{
	using System;

	/// <summary>
	/// Class MediaFileNotFoundException.
	/// </summary>
	public class MediaFileNotFoundException
		: Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MediaFileNotFoundException" /> class.
		/// </summary>
		/// <param name="path">The path.</param>
		public MediaFileNotFoundException(string path)
			: base("Unable to locate media file at " + path)
		{
			Path = path;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MediaFileNotFoundException" /> class.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="innerException">The inner exception.</param>
		public MediaFileNotFoundException(string path, Exception innerException)
			: base("Unable to locate media file at " + path, innerException)
		{
			Path = path;
		}

		/// <summary>
		/// Gets the path.
		/// </summary>
		/// <value>The path.</value>
		public string Path
		{
			get;
			private set;
		}
	}
}
