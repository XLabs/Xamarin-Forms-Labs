using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace XForms.Toolkit
{
    /// <summary>
    /// Battery portion for Apple devices.
    /// </summary>
    public partial class Battery
    {
        /// <summary>
        ///  Gets the battery level. 
        /// </summary>
        /// <returns>Battery level in percentage, 0-100</returns>
        public int Level
        {
            get
            {
                UIDevice.CurrentDevice.BatteryMonitoringEnabled = true;
                return (int)(UIDevice.CurrentDevice.BatteryLevel * 100);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="SimplyMobile.Device.Battery"/> is charging.
        /// </summary>
        /// <value><c>true</c> if charging; otherwise, <c>false</c>.</value>
        public bool Charging
        {
            get
            {
                return UIDevice.CurrentDevice.BatteryState != UIDeviceBatteryState.Unplugged;
            }
        }

        /// <summary>
        /// Starts the level monitor.
        /// </summary>
        partial void StartLevelMonitoring()
        {
            UIDevice.CurrentDevice.BatteryMonitoringEnabled = true;
            NSNotificationCenter.DefaultCenter.AddObserver
            (
                UIDevice.BatteryLevelDidChangeNotification,
                (NSNotification n) =>
                {
                    if (onLevelChange != null)
                    {
                        onLevelChange(onLevelChange, new EventArgs<int>(Level));
                    }
                }
            );
        }

        /// <summary>
        /// Stops the level monitoring.
        /// </summary>
        partial void StopLevelMonitoring()
        {
            NSNotificationCenter.DefaultCenter.RemoveObserver
            (
                UIDevice.BatteryLevelDidChangeNotification
            );

            // if charger monitor does not have subscribers then lets disable battery monitoring
            UIDevice.CurrentDevice.BatteryMonitoringEnabled = (onChargerStatusChanged != null);
        }

        /// <summary>
        /// Stops the charger monitoring.
        /// </summary>
        partial void StopChargerMonitoring()
        {
            NSNotificationCenter.DefaultCenter.RemoveObserver
            (
                UIDevice.BatteryStateDidChangeNotification
            );

            // if level monitor does not have subscribers then lets disable battery monitoring
            UIDevice.CurrentDevice.BatteryMonitoringEnabled = (onLevelChange != null);
        }

        /// <summary>
        /// Starts the charger monitoring.
        /// </summary>
        partial void StartChargerMonitoring()
        {
            UIDevice.CurrentDevice.BatteryMonitoringEnabled = true;
            NSNotificationCenter.DefaultCenter.AddObserver
            (
                UIDevice.BatteryStateDidChangeNotification,
                (NSNotification n) =>
                {
                    if (onChargerStatusChanged != null)
                    {
                        onChargerStatusChanged(onChargerStatusChanged, new EventArgs<bool>(Charging));
                    }
                }
            );
        }
    }
}