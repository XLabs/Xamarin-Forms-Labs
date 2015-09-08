namespace XLabs.Sample.Pages.Services
{
    using Xamarin.Forms;

    using XLabs;
    using XLabs.Forms.Controls.SensorBar;
    using XLabs.Ioc;
    using XLabs.Platform.Device;

    /// <summary>
    /// Class AcceleratorSensorPage.
    /// </summary>
    public class AcceleratorSensorPage : ContentPage
    {
        /// <summary>
        /// The accelerometer
        /// </summary>
        private readonly IAccelerometer _accelerometer;
        /// <summary>
        /// The _xsensor
        /// </summary>
        private readonly SensorBarView _xsensor;

        /// <summary>
        /// The ysensor
        /// </summary>
        private readonly SensorBarView _ysensor;

        /// <summary>
        /// The zsensor
        /// </summary>
        private readonly SensorBarView _zsensor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AcceleratorSensorPage"/> class.
        /// </summary>
        public AcceleratorSensorPage()
        {
            var device = Resolver.Resolve<IDevice> ();

            Title ="Accelerator Sensor";
          
            if (device.Accelerometer == null)
            {
                Content = new Label () 
                {
                    TextColor = Color.Red,
                    Text = "Device does not have accelerometer sensor or it is not enabled."
                };

                return;
            }

            _accelerometer = device.Accelerometer;

            var grid = new StackLayout ();

            _xsensor = new SensorBarView () 
            {
                HeightRequest = 75,
                WidthRequest = 250,
                MinimumHeightRequest = 10,
                MinimumWidthRequest = 50,
                BackgroundColor = BackgroundColor
//                VerticalOptions = LayoutOptions.Fill,
//                HorizontalOptions = LayoutOptions.Fill
            };

            _ysensor = new SensorBarView()
            {
                HeightRequest = 75,
                WidthRequest = 250,
                MinimumHeightRequest = 10,
                MinimumWidthRequest = 50,
                BackgroundColor = BackgroundColor
//                VerticalOptions = LayoutOptions.Fill,
//                HorizontalOptions = LayoutOptions.Fill
            };

            _zsensor = new SensorBarView()
            {
                HeightRequest = 75,
                WidthRequest = 250,
                MinimumHeightRequest = 10,
                MinimumWidthRequest = 50,
                BackgroundColor = BackgroundColor
//                VerticalOptions = LayoutOptions.Fill,
//                HorizontalOptions = LayoutOptions.Fill
            };


            grid.Children.Add (new Label () { Text = string.Format ("Accelerometer data for {0}", device.Name) });
            grid.Children.Add (new Label () { Text = "X", XAlign = TextAlignment.Center });
            grid.Children.Add (_xsensor);
            grid.Children.Add (new Label () { Text = "Y", XAlign = TextAlignment.Center });
            grid.Children.Add (_ysensor);
            grid.Children.Add (new Label () { Text = "Z", XAlign = TextAlignment.Center });
            grid.Children.Add (_zsensor);

            Content = grid;
        }

        /// <summary>
        /// When overridden, allows application developers to customize behavior immediately prior to the <see cref="T:Xamarin.Forms.Page" /> becoming visible.
        /// </summary>
        /// <remarks>To be added.</remarks>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (this._accelerometer == null) return;

            _accelerometer.ReadingAvailable += AccelerometerReadingAvailable;
        }

        /// <summary>
        /// When overridden, allows the application developer to customize behavior as the <see cref="T:Xamarin.Forms.Page" /> disappears.
        /// </summary>
        /// <remarks>To be added.</remarks>
        protected override void OnDisappearing()
        {
            if (this._accelerometer != null)
            {
                _accelerometer.ReadingAvailable -= AccelerometerReadingAvailable;
            }
            
            base.OnDisappearing();
        }

        /// <summary>
        /// Accelerometers the reading available.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        void AccelerometerReadingAvailable(object sender, EventArgs<Vector3> e)
        {
            _xsensor.CurrentValue = e.Value.X;
            _ysensor.CurrentValue = e.Value.Y;
            _zsensor.CurrentValue = e.Value.Z;
        }
    }
}

