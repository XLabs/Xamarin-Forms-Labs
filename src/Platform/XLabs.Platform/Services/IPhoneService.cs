// ***********************************************************************
// Assembly         : XLabs.Platform
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="IPhoneService.cs" company="XLabs Team">
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
    /// Interface for phone functionality and information.
    /// </summary>
    public interface IPhoneService
    {
        /// <summary>
        /// Gets the cellular provider.
        /// </summary>
        string CellularProvider { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has cellular data enabled.
        /// </summary>
        bool? IsCellularDataEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has cellular data roaming enabled.
        /// </summary>
        bool? IsCellularDataRoamingEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has network available.
        /// </summary>
        bool? IsNetworkAvailable { get; }

        /// <summary>
        /// Gets the ISO Country Code.
        /// </summary>
        string ICC { get; }

        /// <summary>
        /// Gets the Mobile Country Code.
        /// </summary>
        string MCC { get; }

        /// <summary>
        /// Gets the Mobile Network Code.
        /// </summary>
        string MNC { get; }

        /// <summary>
        /// Gets whether the service can send SMS
        /// </summary>
        bool CanSendSMS { get; }

        /// <summary>
        /// Opens native dialog to dial the specified number.
        /// </summary>
        /// <param name="number">Number to dial.</param>
        void DialNumber(string number);

        /// <summary>
        /// Sends the SMS.
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="body">The body.</param>
        void SendSMS(string to, string body);
    }
}

