namespace XLabs.Sample.WP
{
    using System;
    using System.Windows.Navigation;

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
