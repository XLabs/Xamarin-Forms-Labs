using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XForms.Toolkit.Controls
{
    public partial class HybridWebViewRenderer
    {
#if !WINDOWS_PHONE
        private const string Format = "(file|http|https)://(local|LOCAL)/Action=(?<Action>[\\w]+)/";
        private static readonly Regex Expression = new Regex(Format);
#endif

        private void InjectNativeFunctionScript()
        {
            var builder = new StringBuilder();
            builder.Append("function Native(action, data){ ");
#if WINDOWS_PHONE
            builder.Append("window.external.notify(");
#else
            builder.Append("window.location = \"f//LOCAL/Action=\" + ");
#endif
            builder.Append("action + \"/\"");
            builder.Append(" + ((typeof data == \"object\") ? JSON.stringify(data) : data)");
#if WINDOWS_PHONE
            builder.Append(")");
#endif
            builder.Append(" ;}");

            this.Inject(builder.ToString());
        }

        void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Uri")
            {
                this.Load(this.Element.Uri);
            }
        }

        private void Initialize()
        {
            this.Element.PropertyChanged += Model_PropertyChanged;
            if (this.Element.Uri != null)
            {
                this.Load (this.Element.Uri);
            }

            this.Element.JavaScriptLoadRequested += (s, e) => Inject(e);
        }

        partial void Inject(string script);

        partial void Load(Uri uri);

#if !WINDOWS_PHONE
        private bool CheckRequest(string request)
        {
            var m = Expression.Match(request);

            if (m.Success)
            {
                //request = request.Remove(0, 
                Action<string> action;
                var name = m.Groups["Action"].Value;

                if (this.Element.TryGetAction (name, out action))
                {
                    var data = Uri.UnescapeDataString (request.Remove (m.Index, m.Length));
                    action.Invoke (data);
//                    return true;
                } else
                {
                    System.Diagnostics.Debug.WriteLine ("Unhandled callback {0} was called from JavaScript", name);
                }
            }

            return m.Success;
        }
#endif
    }
}
