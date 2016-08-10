// ***********************************************************************
// Assembly         : XLabs.Platform.iOS
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

using Foundation;
using UIKit;

namespace XLabs.Platform.Device
{
	/// <summary>
	///     Battery portion for Apple devices.
	/// </summary>
	public partial class Battery
	{
		/// <summary>
		///     Gets the battery level.
		/// </summary>
		/// <returns>Battery level in percentage, 0-100</returns>
		public int Level
		{
			get
			{
				UIDevice.CurrentDevice.BatteryMonitoringEnabled = true;
				return (int) (UIDevice.CurrentDevice.BatteryLevel*100);
			}
		}

		/// <summary>
		///     Gets a value indicating whether this <see cref="Battery" /> is charging.
		/// </summary>
		/// <value><c>true</c> if charging; otherwise, <c>false</c>.</value>
		public bool Charging
		{
			get { return UIDevice.CurrentDevice.BatteryState != UIDeviceBatteryState.Unplugged; }
		}

		/// <summary>
		///     Starts the level monitor.
		/// </summary>
		partial void StartLevelMonitoring()
		{
			UIDevice.CurrentDevice.BatteryMonitoringEnabled = true;
			NSNotificationCenter.DefaultCenter.AddObserver(
				UIDevice.BatteryLevelDidChangeNotification,
				(NSNotification n) =>
				{
					if (onLevelChange != null)
					{
						onLevelChange(onLevelChange, new EventArgs<int>(Level));
					}
				});
		}

		/// <summary>
		///     Stops the level monitoring.
		/// </summary>
		partial void StopLevelMonitoring()
		{
			NSNotificationCenter.DefaultCenter.RemoveObserver(UIDevice.BatteryLevelDidChangeNotification);

			// if charger monitor does not have subscribers then lets disable battery monitoring
			UIDevice.CurrentDevice.BatteryMonitoringEnabled = (onChargerStatusChanged != null);
		}

		/// <summary>
		///     Stops the charger monitoring.
		/// </summary>
		partial void StopChargerMonitoring()
		{
			NSNotificationCenter.DefaultCenter.RemoveObserver(UIDevice.BatteryStateDidChangeNotification);

			// if level monitor does not have subscribers then lets disable battery monitoring
			UIDevice.CurrentDevice.BatteryMonitoringEnabled = (onLevelChange != null);
		}

		/// <summary>
		///     Starts the charger monitoring.
		/// </summary>
		partial void StartChargerMonitoring()
		{
			UIDevice.CurrentDevice.BatteryMonitoringEnabled = true;
			NSNotificationCenter.DefaultCenter.AddObserver(
				UIDevice.BatteryStateDidChangeNotification,
				(NSNotification n) =>
				{
					if (onChargerStatusChanged != null)
					{
						onChargerStatusChanged(
							onChargerStatusChanged,
							new EventArgs<bool>(Charging));
					}
				});
		}
	}
}