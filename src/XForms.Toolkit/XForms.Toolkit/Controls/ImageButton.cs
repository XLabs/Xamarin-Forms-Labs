using Xamarin.Forms;
using XForms.Toolkit.Enums;

namespace XForms.Toolkit.Controls
{
    /// <summary>
    /// Creates a button with text and an image.
    /// The image can be on the left, above, on the right or below the text.
    /// </summary>
    public class ImageButton : Button
    {
        /// <summary>
        /// The name of the image without path or file type information.
        /// Android: There should be a drable resource with the same name
        /// iOS: There should be an image in the Resources folder with a build action of BundleResource.
        /// Windows Phone: There soudl be an image in the Images folder with a type of .png and build action set to resource.
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// The orientation of the image relative to the text.
        /// </summary>
        public ImageOrientation Orientation { get; set; }
        /// <summary>
        /// The requested height of the image.  If less than or equal to zero than a 
        /// height of 50 will be used.
        /// </summary>
        public int ImageHeightRequest { get; set; }
        /// <summary>
        /// The requested width of the image.  If less than or equal to zero than a 
        /// width of 50 will be used.
        /// </summary>
        public int ImageWidthRequest { get; set; }
    }
}