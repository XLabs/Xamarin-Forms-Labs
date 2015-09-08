
using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
    public class WebImage : Image
    {
        public static readonly BindableProperty ImageUrlProperty = BindableProperty.Create<WebImage, string>(p => p.ImageUrl, default(string));

        /// <summary>
        /// The URL of the image to display from the web
        /// </summary>
        public string ImageUrl
        {
            get { return (string)GetValue(ImageUrlProperty); }
            set { SetValue(ImageUrlProperty, value); }
        }

        public static readonly BindableProperty DefaultImageProperty = BindableProperty.Create<WebImage, string>(p => p.DefaultImage, default(string));

        /// <summary>
        /// The path to the local image to display if the <c>ImageUrl</c> can't be loaded
        /// </summary>
        public string DefaultImage
        {
            get { return (string)GetValue(DefaultImageProperty); }
            set { SetValue(DefaultImageProperty, value); }
        }

    }
}
