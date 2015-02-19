using System;
using System.Globalization;
using Xamarin.Forms;

namespace XLabs.Forms.Mvvm
{
	/// <summary>
	/// Converts the Xamarin Forms page navigation to our <see cref="ViewModelNavigation" /> instance.
	/// </summary>
	class NavigationConverter : IValueConverter
	{
		/// <summary>
		/// Implement this method to convert <paramref name="value" /> to <paramref name="targetType" /> by using <paramref name="parameter" /> and <paramref name="culture" />.
		/// </summary>
		/// <param name="value">To be added.</param>
		/// <param name="targetType">To be added.</param>
		/// <param name="parameter">To be added.</param>
		/// <param name="culture">To be added.</param>
		/// <returns>To be added.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		/// <remarks>To be added.</remarks>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Implement this method to convert <paramref name="value" /> back from <paramref name="targetType" /> by using <paramref name="parameter" /> and <paramref name="culture" />.
		/// </summary>
		/// <param name="value">To be added.</param>
		/// <param name="targetType">To be added.</param>
		/// <param name="parameter">To be added.</param>
		/// <param name="culture">To be added.</param>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return new ViewModelNavigation((INavigation)value);
		}
	}

	public class BaseView : ContentPage
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BaseView"/> class.
		/// Binds the Navigation and IsBusy property.
		/// </summary>
		public BaseView()
		{
			SetBinding(NavigationProperty, new Binding("Navigation", converter: new NavigationConverter()));
			SetBinding(IsBusyProperty, new Binding("IsBusy"));
		}
		/// <summary>
		/// Passes the event of the view appearing through to the view model.
		/// </summary>
		protected override void OnAppearing()
		{
			base.OnAppearing();
			if (BindingContext != null && BindingContext is ViewModel)
			{
				var vm = (ViewModel)BindingContext;
				vm.OnViewAppearing();
			}
        	}
		/// <summary>
		/// Passes the event of the view disappearing through to the view model.
		/// </summary>
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			if (BindingContext != null && BindingContext is ViewModel)
			{
				var vm = (ViewModel)BindingContext;
				vm.OnViewDisappearing();
			}
        	}
	}
}
