// ***********************************************************************
// Assembly         : XLabs.Core
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Report.cs" company="XLabs Team">
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

namespace XLabs.Reporting
{
    /// <summary>
    /// Static Report class.
    /// </summary>
    public static class Report
    {
        /// <summary>
        /// The reporters
        /// </summary>
        private static readonly List<IReport> Reporters;

        /// <summary>
        /// Initializes static members of the <see cref="Report"/> class.
        /// </summary>
        static Report()
        {
            Reporters = new List<IReport>(); 
        }

        /// <summary>
        /// Adds the specified report.
        /// </summary>
        /// <param name="report">The report.</param>
        public static void Add(IReport report)
        {
            Reporters.Add(report);
        }

        /// <summary>
        /// Removes the specified report.
        /// </summary>
        /// <param name="report">The report.</param>
        public static void Remove(IReport report)
        {
            Reporters.Remove(report);
        }

        #region IReport calls

        /// <summary>
        /// Exceptions the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public static void Exception(System.Exception exception)
        {
            foreach (var reporter in Reporters)
            {
                reporter.Exception(exception);
            }
        }

        #endregion
    }
}