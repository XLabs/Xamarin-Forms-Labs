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

		private const string Format = "^(file|http|https)://(local|LOCAL)/Action(=|%3D)(?<Action>[\\w]+)/";
		private const string FuncFormat = "^(file|http|https)://(local|LOCAL)/Func(=|%3D)(?<CallbackIdx>[\\d]+)(&|%26)(?<FuncName>[\\w]+)/";
		private static readonly Regex Expression = new Regex(Format);
		private static readonly Regex FuncExpression = new Regex(FuncFormat);
#if __ANDROID__
		private void InjectNativeFunctionScript()
		{
			var builder = new StringBuilder();
			builder.Append("function Native(action, data){ ");
			builder.Append("Xamarin.call(action,  (typeof data == \"object\") ? JSON.stringify(data) : data);");
			builder.Append("}");
			this.Inject(builder.ToString());
		}
#else
		private void InjectNativeFunctionScript()
		{
			var builder = new StringBuilder();
			builder.Append("function Native(action, data){ ");
#if WINDOWS_PHONE
			builder.Append("window.external.notify(");
#else
			builder.Append("window.location = \"//LOCAL/Action=\" + ");
#endif
			builder.Append("action + \"/\"");
			builder.Append(" + ((typeof data == \"object\") ? JSON.stringify(data) : data)");
#if WINDOWS_PHONE
			builder.Append(")");
#endif
			builder.Append(" ;}");

			builder.Append("NativeFuncs = [];");
			builder.Append("function NativeFunc(action, data, callback){ ");

			builder.Append("  var callbackIdx = NativeFuncs.push(callback) - 1;");

#if WINDOWS_PHONE
			builder.Append("window.external.notify(");
#else
			builder.Append("window.location = '//LOCAL/Func=' + ");
#endif
			builder.Append("callbackIdx + '&' + ");
			builder.Append("action + '/'");
			builder.Append(" + ((typeof data == 'object') ? JSON.stringify(data) : data)");
#if WINDOWS_PHONE
			builder.Append(")");
#endif
			builder.Append(" ;}");
			builder.Append(" if (typeof(window.NativeFuncsReady) !== 'undefined') { ");
			builder.Append("   window.NativeFuncsReady(); ");
			builder.Append(" } ");

			this.Inject(builder.ToString());
		}
#endif
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == "Uri")
			{
				this.Load(this.Element.Uri);
			}
			else if (e.PropertyName == "Source")
			{
				LoadSource();
			}
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

				this.Element.JavaScriptLoadRequested += OnInjectRequest;
				this.Element.LoadFromContentRequested += LoadFromContent;
				this.Element.LoadContentRequested += LoadContent;
	            this.Element.Navigating += this.OnNavigating;
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
                oldElement.Navigating -= this.OnNavigating;
			}
		}

		private void OnInjectRequest(object sender, string script)
		{
			this.Inject(script);
		}

	    private void OnNavigating(object sender, EventArgs<Uri> e)
	    {
            //this.InjectNativeFunctionScript();
	    }

		partial void Inject(string script);

		partial void Load(Uri uri);

		partial void LoadFromContent(object sender, string contentFullName);

		partial void LoadContent(object sender, string contentFullName);

		partial void LoadFromString(string html);

		private bool CheckRequest(string request)
		{
			var m = Expression.Match(request);

			if (m.Success)
			{
				Action<string> action;
				var name = m.Groups["Action"].Value;

				if (this.Element.TryGetAction (name, out action))
				{
					var data = Uri.UnescapeDataString (request.Remove (m.Index, m.Length));
					action.Invoke (data);
				} 
				else
				{
					System.Diagnostics.Debug.WriteLine(string.Format("Unhandled callback {0} was called from JavaScript", name));
				}
			}

			var mFunc = FuncExpression.Match(request);

			if (mFunc.Success)
			{
				Func<string, object[]> func;
				var name = mFunc.Groups["FuncName"].Value;
				var callBackIdx = mFunc.Groups["CallbackIdx"].Value;

				if (this.Element.TryGetFunc (name, out func))
				{
					var data = Uri.UnescapeDataString (request.Remove (mFunc.Index, mFunc.Length));
					ThreadPool.QueueUserWorkItem(o =>
						{
							var result = func.Invoke (data);
							Element.CallJsFunction(string.Format("NativeFuncs[{0}]", callBackIdx), result);                            
						});
				}
				else
				{
					System.Diagnostics.Debug.WriteLine ("Unhandled callback {0} was called from JavaScript", name);
				}
			}

			return m.Success || mFunc.Success;
		}

		private void TryInvoke(string function, string data)
		{
			Action<string> action;

			if (this.Element.TryGetAction(function, out action))
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
