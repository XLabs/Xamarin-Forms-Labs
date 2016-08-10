// ***********************************************************************
// Assembly         : XLabs.Platform
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="INavigationService.cs" company="XLabs Team">
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

namespace XLabs.Platform.Services
{
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