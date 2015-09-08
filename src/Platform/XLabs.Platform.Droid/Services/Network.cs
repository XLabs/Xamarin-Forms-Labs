namespace XLabs.Platform.Services
{
    using System;
    using System.Threading.Tasks;
    using Android.App;
    using Android.Content;
    using Android.Net;
    using Device;
    using Java.Net;

    /// <summary>
    /// Android <see cref="INetwork" /> implementation.
    /// </summary>
    public class Network : BroadcastMonitor, INetwork
    {
        private Action<NetworkStatus> reachabilityChanged;
        private readonly object lockObject = new object();

        /// <summary>
        /// Internets the connection status.
        /// </summary>
        /// <returns>NetworkStatus.</returns>
        public NetworkStatus InternetConnectionStatus()
        {
            var status = NetworkStatus.NotReachable;

            using (var cm = (ConnectivityManager) Application.Context.GetSystemService(Context.ConnectivityService))
            using (var ni = cm.ActiveNetworkInfo)
            {
                if (ni != null && ni.IsConnectedOrConnecting)
                {
                    var name = ni.TypeName.ToUpper();
                    if (name.Contains("WIFI"))
                    {
                        status = NetworkStatus.ReachableViaWiFiNetwork;
                    }
                    else if (name.Contains("MOBILE"))
                    {
                        status = NetworkStatus.ReachableViaCarrierDataNetwork;
                    }
                    else
                    {
                        status = NetworkStatus.ReachableViaUnknownNetwork;
                    }
                }
            }

            return status;
        }

        /// <summary>
        /// Occurs when [reachability changed].
        /// </summary>
        public event Action<NetworkStatus> ReachabilityChanged
        {
            add
            {
                lock (this.lockObject)
                {
                    if (this.reachabilityChanged == null)
                    {
                        this.Start();
                    }

                    this.reachabilityChanged += value;
                }
            }

            remove
            {
                lock (this.lockObject)
                {
                    this.reachabilityChanged -= value;

                    if (this.reachabilityChanged == null)
                    {
                        this.Stop();
                    }
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
                        try
                        {
                            var address = InetAddress.GetByName(host);

                            return address != null; // && (address.IsReachable((int)timeout.TotalMilliseconds) || );
                        }
                        catch (UnknownHostException)
                        {
                            return false;
                        }
                    });
        }

        //        public bool CanPing(string host)
        //        {
        //            Process p1 = Java.Lang.Runtime.GetRuntime().Exec(string.Format("ping -c 1 {0}", host));
        //
        //
        //            int returnVal = p1.();
        //            boolean reachable = (returnVal==0);
        //        }

        /// <summary>
        /// Determines whether [is reachable by wifi] [the specified host].
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="timeout">The timeout.</param>
        public async Task<bool> IsReachableByWifi(string host, TimeSpan timeout)
        {
            return InternetConnectionStatus() == NetworkStatus.ReachableViaWiFiNetwork && await IsReachable(host, timeout);
        }

        /// <summary>
        /// This gets called by OS when the <see cref="ConnectivityManager.ConnectivityAction"/> <see cref="Intent"/> fires.
        /// </summary>
        /// <param name="context">Context for the intent.</param>
        /// <param name="intent">Intent information.</param>
        public override void OnReceive(Context context, Intent intent)
        {
            var handler = this.reachabilityChanged;
            if (handler != null)
            {
                handler(this.InternetConnectionStatus());
            }
        }

        /// <summary>
        /// <see cref="ConnectivityManager.ConnectivityAction"/> <see cref="Intent"/>
        /// </summary>
        protected override IntentFilter Filter
        {
            get { return new IntentFilter(ConnectivityManager.ConnectivityAction); }
        }
    }
}