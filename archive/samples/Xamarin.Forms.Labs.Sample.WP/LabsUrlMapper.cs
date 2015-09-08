using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Xamarin.Forms.Labs.Sample.WP
{
    internal class LabsUrlMapper : UriMapperBase
    {
        public override Uri MapUri(Uri uri)
        {
            var tempUri = System.Net.HttpUtility.UrlDecode(uri.ToString());

            if (tempUri.Contains("xamarin.forms.labs"))
            {
                //string URI = string.Format("/MainPage.xaml");
                return new Uri("/MainPage.xaml", UriKind.Relative);
            }

            return uri;
        }
    }
}
