// ***********************************************************************
// Assembly         : XLabs.Core
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="GroupedListSource.cs" company="XLabs Team">
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

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace XLabs.Helpers
{
    /// <summary>
    /// Observable collection 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="T"></typeparam>
    public class GroupedListSource<TKey, T> : ObservableCollection<ListGroup<TKey, T>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupedListSource{TKey, T}"/> class.
        /// </summary>
        public GroupedListSource()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupedListSource{TKey, T}"/> class that contains elements copied from the specified collection.
        /// </summary>
        /// <param name="items">The collection from which the elements are copied.</param>
        public GroupedListSource(IEnumerable<ListGroup<TKey, T>> items)
            : base(items)
        {

        }
    }
}
