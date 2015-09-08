using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XLabs.Sample.Pages.Services
{
	using XLabs.Ioc;
	using XLabs.Platform.Device;

	/// <summary>
	/// Class BluetoothPage.
	/// </summary>
	public class BluetoothPage : ContentPage
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BluetoothPage"/> class.
		/// </summary>
		public BluetoothPage()
		{
			var device = Resolver.Resolve<IDevice>();
		   // var bt = device.BluetoothHub;

			var stack = new StackLayout()
				{
					
				};

			var button = new Button()
				{
					Text = "Open BT settings"
				};

			button.SetBinding(Button.CommandProperty, "OpenSettings");

			stack.Children.Add(button);

			var scanButton = new Button()
				{
					Text = "Get paired devices"
				};

			stack.Children.Add(scanButton);

			var deviceList = new ListView()
				{
					ItemTemplate = new DataTemplate(() =>
						{
							var nameLabel = new Label();
							nameLabel.SetBinding(Label.TextProperty, "Name");

							var addressLabel = new Label();
							addressLabel.SetBinding(Label.TextProperty, "Address");

							var s = new StackLayout()
								{
									Children = {nameLabel, addressLabel}
								};

							return new ViewCell()
								{
									View = s
								};
						})
				};

			//deviceList.ItemSelected += async (s, e) =>
			//    {
			//        var btDevice = e.SelectedItem as IBluetoothDevice;

			//        try
			//        {
			//            await btDevice.Connect();
			//        }
			//        catch (Exception ex)
			//        {
			//            System.Diagnostics.Debug.WriteLine(ex.Message);
			//        }
					
			//    };

			stack.Children.Add(deviceList);

			scanButton.Clicked += async (s, e) =>
				{
//                    var devices = await bt.GetPairedDevices();
//                    deviceList.ItemsSource = devices;
				};

		   // this.BindingContext = bt;

			this.Content = stack;
		}
	}
}
