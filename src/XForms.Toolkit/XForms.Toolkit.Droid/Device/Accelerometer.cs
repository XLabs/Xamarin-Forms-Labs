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
    public partial class Accelerometer : Java.Lang.Object, ISensorEventListener
    {
        private SensorDelay delay;
        private SensorManager sensorManager;
        private Sensor accelerometer;

        public static bool IsSupported
        {
            get
            {
                var sensorManager = Application.Context.GetSystemService(Context.SensorService) as SensorManager;
                return sensorManager != null && sensorManager.GetDefaultSensor(SensorType.Accelerometer) != null;
            }
        }

        #region IAccelerometer Members

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

        #endregion

        partial void Start()
        {
            this.sensorManager = Application.Context.GetSystemService(Context.SensorService) as SensorManager;

            this.accelerometer = sensorManager.GetDefaultSensor(SensorType.Accelerometer);

            this.sensorManager.RegisterListener(this, accelerometer, this.delay);
        }

        partial void Stop()
        {
            this.sensorManager.UnregisterListener(this);
            this.sensorManager = null;
            this.accelerometer = null;
        }

        public void OnAccuracyChanged (Sensor sensor, SensorStatus accuracy)
        {
        //              throw new NotImplementedException ();
        }

        public void OnSensorChanged (SensorEvent e)
        {
            if (e.Sensor.Type != SensorType.Accelerometer)
            {
                return;
            }

            this.LatestReading = new Vector3(e.Values[0] / Gravitation, e.Values[1] / Gravitation, e.Values[2] / Gravitation);

            this.readingAvailable.Invoke(this, this.LatestReading);
        }
    }
}