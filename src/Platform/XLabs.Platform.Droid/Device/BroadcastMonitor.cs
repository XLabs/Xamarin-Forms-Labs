namespace XLabs.Platform.Device
{
	using Android.Content;

	/// <summary>
	/// Broadcast monitor.
	/// </summary>
	public abstract class BroadcastMonitor : BroadcastReceiver
	{
		/// <summary>
		///  Start monitoring. 
		/// </summary>
		public virtual bool Start()
		{
			var intent = this.RegisterReceiver(this.Filter);
			if (intent == null)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		///  Stop monitoring. 
		/// </summary>
		public virtual void Stop()
		{
			this.UnregisterReceiver();
		}

		/// <summary>
		/// Gets the intent filter to use for monitoring.
		/// </summary>
		protected abstract IntentFilter Filter { get; }
	}
}