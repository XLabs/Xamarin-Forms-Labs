// ***********************************************************************
// Assembly         : XLabs.Platform.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Battery.cs" company="XLabs Team">
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

using Android.Content;
using Android.OS;

namespace XLabs.Platform.Device
{
	/// <summary>
	/// Battery information class.
	/// </summary>
	public partial class Battery
	{
		/// <summary>
		/// The level
		/// </summary>
		private static int? level;
		/// <summary>
		/// The level monitor
		/// </summary>
		private static LevelMonitor levelMonitor;
		/// <summary>
		/// The charger monitor
		/// </summary>
		private static ChargerMonitor chargerMonitor;

		/// <summary>
		/// The charger connected
		/// </summary>
		private static bool? chargerConnected;

		/// <summary>
		/// Starts the level monitoring.
		/// </summary>
		partial void StartLevelMonitoring ()
		{
			if (levelMonitor == null) {
				levelMonitor = new LevelMonitor (this);
			}
			levelMonitor.Start ();
		}

		/// <summary>
		/// Stops the level monitoring.
		/// </summary>
		partial void StopLevelMonitoring ()
		{
			if (levelMonitor == null)
				return;
			levelMonitor.Stop ();
			levelMonitor = null;
		}

		/// <summary>
		/// Starts the charger monitoring.
		/// </summary>
		partial void StartChargerMonitoring ()
		{
			if (chargerMonitor == null) {
				chargerMonitor = new ChargerMonitor (this);
			}
			chargerMonitor.Start ();
		}

		/// <summary>
		/// Stops the charger monitoring.
		/// </summary>
		partial void StopChargerMonitoring ()
		{
			if (chargerMonitor == null)
				return;
			chargerMonitor.Stop ();
			chargerMonitor = null;
		}

