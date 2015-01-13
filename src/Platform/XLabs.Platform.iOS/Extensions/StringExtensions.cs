using System;

namespace XLabs.Platform.Extensions
{
	using CoreGraphics;

	using Foundation;
	using UIKit;

	/// <summary>
	/// Class StringExtensions.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Strings the height.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="font">The font.</param>
		/// <param name="width">The width.</param>
		/// <returns>System.Single.</returns>
		public static nfloat StringHeight(this string text, UIFont font, nfloat width)
		{
			var nativeString = new NSString(text);

			var rect = nativeString.GetBoundingRect(
				new CGSize(width, float.MaxValue),
				NSStringDrawingOptions.UsesLineFragmentOrigin,
				new UIStringAttributes { Font = font },
				null);

			return rect.Height;
		}
	}
}