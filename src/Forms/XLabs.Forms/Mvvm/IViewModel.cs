// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="IViewModel.cs" company="XLabs Team">
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

using XLabs.Platform.Services;

namespace XLabs.Forms.Mvvm
{
	/// <summary>
	/// Interface IViewModel
	/// </summary>
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