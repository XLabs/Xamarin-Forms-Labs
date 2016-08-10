// ***********************************************************************
// Assembly         : XLabs.Core
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="EventArgs{T}.cs" company="XLabs Team">
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

namespace XLabs
{
    /// <summary>
    /// Generic event argument class
    /// </summary>
    /// <typeparam name="T">Type of the argument</typeparam>
    public class EventArgs<T> : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgs"/> class.
        /// </summary>
        /// <param name="value">Value of the argument</param>
        public EventArgs(T value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets the value of the event argument
        /// </summary>
        public T Value { get; private set; }
    }
}
