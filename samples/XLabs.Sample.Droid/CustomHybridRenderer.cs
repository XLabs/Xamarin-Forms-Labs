using System;
using XLabs.Forms.Controls;
using Xamarin.Forms;
using XLabs.Sample.Droid;

namespace XLabs.Sample.Droid
{
    public class CustomClient : HybridWebViewRenderer.Client
    {
        public CustomClient(HybridWebViewRenderer webHybrid) : base(webHybrid) {}

        public override void OnReceivedSslError(Android.Webkit.WebView view, Android.Webkit.SslErrorHandler handler, Android.Net.Http.SslError error)
        {
            handler.Proceed();
        }

        public override void OnPageFinished(Android.Webkit.WebView view, string url)
        {
            System.Diagnostics.Debug.WriteLine("Webview OnPageFinished: " + url);
            base.OnPageFinished(view, url);
        }

        public override void OnReceivedError(Android.Webkit.WebView view, Android.Webkit.ClientError errorCode, string description, string failingUrl)
        {
            System.Diagnostics.Debug.WriteLine("Webview OnReceivedError: " + description);
            base.OnReceivedError(view, errorCode, description, failingUrl);
        }
    }

    public class CustomChromeClient : HybridWebViewRenderer.ChromeClient
    {
        public override bool OnConsoleMessage(Android.Webkit.ConsoleMessage consoleMessage)
        {
            System.Diagnostics.Debug.WriteLine(consoleMessage.Message());
            return base.OnConsoleMessage(consoleMessage);
        }

        public override void OnProgressChanged(Android.Webkit.WebView view, int newProgress)
        {
            System.Diagnostics.Debug.WriteLine("Webview progress: " + newProgress);
            base.OnProgressChanged(view, newProgress);
        }
    }
}

