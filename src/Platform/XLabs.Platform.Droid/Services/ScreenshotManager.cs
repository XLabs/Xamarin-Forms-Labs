using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.IO;

namespace XLabs.Platform.Services
{
    public class ScreenshotManager : IScreenshotManager
    {
        public Activity Activity { get; set; }

        public async System.Threading.Tasks.Task<Screenshot> CaptureAsync()
        {
            if (Activity == null)
            {
                throw new Exception("You have to set ScreenshotManager.Activity in your Android project");
            }

            var view = Activity.Window.DecorView;
            view.DrawingCacheEnabled = true;

            Bitmap bitmap = view.GetDrawingCache(true);

            byte[] bytes;

            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 0, stream);
                bytes = stream.ToArray();
            }

            return new Screenshot()
            {
                ContentType = "image/jpeg",
                Filebytes = bytes
            };
        }
    }
}