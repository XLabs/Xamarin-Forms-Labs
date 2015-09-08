namespace XLabs.Platform.Services
{
	using System;

	/// <summary>
	/// Interface INavigationService
	/// </summary>
	public interface INavigationService
	{
		/// <summary>
		/// Registers the page (this must be called if you want to use Navigation by pageKey).
		/// </summary>
		/// <param name="pageKey">The page key.</param>
		/// <param name="pageType">Type of the page.</param>
		void RegisterPage(string pageKey, Type pageType);

		/// <summary>
		/// Navigates to.
		/// </summary>
		/// <param name="pageKey">The page key.</param>
		/// <param name="args">The arguments.</param>
		/// <param name="animated">if set to <c>true</c> [animated].</param>
		void NavigateTo(string pageKey, bool animated = true, params object[] args);

		/// <summary>
		/// Navigates to.
		/// </summary>
		/// <param name="pageType">Type of the page.</param>
		/// <param name="args">The arguments.</param>
		/// <param name="animated">if set to <c>true</c> [animated].</param>
		void NavigateTo(Type pageType, bool animated = true, params object[] args);


		/// <summary>
		/// Navigates to.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="args">The arguments.</param>
		/// <param name="animated">if set to <c>true</c> [animated].</param>
		void NavigateTo<T>(bool animated = true, params object[] args) where T : class;

		/// <summary>
		/// Goes back.
		/// </summary>
		void GoBack();

		/// <summary>
		/// Goes forward.
		/// </summary>
		void GoForward();
	}
}