using System;

namespace XForms.Toolkit.Services
{
    /// <summary>
    /// Interface for phone functionality and information
    /// </summary>
    public interface IPhoneService
    {
        /// <summary>
        /// Gets the cellular provider.
        /// </summary>
        string CellularProvider { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has cellular data enabled.
        /// </summary>
        bool? IsCellularDataEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has cellular data roaming enabled.
        /// </summary>
        bool? IsCellularDataRoamingEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has network available.
        bool? IsNetworkAvailable { get; }

        /// <summary>
        /// Gets the ISO Country Code
        /// </summary>
        string ICC { get; }

        /// <summary>
        /// Gets the Mobile Country Code
        /// </summary>
        string MCC { get; }

        /// <summary>
        /// Gets the Mobile Network Code
        /// </summary>
        string MNC { get; }

        /// <summary>
        /// Opens native dialog to dial the specified number
        /// </summary>
        /// <param name="number">Number to dial.</param>
        void DialNumber(string number);
    }
}

