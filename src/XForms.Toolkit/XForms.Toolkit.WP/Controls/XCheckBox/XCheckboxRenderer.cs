#region

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using XControls.WinPhone;
using XControls;

#endregion

[assembly: ExportRenderer(typeof (XCheckBox), typeof (XCheckBoxRenderer))]
namespace XControls.WinPhone
{
    public class XCheckBoxRenderer : ViewRenderer
    {

        public event OnClicked Clicked;
        public event OnCheckChanged CheckChanged;

        private CheckBox nativeCheckBox = new CheckBox();

        public XCheckBoxRenderer()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);
            var xElement = Element;
            nativeCheckBox.IsChecked = (xElement as XCheckBox).IsChecked;
            nativeCheckBox.Content = (xElement as XCheckBox).Text;
            nativeCheckBox.Checked += NativeCheckBoxCheckChanged;
            nativeCheckBox.Unchecked += NativeCheckBoxCheckChanged;

            SetNativeControl(nativeCheckBox);
        }

        private void NativeCheckBoxCheckChanged(object sender, RoutedEventArgs args
            )
        {
            var nativeCheckbox = sender as CheckBox;

            var clickedEventArgs = new XCheckBoxClickedEventArgs();
            clickedEventArgs.Checked = nativeCheckbox.IsChecked.Value;
            clickedEventArgs.Text = nativeCheckbox.Content.ToString();
            clickedEventArgs.Context = nativeCheckbox.DataContext;
            
            //Fire the event if there's a subscription
            if(CheckChanged!=null)
                CheckChanged(sender, args);
            
            (Element as XControls.XCheckBox).OnClicked(this, clickedEventArgs);
        }
    }

    public delegate void OnCheckChanged(object sender, RoutedEventArgs args);

    public delegate void OnClicked(object sender, EventArgs args);
}