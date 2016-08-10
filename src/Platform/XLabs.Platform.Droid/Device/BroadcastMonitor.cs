// ***********************************************************************
// Assembly         : XLabs.Platform.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="BroadcastMonitor.cs" company="XLabs Team">
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

using Android.Content;

namespace XLabs.Platform.Device
{
	/// <summary>
	/// Broadcast monitor.
	/// </summary>
	public abstract class BroadcastMonitor : BroadcastReceiver
	{
		/// <summary>
		///  Start monitoring. 
		/// </summary>
		public virtual bool Start()
		{
			var intent = this.RegisterReceiver(this.Filter);
			if (intent == null)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		///  Stop monitoring. 
		/// </summary>
		public virtual void Stop()
		{
			this.UnregisterReceiver();
		}

		/// <summary>
		/// Gets the intent filter to use for monitoring.
		/// </summary>
		protected abstract IntentFilter Filter { get; }
	}
}