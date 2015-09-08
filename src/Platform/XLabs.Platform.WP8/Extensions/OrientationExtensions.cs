namespace XLabs.Platform
{
	using Microsoft.Phone.Controls;

	using XLabs.Enums;

	/// <summary>
	/// Class OrientationExtensions.
	/// </summary>
	public static class OrientationExtensions
	{
		/// <summary>
		/// To the orientation.
		/// </summary>
		/// <param name="orientation">The orientation.</param>
		/// <returns>Orientation.</returns>
		public static Orientation ToOrientation(this PageOrientation orientation)
		{
			return (Orientation)((int)orientation);
		}

		/// <summary>
		/// To the page orientation.
		/// </summary>
		/// <param name="orientation">The orientation.</param>
		/// <returns>PageOrientation.</returns>
		public static PageOrientation ToPageOrientation(this Orientation orientation)
		{
			return (PageOrientation)((int)orientation);
		}
	}
}