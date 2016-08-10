// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="INavigationAware.cs" company="XLabs Team">
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

using Xamarin.Forms;

namespace XLabs.Forms.Mvvm
{
    /// <summary>
    /// Interface INavigationAware
    /// </summary>
    public interface INavigationAware
    {
        /// <summary>
        /// Called when being navigated to.
        /// </summary>
        /// <remarks>
        /// Can be implemented on either viewmodel or view.
        /// </remarks>
        /// <param name="previousView">The view being navigated away from.</param>
        void OnNavigatingTo(Page previousView);

        /// <summary>
        /// Called when being navigated away from.
        /// </summary>
        /// <remarks>
        /// Can be implemented on either viewmodel or view.
        /// </remarks>
        /// <param name="nextView">The view being navigated to.</param>
        void OnNavigatingFrom(Page nextView);
    }
}
