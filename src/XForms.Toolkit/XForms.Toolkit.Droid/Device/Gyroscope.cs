using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using XForms.Toolkit.Helpers;

namespace XForms.Toolkit
{
    public partial class Gyroscope : Java.Lang.Object, ISensorEventListener
    {
        private SensorDelay delay;
        private SensorManager sensorManager;
        private Sensor gyroscope;

        public static bool IsSupported
        {
            get
            {
                var sensorManager = Application.Context.GetSystemService(Context.SensorService) as SensorManager;
                return sensorManager != null && sensorManager.GetDefaultSensor(SensorType.Gyroscope) != null;
            }
        }

        public AccelerometerInterval Interval
        {
            get
            {
                switch (this.delay)
                {
                    case SensorDelay.Fastest:
                        return AccelerometerInterval.Fastest;
                    case SensorDelay.Game:
                        return AccelerometerInterval.Game;
                    case SensorDelay.Normal:
                        return AccelerometerInterval.Normal;
                    default:
                        return AccelerometerInterval.Ui;
                }
            }
            set
            {
                switch (value)
                {
                    case AccelerometerInterval.Fastest:
                        this.delay = SensorDelay.Fastest;
                        break;
                    case AccelerometerInterval.Game:
                        this.delay = SensorDelay.Game;
                        break;
                    case AccelerometerInterval.Normal:
                        this.delay = SensorDelay.Normal;
                        break;
                    case AccelerometerInterval.Ui:
                        this.delay = SensorDelay.Ui;
                        break;
                }
            }
        }

        partial void Start()
        {
            this.sensorManager = Application.Context.GetSystemService(Context.SensorService) as SensorManager;

            this.gyroscope = sensorManager.GetDefaultSensor(SensorType.Gyroscope);

            this.sensorManager.RegisterListener(this, this.gyroscope, this.delay);
        }

        partial void Stop()
        {
            this.sensorManager.UnregisterListener(this);
            this.sensorManager = null;
            this.gyroscope = null;
        }

        #region ISensorEventListener Members

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
            throw new NotImplementedException();
        }

        public void OnSensorChanged(SensorEvent e)
        {
            if (e.Sensor.Type != SensorType.Gyroscope)
            {
                return;
            }

            this.LatestReading = new Vector3(e.Values[0], e.Values[1], e.Values[2]);

            this.readingAvailable.Invoke(this, this.LatestReading);
        }

        #endregion
    }
}