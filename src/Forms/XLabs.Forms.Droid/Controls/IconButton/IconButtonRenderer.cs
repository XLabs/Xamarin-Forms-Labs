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
using XLabs.Forms.Controls;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using Android.Text;
using XLabs.Enums;
using XLabs.Forms.Services;
using Android.Text.Style;

[assembly: ExportRenderer(typeof(IconButton), typeof(IconButtonRenderer))]
namespace XLabs.Forms.Controls
{
    public class IconButtonRenderer : ButtonRenderer
    {
        Typeface iconFont;
        Typeface textFont;
        IconButton iconButton;
        SpannableString iconSpan;
        int textStartIndex = -1;
        int textStopIndex = -1;
       
        Android.Widget.Button nativeBtn;

        private static SpannableString span;


        public IconButtonRenderer()
            : base()
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {

            base.OnElementChanged(e);
            if (iconSpan == null)
            {
                nativeBtn = (Android.Widget.Button)this.Control;
                iconButton = (IconButton)e.NewElement;

                iconFont = TrySetFont("fontawesome-webfont.ttf");
                textFont = iconButton.Font.ToTypeface();
                iconButton.IconSize = iconButton.IconSize == 0 ? (float)iconButton.FontSize : iconButton.IconSize;
                var computedString = BuildRawTextString();

                iconSpan = BuildSpannableString(computedString);
                if (iconButton.TextAlignement == Xamarin.Forms.TextAlignment.Center)
                {
                    nativeBtn.Gravity = Android.Views.GravityFlags.Center;

                }
                else if (iconButton.TextAlignement == Xamarin.Forms.TextAlignment.End)
                {
                    nativeBtn.Gravity = Android.Views.GravityFlags.Right;
                }
                else if (iconButton.TextAlignement == Xamarin.Forms.TextAlignment.Start)
                {
                    nativeBtn.Gravity = Android.Views.GravityFlags.Left;
                }
                nativeBtn.TransformationMethod = null;
                nativeBtn.SetPadding(0, 0, 0, 0);
                nativeBtn.AfterTextChanged += nativeBtn_AfterTextChanged;
            }


        }

        void nativeBtn_AfterTextChanged(object sender, AfterTextChangedEventArgs e)
        {

            ISpannable spannable = e.Editable;
            var indice = spannable.ToString().IndexOf(iconButton.Icon);
            var spans = spannable.GetSpans(indice, indice + iconButton.Icon.Length, Java.Lang.Class.FromType(typeof(TypefaceSpan))).ToList();
            if (spans.Count == 0)
                nativeBtn.SetText(iconSpan, TextView.BufferType.Spannable);

        }

      
        

        private string BuildRawTextString()
        {
            string computedText = string.Empty;
            if (!string.IsNullOrEmpty(iconButton.Icon) && !string.IsNullOrEmpty(iconButton.Text))
            {
                string iconSeparator = iconButton.ShowIconSeparator ? " | " : " ";

                switch (iconButton.Orientation)
                {
                    case ImageOrientation.ImageToLeft:

                        computedText = iconButton.Icon + iconSeparator + iconButton.Text;
                        textStartIndex = computedText.IndexOf(iconSeparator);
                        textStopIndex = computedText.Length;

                        break;
                    case ImageOrientation.ImageToRight:
                        computedText = iconButton.Text + iconSeparator + iconButton.Icon;
                        textStartIndex = 0;
                        textStopIndex = computedText.IndexOf(iconSeparator) + iconSeparator.Length;
                        break;
                    case ImageOrientation.ImageOnTop:
                        computedText = iconButton.Icon + System.Environment.NewLine + iconButton.Text;
                        textStartIndex = computedText.IndexOf(iconButton.Text);
                        textStopIndex = computedText.Length - 1;
                        break;
                    case ImageOrientation.ImageOnBottom:
                        computedText = iconButton.Text + System.Environment.NewLine + iconButton.Icon;
                        textStartIndex = 0;
                        textStopIndex = computedText.IndexOf(System.Environment.NewLine) - 1;
                        break;
                }
            }
            else if (!string.IsNullOrEmpty(iconButton.Text) && string.IsNullOrEmpty(iconButton.Icon))
            {
                computedText = iconButton.Text;
            }
            else if (string.IsNullOrEmpty(iconButton.Text) && !string.IsNullOrEmpty(iconButton.Icon))
            {
                computedText = iconButton.Icon;
            }
            return computedText;
        }

        private SpannableString BuildSpannableString(string computedString)
        {
            SpannableString span = new SpannableString(computedString);
            if (!string.IsNullOrEmpty(iconButton.Icon))
            {
                //set icon
                span.SetSpan(new CustomTypefaceSpan("fontawesome", iconFont, iconButton.IconColor.ToAndroid()),
                    computedString.IndexOf(iconButton.Icon),
                    computedString.IndexOf(iconButton.Icon) + iconButton.Icon.Length,
                    SpanTypes.ExclusiveExclusive);
                //set icon size
                span.SetSpan(new AbsoluteSizeSpan((int)iconButton.IconSize, true),
                     computedString.IndexOf(iconButton.Icon),
                     computedString.IndexOf(iconButton.Icon) + iconButton.Icon.Length,
                     SpanTypes.ExclusiveExclusive);


            }
            if (!string.IsNullOrEmpty(iconButton.Text))
            {
                span.SetSpan(new CustomTypefaceSpan("", textFont, iconButton.TextColor.ToAndroid()),
                     textStartIndex,
                     textStopIndex,
                     SpanTypes.ExclusiveExclusive);
                span.SetSpan(new AbsoluteSizeSpan((int)iconButton.FontSize, true),
                    textStartIndex,
                     textStopIndex,
                    SpanTypes.ExclusiveExclusive);


            }

            return span;

        }

       


        private Typeface TrySetFont(string fontName)
        {
            try
            {
                var tp = Typeface.CreateFromAsset(Context.Assets, "fonts/" + fontName);

                return tp;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("not found in assets. Exception: {0}", ex));
                try
                {
                    return Typeface.CreateFromFile("fonts/" + fontName);
                }
                catch (Exception ex1)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("not found by file. Exception: {0}", ex1));

                    return Typeface.Default;
                }
            }
        }
    }
}