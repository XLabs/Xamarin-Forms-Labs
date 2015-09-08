namespace XLabs.Platform.Services.Media
{
    using System;

    /// <summary>
	/// Class SoundFile.
	/// </summary>
	public class SoundFile
	{
		/// <summary>
		/// Gets or sets the filename.
		/// </summary>
		/// <value>The filename.</value>
		public string Filename { get; set; }

		/// <summary>
		/// Gets or sets the duration.
		/// </summary>
		/// <value>The duration.</value>
		public TimeSpan Duration { get; set; }
	}
}