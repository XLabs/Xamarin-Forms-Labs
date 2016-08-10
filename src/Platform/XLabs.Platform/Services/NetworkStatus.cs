// ***********************************************************************
// Assembly         : XLabs.Platform
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="NetworkStatus.cs" company="XLabs Team">
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

namespace XLabs.Platform.Services
{
    /// <summary>
    /// The network status.
    /// </summary>
    public enum NetworkStatus
    {
        /// <summary>
        /// Network not reachable.
        /// </summary>
        NotReachable,

        /// <summary>
        /// Network reachable via carrier data network.
        /// </summary>
        ReachableViaCarrierDataNetwork,

        /// <summary>
        /// Network reachable via WiFi network.
        /// </summary>
        ReachableViaWiFiNetwork,
        
        /// <summary>
        /// Network reachable via an unknown network
        /// </summary>
        ReachableViaUnknownNetwork
    }
}
