// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="NavigationAwareViewModel.cs" company="XLabs Team">
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
    /// Class NavigationAwareViewModel.
    /// </summary>
    public class NavigationAwareViewModel : ViewModel, INavigationAware
    {
        /// <summary>
        /// Called when being navigated to.
        /// </summary>
        /// <param name="previousView">The view being navigated away from.</param>
        public virtual void OnNavigatingTo(Page previousView)
        {
        }

        /// <summary>
        /// Called when being navigated away from.
        /// </summary>
        /// <param name="nextView">The view being navigated to.</param>
        public virtual void OnNavigatingFrom(Page nextView)
        {
        }
    }
}
