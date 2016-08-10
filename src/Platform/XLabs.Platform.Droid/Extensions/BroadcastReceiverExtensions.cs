// ***********************************************************************
// Assembly         : XLabs.Platform.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="BroadcastReceiverExtensions.cs" company="XLabs Team">
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

namespace XLabs.Platform
{
	/// <summary>
	/// Broadcast receiver extensions.
	/// </summary>
	public static class BroadcastReceiverExtensions
	{
		/// <summary>
		/// Registers the receiver using <see cref="Application.Context"/>.
		/// </summary>
		/// <returns>The receiver intent.</returns>
		/// <param name="receiver">Receiver.</param>
		/// <param name="intentFilter">Intent filter.</param>
		public static Intent RegisterReceiver(this BroadcastReceiver receiver, IntentFilter intentFilter)
		{
			return Application.Context.RegisterReceiver(receiver, intentFilter);
		}

		/// <summary>
		/// Unregisters the receiver using <see cref="Application.Context"/>.
		/// </summary>
		/// <param name="receiver">Receiver to unregister.</param>
		public static void UnregisterReceiver(this BroadcastReceiver receiver)
		{
			Application.Context.UnregisterReceiver(receiver);
		}
	}
}