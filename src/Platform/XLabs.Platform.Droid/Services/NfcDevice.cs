namespace XLabs.Platform.Services
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;

	using Android.App;
	using Android.Content;
	using Android.Nfc;
	using Android.Util;

	using XLabs.Ioc;
	using XLabs.Platform.Device;

	using Object = Java.Lang.Object;

	/// <summary>
	///     Class NfcDevice.
	/// </summary>
	public class NfcDevice : Object, INfcDevice, NfcAdapter.ICreateNdefMessageCallback
	{
		/// <summary>
		///     The _monitor
		/// </summary>
		private NfcMonitor _monitor;

		/// <summary>
		///     The _device
		/// </summary>
		private readonly NfcAdapter _device;

		/// <summary>
		///     The _published
		/// </summary>
		private readonly Dictionary<Guid, NdefRecord> _published = new Dictionary<Guid, NdefRecord>();
		/// <summary>
		/// Gets the context.
		/// </summary>
		/// <value>The context.</value>
		private static Context Context
		{
			get { return Application.Context; }
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="NfcDevice" /> class.
		/// </summary>
		public NfcDevice()
			: this(Manager.DefaultAdapter)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="NfcDevice" /> class.
		/// </summary>
		/// <param name="adapter">The adapter.</param>
		public NfcDevice(NfcAdapter adapter)
		{
			_device = adapter;

			if (_device != null)
			{
				//var app = Resolver.Resolve<IXFormsApp>();
				//var tapp = app as IXFormsApp<XFormsApplicationDroid>;

				//_device.SetNdefPushMessageCallback(this, Context.ApplicationInfo.);
				throw new NotImplementedException("Need to get main Application Activity here");
			}
			else
			{
				Log.Info(
					"INfcDevice",
					"NFC adapter is null. Either device does not support NFC or the application does not have NFC priviledges.");
			}
		}

		/// <summary>
		///     Gets the manager.
		/// </summary>
		/// <value>The manager.</value>
		public static NfcManager Manager
		{
			get
			{
				return (NfcManager)Application.Context.GetSystemService(Context.NfcService);
			}
		}

		/// <summary>
		///     Gets a value indicating whether [supports NFC].
		/// </summary>
		/// <value><c>true</c> if [supports NFC]; otherwise, <c>false</c>.</value>
		public static bool SupportsNfc
		{
			get
			{
				return Manager.DefaultAdapter != null;
			}
		}

		#region ICreateNdefMessageCallback Members

		/// <summary>
		///     Creates the ndef message.
		/// </summary>
		/// <param name="e">The e.</param>
		/// <returns>NdefMessage.</returns>
		public NdefMessage CreateNdefMessage(NfcEvent e)
		{
			InRange.Invoke<INfcDevice>(this, this);
			return new NdefMessage(_published.Values.ToArray());
		}

		#endregion

		/// <summary>
		///     Occurs when [in range].
		/// </summary>
		private event EventHandler<EventArgs<INfcDevice>> InRange;

		/// <summary>
		///     Occurs when [out of range].
		/// </summary>
		private event EventHandler<EventArgs<INfcDevice>> OutOfRange;

		/// <summary>
		///     Registers the NFC callback.
		/// </summary>
		private void RegisterNfcCallback()
		{
			UnregisterNfcCallback();
			_monitor = new NfcMonitor(this);
			_monitor.Start();
		}

		/// <summary>
		///     Unregisters the NFC callback.
		/// </summary>
		private void UnregisterNfcCallback()
		{
			if (_monitor != null)
			{
				_monitor.Stop();
				_monitor = null;
			}
		}

		/// <summary>
		///     Class NfcMonitor.
		/// </summary>
		private class NfcMonitor : BroadcastMonitor
		{
			/// <summary>
			///     The _device reference
			/// </summary>
			private readonly WeakReference<NfcDevice> _deviceReference;

			/// <summary>
			///     Initializes a new instance of the <see cref="NfcMonitor" /> class.
			/// </summary>
			/// <param name="device">The device.</param>
			public NfcMonitor(NfcDevice device)
			{
				_deviceReference = new WeakReference<NfcDevice>(device);
			}

			/// <summary>
			///     Gets the intent filter to use for monitoring.
			/// </summary>
			/// <value>The filter.</value>
			protected override IntentFilter Filter
			{
				get
				{
					var filter = new IntentFilter(NfcAdapter.ActionTechDiscovered);
					filter.AddAction(NfcAdapter.ActionTagDiscovered);
					filter.AddAction(NfcAdapter.ActionNdefDiscovered);

					return filter;
				}
			}

			/// <summary>
			///     This method is called when the BroadcastReceiver is receiving an Intent
			///     broadcast.
			/// </summary>
			/// <param name="context">The Context in which the receiver is running.</param>
			/// <param name="intent">The Intent being received.</param>
			/// <since version="Added in API level 1" />
			/// <remarks>
			///     <para tool="javadoc-to-mdoc">
			///         This method is called when the BroadcastReceiver is receiving an Intent
			///         broadcast.  During this time you can use the other methods on
			///         BroadcastReceiver to view/modify the current result values.  This method
			///         is always called within the main thread of its process, unless you
			///         explicitly asked for it to be scheduled on a different thread using
			///         <c>
			///             <see
			///                 cref="M:Android.Content.Context.RegisterReceiver(Android.Content.BroadcastReceiver, Android.Content.IntentFilter, Android.Content.IntentFilter, Android.Content.IntentFilter)" />
			///         </c>
			///         . When it runs on the main
			///         thread you should
			///         never perform long-running operations in it (there is a timeout of
			///         10 seconds that the system allows before considering the receiver to
			///         be blocked and a candidate to be killed). You cannot launch a popup dialog
			///         in your implementation of onReceive().
			///     </para>
			///     <para tool="javadoc-to-mdoc">
			///         <format type="text/html">
			///             <b>
			///                 If this BroadcastReceiver was launched through a &lt;receiver&gt; tag,
			///                 then the object is no longer alive after returning from this
			///                 function.
			///             </b>
			///         </format>
			///         This means you should not perform any operations that
			///         return a result to you asynchronously -- in particular, for interacting
			///         with services, you should use
			///         <c>
			///             <see cref="M:Android.Content.Context.StartService(Android.Content.Intent)" />
			///         </c>
			///         instead of
			///         <c>
			///             <see
			///                 cref="M:Android.Content.Context.BindService(Android.Content.Intent, Android.Content.IServiceConnection, Android.Content.IServiceConnection)" />
			///         </c>
			///         .  If you wish
			///         to interact with a service that is already running, you can use
			///         <c>
			///             <see
			///                 cref="M:Android.Content.BroadcastReceiver.PeekService(Android.Content.Context, Android.Content.Intent)" />
			///         </c>
			///         .
			///     </para>
			///     <para tool="javadoc-to-mdoc">
			///         The Intent filters used in
			///         <c>
			///             <see
			///                 cref="M:Android.Content.Context.RegisterReceiver(Android.Content.BroadcastReceiver, Android.Content.IntentFilter)" />
			///         </c>
			///         and in application manifests are <i>not</i> guaranteed to be exclusive. They
			///         are hints to the operating system about how to find suitable recipients. It is
			///         possible for senders to force delivery to specific recipients, bypassing filter
			///         resolution.  For this reason,
			///         <c>
			///             <see cref="M:Android.Content.BroadcastReceiver.OnReceive(Android.Content.Context, Android.Content.Intent)" />
			///         </c>
			///         implementations should respond only to known actions, ignoring any unexpected
			///         Intents that they may receive.
			///     </para>
			///     <para tool="javadoc-to-mdoc">
			///         <format type="text/html">
			///             <a
			///                 href="http://developer.android.com/reference/android/content/BroadcastReceiver.html#onReceive(android.content.Context, android.content.Intent)"
			///                 target="_blank">
			///                 [Android Documentation]
			///             </a>
			///         </format>
			///     </para>
			/// </remarks>
			public override void OnReceive(Context context, Intent intent)
			{
				Debug.WriteLine(intent.Action);
			}
		}

		#region INfcDevice Members

		/// <summary>
		///     Gets the device identifier.
		/// </summary>
		/// <value>The device identifier.</value>
		/// TODO: figure out if the NFC device has an ID or name.
		/// The below method will not identify external NFC devices.
		public string DeviceId
		{
			get
			{
				var d = Resolver.Resolve<IDevice>();

				if (_device == null || d == null)
				{
					return "Unknown";
				}

				return d.Name;
			}
		}

		/// <summary>
		///     Gets a value indicating whether this instance is enabled.
		/// </summary>
		/// <value><c>true</c> if this instance is enabled; otherwise, <c>false</c>.</value>
		public bool IsEnabled
		{
			get
			{
				return _device != null && _device.IsEnabled;
			}
		}

		/// <summary>
		///     Occurs when [device in range].
		/// </summary>
		public event EventHandler<EventArgs<INfcDevice>> DeviceInRange
		{
			add
			{
				if (InRange == null)
				{
					RegisterNfcCallback();
				}

				InRange += value;
			}
			remove
			{
				InRange -= value;

				if (InRange == null)
				{
					UnregisterNfcCallback();
				}
			}
		}

		/// <summary>
		///     Occurs when [device out of range].
		/// </summary>
		public event EventHandler<EventArgs<INfcDevice>> DeviceOutOfRange
		{
			add
			{
				if (OutOfRange == null)
				{
					//this.device.DeviceDeparted += DeviceDeparted;
				}

				OutOfRange += value;
			}

			remove
			{
				OutOfRange -= value;

				if (OutOfRange == null)
				{
					//this.device.DeviceDeparted -= DeviceDeparted;
				}
			}
		}

		/// <summary>
		///     Publishes the URI.
		/// </summary>
		/// <param name="uri">The URI.</param>
		/// <returns>Guid.</returns>
		public Guid PublishUri(Uri uri)
		{
			var key = Guid.NewGuid();

			_published.Add(key, NdefRecord.CreateUri(uri.AbsoluteUri));

			return key;
		}

		/// <summary>
		///     Unpublishes the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		public void Unpublish(Guid id)
		{
			if (_published.ContainsKey(id))
			{
				_published.Remove(id);
			}
		}

		#endregion
	}
}