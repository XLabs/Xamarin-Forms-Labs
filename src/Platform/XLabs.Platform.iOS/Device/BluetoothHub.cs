// ***********************************************************************
// Assembly         : XLabs.Platform.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="BluetoothHub.cs" company="XLabs Team">
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreBluetooth;
using CoreFoundation;

namespace XLabs.Platform.Device
{
    /// <summary>
    /// Class BluetoothHub.
    /// </summary>
    public class BluetoothHub : CBCentralManagerDelegate, IBluetoothHub
    {
        /// <summary>
        /// The serial port UUID
        /// </summary>
        private const string SerialPortUuid = "00001101-0000-1000-8000-00805f9b34fb";

        /// <summary>
        /// The transfer service UUID
        /// </summary>
        private const string TransferServiceUuid = @"E20A39F4-73F5-4BC4-A12F-17D1AD07A961";
        /// <summary>
        /// The transfer characteristic UUID
        /// </summary>
        private const string TransferCharacteristicUuid = @"08590F7E-DB05-467E-8757-72F6FAEB13D4";


        /// <summary>
        /// The manager
        /// </summary>
        private readonly CBCentralManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="BluetoothHub"/> class.
        /// </summary>
        public BluetoothHub()
        {
            this.manager = new CBCentralManager(this, DispatchQueue.MainQueue);
        }

        #region IBluetoothHub implementation
        /// <summary>
        /// Gets a value indicating whether this <see cref="BluetoothHub"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get
            {
                return this.manager.State == CBCentralManagerState.PoweredOn;
            }
        }

        /// <summary>
        /// Gets the paired devices.
        /// </summary>
        /// <returns>Task&lt;IReadOnlyList&lt;IBluetoothDevice&gt;&gt;.</returns>
        public async Task<IReadOnlyList<IBluetoothDevice>> GetPairedDevices()
        {
            return await Task.Factory.StartNew(() =>
            {
                var devices = new List<IBluetoothDevice>();

                var action = new EventHandler<CBPeripheralsEventArgs>((s, e) =>
                    devices.AddRange(e.Peripherals.Select(a => new BluetoothDevice(a))));

                this.manager.RetrievedPeripherals += action;

                this.manager.RetrievedConnectedPeripherals += ManagerOnRetrievedConnectedPeripherals;
                this.manager.DiscoveredPeripheral += manager_DiscoveredPeripheral;
                //CBUUID id = null;

                // Bug in Xamarin? https://bugzilla.xamarin.com/show_bug.cgi?id=5808
                //this.manager.ScanForPeripherals(id, null);
                this.manager.ScanForPeripherals(CBUUID.FromString(TransferServiceUuid));

                this.manager.RetrievePeripherals(CBUUID.FromString(TransferServiceUuid));
                //this.manager.RetrieveConnectedPeripherals(new[] { CBUUID.FromString(TransferServiceUuid) });
                this.manager.RetrievedPeripherals -= action;
                this.manager.RetrievedConnectedPeripherals -= ManagerOnRetrievedConnectedPeripherals;
                this.manager.DiscoveredPeripheral -= manager_DiscoveredPeripheral;
                
                return devices;
            });
        }

        /// <summary>
        /// Handles the DiscoveredPeripheral event of the manager control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CBDiscoveredPeripheralEventArgs"/> instance containing the event data.</param>
        void manager_DiscoveredPeripheral(object sender, CBDiscoveredPeripheralEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e);
        }

        /// <summary>
        /// Managers the on retrieved connected peripherals.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="cbPeripheralsEventArgs">The <see cref="CBPeripheralsEventArgs"/> instance containing the event data.</param>
        private void ManagerOnRetrievedConnectedPeripherals(object sender, CBPeripheralsEventArgs cbPeripheralsEventArgs)
        {
            System.Diagnostics.Debug.WriteLine(cbPeripheralsEventArgs);
        }

        /// <summary>
        /// Opens the settings.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exception cref="System.NotSupportedException">iOS does not support opening Bluetooth settings.</exception>
        public Task OpenSettings()
        {
            throw new NotSupportedException("iOS does not support opening Bluetooth settings.");
        }

        #endregion

        #region implemented abstract members of CBCentralManagerDelegate

        /// <summary>
        /// Updateds the state.
        /// </summary>
        /// <param name="central">The central.</param>
        public override void UpdatedState(CBCentralManager central)
        {
            // see http://docs.xamarin.com/guides/ios/application_fundamentals/delegates,_protocols,_and_events
            // NOTE: Don't call the base implementation on a Model class
            //            throw new NotImplementedException ();
        }

        #endregion
    }
}