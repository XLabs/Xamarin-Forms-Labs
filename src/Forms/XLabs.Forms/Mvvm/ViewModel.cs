using System;
using System.Runtime.CompilerServices;
using XLabs.Ioc;

namespace XLabs.Forms.Mvvm
{
	using XLabs.Data;
	using XLabs.Platform.Services;

	/// <summary>
	/// View model base class.
	/// </summary>
	/// <example>
	/// To implement observable property:
	/// private object propertyBackField;
	/// public object Property
	/// {
	/// get { return this.propertyBackField; }
	/// set
	/// {
	/// this.ChangeAndNotify(ref this.propertyBackField, value);
	/// }
	/// </example>
	public abstract class ViewModel : ObservableObject, IViewModel
	{
		/// <summary>
		/// Gets or sets the navigation service.
		/// </summary>
		/// <value>The navigation service.</value>
		public INavigationService NavigationService
		{
			get { return _navigationService ?? Resolver.Resolve<INavigationService>(); }
			set { _navigationService = value; }
		}

		private bool _isBusy;
		private INavigationService _navigationService;

		/// <summary>
		/// Gets or sets the forms navigation.
		/// </summary>
		/// <value>The forms navigation.</value>
		public ViewModelNavigation Navigation {
			get;
			set;
		}
		/// <summary>
		/// Gets or sets a value indicating whether this instance is busy.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is busy; otherwise, <c>false</c>.
		/// </value>
		public bool IsBusy
		{
			get { return _isBusy; }
			set
			{
				SetProperty<bool>(ref _isBusy, value);
			}
		}
		
		/// <summary>
		/// Called when the view appears.
		/// </summary>
		public virtual void OnViewAppearing() { }
		
		/// <summary>
		/// Called when the view disappears.
		/// </summary>
		public virtual void OnViewDisappearing() { }
		
		#region Protected methods
		/// <summary>
		/// Changes the property if the value is different and invokes PropertyChangedEventHandler.
		/// </summary>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		/// <param name="property">Property.</param>
		/// <param name="value">Value.</param>
		/// <param name="propertyName">Property name.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		[Obsolete("Use the SetProperty method instead.")]
		protected bool ChangeAndNotify<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
		{
			return SetProperty(ref property, value, propertyName);
		}

		#endregion
	}
}

