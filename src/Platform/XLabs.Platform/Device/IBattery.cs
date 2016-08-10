// ***********************************************************************
// Assembly         : XLabs.Platform
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="IBattery.cs" company="XLabs Team">
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
	/// Defines battery interface
	/// </summary>
	public interface IBattery
	{
		/// <summary>
		/// Gets the level.
		/// </summary>
		/// <value>The level in percentage 0-100.</value>
		int Level { get; }

		/// <summary>
		/// Gets a value indicating whether battery is charging
		/// </summary>
		/// <value><c>true</c> if charging; otherwise, <c>false</c>.</value>
		bool Charging { get; }

		/// <summary>
		/// Occurs when level changes.
		/// </summary>
		event EventHandler<EventArgs<int>> OnLevelChange;

		/// <summary>
		/// Occurs when charger is connected or disconnected.
		/// </summary>
		event EventHandler<EventArgs<bool>> OnChargerStatusChanged;
	}
}
