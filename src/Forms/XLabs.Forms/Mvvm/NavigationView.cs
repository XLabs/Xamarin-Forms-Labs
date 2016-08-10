// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="NavigationView.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System.Diagnostics;
using Xamarin.Forms;

namespace XLabs.Forms.Mvvm
{
	/// <summary>
	/// Class NavigationView.
	/// </summary>
	public class NavigationView : NavigationPage
	{
		/// <summary>
		/// The current page property name
		/// </summary>
		private const string CURRENT_PAGE_PROPERTY_NAME = "CurrentPage";

		/// <summary>
		/// The _previous page
		/// </summary>
		private Page _previousPage;
		/// <summary>
		/// The _main page
		/// </summary>
		private Page _mainPage;

		/// <summary>
		/// Initializes a new instance of the <see cref="NavigationView"/> class.
		/// </summary>
		public NavigationView()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NavigationView"/> class.
		/// </summary>
		/// <param name="root">The root.</param>
		public NavigationView(Page root)
			: base(root)
		{
		}

		/// <summary>
		/// Invoked whenever the <see cref="E:Xamarin.Forms.Element.ChildAdded" /> event needs to be emitted. Implement this method to add class handling for this event.
		/// </summary>
		/// <param name="child">The element that was added.</param>
		/// <remarks>This method has no default implementation. You should still call the base implementation in case an intermediate class has implemented this method.</remarks>
		protected override void OnChildAdded(Element child)
		{
			base.OnChildAdded(child);

			Page view = (Page)child;

			if (_mainPage == null)
			{
				_mainPage = view;
			}

			// Since OnChildRemoved event is not triggered for main page.
			if (CurrentPage == _mainPage)
			{
				OnNavigatingFrom(_mainPage, view);
			}

			OnNavigatingTo(view, CurrentPage);
		}

		/// <summary>
		/// Invoked whenever the <see cref="E:Xamarin.Forms.Element.ChildRemoved" /> event needs to be emitted. Implement this method to add class handling for this event.
		/// </summary>
		/// <param name="child">The element that was removed.</param>
		/// <remarks>This method has no default implementation. You should still call the base implementation in case an intermediate class has implemented this method.</remarks>
		protected override void OnChildRemoved(Element child)
		{
			base.OnChildRemoved(child);

			Page view = (Page)child;

			OnNavigatingFrom(view, _previousPage);

			// Since OnChildAdded is not triggered for main page.
			if (_previousPage == _mainPage)
			{
				OnNavigatingTo(_mainPage, view);
			}
		}

		/// <summary>
		/// Call this method from a child class to notify that a change is going to happen on a property.
		/// </summary>
		/// <param name="propertyName">The name of the property that is changing.</param>
		/// <remarks>A <see cref="T:Xamarin.Forms.BindableProperty" /> triggers this by itself. An inheritor only needs to call this for properties without <see cref="T:Xamarin.Forms.BindableProperty" /> as the backend store.</remarks>
		protected override void OnPropertyChanging(string propertyName = null)
		{
			if (propertyName == CURRENT_PAGE_PROPERTY_NAME)
			{
				_previousPage = CurrentPage;
			}

			base.OnPropertyChanging(propertyName);
		}

		/// <summary>
		/// Ases the navigation aware.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>INavigationAware.</returns>
		private INavigationAware AsNavigationAware(VisualElement element)
		{
			var navigationAware = element.BindingContext as INavigationAware;
			if (navigationAware == null)
			{
				navigationAware = element as INavigationAware;
			}

			return navigationAware;
		}

		/// <summary>
		/// Called when [navigating to].
		/// </summary>
		/// <param name="targetView">The target view.</param>
		/// <param name="previousView">The previous view.</param>
		protected void OnNavigatingTo(Page targetView, Page previousView)
		{
			Debug.WriteLine("OnNavigatingTo: targetView={0}, nextView={1}", targetView.GetType().Name, previousView != null ? previousView.GetType().Name : string.Empty);

			var navigationAware = AsNavigationAware(targetView);
			if (navigationAware != null)
			{
				navigationAware.OnNavigatingTo(previousView);
			}
		}

		/// <summary>
		/// Called when [navigating from].
		/// </summary>
		/// <param name="targetView">The target view.</param>
		/// <param name="nextView">The next view.</param>
		protected void OnNavigatingFrom(Page targetView, Page nextView)
		{
			Debug.WriteLine("OnNavigatingFrom: targetView={0}, previousView={1}", targetView.GetType().Name, nextView.GetType().Name);

			var navigationAware = AsNavigationAware(targetView);
			if (navigationAware != null)
			{
				navigationAware.OnNavigatingFrom(nextView);
			}
		}
	}
}
