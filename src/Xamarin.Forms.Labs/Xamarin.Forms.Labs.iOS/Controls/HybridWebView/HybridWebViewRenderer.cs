using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Labs.Controls;
using System.Drawing;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]

namespace Xamarin.Forms.Labs.Controls
{
    /// <summary>
    /// The hybrid web view renderer.
    /// </summary>
    public partial class HybridWebViewRenderer : ViewRenderer<HybridWebView, UIWebView>
    {
        private UIWebView webView;
        private UISwipeGestureRecognizer leftSwipeGestureRecognizer;
        private UISwipeGestureRecognizer rightSwipeGestureRecognizer;

        /// <summary>
        /// The on element changed callback.
        /// </summary>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
        {
            base.OnElementChanged(e);

            if (this.webView == null)
            {
                this.webView = new UIWebView();
                this.webView.LoadFinished += LoadFinished;
                this.webView.ShouldStartLoad += this.HandleStartLoad;
                this.InjectNativeFunctionScript();
                this.SetNativeControl(this.webView);

                this.leftSwipeGestureRecognizer = new UISwipeGestureRecognizer(() => this.Element.OnLeftSwipe(this, EventArgs.Empty))
                {
                    Direction = UISwipeGestureRecognizerDirection.Left
                };

                this.rightSwipeGestureRecognizer = new UISwipeGestureRecognizer(()=> this.Element.OnRightSwipe(this, EventArgs.Empty))
                {
                    Direction = UISwipeGestureRecognizerDirection.Right
                };

                this.Control.AddGestureRecognizer(this.leftSwipeGestureRecognizer);
                this.Control.AddGestureRecognizer(this.rightSwipeGestureRecognizer);
            }

            if (e.NewElement == null)
            {
                this.Control.RemoveGestureRecognizer(this.leftSwipeGestureRecognizer);
                this.Control.RemoveGestureRecognizer(this.rightSwipeGestureRecognizer);
            }

            Element.SizeChanged += HandleSizeChanged;

            this.Unbind(e.OldElement);
            this.Bind();
        }

        void HandleSizeChanged (object sender, EventArgs e)
        {
            LayoutViews ();
        }

        void LoadFinished(object sender, EventArgs e)
        {
            this.Element.OnLoadFinished(sender, e);
            InjectNativeFunctionScript();
        }

        private bool HandleStartLoad(UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
        {
            var shouldStartLoad = !this.CheckRequest(request.Url.RelativeString);
            if (shouldStartLoad) 
            {
                this.Element.Uri = new Uri(request.Url.AbsoluteUrl.AbsoluteString);
            }
            return shouldStartLoad;
        }

        partial void Inject(string script)
        {
            InvokeOnMainThread(() => {
                this.webView.EvaluateJavascript(script);
            });
        }

        /* 
         * This is a hack to because the base wasn't working 
         * when within a stacklayout
         */
        public override void LayoutSubviews()
        {
            LayoutViews ();
        }

        void LayoutViews()
        {
            if (this.Control != null) {
                var control = this.Control;
                var element = base.Element;
                control.Frame = new RectangleF ((float)element.X, (float)element.Y, (float)element.Width, (float)element.Height);
                Frame = new RectangleF ((float)element.X, (float)element.Y, (float)element.Width, (float)element.Height);
                Bounds = new RectangleF ((float)element.X, (float)element.Y, (float)element.Width, (float)element.Height);
            }
        }

        partial void Load(Uri uri)
        {
            if (uri != null)
            {
                this.webView.LoadRequest(new NSUrlRequest(new NSUrl(uri.AbsoluteUri)));
            }
        }

        partial void LoadFromContent(object sender, string contentFullName)
        {
            this.Element.Uri = new Uri(NSBundle.MainBundle.BundlePath + "/" + contentFullName);
            //string homePageUrl = NSBundle.MainBundle.BundlePath + "/" + contentFullName;
            //this.webView.LoadRequest(new NSUrlRequest(new NSUrl(homePageUrl, false)));
        }

        partial void LoadContent(object sender, string contentFullName)
        {
            this.webView.LoadHtmlString(contentFullName, new NSUrl(NSBundle.MainBundle.BundlePath, true));
        }
    }
}