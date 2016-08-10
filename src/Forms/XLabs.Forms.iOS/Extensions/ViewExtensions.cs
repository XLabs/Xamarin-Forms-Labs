// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ViewExtensions.cs" company="XLabs Team">
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

using System.Collections.ObjectModel;
using System.Reflection;
using Xamarin.Forms;

namespace XLabs.Forms
{
    /// <summary>
    /// Class ViewExtensions.
    /// </summary>
    public static class ViewExtensions
    {
        //private static Lazy<PropertyInfo> InternalChildrenPropertyInfo = new Lazy<PropertyInfo>(() => typeof(View).GetProperty("InternalChildren", BindingFlags.NonPublic | BindingFlags.Instance));

        /// <summary>
        /// Gets the internal children.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns>ObservableCollection&lt;Element&gt;.</returns>
        public static ObservableCollection<Element> GetInternalChildren(this View view)
        {
            var internalPropertyInfo = view.GetType().GetProperty("InternalChildren", BindingFlags.NonPublic | BindingFlags.Instance);

            return (internalPropertyInfo == null) ? null : internalPropertyInfo.GetValue(view) as ObservableCollection<Element>;
        }
    }
}
