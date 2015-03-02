using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XLabs.Platform.Services;

namespace XLabs.Sample.ViewModel
{
    public class ScreenshotManagerViewModel : XLabs.Forms.Mvvm.ViewModel
    {
        private bool _showConfirm;
        public bool ShowConfirm
        {
            get
            {
                return _showConfirm;
            }
            set
            {
                _showConfirm = value;
                NotifyPropertyChanged("ShowConfirm");
            }
        }

        public ICommand Capture
        {
            get
            {
                return new Command(async () =>
                {
                    var screenshotManager = DependencyService.Get<IScreenshotManager>();

                    var screenshot = await screenshotManager.CaptureAsync();

                    //Do somthing with the result.

                    ShowConfirm = true;
                });
            }
        }
    }
}
