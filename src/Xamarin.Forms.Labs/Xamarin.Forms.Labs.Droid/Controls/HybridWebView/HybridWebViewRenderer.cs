using System;
using Xamarin.Forms.Platform.Android;
using Android.Webkit;
using Xamarin.Forms.Labs.Controls;
using Android.Views;

[assembly: Xamarin.Forms.ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]

namespace Xamarin.Forms.Labs.Controls
{
    public partial class HybridWebViewRenderer : ViewRenderer<HybridWebView, HybridWebViewRenderer.NativeWebView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
        {
            base.OnElementChanged (e);

            if (this.Control == null)
            {
                var webView = new NativeWebView(this);

                webView.Settings.JavaScriptEnabled = true;

                webView.SetWebViewClient(new Client(this));
                webView.SetWebChromeClient(new ChromeClient(this));

                webView.AddJavascriptInterface(new Xamarin(this), "Xamarin");

                this.SetNativeControl(webView);
            }

            this.Unbind(e.OldElement);

            this.Bind();
        }
            
        partial void Inject(string script)
        {
            this.Control.LoadUrl(string.Format("javascript: {0}", script));
        }

        partial void Load(Uri uri)
        {
            if (uri != null)
            {
                this.Control.LoadUrl(uri.AbsoluteUri);
                this.InjectNativeFunctionScript();
            }
        }

        partial void LoadFromContent(object sender, string contentFullName)
        {
            this.Element.Uri = new Uri("file:///android_asset/" + contentFullName);
        }

        partial void LoadContent(object sender, string contentFullName)
        {
            this.Control.LoadDataWithBaseURL("file:///android_asset/", contentFullName, "text/html", "UTF-8", null);
            // we can't really set the URI and fire up native function injection so the workaround is to do it here
            this.InjectNativeFunctionScript();
        }

        private class Client : WebViewClient
        {
            private readonly WeakReference<HybridWebViewRenderer> webHybrid;

            public Client(HybridWebViewRenderer webHybrid)
            {
                this.webHybrid = new WeakReference<HybridWebViewRenderer>(webHybrid);
            }

            public override void OnPageFinished(Android.Webkit.WebView view, string url)
            {
                base.OnPageFinished(view, url);

                HybridWebViewRenderer hybrid;
                if (!this.webHybrid.TryGetTarget(out hybrid))
                {
                    hybrid.Element.OnLoadFinished(this, EventArgs.Empty);
                }
            }

            public override bool ShouldOverrideUrlLoading(Android.Webkit.WebView view, string url)
            {
                HybridWebViewRenderer hybrid;

                if (!this.webHybrid.TryGetTarget (out hybrid))
                {
                    return base.ShouldOverrideUrlLoading(view, url);
                }
                if (!hybrid.CheckRequest(url))
                {
                    hybrid.Element.Uri = new Uri(url);
                    return base.ShouldOverrideUrlLoading(view, url);
                }

                return true;
            }
        }

        /// <summary>
        /// Java callback class for JavaScript.
        /// </summary>
        public class Xamarin : Java.Lang.Object
        {
            private readonly WeakReference<HybridWebViewRenderer> webHybrid;

            public Xamarin(HybridWebViewRenderer webHybrid)
            {
                this.webHybrid = new WeakReference<HybridWebViewRenderer>(webHybrid);
            }

            [JavascriptInterface]
            [Java.Interop.Export("call")]
            public void Call(string function, string data)
            {
                HybridWebViewRenderer hybrid;

                if (this.webHybrid.TryGetTarget(out hybrid))
                {
                    hybrid.TryInvoke(function, data);
                }
            }
        }

        private class ChromeClient : WebChromeClient 
        {
            private readonly HybridWebViewRenderer webHybrid;

            internal ChromeClient(HybridWebViewRenderer webHybrid)
            {
                this.webHybrid = webHybrid;
            }

//            public override void OnProgressChanged(Android.Webkit.WebView view, int newProgress)
//            {
//                base.OnProgressChanged(view, newProgress);
//
//                if (newProgress >= 100)
//                {
//                    this.webHybrid.Element.OnLoadFinished(this, EventArgs.Empty);
//                }
//            }

            public override bool OnJsAlert(Android.Webkit.WebView view, string url, string message, JsResult result)
            {
                // the built-in alert is pretty ugly, you could do something different here if you wanted to
                return base.OnJsAlert(view, url, message, result);
            }
        }

        public class NativeWebView : Android.Webkit.WebView
        {
            private readonly MyGestureListener listener;
            private readonly GestureDetector detector;

            public NativeWebView(HybridWebViewRenderer renderer) : base(renderer.Context)
            {
                this.listener = new MyGestureListener(renderer);
                this.detector = new GestureDetector(this.Context, this.listener);
            }

            public override bool OnTouchEvent(MotionEvent e)
            {
                this.detector.OnTouchEvent(e);
                return base.OnTouchEvent(e);
            }

            private class MyGestureListener : GestureDetector.SimpleOnGestureListener
            {
                private const int SWIPE_MIN_DISTANCE = 120;
                private const int SWIPE_MAX_OFF_PATH = 200;
                private const int SWIPE_THRESHOLD_VELOCITY = 200;

                private readonly WeakReference<HybridWebViewRenderer> webHybrid;

                public MyGestureListener(HybridWebViewRenderer renderer)
                {
                    this.webHybrid = new WeakReference<HybridWebViewRenderer>(renderer);
                }

//                public override void OnLongPress(MotionEvent e)
//                {
//                    Console.WriteLine("OnLongPress");
//                    base.OnLongPress(e);
//                }
//
//                public override bool OnDoubleTap(MotionEvent e)
//                {
//                    Console.WriteLine("OnDoubleTap");
//                    return base.OnDoubleTap(e);
//                }
//
//                public override bool OnDoubleTapEvent(MotionEvent e)
//                {
//                    Console.WriteLine("OnDoubleTapEvent");
//                    return base.OnDoubleTapEvent(e);
//                }
//
//                public override bool OnSingleTapUp(MotionEvent e)
//                {
//                    Console.WriteLine("OnSingleTapUp");
//                    return base.OnSingleTapUp(e);
//                }
//
//                public override bool OnDown(MotionEvent e)
//                {
//                    Console.WriteLine("OnDown");
//                    return base.OnDown(e);
//                }

                public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
                {
                    HybridWebViewRenderer hybrid;

                    if (this.webHybrid.TryGetTarget(out hybrid) && Math.Abs(velocityX) > SWIPE_THRESHOLD_VELOCITY)
                    {
                        if(e1.GetX() - e2.GetX() > SWIPE_MIN_DISTANCE) 
                        {
                            hybrid.Element.OnLeftSwipe(this, EventArgs.Empty);
                        }  
                        else if (e2.GetX() - e1.GetX() > SWIPE_MIN_DISTANCE) 
                        {
                            hybrid.Element.OnRightSwipe(this, EventArgs.Empty);
                        }
                    }

                    return base.OnFling(e1, e2, velocityX, velocityY);
                }

//                public override bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
//                {
//                    Console.WriteLine("OnScroll");
//                    return base.OnScroll(e1, e2, distanceX, distanceY);
//                }
//
//                public override void OnShowPress(MotionEvent e)
//                {
//                    Console.WriteLine("OnShowPress");
//                    base.OnShowPress(e);
//                }
//
//                public override bool OnSingleTapConfirmed(MotionEvent e)
//                {
//                    Console.WriteLine("OnSingleTapConfirmed");
//                    return base.OnSingleTapConfirmed(e);
//                }

            }
        }
    }




}

