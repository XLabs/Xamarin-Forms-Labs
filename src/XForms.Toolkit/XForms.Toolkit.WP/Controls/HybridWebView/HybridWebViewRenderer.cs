using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.Toolkit.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using Microsoft.Phone.Controls;
using XForms.Toolkit.WP.Controls;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace XForms.Toolkit.Controls
{
    public partial class HybridWebViewRenderer : ViewRenderer<HybridWebView, WebBrowser>
    {
        protected WebBrowser webView;

        protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
        {
            base.OnElementChanged(e);

            this.webView = new WebBrowser()
            {
                Source = this.Element.Uri
            };

            this.webView.IsScriptEnabled = true;
            this.webView.Navigating += webView_Navigating;
            this.webView.LoadCompleted += webView_LoadCompleted;
            this.webView.ScriptNotify += WebViewOnScriptNotify;

            this.SetNativeControl(this.webView);

            this.Initialize ();
         
        }

        private void WebViewOnScriptNotify(object sender, NotifyEventArgs notifyEventArgs)
        {
            Action<string> action;
            var values = notifyEventArgs.Value.Split('/');
            var name = values.FirstOrDefault();

            if (name != null && this.Element.TryGetAction(name, out action))
            {
                var data = Uri.UnescapeDataString(values.ElementAt(1));
                action.Invoke(data);
            }
        }

        void webView_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            this.InjectNativeFunctionScript();
        }

        void webView_Navigating(object sender, NavigatingEventArgs e)
        {

        }

        partial void Inject(string script)
        {
            this.webView.InvokeScript("eval", script);
        }

        partial void Load(Uri uri)
        {
            if (uri != null)
            {
                this.webView.Source = uri;
            }
        }
    }
}
