using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Labs.Services;

namespace Xamarin.Forms.Labs.Sample.Pages.Services
{

  using Xamarin.Forms.Labs.Mvvm.Views;

  public class NfcDevicePage : BaseView
    {
        private INfcDevice device;

        private bool connected;

        private Guid? uriId;

        public NfcDevicePage()
        {
            this.Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);

            this.device = DependencyService.Get<INfcDevice>();

            var stack = new StackLayout();

            if (this.device == null || !this.device.IsEnabled)
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

            this.Content = stack;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (this.device != null && this.device.IsEnabled)
            {
                device.DeviceInRange += device_DeviceInRange;
                device.DeviceOutOfRange += device_DeviceOutOfRange;

                this.uriId = this.device.PublishUri(new Uri("xamarin.forms.labs:/hello"));
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (this.device != null && this.device.IsEnabled)
            {
                device.DeviceInRange -= device_DeviceInRange;
                device.DeviceOutOfRange -= device_DeviceOutOfRange;

                if (this.uriId.HasValue)
                {
                    this.device.Unpublish(this.uriId.Value);
                }
            }
        }

        public bool Connected
        {
            get
            {
                return this.connected;
            }

            private set
            {
                this.connected = value;
                this.OnPropertyChanged();
            }
        }

        void device_DeviceOutOfRange(object sender, EventArgs<INfcDevice> e)
        {
            this.Connected = false;
        }

        void device_DeviceInRange(object sender, EventArgs<INfcDevice> e)
        {
            this.Connected = true;
        }
    }
}
