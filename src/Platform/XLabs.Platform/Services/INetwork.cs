// ***********************************************************************
// Assembly         : XLabs.Platform
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="INetwork.cs" company="XLabs Team">
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
using System.Threading.Tasks;

namespace XLabs.Platform.Services
{
	/// <summary>
	/// Interface INetwork
	/// </summary>
	public interface INetwork
	{
		/// <summary>
		/// Determines whether the specified host is reachable.
		/// </summary>
		/// <param name="host">The host.</param>
		/// <param name="timeout">The timeout.</param>
		Task<bool> IsReachable(string host, TimeSpan timeout);
		/// <summary>
		/// Determines whether [is reachable by wifi] [the specified host].
		/// </summary>
		/// <param name="host">The host.</param>
		/// <param name="timeout">The timeout.</param>
		Task<bool> IsReachableByWifi(string host, TimeSpan timeout);
		/// <summary>
		/// Internets the connection status.
		/// </summary>
		/// <returns>NetworkStatus.</returns>
		NetworkStatus InternetConnectionStatus ();
		/// <summary>
		/// Occurs when [reachability changed].
		/// </summary>
		event Action<NetworkStatus> ReachabilityChanged;
	}
}
