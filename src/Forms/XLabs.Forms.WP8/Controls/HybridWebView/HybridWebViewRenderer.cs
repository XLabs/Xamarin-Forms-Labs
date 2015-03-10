using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]

namespace XLabs.Forms.Controls
{
	using System;
	using System.Diagnostics;
	using System.Linq;
	using System.Windows.Input;
	using System.Windows.Navigation;

	using Microsoft.Phone.Controls;

	using Xamarin.Forms.Platform.WinPhone;

	/// <summary>
	///     The hybrid web view renderer.
	/// </summary>
	public partial class HybridWebViewRenderer : ViewRenderer<HybridWebView, WebBrowser>
	{
		/// <summary>
		///     The web view.
		/// </summary>
		protected WebBrowser WebView;

		/// <summary>
		///     Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
		{
			base.OnElementChanged(e);

			if (WebView == null)
			{
				WebView = new WebBrowser { IsScriptEnabled = true, IsGeolocationEnabled = true };

				//Touch.FrameReported += Touch_FrameReported;

				//this.WebView.ManipulationStarted += WebView_ManipulationStarted;
				//this.ManipulationCompleted += HybridWebViewRenderer_ManipulationCompleted;

				//this.WebView.ManipulationDelta += WebView_ManipulationDelta;
				//this.WebView.ManipulationCompleted += WebView_ManipulationCompleted;

				WebView.Navigating += WebViewNavigating;
				WebView.LoadCompleted += WebViewLoadCompleted;
				WebView.ScriptNotify += WebViewOnScriptNotify;

				SetNativeControl(WebView);
			}

			Unbind(e.OldElement);
			Bind();
		}

		/// <summary>
		///     Touches the frame reported.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="TouchFrameEventArgs" /> instance containing the event data.</param>
		private void TouchFrameReported(object sender, TouchFrameEventArgs e)
		{
			Debug.WriteLine(e);

			var points = e.GetTouchPoints(WebView);

			if (points.Count == 1)
			{
				var point = points[0];

				if (point.Action == TouchAction.Move)
				{
					var pos = point.Position;
				}
			}
		}

		/// <summary>
		///     Webs the view manipulation delta.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ManipulationDeltaEventArgs" /> instance containing the event data.</param>
		private void WebViewManipulationDelta(object sender, ManipulationDeltaEventArgs e)
		{
			var delta = e.CumulativeManipulation.Translation;

			//If Change in X > Change in Y, its considered a horizontal swipe
			if (Math.Abs(delta.X) > Math.Abs(delta.Y))
			{
				if (delta.X > 0)
				{
					Element.OnRightSwipe(this, EventArgs.Empty);
				}
				else
				{
					Element.OnLeftSwipe(this, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		///     Webs the view manipulation completed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ManipulationCompletedEventArgs" /> instance containing the event data.</param>
		private void WebViewManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
		{
		}

		/// <summary>
		///     Webs the view on script notify.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="notifyEventArgs">The <see cref="NotifyEventArgs" /> instance containing the event data.</param>
		private void WebViewOnScriptNotify(object sender, NotifyEventArgs notifyEventArgs)
		{
			Action<string> action;
			var values = notifyEventArgs.Value.Split('/');
			var name = values.FirstOrDefault();

			if (name != null && Element.TryGetAction(name, out action))
			{
				var data = Uri.UnescapeDataString(values.ElementAt(1));
				action.Invoke(data);
			}
		}

		/// <summary>
		///     Webs the view load completed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.Windows.Navigation.NavigationEventArgs" /> instance containing the event data.</param>
		private void WebViewLoadCompleted(object sender, NavigationEventArgs e)
		{
			InjectNativeFunctionScript();
			Element.OnLoadFinished(sender, EventArgs.Empty);
		}

		/// <summary>
		///     Loads the content.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="contentFullName">Full name of the content.</param>
		partial void LoadContent(object sender, string contentFullName)
		{
			LoadFromContent(sender, contentFullName);
		}

		/// <summary>
		///     Webs the view navigating.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="NavigatingEventArgs" /> instance containing the event data.</param>
		private void WebViewNavigating(object sender, NavigatingEventArgs e)
		{
			if (e.Uri.IsAbsoluteUri && CheckRequest(e.Uri.AbsoluteUri))
			{
				Debug.WriteLine(e.Uri);
			}
			else
			{
				Element.OnNavigating(e.Uri);
			}
		}

		/// <summary>
		///     Injects the specified script.
		/// </summary>
		/// <param name="script">The script.</param>
		partial void Inject(string script)
		{
			try
			{
				WebView.InvokeScript("eval", script);
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
				WebView.Source = uri;
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