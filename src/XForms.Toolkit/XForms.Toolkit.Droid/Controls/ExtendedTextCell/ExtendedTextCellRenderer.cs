
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

[assembly: ExportRenderer (typeof (ExtendedTextCell), typeof (ExtendedTextCellRenderer))]
namespace XForms.Toolkit.Droid
{
	public class ExtendedTextCellRenderer :  Xamarin.Forms.Platform.Android.TextCellRenderer
	{
		public ExtendedTextCellRenderer (){}
		private Context _context;

		protected override void OnCellPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs args)
		{
			base.OnCellPropertyChanged (sender, args);
		}
		protected override Android.Views.View GetCellCore (Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
		{
			_context = context;

			var view = (ExtendedTextCell)item;
			if (convertView == null) {
				convertView = new BaseCellView (context);
			} 
			var control = ((LinearLayout)convertView);

			var mainview = (TextView)(control.GetChildAt (1) as LinearLayout).GetChildAt (0);
			var detailview = (TextView)(control.GetChildAt (1) as LinearLayout).GetChildAt (1);

			UpdateUi (view, mainview);
			UpdateUi (view, detailview);
			return  convertView;;
		}

		void UpdateUi (ExtendedTextCell view, TextView control)
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
				tf = Typeface.CreateFromAsset (_context.Assets,fontName);
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

