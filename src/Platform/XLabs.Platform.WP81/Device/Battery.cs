namespace XLabs.Platform.Device
{
    using System;


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
                throw new NotImplementedException("Battery.Charging currently unavailable.");
            }
        }

        #region partial implementations

        /// <summary>
        /// Starts the charger monitoring.
        /// </summary>
        partial void StartChargerMonitoring()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Stops the charger monitoring.
        /// </summary>
        partial void StopChargerMonitoring()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}