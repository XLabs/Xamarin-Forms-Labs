using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XForms.Toolkit.Controls;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace XForms.Toolkit.Controls
{
    public partial class HybridWebViewRenderer : ViewRenderer<HybridWebView, UIWebView>
    {
        protected UIWebView WebView;

        protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
        {
            base.OnElementChanged(e);

            this.WebView = new UIWebView();

            this.WebView.ShouldStartLoad += HandleStartLoad;
            this.InjectNativeFunctionScript();
            base.SetNativeControl(this.WebView);

            this.Initialize();
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        this.WebView.Dispose();
        //    }

        //    base.Dispose(disposing);
        //}

        private bool HandleStartLoad(UIWebView webView, NSUrlRequest request,
            UIWebViewNavigationType navigationType)
        {
            return !this.CheckRequest(request.Url.RelativeString);
        }

        partial void Inject(string script)
        {
            this.WebView.EvaluateJavascript(script);
        }

        partial void Load(Uri uri)
        {
            if (uri != null)
            {
                this.WebView.LoadRequest(new NSUrlRequest(new NSUrl(uri.AbsoluteUri)));
            }
        }
    }
}