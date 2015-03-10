namespace XLabs.Platform.Services.Media
{
    using System;

    /// <summary>
	/// Class SoundFinishedEventArgs.
	/// </summary>
	public class SoundFinishedEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SoundFinishedEventArgs"/> class.
		/// </summary>
		/// <param name="f">The f.</param>
		public SoundFinishedEventArgs(SoundFile f)
		{
			File = f;
		}

		/// <summary>
		/// Gets or sets the file.
		/// </summary>
		/// <value>The file.</value>
		public SoundFile File { get; set; }
	}
}