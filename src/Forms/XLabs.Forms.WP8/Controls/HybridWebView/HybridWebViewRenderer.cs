using Xamarin.Forms;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]

namespace XLabs.Forms.Controls
{
    using System;
    using System.Diagnostics;
    using System.Windows.Navigation;
    using Microsoft.Phone.Controls;
    using Xamarin.Forms.Platform.WinPhone;

    /// <summary>
    /// The hybrid web view renderer.
    /// </summary>
    public partial class HybridWebViewRenderer : ViewRenderer<HybridWebView, WebBrowser>
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
                var webView = new WebBrowser { IsScriptEnabled = true, IsGeolocationEnabled = true };

                webView.Navigating += WebViewNavigating;
                webView.LoadCompleted += WebViewLoadCompleted;
                webView.ScriptNotify += WebViewOnScriptNotify;

                SetNativeControl(webView);
            }

            Unbind(e.OldElement);
            Bind();
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
        ///     Webs the view load completed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Navigation.NavigationEventArgs" /> instance containing the event data.</param>
        private void WebViewLoadCompleted(object sender, NavigationEventArgs e)
        {
            this.Inject(NativeFunction + GetFuncScript());
            //this.Inject(GetFuncScript());
            Element.OnLoadFinished(sender, EventArgs.Empty);
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
        /// Handles navigation started events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NavigatingEventArgs" /> instance containing the event data.</param>
        private void WebViewNavigating(object sender, NavigatingEventArgs e)
        {
            Element.OnNavigating(e.Uri);
        }

        /// <summary>
        ///     Injects the specified script.
        /// </summary>
        /// <param name="script">The script.</param>
        partial void Inject(string script)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() => this.Control.InvokeScript("eval", script));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
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
            Element.Uri = new Uri(contentFullName, UriKind.Relative);
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