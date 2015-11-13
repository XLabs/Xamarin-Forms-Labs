using Xamarin.Forms;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]

namespace XLabs.Forms.Controls
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using Foundation;
    using Platform.Services.IO;
    using UIKit;
    using WebKit;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.iOS;

    /// <summary>
    /// The hybrid web view renderer.
    /// </summary>
    public partial class HybridWebViewRenderer : ViewRenderer<HybridWebView, WKWebView>, IWKScriptMessageHandler
    {
        private const string ScriptMessageHandlerName = "native";

        private UISwipeGestureRecognizer leftSwipeGestureRecognizer;
        private UISwipeGestureRecognizer rightSwipeGestureRecognizer;
        private WKUserContentController userController;

        /// <summary>
        /// Gets the desired size of the view.
        /// </summary>
        /// <returns>The desired size.</returns>
        /// <param name="widthConstraint">Width constraint.</param>
        /// <param name="heightConstraint">Height constraint.</param>
        /// <remarks>
        /// We need to override this method and set the request to 0. Otherwise on view refresh
        /// we will get incorrect view height and might lose the ability to scroll the webview
        /// completely.
        /// </remarks>
        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            return new SizeRequest(Size.Zero, Size.Zero);
        }

        #region Navigation delegates

        /// <summary>
        /// Handles <see cref="WKWebView"/> load finished event.
        /// </summary>
        /// <param name="webView">Web view who has finished loading.</param>
        /// <param name="navigation">Navigation object.</param>
        [Export("webView:didFinishNavigation:")]
        public void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
        {
            if (this.Element != null)
            {
                this.Element.OnLoadFinished(webView, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Handles <see cref="WKWebView"/> load start event.
        /// </summary>
        /// <param name="webView">Web view who has started loading.</param>
        /// <param name="navigation">Navigation object.</param>
        [Export("webView:didStartProvisionalNavigation:")]
        public void DidStartProvisionalNavigation(WKWebView webView, WKNavigation navigation)
        {
            if (this.Element != null)
            {
                this.Element.OnNavigating(webView.Url);
            }
        }

        #endregion

        /// <summary>
        /// Layouts the subviews.
        /// This is a hack to because the base wasn't working 
        /// when within a stacklayout
        /// </summary>
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            Control.ScrollView.Frame = Control.Bounds;
        }

        /// <summary>
        /// Implements a method from interface <see cref="IWKScriptMessageHandler"/>.
        /// </summary>
        /// <param name="userContentController">User controller sending the message.</param>
        /// <param name="message">The message being sent.</param>
        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            if (this.Element != null)
            {
                this.Element.MessageReceived(message.Body.ToString());
            }
        }

        /// <summary>
        /// The on element changed callback.
        /// </summary>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
        {
            base.OnElementChanged(e);

            if (Control == null && e.NewElement != null)
            {
                this.userController = new WKUserContentController();
                var config = new WKWebViewConfiguration
                {
                    UserContentController = this.userController
                };

                var script = new WKUserScript(new NSString(NativeFunction + GetFuncScript()), WKUserScriptInjectionTime.AtDocumentEnd, false);

                this.userController.AddUserScript(script);

                this.userController.AddScriptMessageHandler(this, ScriptMessageHandlerName);

                var webView = new WKWebView(this.Frame, config) { WeakNavigationDelegate = this };

                SetNativeControl(webView);

                //webView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
                //webView.ScalesPageToFit = true;

                this.leftSwipeGestureRecognizer = new UISwipeGestureRecognizer(() => Element.OnLeftSwipe(this, EventArgs.Empty))
                {
                    Direction = UISwipeGestureRecognizerDirection.Left
                };

                this.rightSwipeGestureRecognizer = new UISwipeGestureRecognizer(() => Element.OnRightSwipe(this, EventArgs.Empty))
                {
                    Direction = UISwipeGestureRecognizerDirection.Right
                };

                webView.AddGestureRecognizer(this.leftSwipeGestureRecognizer);
                webView.AddGestureRecognizer(this.rightSwipeGestureRecognizer);
            }

            if (e.NewElement == null)
            {
				HandleCleanup ();
            }

            this.Unbind(e.OldElement);
            this.Bind();
        }

        partial void HandleCleanup()
        {
			this.userController.RemoveAllUserScripts();
			this.userController.RemoveScriptMessageHandler(ScriptMessageHandlerName);

            if (Control == null) return;
            Control.RemoveGestureRecognizer(this.leftSwipeGestureRecognizer);
            Control.RemoveGestureRecognizer(this.rightSwipeGestureRecognizer);
        }

        partial void Inject(string script)
        {
            InvokeOnMainThread(() => Control.EvaluateJavaScript(new NSString(script), (r, e) =>
            {
                if (e != null) Debug.WriteLine(e);
            }));
        }

        partial void Load(Uri uri)
        {
            if (uri != null)
            {
                Control.LoadRequest(new NSUrlRequest(new NSUrl(uri.AbsoluteUri)));
            }
        }

        partial void LoadFromContent(object sender, HybridWebView.LoadContentEventArgs contentArgs)
        {
            var baseUri = contentArgs.BaseUri ?? GetTempDirectory();
            Element.Uri = new Uri(baseUri + "/" + contentArgs.Content);
            //Element.Uri = new Uri(NSBundle.MainBundle.BundlePath + "/" + contentFullName);
            //Control.LoadHtmlString(new NSString(contentFullName), new NSUrl(NSBundle.MainBundle.BundlePath, true));
        }

        partial void LoadContent(object sender, HybridWebView.LoadContentEventArgs contentArgs)
        {
            var baseUri = contentArgs.BaseUri ?? GetTempDirectory();
            Control.LoadHtmlString(new NSString(contentArgs.Content), new NSUrl(baseUri, true));
        }

        partial void LoadFromString(string html)
        {
            this.LoadContent(null, new HybridWebView.LoadContentEventArgs(html, null));
        }

        /// <summary>
        /// Copies bundle directory to temp directory.
        /// </summary>
        /// <param name="path">Directory to copy.</param>
        public static void CopyBundleDirectory(string path)
        {
            var source = Path.Combine(NSBundle.MainBundle.BundlePath, path);
            var dest = Path.Combine(GetTempDirectory(), path);

            FileManager.CopyDirectory(new DirectoryInfo(source), new DirectoryInfo(dest));
        }

        private static string GetTempDirectory()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Personal).Replace("Documents", "tmp");
        }
    }
}
