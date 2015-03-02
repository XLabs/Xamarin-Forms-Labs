using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace XLabs.Platform.Services
{
    public class ScreenshotManager : IScreenshotManager
    {
        public async Task<Screenshot> CaptureAsync()
        {
            var rootFrame = Application.Current.RootVisual as PhoneApplicationFrame;

            var screenImage = new WriteableBitmap((int)rootFrame.ActualWidth, (int)rootFrame.ActualHeight);
            screenImage.Render(rootFrame, new MatrixTransform());
            screenImage.Invalidate();
           
          using(var stream = new MemoryStream())
          {
              screenImage.SaveJpeg(stream, screenImage.PixelWidth, screenImage.PixelHeight, 0, 100);

              var bytes = stream.ToArray();

              return new Screenshot()
              {
                  ContentType = "image/jpeg",
                  Filebytes = bytes
              };
          }
        }
    }
}
