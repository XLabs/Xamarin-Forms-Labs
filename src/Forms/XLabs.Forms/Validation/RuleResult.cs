// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="RuleResult.cs" company="XLabs Team">
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
    /// The result of a validation (a single rule or an entire validationset)
    /// </summary>
    /// Element created at 08/11/2014,12:57 PM by Charles
    public enum RuleResult
    {
        /// <summary>Empty value, we should never see it in the wild</summary>
        /// Element created at 08/11/2014,12:57 PM by Charles
        Unknown=0,
        /// <summary>The validation was successful</summary>
        /// Element created at 08/11/2014,12:58 PM by Charles
        ValidationSuccess,

        /// <summary>The validation failed</summary>
        /// Element created at 08/11/2014,12:58 PM by Charles
        ValidationFailure,

        /// <summary>
        /// There was no change from the last validation attempt
        /// </summary>
        /// Element created at 08/11/2014,12:58 PM by Charles
        ValidationNoChange
    }
}