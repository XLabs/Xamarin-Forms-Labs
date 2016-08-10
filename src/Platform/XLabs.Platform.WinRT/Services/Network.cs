// ***********************************************************************
// Assembly         : XLabs.Platform.WinUniversal
// Author           : XLabs Team
// Created          : 01-01-2016
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Network.cs" company="XLabs Team">
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

using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Networking.Sockets;

namespace XLabs.Platform.Services
{
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
            if (IsConnected)
            {
                // 2 for 2G, 3 for 3G, 4 for 4G
                // 100 for WiFi
                // 0 for unknown or not connected</returns>
                var connectionType = GetConnectionGeneration();

                switch (connectionType)
                {
                    case 2:
                    case 3:
                    case 4:
                        return NetworkStatus.ReachableViaCarrierDataNetwork;
                    case 100:
                        return NetworkStatus.ReachableViaWiFiNetwork;
                    case 0:
                        return NetworkStatus.ReachableViaUnknownNetwork;
                }
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
                    Windows.Networking.Connectivity.NetworkInformation.NetworkStatusChanged += NetworkInformationOnNetworkStatusChanged;
                }

                this.reachabilityChanged += value;
            }

            remove
            {
                this.reachabilityChanged -= value;

                if (this.reachabilityChanged == null)
                {
                    Windows.Networking.Connectivity.NetworkInformation.NetworkStatusChanged -= NetworkInformationOnNetworkStatusChanged;
                }
            }
        }

        private void NetworkInformationOnNetworkStatusChanged(object sender)
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

        /// <summary>
        /// Determines whether the specified host is reachable.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="timeout">The timeout.</param>
        public async Task<bool> IsReachable(string host, TimeSpan timeout)
        {
            var task = Task.Factory.StartNew<bool>(
                () =>
                    {
                        if (NetworkInformation.GetInternetConnectionProfile()?.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.None)
                        {
                            return false;
                        }

                        try
                        {
                            var endPointPairListTask = DatagramSocket.GetEndpointPairsAsync(new HostName(host), "0");
                            
                            var endPointPairList = endPointPairListTask.GetResults();

                            var endPointPair = endPointPairList.First();

                            return true;
                        }
                        catch (Exception)
                        {
                        }

                        return false;
                    });

            return await task;
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

        private bool IsConnected
        {
            get
            {
                return
                    Windows.Networking.Connectivity.NetworkInformation.GetInternetConnectionProfile()?.GetNetworkConnectivityLevel() ==
                    NetworkConnectivityLevel.InternetAccess;
            }
        }

        /// <summary>
        /// Detect the current connection type
        /// </summary>
        /// <returns>
        /// 2 for 2G, 3 for 3G, 4 for 4G
        /// 100 for WiFi
        /// 0 for unknown or not connected</returns>
        internal static byte GetConnectionGeneration()
        {
            ConnectionProfile profile = NetworkInformation.GetInternetConnectionProfile();
            if (profile.IsWwanConnectionProfile)
            {
                WwanDataClass connectionClass = profile.WwanConnectionProfileDetails.GetCurrentDataClass();
                switch (connectionClass)
                {
                    //2G-equivalent
                    case WwanDataClass.Edge:
                    case WwanDataClass.Gprs:
                        return 2;

                    //3G-equivalent
                    case WwanDataClass.Cdma1xEvdo:
                    case WwanDataClass.Cdma1xEvdoRevA:
                    case WwanDataClass.Cdma1xEvdoRevB:
                    case WwanDataClass.Cdma1xEvdv:
                    case WwanDataClass.Cdma1xRtt:
                    case WwanDataClass.Cdma3xRtt:
                    case WwanDataClass.CdmaUmb:
                    case WwanDataClass.Umts:
                    case WwanDataClass.Hsdpa:
                    case WwanDataClass.Hsupa:
                        return 3;

                    //4G-equivalent
                    case WwanDataClass.LteAdvanced:
                        return 4;

                    //not connected
                    case WwanDataClass.None:
                        return 0;

                    //unknown
                    case WwanDataClass.Custom:
                    default:
                        return 0;
                }
            }
            else if (profile.IsWlanConnectionProfile)
            {
                return 100;
            }
            return 0;
        }
    }
}