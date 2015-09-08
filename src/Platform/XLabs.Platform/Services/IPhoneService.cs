namespace XLabs.Platform.Services
{
    /// <summary>
    /// Interface for phone functionality and information.
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
        /// </summary>
        bool? IsNetworkAvailable { get; }

        /// <summary>
        /// Gets the ISO Country Code.
        /// </summary>
        string ICC { get; }

        /// <summary>
        /// Gets the Mobile Country Code.
        /// </summary>
        string MCC { get; }

        /// <summary>
        /// Gets the Mobile Network Code.
        /// </summary>
        string MNC { get; }

        /// <summary>
        /// Gets whether the service can send SMS
        /// </summary>
        bool CanSendSMS { get; }

        /// <summary>
        /// Opens native dialog to dial the specified number.
        /// </summary>
        /// <param name="number">Number to dial.</param>
        void DialNumber(string number);

        void SendSMS(string to, string body);
    }
}

