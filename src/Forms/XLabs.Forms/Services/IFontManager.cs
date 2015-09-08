namespace XLabs.Forms.Services
{
	using System.Collections.Generic;

	using Xamarin.Forms;

	public interface IFontManager
	{
		/// <summary>
		/// Gets all available system fonts.
		/// </summary>
		IEnumerable<string> AvailableFonts { get; }

		/// <summary>
		/// Gets height for the font.
		/// </summary>
		/// <param name="font">Font whose height is calculated.</param>
		/// <returns>Height of the font in inches.</returns>
		double GetHeight(Font font);

		/// <summary>
		/// Finds the closest font to the desired height.
		/// </summary>
		/// <param name="name">Name of the font.</param>
		/// <param name="desiredHeight">Desired height in inches.</param>
		/// <returns></returns>
		Font FindClosest(string name, double desiredHeight);
	}
}
