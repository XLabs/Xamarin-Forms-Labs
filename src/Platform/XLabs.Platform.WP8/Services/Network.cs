namespace XLabs.Platform.Services
{
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Phone.Net.NetworkInformation;

    /// <summary>
    /// Class Network.
    /// </summary>
    public class Network : INetwork
    {
        /// <summary>
        /// The _network status
        /// </summary>
        private readonly NetworkStatus _networkStatus;

        /// <summary>
        /// Initializes a new instance of the <see cref="Network"/> class.
        /// </summary>
        public Network()
        {
            _networkStatus = InternetConnectionStatus();
        }

        /// <summary>
        /// Internets the connection status.
        /// </summary>
        /// <returns>NetworkStatus.</returns>
        public NetworkStatus InternetConnectionStatus()
        {
            if (DeviceNetworkInformation.IsNetworkAvailable)
            {
                if (DeviceNetworkInformation.IsWiFiEnabled
                    && NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    return NetworkStatus.ReachableViaWiFiNetwork;
                }

                if (NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.MobileBroadbandCdma
                    || NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.MobileBroadbandGsm)
                {
                    return NetworkStatus.ReachableViaCarrierDataNetwork;
                }

                return NetworkStatus.ReachableViaUnknownNetwork;
            }

            return NetworkStatus.NotReachable;
        }

        /// <summary>
        /// Occurs when [reachability changed].
        /// </summary>
        private event Action<NetworkStatus> reachabilityChanged;

        /// <summary>
        /// Occurs when [reachability changed].
        /// </summary>
        public event Action<NetworkStatus> ReachabilityChanged
        {
            add
            {
                if (this.reachabilityChanged == null)
                {
                    DeviceNetworkInformation.NetworkAvailabilityChanged += DeviceNetworkInformationNetworkAvailabilityChanged;
                }

                this.reachabilityChanged += value;
            }

            remove
            {
                this.reachabilityChanged -= value;

                if (this.reachabilityChanged == null)
                {
                    DeviceNetworkInformation.NetworkAvailabilityChanged -= DeviceNetworkInformationNetworkAvailabilityChanged;
                }
            }
        }

        /// <summary>
        /// Determines whether the specified host is reachable.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="timeout">The timeout.</param>
        public Task<bool> IsReachable(string host, TimeSpan timeout)
        {
            return Task.Run(
                () =>
                    {
                        if (!DeviceNetworkInformation.IsNetworkAvailable)
                        {
                            return false;
                        }

                        var e = new AutoResetEvent(false);

                        var isReachable = false;
                        NameResolutionCallback d = delegate(NameResolutionResult result)
                            {
                                isReachable = result.NetworkErrorCode == NetworkError.Success;
                                e.Set();
                            };

                        DeviceNetworkInformation.ResolveHostNameAsync(new DnsEndPoint(host, 0), d, this);

                        e.WaitOne(timeout);

                        return isReachable;
                    });
        }

        /// <summary>
        /// Determines whether [is reachable by wifi] [the specified host].
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="timeout">The timeout.</param>
        public async Task<bool> IsReachableByWifi(string host, TimeSpan timeout)
        {
            return (InternetConnectionStatus() == NetworkStatus.ReachableViaWiFiNetwork && await IsReachable(host, timeout));
        }

        /// <summary>
        /// Devices the network information network availability changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NetworkNotificationEventArgs"/> instance containing the event data.</param>
        private void DeviceNetworkInformationNetworkAvailabilityChanged(object sender, NetworkNotificationEventArgs e)
        {
            var status = InternetConnectionStatus();

            if (status == _networkStatus)
            {
                return;
            }

            var handler = reachabilityChanged;

            if (handler != null)
            {
                handler(status);
            }
        }
    }
}