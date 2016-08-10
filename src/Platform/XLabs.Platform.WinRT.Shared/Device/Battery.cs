// ***********************************************************************
// Assembly         : XLabs.Platform
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
namespace XLabs.Platform.Device
{
    /// <summary>
    /// Windows Phone Battery class.
    /// </summary>
    public partial class Battery
    {
        /// <summary>
        /// Gets the level.
        /// </summary>
        /// <value>The level in percentage 0-100.</value>
        public int Level
        {
            get
            {
#if !NETFX_CORE
                return Windows.Phone.Devices.Power.Battery.GetDefault().RemainingChargePercent;
#else
                return 0;
#endif
            }
        }

        /// <summary>
        /// Called when [remaining charge percent changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="o">The o.</param>
        private void OnRemainingChargePercentChanged(object sender, object o)
        {
            onLevelChange?.Invoke(sender, Level);
        }

        /// <summary>
        /// Handles the <see cref="E:PowerSourceChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnPowerSourceChanged(object sender, System.EventArgs eventArgs)
        {
            onChargerStatusChanged?.Invoke(sender, Charging);
        }

        #region partial implementations

        /// <summary>
        /// Starts the level monitoring.
        /// </summary>
        partial void StartLevelMonitoring()
        {
#if !NETFX_CORE
            Windows.Phone.Devices.Power.Battery.GetDefault().RemainingChargePercentChanged += OnRemainingChargePercentChanged;
#else
#endif
        }

        /// <summary>
        /// Stops the level monitoring.
        /// </summary>
        partial void StopLevelMonitoring()
        {
#if !NETFX_CORE
            Windows.Phone.Devices.Power.Battery.GetDefault().RemainingChargePercentChanged -= OnRemainingChargePercentChanged;
#else
#endif
        }

        #endregion
    }
}