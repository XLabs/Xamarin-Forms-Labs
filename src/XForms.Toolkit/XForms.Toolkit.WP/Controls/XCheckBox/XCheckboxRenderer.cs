#region

using System;
using System.Windows;
using System.Windows.Controls;

using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using XForms.Toolkit.Controls;
using XForms.Toolkit.WP.Controls.XCheckBox;

#endregion

[assembly: ExportRenderer(typeof (XCheckBox), typeof (XCheckBoxRenderer))]

namespace XForms.Toolkit.WP.Controls.XCheckBox
{
    internal class XCheckBoxRenderer : ViewRenderer
    {
        /*private bool isChecked;
        private string _text;
*/
        public event OnClicked Clicked;

        private CheckBox cb = new CheckBox();

        public XCheckBoxRenderer()
        {
        }

        protected override void OnModelSet()
        {
            base.OnModelSet();
            var y = Model;
            cb.IsChecked = (y as XForms.Toolkit.Controls.XCheckBox).IsChecked;
            cb.Content = (y as XForms.Toolkit.Controls.XCheckBox).Text;
            cb.Checked += cb_CheckChanged;
            cb.Unchecked += cb_CheckChanged;

            SetNativeControl(cb);
        }

        private void cb_CheckChanged(object sender, RoutedEventArgs args
            )
        {
            var x = Model as XForms.Toolkit.Controls.XCheckBox;
            var y = sender as CheckBox;
            //(Model as XCheckBox).IsChecked = y.IsChecked.Value;
            var e = new MyCbEventArgs();
            e.Checked = y.IsChecked.Value;
            e.Text = y.Content.ToString();
            e.Context = y.DataContext;
            (Model as XForms.Toolkit.Controls.XCheckBox).OnClicked(this, e);
        }
    }

    public delegate void OnClicked(object sender, EventArgs args);
}