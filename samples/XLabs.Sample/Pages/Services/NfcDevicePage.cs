namespace XLabs.Sample.Pages.Services
{
	using System;

	using Xamarin.Forms;

	using XLabs;
	using XLabs.Platform.Services;

	/// <summary>
	/// Class NfcDevicePage.
	/// </summary>
	public class NfcDevicePage : ContentPage
	{
		/// <summary>
		/// The device
		/// </summary>
		private readonly INfcDevice _device;

		/// <summary>
		/// The connected
		/// </summary>
		private bool _connected;

		/// <summary>
		/// The uri identifier
		/// </summary>
		private Guid? _uriId;

		/// <summary>
		/// Initializes a new instance of the <see cref="NfcDevicePage"/> class.
		/// </summary>
		public NfcDevicePage()
		{
			Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);

			_device = DependencyService.Get<INfcDevice>();

			var stack = new StackLayout();

			if (_device == null || !_device.IsEnabled)
			{
				stack.Children.Add(new Label()
				{
					TextColor = Color.Red,
					Text = "No NFC support",

				});
			}
			else
			{
				stack.Children.Add(new Label()
				{
					Text = "Connection state",
				});

				var s = new Switch()
				{
					IsEnabled = false,
					HorizontalOptions = LayoutOptions.FillAndExpand
				};

				s.SetBinding(Switch.IsToggledProperty, "Connected");
				s.BindingContext = this;

				stack.Children.Add(s);
			}

			Content = stack;
		}

		/// <summary>
		/// When overridden, allows application developers to customize behavior immediately prior to the <see cref="T:Xamarin.Forms.Page" /> becoming visible.
		/// </summary>
		/// <remarks>To be added.</remarks>
		protected override void OnAppearing()
		{
			base.OnAppearing();

			if (_device != null && _device.IsEnabled)
			{
				_device.DeviceInRange += DeviceDeviceInRange;
				_device.DeviceOutOfRange += DeviceDeviceOutOfRange;

				_uriId = _device.PublishUri(new Uri("xamarin.forms.labs:/hello"));
			}
		}

		/// <summary>
		/// When overridden, allows the application developer to customize behavior as the <see cref="T:Xamarin.Forms.Page" /> disappears.
		/// </summary>
		/// <remarks>To be added.</remarks>
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			if (_device != null && _device.IsEnabled)
			{
				_device.DeviceInRange -= DeviceDeviceInRange;
				_device.DeviceOutOfRange -= DeviceDeviceOutOfRange;

				if (_uriId.HasValue)
				{
					_device.Unpublish(_uriId.Value);
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="NfcDevicePage"/> is connected.
		/// </summary>
		/// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
		public bool Connected
		{
			get
			{
				return _connected;
			}

			private set
			{
				_connected = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Devices the device out of range.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The e.</param>
		void DeviceDeviceOutOfRange(object sender, EventArgs<INfcDevice> e)
		{
			Connected = false;
		}

		/// <summary>
		/// Devices the device in range.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The e.</param>
		void DeviceDeviceInRange(object sender, EventArgs<INfcDevice> e)
		{
			Connected = true;
		}
	}
}
