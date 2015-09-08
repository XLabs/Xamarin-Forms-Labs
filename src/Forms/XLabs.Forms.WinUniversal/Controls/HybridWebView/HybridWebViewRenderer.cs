using Xamarin.Forms.Platform.WinRT;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]

namespace XLabs.Forms.Controls
{
    using System;
    using Windows.UI.Xaml.Controls;
    using Xamarin.Forms.Platform.WinRT;

    /// <summary>
    /// The hybrid web view renderer.
    /// </summary>
    public partial class HybridWebViewRenderer : ViewRenderer<HybridWebView, WebView>
    {
        /// <summary>
        ///     Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
        {
            base.OnElementChanged(e);

            if (this.Control == null)
            {
                var webView = new WebView();

                webView.NavigationStarting += WebViewOnNavigationStarting;
                webView.NavigationCompleted += WebViewOnNavigationCompleted;
                webView.ScriptNotify += WebViewOnScriptNotify;

                SetNativeControl(webView);
            }

            Unbind(e.OldElement);
            Bind();
        }

        private void WebViewOnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            this.Inject(NativeFunction + GetFuncScript());
            Element.OnLoadFinished(sender, EventArgs.Empty);
        }

        private void WebViewOnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            Element.OnNavigating(args.Uri);
        }

        /// <summary>
        ///     Webs the view on script notify.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="notifyEventArgs">The <see cref="NotifyEventArgs" /> instance containing the event data.</param>
        private void WebViewOnScriptNotify(object sender, NotifyEventArgs notifyEventArgs)
        {
            this.Element.MessageReceived(notifyEventArgs.Value);
        }

        /// <summary>
        ///     Loads the content.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="contentFullName">Full name of the content.</param>
        partial void LoadContent(object sender, string contentFullName)
        {
            this.Control.NavigateToString(contentFullName);
            //LoadFromContent(sender, contentFullName);
        }

        /// <summary>
        ///     Injects the specified script.
        /// </summary>
        /// <param name="script">The script.</param>
        partial void Inject(string script)
        {
            this.Control.InvokeScriptAsync("eval", new[] {script});
        }

        /// <summary>
        ///     Loads the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        partial void Load(Uri uri)
        {
            if (uri != null)
            {
                this.Control.Source = uri;
            }
        }

        /// <summary>
        ///     Loads from content.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="contentFullName">Full name of the content.</param>
        partial void LoadFromContent(object sender, string contentFullName)
        {
            Element.Uri = new Uri("ms-appx-web:///" + contentFullName);
        }

        /// <summary>
        ///     Loads from string.
        /// </summary>
        /// <param name="html">The HTML.</param>
        partial void LoadFromString(string html)
        {
            Control.NavigateToString(html);
        }
    }
}