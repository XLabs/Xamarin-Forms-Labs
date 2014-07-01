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
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.Droid.Controls.ExtendedViewCell;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;


[assembly: ExportRenderer(typeof(ExtendedViewCell), typeof(ExtendedViewCellRenderer))]

namespace Xamarin.Forms.Labs.Droid.Controls.ExtendedViewCell
{
    public class ExtendedViewCellRenderer : Xamarin.Forms.Platform.Android.TextCellRenderer
    {

        private Context _context;

        protected override void OnCellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs args)
        {
            base.OnCellPropertyChanged(sender, args);
        }
        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {
            _context = context;

            var view = (ExtendedTextCell)item;
            if (convertView == null)
            {
                convertView = new BaseCellView(context);
            }
         //   var control = ((LinearLayout)convertView);

         
            return convertView; ;
        }

        void UpdateUi(ExtendedTextCell view, TextView control)
        {
            if (!string.IsNullOrEmpty(view.FontName))
            {
                control.Typeface = TrySetFont(view.FontName);
            }
            if (!string.IsNullOrEmpty(view.FontNameAndroid))
            {
                control.Typeface = TrySetFont(view.FontNameAndroid); ;
            }
            if (view.FontSize > 0)
                control.TextSize = (float)view.FontSize;
        }

        private Typeface TrySetFont(string fontName)
        {
            Typeface tf = Typeface.Default;
            try
            {
                tf = Typeface.CreateFromAsset(_context.Assets, fontName);
                return tf;
            }
            catch (Exception ex)
            {
                Console.Write("not found in assets {0}", ex);
                try
                {
                    tf = Typeface.CreateFromFile(fontName);
                    return tf;
                }
                catch (Exception ex1)
                {
                    Console.Write(ex1);
                    return Typeface.Default;
                }
            }
        }
    }
}