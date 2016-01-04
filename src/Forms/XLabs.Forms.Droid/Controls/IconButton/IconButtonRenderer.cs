using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        //Final span including font and icon size and color
        SpannableString iconSpan;
        int textStartIndex = -1;
        int textStopIndex = -1;
       
        Android.Widget.Button nativeBtn;

        public IconButtonRenderer()
            : base()
        {

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == IconButton.IconColorProperty.PropertyName 
                || e.PropertyName == IconButton.IconProperty.PropertyName 
                || e.PropertyName == IconButton.TextProperty.PropertyName 
                || e.PropertyName == IconButton.TextColorProperty.PropertyName 
                || e.PropertyName == IconButton.IsVisibleProperty.PropertyName 
                || e.PropertyName == IconButton.IconSizeProperty.PropertyName 
                || e.PropertyName == IconButton.FontSizeProperty.PropertyName 
                || e.PropertyName == IconButton.OrientationProperty.PropertyName)
            {
                SetText();
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {

            base.OnElementChanged(e);
            if (e.NewElement != null && this.Control != null)
            {
                if (iconSpan == null)
                {
                    if (nativeBtn != null)
                        nativeBtn.AfterTextChanged -= nativeBtn_AfterTextChanged;
                    nativeBtn = (Android.Widget.Button)this.Control;
                    iconButton = (IconButton)e.NewElement;

                    iconFont = TrySetFont(iconButton.IconFontName ?? "fontawesome-webfont.ttf");
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


        }

        private void SetText()
        {
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

            nativeBtn.SetText(iconSpan, TextView.BufferType.Spannable);
        }
        /// <summary>
        /// Since they are several over write of the Test property during layout we have to set this field as long as it is not definitly set
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void nativeBtn_AfterTextChanged(object sender, AfterTextChangedEventArgs e)
        {

            if (iconButton.Icon == null)
            {
                iconSpan = null;
                return;
            }

            ISpannable spannable = e.Editable;
            var indice = spannable.ToString().IndexOf(iconButton.Icon);
            var spans = spannable.GetSpans(indice, indice + iconButton.Icon.Length, Java.Lang.Class.FromType(typeof(TypefaceSpan))).ToList();
            if (spans.Count == 0)
                nativeBtn.SetText(iconSpan, TextView.BufferType.Spannable);

        }

      
        
        /// <summary>
        /// Build the content string by concating icon and text according to control options
        /// </summary>
        /// <returns></returns>
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

        private Android.Graphics.Color GetSpanColor(Xamarin.Forms.Color color)
        {
            if (color == Xamarin.Forms.Color.Default)
                return new Android.Graphics.Color(Control.TextColors.DefaultColor);

            return color.ToAndroid();
        }

        /// <summary>
        /// Build the spannable according to the computed text, meaning set the right font, color and size to the text and icon char index
        /// </summary>
        /// <param name="computedString"></param>
        /// <returns></returns>
        private SpannableString BuildSpannableString(string computedString)
        {
            SpannableString span = new SpannableString(computedString);
            //if there is an icon
            if (!string.IsNullOrEmpty(iconButton.Icon))
            {
                //set icon
                span.SetSpan(new CustomTypefaceSpan("fontawesome", iconFont, GetSpanColor(iconButton.IconColor)),
                    computedString.IndexOf(iconButton.Icon),
                    computedString.IndexOf(iconButton.Icon) + iconButton.Icon.Length,
                    SpanTypes.ExclusiveExclusive);
                //set icon size
                span.SetSpan(new AbsoluteSizeSpan((int)iconButton.IconSize, true),
                     computedString.IndexOf(iconButton.Icon),
                     computedString.IndexOf(iconButton.Icon) + iconButton.Icon.Length,
                     SpanTypes.ExclusiveExclusive);


            }
            //if there is text
            if (!string.IsNullOrEmpty(iconButton.Text))
            {
                span.SetSpan(new CustomTypefaceSpan("", textFont, GetSpanColor(iconButton.TextColor)),
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

       

        /// <summary>
        /// Load the FA font from assets
        /// </summary>
        /// <param name="fontName"></param>
        /// <returns></returns>
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

        protected override void Dispose(bool disposing)
        {
            nativeBtn.AfterTextChanged -= nativeBtn_AfterTextChanged;
            base.Dispose(disposing);
        }
    }
}