		/// <summary>
		/// Gets the level percentage from 0-100.
		/// </summary>
		/// <value>The level.</value>
		public int Level {
			get { return GetLevel (); }
			private set {
				level = value;
				onLevelChange.Invoke (this, level.Value);
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="Device.Battery" /> is charging.
		/// </summary>
		/// <value><c>true</c> if charging; otherwise, <c>false</c>.</value>
		public bool Charging {
			get {
				return GetChargerState ();
			}
			private set {
				chargerConnected = value;
				onChargerStatusChanged.Invoke (this, chargerConnected.Value);
			}
		}

		/// <summary>
		/// Gets the level.
		/// </summary>
		/// <returns>The level.</returns>
		private static int GetLevel ()
		{
			if (levelMonitor != null && level.HasValue) {
				return level.Value;
			}

			var f = -1;
			var intent = new IntentFilter (Intent.ActionBatteryChanged).RegisterReceiver ();
			if (intent != null) {
				f = LevelMonitor.GetMonitorLevel (intent);
			}

			return f;
		}

		/// <summary>
		/// Gets the state of the charger.
		/// </summary>
		/// <returns><c>true</c>, if charger state was gotten, <c>false</c> otherwise.</returns>
		private static bool GetChargerState ()
		{
			if (chargerMonitor != null && chargerConnected.HasValue) {
				return chargerConnected.Value;
			}

			var intent = new IntentFilter (Intent.ActionBatteryChanged).RegisterReceiver ();
			if (intent == null) {
				return false;
			}

			int status = intent.GetIntExtra (Android.OS.BatteryManager.ExtraStatus, -1);
			return (status == (int)Android.OS.BatteryPlugged.Ac || status == (int)Android.OS.BatteryPlugged.Usb);
		}

		/// <summary>
		/// Class LevelMonitor.
		/// </summary>
		private class LevelMonitor : BroadcastMonitor
		{
			/// <summary>
			/// The battery
			/// </summary>
			private Battery _battery;

			/// <summary>
			/// Initializes a new instance of the <see cref="LevelMonitor"/> class.
			/// </summary>
			/// <param name="battery">The battery.</param>
			public LevelMonitor (Battery battery)
			{
				this._battery = battery;
			}

			/// <summary>
			/// This method is called when the BroadcastReceiver is receiving an Intent
			/// broadcast.
			/// </summary>
			/// <param name="context">The Context in which the receiver is running.</param>
			/// <param name="intent">The Intent being received.</param>
			/// <since version="Added in API level 1" />
			/// <remarks><para tool="javadoc-to-mdoc">This method is called when the BroadcastReceiver is receiving an Intent
			/// broadcast.  During this time you can use the other methods on
			/// BroadcastReceiver to view/modify the current result values.  This method
			/// is always called within the main thread of its process, unless you
			/// explicitly asked for it to be scheduled on a different thread using
			/// <c><see cref="M:Android.Content.Context.RegisterReceiver(Android.Content.BroadcastReceiver, Android.Content.IntentFilter, Android.Content.IntentFilter, Android.Content.IntentFilter)" /></c>. When it runs on the main
			/// thread you should
			/// never perform long-running operations in it (there is a timeout of
			/// 10 seconds that the system allows before considering the receiver to
			/// be blocked and a candidate to be killed). You cannot launch a popup dialog
			/// in your implementation of onReceive().
			/// </para>
			/// <para tool="javadoc-to-mdoc">
			///   <format type="text/html">
			///     <b>If this BroadcastReceiver was launched through a &lt;receiver&gt; tag,
			/// then the object is no longer alive after returning from this
			/// function.</b>
			///   </format>  This means you should not perform any operations that
			/// return a result to you asynchronously -- in particular, for interacting
			/// with services, you should use
			/// <c><see cref="M:Android.Content.Context.StartService(Android.Content.Intent)" /></c> instead of
			/// <c><see cref="M:Android.Content.Context.BindService(Android.Content.Intent, Android.Content.IServiceConnection, Android.Content.IServiceConnection)" /></c>.  If you wish
			/// to interact with a service that is already running, you can use
			/// <c><see cref="M:Android.Content.BroadcastReceiver.PeekService(Android.Content.Context, Android.Content.Intent)" /></c>.
			/// </para>
			/// <para tool="javadoc-to-mdoc">The Intent filters used in <c><see cref="M:Android.Content.Context.RegisterReceiver(Android.Content.BroadcastReceiver, Android.Content.IntentFilter)" /></c>
			/// and in application manifests are <i>not</i> guaranteed to be exclusive. They
			/// are hints to the operating system about how to find suitable recipients. It is
			/// possible for senders to force delivery to specific recipients, bypassing filter
			/// resolution.  For this reason, <c><see cref="M:Android.Content.BroadcastReceiver.OnReceive(Android.Content.Context, Android.Content.Intent)" /></c>
			/// implementations should respond only to known actions, ignoring any unexpected
			/// Intents that they may receive.</para>
			/// <para tool="javadoc-to-mdoc">
			///   <format type="text/html">
			///     <a href="http://developer.android.com/reference/android/content/BroadcastReceiver.html#onReceive(android.content.Context, android.content.Intent)" target="_blank">[Android Documentation]</a>
			///   </format>
			/// </para></remarks>
			public override void OnReceive (Context context, Intent intent)
			{
				this._battery.Level = GetMonitorLevel (intent);
			}

			/// <summary>
			/// Gets the intent filter to use for monitoring.
			/// </summary>
			/// <value>The filter.</value>
			protected override IntentFilter Filter {
				get { return new IntentFilter (Intent.ActionBatteryChanged); }
			}

			/// <summary>
			/// Gets the monitor level.
			/// </summary>
			/// <param name="intent">The intent.</param>
			/// <returns>System.Int32.</returns>
			public static int GetMonitorLevel (Intent intent)
			{
				var rawlevel = intent.GetIntExtra (BatteryManager.ExtraLevel, -1);
				var scale = intent.GetIntExtra (BatteryManager.ExtraScale, -1);

				if (rawlevel >= 0 && scale > 0) {
					return rawlevel * 100 / scale;
				}

				return -1;
			}
		}

		/// <summary>
		/// Class ChargerMonitor.
		/// </summary>
		private class ChargerMonitor : BroadcastMonitor
		{
			/// <summary>
			/// The battery
			/// </summary>
			private Battery _battery;

			/// <summary>
			/// Initializes a new instance of the <see cref="ChargerMonitor"/> class.
			/// </summary>
			/// <param name="battery">The battery.</param>
			public ChargerMonitor (Battery battery)
			{
				this._battery = battery;
			}

			/// <summary>
			/// Gets the intent filter to use for monitoring.
			/// </summary>
			/// <value>The filter.</value>
			protected override IntentFilter Filter {
				get {
					var filter = new IntentFilter (Intent.ActionPowerConnected);
					filter.AddAction (Intent.ActionPowerDisconnected);
					return filter;
				}
			}

			/// <summary>
			/// This method is called when the BroadcastReceiver is receiving an Intent
			/// broadcast.
			/// </summary>
			/// <param name="context">The Context in which the receiver is running.</param>
			/// <param name="intent">The Intent being received.</param>
			/// <since version="Added in API level 1" />
			/// <remarks><para tool="javadoc-to-mdoc">This method is called when the BroadcastReceiver is receiving an Intent
			/// broadcast.  During this time you can use the other methods on
			/// BroadcastReceiver to view/modify the current result values.  This method
			/// is always called within the main thread of its process, unless you
			/// explicitly asked for it to be scheduled on a different thread using
			/// <c><see cref="M:Android.Content.Context.RegisterReceiver(Android.Content.BroadcastReceiver, Android.Content.IntentFilter, Android.Content.IntentFilter, Android.Content.IntentFilter)" /></c>. When it runs on the main
			/// thread you should
			/// never perform long-running operations in it (there is a timeout of
			/// 10 seconds that the system allows before considering the receiver to
			/// be blocked and a candidate to be killed). You cannot launch a popup dialog
			/// in your implementation of onReceive().
			/// </para>
			/// <para tool="javadoc-to-mdoc">
			///   <format type="text/html">
			///     <b>If this BroadcastReceiver was launched through a &lt;receiver&gt; tag,
			/// then the object is no longer alive after returning from this
			/// function.</b>
			///   </format>  This means you should not perform any operations that
			/// return a result to you asynchronously -- in particular, for interacting
			/// with services, you should use
			/// <c><see cref="M:Android.Content.Context.StartService(Android.Content.Intent)" /></c> instead of
			/// <c><see cref="M:Android.Content.Context.BindService(Android.Content.Intent, Android.Content.IServiceConnection, Android.Content.IServiceConnection)" /></c>.  If you wish
			/// to interact with a service that is already running, you can use
			/// <c><see cref="M:Android.Content.BroadcastReceiver.PeekService(Android.Content.Context, Android.Content.Intent)" /></c>.
			/// </para>
			/// <para tool="javadoc-to-mdoc">The Intent filters used in <c><see cref="M:Android.Content.Context.RegisterReceiver(Android.Content.BroadcastReceiver, Android.Content.IntentFilter)" /></c>
			/// and in application manifests are <i>not</i> guaranteed to be exclusive. They
			/// are hints to the operating system about how to find suitable recipients. It is
			/// possible for senders to force delivery to specific recipients, bypassing filter
			/// resolution.  For this reason, <c><see cref="M:Android.Content.BroadcastReceiver.OnReceive(Android.Content.Context, Android.Content.Intent)" /></c>
			/// implementations should respond only to known actions, ignoring any unexpected
			/// Intents that they may receive.</para>
			/// <para tool="javadoc-to-mdoc">
			///   <format type="text/html">
			///     <a href="http://developer.android.com/reference/android/content/BroadcastReceiver.html#onReceive(android.content.Context, android.content.Intent)" target="_blank">[Android Documentation]</a>
			///   </format>
			/// </para></remarks>
			public override void OnReceive (Context context, Intent intent)
			{
				this._battery.Charging = intent.Action.Equals (Intent.ActionPowerConnected);
			}
		}
	}
}