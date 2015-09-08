using XLabs.Platform.Services;

namespace XLabs.Forms.Mvvm
{
	public interface IViewModel
	{
		/// <summary>
		/// Gets or sets the navigation service.
		/// </summary>
		/// <value>The navigation.</value>
		INavigationService NavigationService { get; set; }

		/// <summary>
		/// Gets or sets the navigation.
		/// </summary>
		/// <value>The Forms navigation.</value>
		ViewModelNavigation Navigation { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether this instance is busy.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is busy; otherwise, <c>false</c>.
		/// </value>
		bool IsBusy { get; set; }
	}
}