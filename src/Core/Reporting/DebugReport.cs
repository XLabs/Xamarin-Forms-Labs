// ***********************************************************************
// Assembly         : XLabs.Core
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="DebugReport.cs" company="XLabs Team">
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

using System.Diagnostics;

namespace XLabs.Reporting
{
    /// <summary>
    /// Writes reports using Debug.WriteLine
    /// </summary>
    public class DebugReport : IReport
    {
        #region IReport Members

        /// <summary>
        /// Report an exception.
        /// </summary>
        /// <param name="exception">Exception that happened.</param>
        public void Exception(System.Exception exception)
        {
            Debug.WriteLine(exception);
        }

        #endregion
    }
}