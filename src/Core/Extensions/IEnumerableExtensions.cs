// ***********************************************************************
// Assembly         : XLabs.Core
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="IEnumerableExtensions.cs" company="XLabs Team">
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
using System.Collections.ObjectModel;
using System.Linq;

namespace XLabs
{
    /// <summary>
    /// Class IEnumerableExtensions.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Converts <see cref="IEnumerable{T}"/> to a <see cref="IReadOnlyCollection{T}"/>
        /// </summary>
        /// <param name="enumerable">Enumerable object.</param>
        /// <typeparam name="T">Type of item.</typeparam>
        /// <returns>IReadOnlyCollection{T}</returns>
        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> enumerable)
        {
            return new ReadOnlyCollection<T>((enumerable as IList<T>) ?? enumerable.ToList());
        }

        /// <summary>
        /// Fors the each.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
                yield return item;
            }
        }
    }
}