// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="PredicatePriority.cs" company="XLabs Team">
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
namespace XLabs.Forms.Validation
{
    /// <summary>
    /// The order that predicates are evaluated in
    /// </summary>
    /// Element created at 08/11/2014,2:37 PM by Charles
    internal enum PredicatePriority
    {
        /// <summary>High Priority, No type conversion or processing required</summary>
        /// Element created at 08/11/2014,2:37 PM by Charles
        High=0,

        /// <summary>Medium Priority Type conversions and evaluation</summary>
        /// Element created at 08/11/2014,2:37 PM by Charles
        Medium=2,
        /// <summary>
        /// Low priority anything with regular expressions
        /// </summary>
        Low=3,
        /// <summary>
        /// User Priority Callback and Pattern
        /// </summary>
        User=4
    }
}