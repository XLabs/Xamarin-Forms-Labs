// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="NavigationService.cs" company="XLabs Team">
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

using System;
using System.Collections.Generic;
using System.Reflection;
using XLabs.Forms.Mvvm;
using XLabs.Platform.Services;

namespace XLabs.Forms.Services
{
	/// <summary>
	/// Class NavigationService.
	/// </summary>
	public class NavigationService : INavigationService
	{
		private Xamarin.Forms.INavigation _navigation;

		/// <summary>
		/// Initializes a new instance of the <see cref="NavigationService"/> class.
		/// </summary>
		public NavigationService()
		{
			_navigation = Xamarin.Forms.DependencyService.Get<Xamarin.Forms.INavigation>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NavigationService"/> class.
		/// </summary>
		/// <param name="nav">The nav.</param>
		public NavigationService(Xamarin.Forms.INavigation nav)
		{
			_navigation = nav;
		}

		/// <summary>
		/// The _page lookup
		/// </summary>
		private readonly IDictionary<string, Type> _pageLookup = new Dictionary<string, Type>();

		/// <summary>
		/// Registers the page (this must be called if you want to use Navigation by pageKey).
		/// </summary>
		/// <param name="pageKey">The page key.</param>
		/// <param name="pageType">Type of the page.</param>
		/// <exception cref="System.ArgumentException">That pagekey is already registered;pageKey</exception>
		public void RegisterPage(string pageKey, Type pageType)
		{
			if (this._pageLookup.ContainsKey(pageKey))
			{
				throw new ArgumentException("That pagekey is already registered", "pageKey");
			}

			this._pageLookup[pageKey] = pageType;
		}

		/// <summary>
		/// Navigates to.
		/// </summary>
		/// <param name="pageKey">The page key.</param>
		/// <param name="animated">if set to <c>true</c> [animated].</param>
		/// <param name="args">The arguments.</param>
		/// <exception cref="System.ArgumentException">That pagekey is not registered;pageKey</exception>
		public void NavigateTo(string pageKey, bool animated = true, params object[] args)
		{
			if (!this._pageLookup.ContainsKey(pageKey))
			{
				throw new ArgumentException("That pagekey is not registered", "pageKey");
			}

			var pageType = this._pageLookup[pageKey];

			this.NavigateTo(pageType, animated, args);
		}

		/// <summary>
		/// Navigates to.
		/// </summary>
		/// <param name="pageType">Type of the page.</param>
		/// <param name="animated">if set to <c>true</c> [animated].</param>
		/// <param name="args">The arguments.</param>
		/// <exception cref="System.ArgumentException">Argument must be derived from type Xamarin.Forms.Page;pageType</exception>
		public void NavigateTo(Type pageType, bool animated = true, params object[] args)
		{
			if (_navigation == null)
			{
				throw new InvalidOperationException("Xamarin Forms Navigation Service not found");
			}

			object page;
			var pInfo = pageType.GetTypeInfo();
			var xfPage = typeof(Xamarin.Forms.Page).GetTypeInfo();
			var xlvm = typeof(IViewModel).GetTypeInfo();

			if (pInfo.IsAssignableFrom(xlvm) || pInfo.IsSubclassOf(typeof(ViewModel)))
			{
				page = ViewFactory.CreatePage(pageType, null, args);
			}
			else if (pInfo.IsAssignableFrom(xfPage) || pInfo.IsSubclassOf(typeof(Xamarin.Forms.Page)))
			{
				page = Activator.CreateInstance(pageType, args);
			}
			else
			{
				throw new ArgumentException("Page Type must be based on Xamarin.Forms.Page");
			}

			_navigation.PushAsync(page as Xamarin.Forms.Page);
		}

		/// <summary>
		/// Navigates to.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="animated">if set to <c>true</c> [animated].</param>
		/// <param name="args">The arguments.</param>
		/// <exception cref="System.ArgumentException">Page Type must be based on Xamarin.Forms.Page</exception>
		public void NavigateTo<T>(bool animated = true, params object[] args) where T : class
		{
			NavigateTo(typeof(T), animated, args);
		}

		/// <summary>
		/// Goes back.
		/// </summary>
		public void GoBack()
		{
			_navigation.PopAsync(true).Start();
		}

		/// <summary>
		/// Goes forward.
		/// </summary>
		/// <exception cref="System.NotImplementedException"></exception>
		public void GoForward()
		{
#if DEBUG
			throw new NotImplementedException();
#endif
		}
	}
}