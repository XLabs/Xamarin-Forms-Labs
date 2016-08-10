// ***********************************************************************
// Assembly         : XLabs.Platform.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Network.cs" company="XLabs Team">
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
	/// Class Network.
	/// </summary>
	public class Network : INetwork
	{
		/// <summary>
		/// Internets the connection status.
		/// </summary>
		/// <returns>NetworkStatus.</returns>
		public NetworkStatus InternetConnectionStatus()
		{
			return Reachability.InternetConnectionStatus();
		}

		/// <summary>
		/// Occurs when [reachability changed].
		/// </summary>
		private event Action<NetworkStatus> reachabilityChanged;

		/// <summary>
		/// Handles the reachability changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void HandleReachabilityChanged(object sender, EventArgs e)
		{
			var reachabilityChanged = this.reachabilityChanged;
			if (reachabilityChanged != null)
			{
				reachabilityChanged(InternetConnectionStatus());
			}
		}

		/// <summary>
		/// Occurs when [reachability changed].
		/// </summary>
		public event Action<NetworkStatus> ReachabilityChanged
		{
			add
			{
				if (reachabilityChanged == null)
				{
					// TODO: check if this actually works
					Reachability.ReachabilityChanged += HandleReachabilityChanged;
				}

				reachabilityChanged += value;
			}

			remove
			{
				reachabilityChanged -= value;

				if (reachabilityChanged == null)
				{
					Reachability.ReachabilityChanged -= HandleReachabilityChanged;
				}
			}
		}

		/// <summary>
		/// Determines whether the specified host is reachable.
		/// </summary>
		/// <param name="host">The host.</param>
		/// <param name="timeout">The timeout.</param>
		public Task<bool> IsReachable(string host, TimeSpan timeout)
		{
			return Task.Run(() => Reachability.IsHostReachable(host));
		}

		/// <summary>
		/// Determines whether [is reachable by wifi] [the specified host].
		/// </summary>
		/// <param name="host">The host.</param>
		/// <param name="timeout">The timeout.</param>
		public Task<bool> IsReachableByWifi(string host, TimeSpan timeout)
		{
			return Task.Run(() => Reachability.RemoteHostStatus(host) == NetworkStatus.ReachableViaWiFiNetwork);
		}
	}
}