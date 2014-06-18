using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreMotion;

namespace XForms.Toolkit
{
    public partial class Gyroscope
    {
        public AccelerometerInterval Interval { get; set; }

        private CMMotionManager motionManager;

        public static bool IsSupported
        {
            get
            {
                return new MonoTouch.CoreMotion.CMMotionManager().GyroAvailable;
            }
        }

        partial void Start()
        {
            this.motionManager = new CMMotionManager();
            this.motionManager.GyroUpdateInterval = (long)this.Interval / 1000;
            this.motionManager.StartGyroUpdates(NSOperationQueue.MainQueue, this.OnUpdate);
        }

        partial void Stop()
        {
            this.motionManager.StopGyroUpdates();
            this.motionManager = null;
        }

        private void OnUpdate(CMGyroData gyroData, NSError error)
        {
            if (error != null)
            {
                this.readingAvailable.Invoke(
                    this, 
                    new Helpers.Vector3(
                        gyroData.RotationRate.x,
                        gyroData.RotationRate.y,
                        gyroData.RotationRate.z
                        ));
            }
        }
    }
}