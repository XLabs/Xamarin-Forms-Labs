namespace XLabs.Platform
{
	using System;

	/// <summary>
	/// Class UriExtensions.
	/// </summary>
	public static class UriExtensions
	{
		/// <summary>
		/// To the android URI.
		/// </summary>
		/// <param name="uri">The URI.</param>
		/// <returns>Android.Net.Uri.</returns>
		public static Android.Net.Uri ToAndroidUri(this Uri uri)
		{
			return Android.Net.Uri.Parse(uri.AbsoluteUri);
		}

		/// <summary>
		/// To the system URI.
		/// </summary>
		/// <param name="uri">The URI.</param>
		/// <returns>Uri.</returns>
		public static Uri ToSystemUri(this Android.Net.Uri uri)
		{
			return new Uri(uri.ToString());
		}
	}
}