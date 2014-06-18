using System;
using Xamarin.Forms;
using XForms.Toolkit.Services;

namespace XForms.Toolkit.Sample
{
    public class AcceleratorSensorPage : ContentPage
    {
        private IAccelerometer accelerometer;
        private SensorBarView xsensor, ysensor, zsensor;

        public AcceleratorSensorPage()
        {
            var device = Resolver.Resolve<IDevice> ();

            if (device.Accelerometer == null)
            {
                this.Content = new Label () 
                {
                    TextColor = Color.Red,
                    Text = "Device does not have accelerometer sensor or it is not enabled."
                };

                return;
            }

            this.accelerometer = device.Accelerometer;

            var grid = new StackLayout ();

            this.xsensor = new SensorBarView () 
            {
                HeightRequest = 75,
                WidthRequest = 250,
                MinimumHeightRequest = 10,
                MinimumWidthRequest = 50,
                BackgroundColor = this.BackgroundColor
//                VerticalOptions = LayoutOptions.Fill,
//                HorizontalOptions = LayoutOptions.Fill
            };

            this.ysensor = new SensorBarView()
            {
                HeightRequest = 75,
                WidthRequest = 250,
                MinimumHeightRequest = 10,
                MinimumWidthRequest = 50,
                BackgroundColor = this.BackgroundColor
//                VerticalOptions = LayoutOptions.Fill,
//                HorizontalOptions = LayoutOptions.Fill
            };

            this.zsensor = new SensorBarView()
            {
                HeightRequest = 75,
                WidthRequest = 250,
                MinimumHeightRequest = 10,
                MinimumWidthRequest = 50,
                BackgroundColor = this.BackgroundColor
//                VerticalOptions = LayoutOptions.Fill,
//                HorizontalOptions = LayoutOptions.Fill
            };


            grid.Children.Add (new Label () { Text = string.Format ("Accelerometer data for {0}", device.Name) });
            grid.Children.Add (new Label () { Text = "X", XAlign = TextAlignment.Center });
            grid.Children.Add (xsensor);
            grid.Children.Add (new Label () { Text = "Y", XAlign = TextAlignment.Center });
            grid.Children.Add (ysensor);
            grid.Children.Add (new Label () { Text = "Z", XAlign = TextAlignment.Center });
            grid.Children.Add (zsensor);

            this.Content = grid;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.accelerometer.ReadingAvailable += accelerometer_ReadingAvailable;
        }

        protected override void OnDisappearing()
        {
            this.accelerometer.ReadingAvailable -= accelerometer_ReadingAvailable;
            base.OnDisappearing();
        }

        void accelerometer_ReadingAvailable(object sender, EventArgs<Helpers.Vector3> e)
        {
            this.xsensor.CurrentValue = e.Value.X;
            this.ysensor.CurrentValue = e.Value.Y;
            this.zsensor.CurrentValue = e.Value.Z;
        }
    }
}

