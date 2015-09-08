namespace XLabs.Platform.Services
{
	using System;
	using System.Threading.Tasks;

	/// <summary>
	/// Class Network.
	/// </summary>
	public class Network : INetwork
	{
		/// <summary>
		/// Internets the connection status.
		/// </summary>
		/// <returns>NetworkStatus.</returns>
		public NetworkStatus InternetConnectionStatus()
		{
			return Reachability.InternetConnectionStatus();
		}

		/// <summary>
		/// Occurs when [reachability changed].
		/// </summary>
		private event Action<NetworkStatus> reachabilityChanged;

		/// <summary>
		/// Handles the reachability changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void HandleReachabilityChanged(object sender, EventArgs e)
		{
			var reachabilityChanged = this.reachabilityChanged;
			if (reachabilityChanged != null)
			{
				reachabilityChanged(InternetConnectionStatus());
			}
		}

		/// <summary>
		/// Occurs when [reachability changed].
		/// </summary>
		public event Action<NetworkStatus> ReachabilityChanged
		{
			add
			{
				if (reachabilityChanged == null)
				{
					// TODO: check if this actually works
					Reachability.ReachabilityChanged += HandleReachabilityChanged;
				}

				reachabilityChanged += value;
			}

			remove
			{
				reachabilityChanged -= value;

				if (reachabilityChanged == null)
				{
					Reachability.ReachabilityChanged -= HandleReachabilityChanged;
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
			return Task.Run(() => Reachability.IsHostReachable(host));
		}

		/// <summary>
		/// Determines whether [is reachable by wifi] [the specified host].
		/// </summary>
		/// <param name="host">The host.</param>
		/// <param name="timeout">The timeout.</param>
		public Task<bool> IsReachableByWifi(string host, TimeSpan timeout)
		{
			return Task.Run(() => Reachability.RemoteHostStatus(host) == NetworkStatus.ReachableViaWiFiNetwork);
		}
	}
}