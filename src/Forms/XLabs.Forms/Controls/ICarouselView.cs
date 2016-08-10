// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ICarouselView.cs" company="XLabs Team">
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
namespace XLabs.Forms.Controls
{
    /// <summary>
    /// An interface carousel views can
    /// implement to receive
    /// lifetime event notifications
    /// </summary>
    /// Element created at 15/11/2014,3:36 PM by Charles
    public interface ICarouselView
    {
        /// <summary>The view is about to be shown</summary>
        /// Element created at 15/11/2014,3:36 PM by Charles
        void Showing();

        /// <summary>The view has been shown</summary>
        /// Element created at 15/11/2014,3:37 PM by Charles
        void Shown();

        /// <summary>The view is about to be hiden</summary>
        /// Element created at 15/11/2014,3:37 PM by Charles
        void Hiding();

        /// <summary>The view has been hiden</summary>
        /// Element created at 15/11/2014,3:37 PM by Charles
        void Hiden();
    }
}