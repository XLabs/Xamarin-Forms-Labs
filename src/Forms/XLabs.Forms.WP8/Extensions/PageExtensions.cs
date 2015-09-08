using XLabs.Platform.Mvvm;

namespace XLabs.Forms.Extensions
{
	using Microsoft.Phone.Controls;

	using XLabs.Ioc;

	/// <summary>
	/// Class PageExtensions.
	/// </summary>
	public static class PageExtensions
	{
		/// <summary>
		/// Sets the orientation.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <param name="orientation">The orientation.</param>
		public static void SetOrientation(this PhoneApplicationPage page, PageOrientation? orientation = null)
		{
			var app = Resolver.Resolve<IXFormsApp>() as XFormsAppWP;

			if (app != null)
			{
				app.SetOrientation(orientation ?? page.Orientation);
			}
		}
	}
}