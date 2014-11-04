using System;
using System.Linq;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using Microsoft.Phone.Controls;
using System.Windows.Input;
using System.Windows;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace Xamarin.Forms.Labs.Controls
{
    /// <summary>
    /// The hybrid web view renderer.
    /// </summary>
    public partial class HybridWebViewRenderer : ViewRenderer<HybridWebView, WebBrowser>
    {
        /// <summary>
        /// The web view.
        /// </summary>
        protected WebBrowser WebView;

        protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
        {
            base.OnElementChanged(e);

            if (this.WebView == null)
            {
                this.WebView = new WebBrowser { IsScriptEnabled = true };

                //Touch.FrameReported += Touch_FrameReported;

                //this.WebView.ManipulationStarted += WebView_ManipulationStarted;
                //this.ManipulationCompleted += HybridWebViewRenderer_ManipulationCompleted;

                //this.WebView.ManipulationDelta += WebView_ManipulationDelta;
                //this.WebView.ManipulationCompleted += WebView_ManipulationCompleted;

                this.WebView.Navigating += webView_Navigating;
                this.WebView.LoadCompleted += webView_LoadCompleted;
                this.WebView.ScriptNotify += WebViewOnScriptNotify;

                this.SetNativeControl(this.WebView);
            }

            this.Unbind(e.OldElement);
            this.Bind();
        }

        void WebView_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void HybridWebViewRenderer_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void Touch_FrameReported(object sender, TouchFrameEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e);

            var points = e.GetTouchPoints(this.WebView);

            if (points.Count == 1)
            {
                var point = points[0];

                

                if (point.Action == TouchAction.Move)
                {
                    var pos = point.Position;
                }
            }
        }

        void WebView_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            var delta = e.CumulativeManipulation.Translation;

            //If Change in X > Change in Y, its considered a horizontal swipe
            if (Math.Abs(delta.X) > Math.Abs(delta.Y))
            {
                if (delta.X > 0)
                {
                    this.Element.OnRightSwipe(this, EventArgs.Empty);
                }
                else
                {
                    this.Element.OnLeftSwipe(this, EventArgs.Empty);
                }
            }
        }

        void WebView_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {

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

       private void webView_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            this.InjectNativeFunctionScript();
            this.Element.OnLoadFinished(sender,EventArgs.Empty);
       }

       partial void LoadContent(object sender, string contentFullName)
       {
           LoadFromContent(sender, contentFullName);
       }

        private void webView_Navigating(object sender, NavigatingEventArgs e)
        {
            if (e.Uri.IsAbsoluteUri && this.CheckRequest(e.Uri.AbsoluteUri))
            {
                System.Diagnostics.Debug.WriteLine(e.Uri);
            } 
            else 
            {
                this.Element.Uri = e.Uri;
            }
        }

        partial void Inject(string script)
        {
            try
            {
                this.WebView.InvokeScript("eval", script);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        partial void Load(Uri uri)
        {
            if (uri != null)
            {
                this.WebView.Source = uri;
            }
        }

        partial void LoadFromContent(object sender, string contentFullName)
        {
            this.Element.Uri = new Uri(contentFullName, UriKind.Relative);
        }
    }
}
