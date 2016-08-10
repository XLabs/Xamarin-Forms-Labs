// ***********************************************************************
// Assembly       : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 01-06-2016
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-06-2016
// ***********************************************************************
// <copyright file="HybridWebViewRenderer.cs" company="XLabs Team">
//        Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//        This project is licensed under the Apache 2.0 license
//        https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
// 
//        XLabs is a open source project that aims to provide a powerfull and cross
//        platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System;
using System.Diagnostics;
using System.IO;
using Foundation;
using UIKit;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Forms.Controls;
using XLabs.Platform.Services.IO;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// The hybrid web view renderer.
    /// </summary>
    public partial class HybridWebViewRenderer : ViewRenderer<HybridWebView, WKWebView>, IWKScriptMessageHandler
    {
        private UISwipeGestureRecognizer _leftSwipeGestureRecognizer;
        private UISwipeGestureRecognizer _rightSwipeGestureRecognizer;
        private WKUserContentController _userController;

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
            Element.OnLoadFinished(webView, EventArgs.Empty);
        }

        /// <summary>
        /// Handles <see cref="WKWebView"/> load start event.
        /// </summary>
        /// <param name="webView">Web view who has started loading.</param>
        /// <param name="navigation">Navigation object.</param>
        [Export("webView:didStartProvisionalNavigation:")]
        public void DidStartProvisionalNavigation(WKWebView webView, WKNavigation navigation)
        {
            Element.OnNavigating(webView.Url);
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
            Element.MessageReceived(message.Body.ToString());
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
                _userController = new WKUserContentController();
                var config = new WKWebViewConfiguration()
                {
                    UserContentController = _userController
                };

                var script = new WKUserScript(new NSString(NativeFunction + GetFuncScript()), WKUserScriptInjectionTime.AtDocumentEnd, false);

                _userController.AddUserScript(script);

                _userController.AddScriptMessageHandler(this, "native");

                var webView = new WKWebView(Frame, config) { WeakNavigationDelegate = this };

                SetNativeControl(webView);

                //webView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
                //webView.ScalesPageToFit = true;

                _leftSwipeGestureRecognizer = new UISwipeGestureRecognizer(() => Element.OnLeftSwipe(this, EventArgs.Empty))
                {
                    Direction = UISwipeGestureRecognizerDirection.Left
                };

                _rightSwipeGestureRecognizer = new UISwipeGestureRecognizer(() => Element.OnRightSwipe(this, EventArgs.Empty))
                {
                    Direction = UISwipeGestureRecognizerDirection.Right
                };

                webView.AddGestureRecognizer(_leftSwipeGestureRecognizer);
                webView.AddGestureRecognizer(_rightSwipeGestureRecognizer);
            }

            if (e.NewElement == null && Control != null)
            {
                Control.RemoveGestureRecognizer(_leftSwipeGestureRecognizer);
                Control.RemoveGestureRecognizer(_rightSwipeGestureRecognizer);
            }

            Unbind(e.OldElement);
            Bind();
        }

        partial void HandleCleanup()
        {
            if (Control == null) return;
            Control.RemoveGestureRecognizer(_leftSwipeGestureRecognizer);
            Control.RemoveGestureRecognizer(_rightSwipeGestureRecognizer);
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
            LoadContent(null, new HybridWebView.LoadContentEventArgs(html, null));
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
