// ***********************************************************************
// Assembly         : XLabs.Core
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ListGroup.cs" company="XLabs Team">
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
    /// Observable collection grouped by a key.
    /// </summary>
    /// <typeparam name="TKey">Key value type.</typeparam>
    /// <typeparam name="T">Group value type.</typeparam>
    public class ListGroup<TKey, T> : ObservableCollection<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListGroup{TKey, T}"/> class.
        /// </summary>
        public ListGroup()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListGroup{TKey, T}"/> class.
        /// </summary>
        /// <param name="items">The collection from which the elements are copied.</param>
        public ListGroup(IEnumerable<T> items)
            : base(items)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListGroup{TKey, T}"/> class.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <param name="items">The collection from which the elements are copied.</param>
        public ListGroup(TKey key, IEnumerable<T> items)
            : base(items)
        {
            this.Key = key;
        }

        /// <summary>
        /// Gets or sets the key for the group.
        /// </summary>
        public TKey Key { get; set; }
    }
}
