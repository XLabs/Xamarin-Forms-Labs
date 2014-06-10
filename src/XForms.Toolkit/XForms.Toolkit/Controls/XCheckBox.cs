#region

using System;
using Xamarin.Forms;

#endregion

namespace XForms.Toolkit.Controls
{
    public class XCheckBox : View
    {
        public bool IsChecked
        {
            get { return (bool) GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        public string Text
        {
            set { SetValue(TextProperty, value); }
            get { return (string) GetValue(TextProperty); }
        }

        public event OnClicked Clicked;

        public XCheckBox()
        {
        }

        public void OnClicked(object sender, MyCbEventArgs e)
        {
            IsChecked = e.Checked;
            Text = e.Text;
            Clicked(sender, e);
        }

        public static BindableProperty TextProperty = BindableProperty.Create("TextProperty", typeof (string),
            typeof (string), string.Empty,
            BindingMode.TwoWay);

        public static BindableProperty IsCheckedProperty = BindableProperty.Create("IsCheckedProperty", typeof (bool),
            typeof (bool), false, BindingMode.TwoWay);
    }

    public delegate void OnClicked(object sender, MyCbEventArgs args);

    public class MyCbEventArgs : EventArgs
    {
        public bool Checked;
        public string Text;
        public object Context;
    }
}