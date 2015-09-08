using Xamarin.Forms;

using XLabs.Platform.Services.Media;

namespace XLabs.Forms.Controls
{
    public class CameraView : View
    {
        public CameraView()
        {
        }

        /// <summary>
        /// The camera device to use.
        /// </summary>
        public static readonly BindableProperty CameraProperty =
            BindableProperty.Create<CameraView, CameraDevice>(
                p => p.Camera, CameraDevice.Rear);

        /// <summary>
        /// Gets or sets the camera device to use.
        /// </summary>
        public CameraDevice Camera
        {
            get { return this.GetValue<CameraDevice>(CameraProperty); }
            set { this.SetValue(CameraProperty, value); }
        }
    }
}

