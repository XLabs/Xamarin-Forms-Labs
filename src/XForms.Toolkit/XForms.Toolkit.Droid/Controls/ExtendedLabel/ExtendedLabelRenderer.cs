
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Graphics;
using XForms.Toolkit.Controls;
using XForms.Toolkit.Droid;

[assembly: ExportRenderer (typeof (ExtendedLabel), typeof (ExtendedLabelRender))]
namespace XForms.Toolkit.Droid
{
	public class ExtendedLabelRender : LabelRenderer
	{
		public ExtendedLabelRender (){}

		protected override void OnElementChanged (ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged (e);
			var view = (ExtendedLabel)this.Element;
			var control = (global::Android.Widget.TextView)Control;
			UpdateUi (view, control);
		}

		void UpdateUi (ExtendedLabel view, TextView control)
		{
			if (!string.IsNullOrEmpty (view.FontName)) {
				control.Typeface = 	TrySetFont (view.FontName);
			}
			if (!string.IsNullOrEmpty (view.FontNameAndroid)) {
				control.Typeface = 	TrySetFont (view.FontNameAndroid);;
			}
			if (view.FontSize > 0)
				control.TextSize = (float)view.FontSize;
		 }

		private Typeface TrySetFont (string fontName)
		{
			Typeface tf = Typeface.Default;
			try {
				tf = Typeface.CreateFromAsset (Context.Assets,fontName);
				return tf;
			}
			catch (Exception ex) {
				Console.Write ("not found in assets {0}", ex);
				try {
					tf = Typeface.CreateFromFile (fontName);
					return tf;
				}
				catch (Exception ex1) {
					Console.Write (ex1);
					return Typeface.Default;
				}
			}
		}
	}
}

