using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XForms.Toolkit
{
    /// <summary>
    /// Defines battery interface
    /// </summary>
    public interface IBattery
    {
        /// <summary>
        /// Gets the level.
        /// </summary>
        /// <value>
        /// The level in percentage 0-100.
        /// </value>
        int Level { get; }

        /// <summary>
        /// Gets a value indicating whether battery is charging
        /// </summary>
        bool Charging { get; }

        /// <summary>
        ///  Occurs when level changes. 
        /// </summary>
        event EventHandler<EventArgs<int>> OnLevelChange;

        /// <summary>
        ///  Occurs when charger is connected or disconnected. 
        /// </summary>
        event EventHandler<EventArgs<bool>> OnChargerStatusChanged;
    }
}
