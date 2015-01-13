using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace XLabs.Forms.Services
{
	using System.Collections.Generic;

	using UIKit;

	using XLabs.Platform.Device;
	using XLabs.Platform.Extensions;

	/// <summary>
	/// Class FontManager.
	/// </summary>
	public partial class FontManager : IFontManager
	{
		/// <summary>
		/// The _display
		/// </summary>
		private readonly IDisplay _display;

		/// <summary>
		/// Initializes a new instance of the <see cref="FontManager"/> class.
		/// </summary>
		/// <param name="display">The display.</param>
		public FontManager(IDisplay display)
		{
			_display = display;
		}

		#region IFontManager Members

		/// <summary>
		/// Gets all available system fonts.
		/// </summary>
		/// <value>The available fonts.</value>
		public IEnumerable<string> AvailableFonts
		{
			get
			{
				return UIFont.FamilyNames;
			}
		}

		/// <summary>
		/// Gets height for the font.
		/// </summary>
		/// <param name="font">Font whose height is calculated.</param>
		/// <returns>Height of the font in inches.</returns>
		public double GetHeight(Font font)
		{
			var height = (double)string.Empty.StringHeight(font.ToUIFont(), float.MaxValue);

			return height * _display.ScreenHeightInches() / _display.Height;
		}

		#endregion
	}
}