// Analysis disable CheckNamespace
namespace XLabs.Platform
{
	using Android.App;
	using Android.Content;

	/// <summary>
	/// Broadcast receiver extensions.
	/// </summary>
	public static class BroadcastReceiverExtensions
	{
		/// <summary>
		/// Registers the receiver using <see cref="Application.Context"/>.
		/// </summary>
		/// <returns>The receiver intent.</returns>
		/// <param name="receiver">Receiver.</param>
		/// <param name="intentFilter">Intent filter.</param>
		public static Intent RegisterReceiver(this BroadcastReceiver receiver, IntentFilter intentFilter)
		{
			return Application.Context.RegisterReceiver(receiver, intentFilter);
		}

		/// <summary>
		/// Unregisters the receiver using <see cref="Application.Context"/>.
		/// </summary>
		/// <param name="receiver">Receiver to unregister.</param>
		public static void UnregisterReceiver(this BroadcastReceiver receiver)
		{
			Application.Context.UnregisterReceiver(receiver);
		}
	}
}