using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XLabs.Forms.Pages;

[assembly: ExportRenderer(typeof(ExtendedMasterDetailPage), typeof(ExtendedMasterDetailRenderer))]

namespace XLabs.Forms.Pages
{
    public class ExtendedMasterDetailRenderer : MasterDetailRenderer
    {
    }
}
