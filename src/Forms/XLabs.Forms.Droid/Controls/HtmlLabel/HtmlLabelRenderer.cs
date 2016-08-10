// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : Wondersborn
// Created          : 22-07-2016
// 
// Last Modified By : Wondersborn
// Last Modified On : 22-07-2016
// ***********************************************************************
// <summary>
//       Android implementation of the HtmlLabel, a simple component to
//       display HTML text in a label without the need of a WebView.
// </summary>
// ***********************************************************************
// 

using System.ComponentModel;
using Android.Text;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]
namespace XLabs.Forms.Controls
{
    public class HtmlLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            Control?.SetText(Html.FromHtml(Element.Text), TextView.BufferType.Spannable);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Label.TextProperty.PropertyName)
                Control?.SetText(Html.FromHtml(Element.Text), TextView.BufferType.Spannable);
        }
    }
}