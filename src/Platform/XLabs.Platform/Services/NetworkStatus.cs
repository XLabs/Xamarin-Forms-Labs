
namespace XLabs.Platform.Services
{
    /// <summary>
    /// The network status.
    /// </summary>
    public enum NetworkStatus
    {
        /// <summary>
        /// Network not reachable.
        /// </summary>
        NotReachable,

        /// <summary>
        /// Network reachable via carrier data network.
        /// </summary>
        ReachableViaCarrierDataNetwork,

        /// <summary>
        /// Network reachable via WiFi network.
        /// </summary>
        ReachableViaWiFiNetwork,
        
        /// <summary>
        /// Network reachable via an unknown network
        /// </summary>
        ReachableViaUnknownNetwork
    }
}
