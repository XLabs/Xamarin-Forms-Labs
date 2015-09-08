namespace XLabs.Sample.Pages.Services
{
    using Forms.Controls.SensorBar;
    using Ioc;
    using Platform.Device;
    using Xamarin.Forms;

    public class GyroscopePage : ContentPage
    {
        private readonly IGyroscope gyroscope;
        private readonly SensorBarView xsensor;
        private readonly SensorBarView ysensor;
        private readonly SensorBarView zsensor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AcceleratorSensorPage"/> class.
        /// </summary>
        public GyroscopePage()
        {
            var device = Resolver.Resolve<IDevice> ();

            Title ="Accelerator Sensor";
          
            if (device.Gyroscope == null)
            {
                Content = new Label () 
                {
                    TextColor = Color.Red,
                    Text = "Device does not have gyroscope sensor or it is not enabled."
                };

                return;
            }

            this.gyroscope = device.Gyroscope;

            var grid = new StackLayout ();

            this.xsensor = new SensorBarView () 
            {
                HeightRequest = 75,
                WidthRequest = 250,
                MinimumHeightRequest = 10,
                MinimumWidthRequest = 50,
                BackgroundColor = BackgroundColor
//                VerticalOptions = LayoutOptions.Fill,
//                HorizontalOptions = LayoutOptions.Fill
            };

            this.ysensor = new SensorBarView()
            {
                HeightRequest = 75,
                WidthRequest = 250,
                MinimumHeightRequest = 10,
                MinimumWidthRequest = 50,
                BackgroundColor = BackgroundColor
//                VerticalOptions = LayoutOptions.Fill,
//                HorizontalOptions = LayoutOptions.Fill
            };

            this.zsensor = new SensorBarView()
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
            grid.Children.Add (this.xsensor);
            grid.Children.Add (new Label () { Text = "Y", XAlign = TextAlignment.Center });
            grid.Children.Add (this.ysensor);
            grid.Children.Add (new Label () { Text = "Z", XAlign = TextAlignment.Center });
            grid.Children.Add (this.zsensor);

            Content = grid;
        }

        /// <summary>
        /// When overridden, allows application developers to customize behavior immediately prior to the <see cref="T:Xamarin.Forms.Page" /> becoming visible.
        /// </summary>
        /// <remarks>To be added.</remarks>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (this.gyroscope == null) return;

            this.gyroscope.ReadingAvailable += GyroscopeReadingAvailable;
        }

        /// <summary>
        /// When overridden, allows the application developer to customize behavior as the <see cref="T:Xamarin.Forms.Page" /> disappears.
        /// </summary>
        /// <remarks>To be added.</remarks>
        protected override void OnDisappearing()
        {
            if (this.gyroscope != null)
            {
                this.gyroscope.ReadingAvailable -= GyroscopeReadingAvailable;
            }
            
            base.OnDisappearing();
        }

        /// <summary>
        /// Accelerometers the reading available.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        void GyroscopeReadingAvailable(object sender, EventArgs<Vector3> e)
        {
            this.xsensor.CurrentValue = e.Value.X;
            this.ysensor.CurrentValue = e.Value.Y;
            this.zsensor.CurrentValue = e.Value.Z;
        }
    }
}
