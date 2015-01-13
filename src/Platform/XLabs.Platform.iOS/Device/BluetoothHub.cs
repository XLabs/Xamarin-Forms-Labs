namespace XLabs.Platform.Device
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using CoreBluetooth;
	using CoreFoundation;

	public class BluetoothHub : CBCentralManagerDelegate, IBluetoothHub
	{
		private const string SerialPortUuid = "00001101-0000-1000-8000-00805f9b34fb";

		private const string TransferServiceUuid = @"E20A39F4-73F5-4BC4-A12F-17D1AD07A961";
		private const string TransferCharacteristicUuid = @"08590F7E-DB05-467E-8757-72F6FAEB13D4";


		private readonly CBCentralManager manager;

		public BluetoothHub()
		{
			this.manager = new CBCentralManager(this, DispatchQueue.MainQueue);
		}

		#region IBluetoothHub implementation
		public bool Enabled
		{
			get
			{
				return this.manager.State == CBCentralManagerState.PoweredOn;
			}
		}

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
				CBUUID id = null;

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

		void manager_DiscoveredPeripheral(object sender, CBDiscoveredPeripheralEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine(e);
		}

		private void ManagerOnRetrievedConnectedPeripherals(object sender, CBPeripheralsEventArgs cbPeripheralsEventArgs)
		{
			System.Diagnostics.Debug.WriteLine(cbPeripheralsEventArgs);
		}

		public Task OpenSettings()
		{
            throw new NotSupportedException("iOS does not support opening Bluetooth settings.");
		}

		#endregion

		#region implemented abstract members of CBCentralManagerDelegate

		public override void UpdatedState(CBCentralManager central)
		{
			// see http://docs.xamarin.com/guides/ios/application_fundamentals/delegates,_protocols,_and_events
			// NOTE: Don't call the base implementation on a Model class
			//            throw new NotImplementedException ();
		}

		#endregion
	}
}