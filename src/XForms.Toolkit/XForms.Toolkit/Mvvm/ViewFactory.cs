using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XForms.Toolkit.Mvvm
{
	[AttributeUsage (AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class ViewTypeAttribute : Attribute
	{
		public Type ViewType { get; private set; }

		public ViewTypeAttribute (Type viewType)
		{
			ViewType = viewType;
		}
	}
	// Can be replaced by all sorts of complexity and auto loading BS but this keeps it simple and loose
	public static class ViewFactory
	{
		static readonly Dictionary<Type, Type> TypeDictionary = new Dictionary<Type, Type> (); 
		public static void Register<TView, TViewModel> ()
			where TView : Page
			where TViewModel : ViewModel
		{
			TypeDictionary[typeof (TViewModel)] = typeof (TView);
		}

		public static Page CreatePage (Type viewModelType)
		{
			Type viewType = null;
			if (TypeDictionary.ContainsKey (viewModelType)) {
				viewType = TypeDictionary[viewModelType];
			} else {
				throw new InvalidOperationException ("Unknown View for ViewModel");
			}

			var page = (Page)Activator.CreateInstance (viewType);
			var viewModel = (ViewModel)Activator.CreateInstance (viewModelType);
			page.BindingContext = viewModel;

			return page;
		}

		public static Page CreatePage<TViewModel> ()
			where TViewModel : ViewModel
		{
			var viewModelType = typeof (TViewModel);
			return CreatePage (viewModelType);
		}
	}
}
