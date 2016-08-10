// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ViewModelNavigation.cs" company="XLabs Team">
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
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XLabs.Forms.Mvvm
{
    /// <summary>
    /// Class ViewModelNavigation.
    /// </summary>
    public class ViewModelNavigation
    {
        /// <summary>
        /// The _implementor
        /// </summary>
        readonly INavigation _implementor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelNavigation"/> class.
        /// </summary>
        /// <param name="implementor">The implementor.</param>
        public ViewModelNavigation(INavigation implementor)
        {
            this._implementor = implementor;
        }

        // This method can be considered unclean in the pure MVVM sense, 
        // however it has been handy to me at times
        /// <summary>
        /// Pushes the asynchronous.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="animated">If set to <c>true</c> the navigation is animated.</param>
        /// <returns>Task.</returns>
        public Task PushAsync(Page page, bool animated = true)
        {
            return this._implementor.PushAsync(page, animated);
        }

        /// <summary>
        /// Pushes the asynchronous.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the t view model.</typeparam>
        /// <param name="animated">If set to <c>true</c> the navigation is animated.</param>
        /// <returns>Task.</returns>
        public Task PushAsync<TViewModel>(bool animated = true)
            where TViewModel : ViewModel
        {
            return this.PushAsync<TViewModel>(null, animated);
        }

        /// <summary>
        /// Pushes the asynchronous.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the t view model.</typeparam>
        /// <param name="activateAction">The activate action.</param>
        /// <param name="animated">If set to <c>true</c> the navigation is animated.</param>
        /// <returns>Task.</returns>
        public Task PushAsync<TViewModel>(Action<TViewModel, Page> activateAction, bool animated = true)
            where TViewModel : ViewModel
        {
            return this.PushAsync(ViewFactory.CreatePage<TViewModel, Page>(activateAction) as Page, animated);
        }

        /// <summary>
        /// Pops the asynchronous.
        /// </summary>
        /// <param name="animated">If set to <c>true</c> the navigation is animated.</param>
        /// <returns>Task.</returns>
        public Task PopAsync(bool animated = true)
        {
            return this._implementor.PopAsync(animated);
        }

        /// <summary>
        /// Pops to root asynchronous.
        /// </summary>
        /// <param name="animated">If set to <c>true</c> the navigation is animated.</param>
        /// <returns>Task.</returns>
        public Task PopToRootAsync(bool animated = true)
        {
            return this._implementor.PopToRootAsync(animated);
        }

        // This method can be considered unclean in the pure MVVM sense, 
        // however it has been handy to me at times
        /// <summary>
        /// Pushes the modal asynchronous.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="animated">If set to <c>true</c> the navigation is animated.</param>
        /// <returns>Task.</returns>
        public Task PushModalAsync(Page page, bool animated = true)
        {
            return this._implementor.PushModalAsync(page, animated);
        }

        /// <summary>
        /// Pushes the modal asynchronous.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="animated">If set to <c>true</c> the navigation is animated.</param>
        /// <returns></returns>
        public Task PushModalAsync<TViewModel>(bool animated = true)
            where TViewModel : ViewModel
        {
            return this.PushModalAsync<TViewModel>(null, animated);
        }

        /// <summary>
        /// Pushes the modal asynchronous.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the t view model.</typeparam>
        /// <param name="activateAction">The create action.</param>
        /// <param name="animated">If set to <c>true</c> the navigation is animated.</param>
        /// <returns>Task.</returns>
        public Task PushModalAsync<TViewModel>(Action<TViewModel, Page> activateAction, bool animated = true)
            where TViewModel : ViewModel
        {
            return this.PushModalAsync(ViewFactory.CreatePage<TViewModel, Page>(activateAction) as Page, animated);
        }

        /// <summary>
        /// Pops the modal asynchronous.
        /// </summary>
        /// <param name="animated">If set to <c>true</c> the navigation is animated.</param>
        /// <returns>Task.</returns>
        public Task PopModalAsync(bool animated = true)
        {
            return this._implementor.PopModalAsync(animated);
        }

        /// <summary>
        /// Removes the specified view model.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="viewModel">The view model.</param>
        /// <param name="animated">If set to <c>true</c> the navigation is animated.</param>
        /// <returns>Task.</returns>
        public async Task RemoveAsync<TViewModel>(TViewModel viewModel, bool animated = true)
            where TViewModel : ViewModel
        {
            foreach (var page in this._implementor.NavigationStack)
            {
                if (page.BindingContext == viewModel)
                {
                    // If the page is on top of the stack it must be popped first
                    if (this._implementor.NavigationStack[this._implementor.NavigationStack.Count - 1] == page)
                    {
                        await this.PopAsync(animated);
                    }

                    // Clear the view model/bindings
                    page.BindingContext = null;

                    // Remove the page from the stack
                    this._implementor.RemovePage(page);
                    return;
                }
            }
        }
    }
}