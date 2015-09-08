namespace XLabs.Platform.Device
{
    using System;
    using Microsoft.Phone.Info;

    /// <summary>
    /// Windows Phone Battery class.
    /// </summary>
    public partial class Battery
    {
        /// <summary>
        /// Gets a value indicating whether battery is charging
        /// </summary>
        /// <value><c>true</c> if charging; otherwise, <c>false</c>.</value>
        public bool Charging
        {
            get
            {
                
                return DeviceStatus.PowerSource == PowerSource.External;
            }
        }

        #region partial implementations

        /// <summary>
        /// Starts the charger monitoring.
        /// </summary>
        partial void StartChargerMonitoring()
        {
            DeviceStatus.PowerSourceChanged += OnPowerSourceChanged;
        }

        /// <summary>
        /// Stops the charger monitoring.
        /// </summary>
        partial void StopChargerMonitoring()
        {
            DeviceStatus.PowerSourceChanged -= OnPowerSourceChanged;
        }

        #endregion
    }
}