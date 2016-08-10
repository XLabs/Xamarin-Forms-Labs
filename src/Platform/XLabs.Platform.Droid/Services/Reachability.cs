// ***********************************************************************
// Assembly         : XLabs.Platform.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Reachability.cs" company="XLabs Team">
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

using Android.App;
using Android.Content;
using Android.Net;

namespace XLabs.Platform.Services
{
	/// <summary>
	/// Class Reachability.
	/// </summary>
	public static class Reachability
	{
		/// <summary>
		/// Gets the connectivity manager.
		/// </summary>
		/// <value>The connectivity manager.</value>
		public static ConnectivityManager ConnectivityManager
		{
			get
			{
				return Application.Context.GetSystemService(Context.ConnectivityService) as ConnectivityManager;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is active network wifi.
		/// </summary>
		/// <value><c>true</c> if this instance is active network wifi; otherwise, <c>false</c>.</value>
		public static bool IsActiveNetworkWifi
		{
			get
			{
				var activeConnection = ConnectivityManager.ActiveNetworkInfo;

				return activeConnection.Type == ConnectivityType.Wifi;
			}
		}

		/// <summary>
		/// Determines whether [is network available].
		/// </summary>
		/// <returns><c>true</c> if [is network available]; otherwise, <c>false</c>.</returns>
		public static bool IsNetworkAvailable()
		{
			var activeConnection = ConnectivityManager.ActiveNetworkInfo;

			if (activeConnection != null && activeConnection.IsConnected)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Internets the connection status.
		/// </summary>
		/// <returns>NetworkStatus.</returns>
		public static NetworkStatus InternetConnectionStatus()
		{
			if (IsNetworkAvailable())
			{
				var wifiState = ConnectivityManager.GetNetworkInfo(ConnectivityType.Wifi).GetState();
				if (wifiState == NetworkInfo.State.Connected)
				{
					return NetworkStatus.ReachableViaWiFiNetwork;
				}

				var mobileState = ConnectivityManager.GetNetworkInfo(ConnectivityType.Mobile).GetState();
				if (mobileState == NetworkInfo.State.Connected)
				{
					return NetworkStatus.ReachableViaCarrierDataNetwork;
				}
			}

			return NetworkStatus.NotReachable;
		}
	}
}