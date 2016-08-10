// ***********************************************************************
// Assembly         : XLabs.Platform.WinUniversal
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Battery.cs" company="XLabs Team">
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

namespace XLabs.Platform.Device
{
    /// <summary>
    /// Windows Phone Battery class.
    /// </summary>
    public partial class Battery
    {
  
        /// <summary>
        /// Gets a value indicating whether battery is charging
        /// </summary>
        /// <value><c>true</c> if charging; otherwise, <c>false</c>.</value>
        public bool Charging
        {
            get { return false; }
        }

        #region partial implementations

        /// <summary>
        /// Starts the charger monitoring.
        /// </summary>
        partial void StartChargerMonitoring()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Stops the charger monitoring.
        /// </summary>
        partial void StopChargerMonitoring()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}