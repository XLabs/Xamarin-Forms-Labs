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
using Android.Graphics;
using Android.Text;
using Android.Text.Style;

namespace XLabs.Forms.Controls
{
    public class CustomTypefaceSpan : TypefaceSpan
    {
        private Typeface newType;
        private Android.Graphics.Color fontColor;
        public CustomTypefaceSpan(String family, Typeface type, Android.Graphics.Color color)
            : base(family)
        {

            newType = type;
            fontColor = color;
        }


        public override void UpdateDrawState(TextPaint ds)
        {
            ApplyCustomTypeFace(ds, newType, fontColor);
        }


        public override void UpdateMeasureState(TextPaint paint)
        {

            ApplyCustomTypeFace(paint, newType, fontColor);
        }

        private static void ApplyCustomTypeFace(Paint paint, Typeface tf, Android.Graphics.Color color)
        {
            int oldStyle;
            Typeface old = paint.Typeface;
            if (old == null)
            {
                oldStyle = 0;
            }
            else
            {
                oldStyle = (int)old.Style;
            }

            int fake = oldStyle & ~(int)tf.Style;
            if ((fake & (int)TypefaceStyle.Bold) != 0)
            {
                paint.FakeBoldText = true;
            }

            if ((fake & (int)TypefaceStyle.Italic) != 0)
            {
                paint.TextSkewX = -0.25f;
            }



            paint.SetARGB(color.A, color.R, color.G, color.B);

            paint.SetTypeface(tf);
        }
    }
}