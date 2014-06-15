#region

using System;
using Xamarin.Forms;

#endregion

namespace XControls
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

        public void OnClicked(object sender, XCheckBoxClickedEventArgs e)
        {
            IsChecked = e.Checked;
            Text = e.Text;

            //Fire the event if there's a subscription
            if(Clicked!=null) 
                Clicked(sender, e);
        }

        public static BindableProperty TextProperty = BindableProperty.Create<XCheckBox, string>(box => box.Text,
            string.Empty, BindingMode.TwoWay);


        public static BindableProperty IsCheckedProperty = BindableProperty.Create<XCheckBox, bool>(
            box => box.IsChecked, false, BindingMode.TwoWay);

    }

    public delegate void OnClicked(object sender, XCheckBoxClickedEventArgs args);

    public class XCheckBoxClickedEventArgs : EventArgs
    {
        public bool Checked;
        public string Text;
        public object Context;
    }
}