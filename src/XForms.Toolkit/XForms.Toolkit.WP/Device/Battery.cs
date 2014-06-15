using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Info;

namespace XForms.Toolkit
{
    /// <summary>
    /// Windows Phone Battery class.
    /// </summary>
    public partial class Battery
    {
        /// <summary>
        /// Gets the level.
        /// </summary>
        /// <value>
        /// The level in percentage 0-100.
        /// </value>
        public int Level
        {
            get { return Windows.Phone.Devices.Power.Battery.GetDefault().RemainingChargePercent; }
        }

        /// <summary>
        /// Gets a value indicating whether battery is charging
        /// </summary>
        public bool Charging
        {
            get
            {
                return DeviceStatus.PowerSource == PowerSource.External;
            }
        }

        #region partial implementations
        partial void StartLevelMonitoring()
        {
            Windows.Phone.Devices.Power.Battery.GetDefault().RemainingChargePercentChanged += OnRemainingChargePercentChanged;
        }

        partial void StopLevelMonitoring()
        {
            Windows.Phone.Devices.Power.Battery.GetDefault().RemainingChargePercentChanged -= OnRemainingChargePercentChanged;
        }

        partial void StartChargerMonitoring()
        {
            DeviceStatus.PowerSourceChanged += OnPowerSourceChanged;
        }

        partial void StopChargerMonitoring()
        {
            DeviceStatus.PowerSourceChanged -= OnPowerSourceChanged;
        }
        #endregion

        private void OnRemainingChargePercentChanged(object sender, object o)
        {
            onLevelChange.Invoke(sender, this.Level);
        }

        private void OnPowerSourceChanged(object sender, EventArgs eventArgs)
        {
            onChargerStatusChanged.Invoke(sender, this.Charging);
        }
    }
}
