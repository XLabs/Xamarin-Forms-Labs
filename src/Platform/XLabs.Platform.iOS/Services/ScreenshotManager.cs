using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace XLabs.Platform.Services
{
    public class ScreenshotManager : IScreenshotManager
    {
        public async System.Threading.Tasks.Task<Screenshot> CaptureAsync()
        {
            var view = UIApplication.SharedApplication.KeyWindow.RootViewController.View;

            UIGraphics.BeginImageContext(view.Frame.Size);
            view.DrawViewHierarchy(view.Frame, true);
            var image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            using (var imageData = image.AsJPEG())
            {
                var bytes = new byte[imageData.Length];
                System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, bytes, 0, Convert.ToInt32(imageData.Length));

                return new Screenshot()
                {
                    Filebytes = bytes,
                    ContentType = "image/jpeg"
                };
            }
        }
    }
}