using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;

namespace XLabs.Forms.Controls
{
    using Xamarin.Forms;

    public partial class HybridWebViewRenderer
    {

#if WINDOWS_PHONE
        private const string NativeFuncCall = "window.external.notify";
        private const string NativeFunction = "function Native(action, data){window.external.notify(JSON.stringify({ a: action, d: data }));}";
#elif __IOS__
        private const string NativeFuncCall = "window.webkit.messageHandlers.native.postMessage";
        private const string NativeFunction = "function Native(action, data){window.webkit.messageHandlers.native.postMessage(JSON.stringify({ a: action, d: data }));}";
#elif __ANDROID__
        private const string NativeFuncCall = "Xamarin.call";
        private const string NativeFunction = "function Native(action, data){Xamarin.call(JSON.stringify({ a: action, d: data }));}";
#endif

        //private const string Format = "^(file|http|https)://(local|LOCAL)/Action(=|%3D)(?<Action>[\\w]+)/";
        private const string FuncFormat = "^(file|http|https)://(local|LOCAL)/Func(=|%3D)(?<CallbackIdx>[\\d]+)(&|%26)(?<FuncName>[\\w]+)/";
        //private static readonly Regex Expression = new Regex(Format);
        private static readonly Regex FuncExpression = new Regex(FuncFormat);

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Xamarin will changed the renderer attached to a view so it is possible that
            // an old renderer gets a property updated.  In this case the Element will be null.
            // In that case, try to clear the property event handler and exit.
            if (Element == null) 
            {
                HybridWebView wv = sender as HybridWebView;
                if (wv != null) 
                {
                    wv.PropertyChanged -= this.OnElementPropertyChanged;
                }

                return;
            }

            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == HybridWebView.UriProperty.PropertyName)
            {
                this.Load(this.Element.Uri);
            }
            else if (e.PropertyName == HybridWebView.SourceProperty.PropertyName)
            {
                LoadSource();
            } 
            else if (e.PropertyName == HybridWebView.CleanupProperty.PropertyName) 
            {
                HandleCleanup ();
            }
        }

        private string GetFuncScript()
        {
            var builder = new StringBuilder();
            builder.Append("NativeFuncs = [];");
            builder.Append("function NativeFunc(action, data, callback){");

            builder.Append("  var callbackIdx = NativeFuncs.push(callback) - 1;");
            builder.Append(NativeFuncCall);
            builder.Append("(JSON.stringify({ a: action, d: data, c: callbackIdx }));}");
            builder.Append(" if (typeof(window.NativeFuncsReady) !== 'undefined') { ");
            builder.Append("   window.NativeFuncsReady(); ");
            builder.Append(" } ");

            return builder.ToString();
        }

        private void Bind()
        {
            if (Element != null)
            {
                if (this.Element.Uri != null)
                {
                    this.Load (this.Element.Uri);
                }
                else
                {
                    LoadSource();
                }

                // There should only be one renderer and thus only one event handler registered.
                // Otherwise, when Xamarin creates a new renderer, the old one stays attached
                // and crashes when called!
                this.Element.JavaScriptLoadRequested = OnInjectRequest;
                this.Element.LoadFromContentRequested = LoadFromContent;
                this.Element.LoadContentRequested = LoadContent;
            }
        }

        private void LoadSource()
        {
            var htmlSource = this.Element.Source as HtmlWebViewSource;
            if (htmlSource != null)
            {
                this.LoadFromString(htmlSource.Html);
                return;
            }

            var webViewSource = this.Element.Source as UrlWebViewSource;

            if (webViewSource != null)
            {
                this.Load(new Uri(webViewSource.Url));
            }
        }

        private void Unbind(HybridWebView oldElement)
        {
            if (oldElement != null)
            {
                oldElement.JavaScriptLoadRequested -= OnInjectRequest;
                oldElement.LoadFromContentRequested -= LoadFromContent;
                oldElement.LoadContentRequested -= LoadContent;
                oldElement.PropertyChanged -= this.OnElementPropertyChanged;
            }
        }

        private void OnInjectRequest(object sender, string script)
        {
            this.Inject(script);
        }

        partial void Inject(string script);

        partial void Load(Uri uri);

        partial void LoadFromContent(object sender, string contentFullName);

        partial void LoadContent(object sender, string contentFullName);

        partial void LoadFromString(string html);

        partial void HandleCleanup();



        //private bool CheckRequest(string request)
        //{
        //    var m = Expression.Match(request);

        //    if (m.Success)
        //    {
        //        Action<string> action;
        //        var name = m.Groups["Action"].Value;

        //        if (this.Element.TryGetAction (name, out action))
        //        {
        //            var data = Uri.UnescapeDataString (request.Remove (m.Index, m.Length));
        //            action.Invoke (data);
        //        } 
        //        else
        //        {
        //            System.Diagnostics.Debug.WriteLine(string.Format("Unhandled callback {0} was called from JavaScript", name));
        //        }
        //    }

        //    var mFunc = FuncExpression.Match(request);

        //    if (mFunc.Success)
        //    {
        //        Func<string, object[]> func;
        //        var name = mFunc.Groups["FuncName"].Value;
        //        var callBackIdx = mFunc.Groups["CallbackIdx"].Value;

        //        if (this.Element.TryGetFunc (name, out func))
        //        {
        //            var data = Uri.UnescapeDataString (request.Remove (mFunc.Index, mFunc.Length));
        //            ThreadPool.QueueUserWorkItem(o =>
        //                {
        //                    var result = func.Invoke (data);
        //                    Element.CallJsFunction(string.Format("NativeFuncs[{0}]", callBackIdx), result);                            
        //                });
        //        }
        //        else
        //        {
        //            System.Diagnostics.Debug.WriteLine ("Unhandled callback {0} was called from JavaScript", name);
        //        }
        //    }

        //    return m.Success || mFunc.Success;
        //}

        private void TryInvoke(string function, string data)
        {
            Action<string> action;

            if (this.Element != null && this.Element.TryGetAction(function, out action))
            {
                action.Invoke(data);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Unhandled callback {0} was called from JavaScript", function);
            }
        }
    }
}
