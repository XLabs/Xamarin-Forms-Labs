namespace XLabs.Forms.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Ioc;
    using Serialization;
    using Xamarin.Forms;

    /// <summary>
    /// The hybrid web view.
    /// </summary>
    public class HybridWebView : View
    {
        /// <summary>
        /// The uri property.
        /// </summary>
        public static readonly BindableProperty UriProperty = BindableProperty.Create<HybridWebView, Uri>(p => p.Uri,
            default(Uri));

        /// <summary>
        /// The source property.
        /// </summary>
        public static readonly BindableProperty SourceProperty =
            BindableProperty.Create<HybridWebView, WebViewSource>(p => p.Source, default(WebViewSource));

        /// <summary>
        /// The java script load requested
        /// </summary>
        internal EventHandler<string> JavaScriptLoadRequested;
        /// <summary>
        /// The left swipe
        /// </summary>
        public EventHandler LeftSwipe;
        /// <summary>
        /// The load content requested
        /// </summary>
        internal EventHandler<string> LoadContentRequested;
        /// <summary>
        /// The load finished
        /// </summary>
        public EventHandler LoadFinished;
        /// <summary>
        /// The load from content requested
        /// </summary>
        internal EventHandler<string> LoadFromContentRequested;
        /// <summary>
        /// The navigating
        /// </summary>
        public EventHandler<EventArgs<Uri>> Navigating;
        /// <summary>
        /// The right swipe
        /// </summary>
        public EventHandler RightSwipe;

        /// <summary>
        /// The inject lock.
        /// </summary>
        private readonly object injectLock = new object();

        /// <summary>
        /// The JSON serializer.
        /// </summary>
        private readonly IStringSerializer jsonSerializer;

        /// <summary>
        /// The registered actions.
        /// </summary>
        private readonly Dictionary<string, Action<string>> registeredActions;

        /// <summary>
        /// The registered actions.
        /// </summary>
        private readonly Dictionary<string, Func<string, object[]>> registeredFunctions;

        /// <summary>
        /// Initializes a new instance of the <see cref="HybridWebView" /> class.
        /// </summary>
        /// <exception cref="Exception">Exception when there is no <see cref="IJsonSerializer"/> implementation registered.</exception>
        /// <remarks>HybridWebView will use <see cref="IJsonSerializer" /> configured
        /// with <see cref="Resolver"/> or <see cref="DependencyService"/>. System JSON serializer was removed due to Xamarin
        /// requirement of having a business license or higher.</remarks>
        public HybridWebView()
        {
            if (!Resolver.IsSet || (this.jsonSerializer = Resolver.Resolve<IJsonSerializer>() ?? DependencyService.Get<IJsonSerializer>()) == null)
            {
#if BUSINESS_LICENSE
                _jsonSerializer = new SystemJsonSerializer();
#else
                throw new Exception("HybridWebView requires IJsonSerializer implementation to be registered.");
#endif
            }

            this.registeredActions = new Dictionary<string, Action<string>>();
            this.registeredFunctions = new Dictionary<string, Func<string, object[]>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HybridWebView" /> class.
        /// </summary>
        /// <param name="jsonSerializer">The JSON serializer.</param>
        public HybridWebView(IJsonSerializer jsonSerializer)
        {
            this.jsonSerializer = jsonSerializer;
            this.registeredActions = new Dictionary<string, Action<string>>();
            this.registeredFunctions = new Dictionary<string, Func<string, object[]>>();
        }

        /// <summary>
        /// Gets or sets the uri.
        /// </summary>
        /// <value>The URI.</value>
        public Uri Uri
        {
            get { return (Uri)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public WebViewSource Source
        {
            get { return (WebViewSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// Registers a native callback.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="action">The action.</param>
        public void RegisterCallback(string name, Action<string> action)
        {
            this.registeredActions.Add(name, action);
        }

        /// <summary>
        /// Removes a native callback.
        /// </summary>
        /// <param name="name">The name of the callback.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool RemoveCallback(string name)
        {
            return this.registeredActions.Remove(name);
        }

        /// <summary>
        /// Registers a native callback and returns data to closure.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="func">The function.</param>
        public void RegisterNativeFunction(string name, Func<string, object[]> func)
        {
            this.registeredFunctions.Add(name, func);
        }

        /// <summary>
        /// Removes a native callback function.
        /// </summary>
        /// <param name="name">The name of the callback.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool RegisterNativeFunction(string name)
        {
            return this.registeredFunctions.Remove(name);
        }

        /// <summary>
        /// Loads from content.
        /// </summary>
        /// <param name="contentFullName">Full name of the content.</param>
        public void LoadFromContent(string contentFullName)
        {
            var handler = this.LoadFromContentRequested;
            if (handler != null)
            {
                handler(this, contentFullName);
            }
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        /// <param name="content">The content.</param>
        public void LoadContent(string content)
        {
            var handler = this.LoadContentRequested;
            if (handler != null)
            {
                handler(this, content);
            }
        }

        /// <summary>
        /// Injects the java script.
        /// </summary>
        /// <param name="script">The script.</param>
        public void InjectJavaScript(string script)
        {
            lock (this.injectLock)
            {
                var handler = this.JavaScriptLoadRequested;
                if (handler != null)
                {
                    handler(this, script);
                }
            }
        }

        /// <summary>
        /// Calls the js function.
        /// </summary>
        /// <param name="funcName">Name of the function.</param>
        /// <param name="parameters">The parameters.</param>
        public void CallJsFunction(string funcName, params object[] parameters)
        {
            var builder = new StringBuilder();

            builder.Append(funcName);
            builder.Append("(");

            for (var n = 0; n < parameters.Length; n++)
            {
                builder.Append(this.jsonSerializer.Serialize(parameters[n]));
                if (n < parameters.Length - 1)
                {
                    builder.Append(", ");
                }
            }

            builder.Append(");");

            InjectJavaScript(builder.ToString());
        }

        /// <summary>
        /// Tries the get action.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="action">The action.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        internal bool TryGetAction(string name, out Action<string> action)
        {
            return this.registeredActions.TryGetValue(name, out action);
        }

        /// <summary>
        /// Tries the get function.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="func">The function.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        internal bool TryGetFunc(string name, out Func<string, object[]> func)
        {
            return this.registeredFunctions.TryGetValue(name, out func);
        }

        /// <summary>
        /// Handles the <see cref="E:LoadFinished" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        internal void OnLoadFinished(object sender, EventArgs e)
        {
            var handler = this.LoadFinished;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:LeftSwipe" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        internal void OnLeftSwipe(object sender, EventArgs e)
        {
            var handler = this.LeftSwipe;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:RightSwipe" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        internal void OnRightSwipe(object sender, EventArgs e)
        {
            var handler = this.RightSwipe;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Called when [navigating].
        /// </summary>
        /// <param name="uri">The URI.</param>
        internal void OnNavigating(Uri uri)
        {
            var handler = this.Navigating;
            if (handler != null)
            {
                handler(this, new EventArgs<Uri>(uri));
            }
        }
    }
}