using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace XForms.Toolkit
{
    /// <summary>
    /// Battery information class.
    /// </summary>
    public partial class Battery
    {
        private static int? level;
        private static LevelMonitor levelMonitor;
        private static ChargerMonitor chargerMonitor;

        private static bool? chargerConnected;

        partial void StartLevelMonitoring()
        {
            if (levelMonitor == null)
            {
                levelMonitor = new LevelMonitor(this);
            }
            levelMonitor.Start();
        }

        partial void StopLevelMonitoring()
        {
            if (levelMonitor == null) return;
            levelMonitor.Stop();
            levelMonitor = null;
        }

        partial void StartChargerMonitoring()
        {
            if (chargerMonitor == null)
            {
                chargerMonitor = new ChargerMonitor(this);
            }
            chargerMonitor.Start();
        }

        partial void StopChargerMonitoring()
        {
            if (chargerMonitor == null) return;
            chargerMonitor.Stop();
            chargerMonitor = null;
        }

        /// <summary>
        /// Gets the level percentage from 0-100.
        /// </summary>
        /// <value>
        /// The level.
        /// </value>
        public int Level
        {
            get { return GetLevel(); }
            private set
            {
                level = value;
                onLevelChange.Invoke(this, level.Value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="SimplyMobile.Device.Battery"/> is charging.
        /// </summary>
        /// <value><c>true</c> if charging; otherwise, <c>false</c>.</value>
        public bool Charging
        {
            get
            {
                return GetChargerState();
            }
            private set
            {
                chargerConnected = value;
                onChargerStatusChanged.Invoke(this, chargerConnected.Value);
            }
        }

        /// <summary>
        /// Gets the level.
        /// </summary>
        /// <returns>The level.</returns>
        private static int GetLevel()
        {
            if (levelMonitor != null && level.HasValue)
            {
                return level.Value;
            }

            var f = -1;
            var intent = new IntentFilter(Intent.ActionBatteryChanged).RegisterReceiver();
            if (intent != null)
            {
                f = LevelMonitor.GetMonitorLevel(intent);
            }

            return f;
        }

        /// <summary>
        /// Gets the state of the charger.
        /// </summary>
        /// <returns><c>true</c>, if charger state was gotten, <c>false</c> otherwise.</returns>
        private static bool GetChargerState()
        {
            if (chargerMonitor != null && chargerConnected.HasValue)
            {
                return chargerConnected.Value;
            }

            var o = new object();

            var intent = new IntentFilter(Intent.ActionBatteryChanged).RegisterReceiver();
            if (intent == null)
            {
                return false;
            }

            int status = intent.GetIntExtra(Android.OS.BatteryManager.ExtraStatus, -1);
            return (status == (int)Android.OS.BatteryPlugged.Ac || status == (int)Android.OS.BatteryPlugged.Usb);
        }
            
        private class LevelMonitor : BroadcastMonitor
        {
            private Battery battery;

            public LevelMonitor(Battery battery)
            {
                this.battery = battery;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                this.battery.Level = GetMonitorLevel(intent);
            }

            protected override IntentFilter Filter
            {
                get { return new IntentFilter(Intent.ActionBatteryChanged); }
            }

            public static int GetMonitorLevel(Intent intent)
            {
                var rawlevel = intent.GetIntExtra(BatteryManager.ExtraLevel, -1);
                var scale = intent.GetIntExtra(BatteryManager.ExtraScale, -1);

                if (rawlevel >= 0 && scale > 0)
                {
                    return rawlevel * 100 / scale;
                }

                return -1;
            }
        }

        private class ChargerMonitor : BroadcastMonitor
        {
            private Battery battery;

            public ChargerMonitor(Battery battery)
            {
                this.battery = battery;
            }

            protected override IntentFilter Filter
            {
                get
                {
                    var filter = new IntentFilter(Intent.ActionPowerConnected);
                    filter.AddAction(Intent.ActionPowerDisconnected);
                    return filter;
                }
            }

            public override void OnReceive(Context context, Intent intent)
            {
                this.battery.Charging = intent.Action.Equals(Intent.ActionPowerConnected);
            }
        }
    }
}