using Xamarin.Forms;

using XLabs.Forms.Pages;

[assembly: ExportRenderer(typeof(ExtendedMasterDetailPage), typeof(ExtendedMasterDetailRenderer))]

namespace XLabs.Forms.Pages
{
    using Xamarin.Forms.Platform.WinPhone;

    public class ExtendedMasterDetailRenderer : MasterDetailRenderer
    {
    }
}
