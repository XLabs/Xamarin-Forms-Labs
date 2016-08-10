// ***********************************************************************
// Assembly         : XLabs.Forms.Droid
// Author           : XLabs Team
// Created          : 01-06-2016
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-06-2016
// ***********************************************************************
// <copyright file="IconButtonRenderer.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System.Linq;
using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XLabs.Enums;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(IconButton), typeof(IconButtonRenderer))]
namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Implementation of Icon ButtonRenderer.
    /// </summary>
    public class IconButtonRenderer : ButtonRenderer
    {
        Typeface _iconFont;
        Typeface _textFont;
        IconButton _iconButton;
        //Final span including font and icon size and color
        SpannableString _iconSpan;
        int _textStartIndex = -1;
        int _textStopIndex = -1;
       
        Android.Widget.Button _nativeBtn;
        private TextViewRenderHelper _helper;

        /// <summary>
        /// Handles the Element Changed event messages
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null && this.Control != null)
            {
                if (_helper == null)
                    _helper = new TextViewRenderHelper(Context);
                if (_iconSpan == null)
                {
                    _nativeBtn = (Android.Widget.Button)this.Control;
                    _iconButton = (IconButton)e.NewElement;

                    _iconFont = _helper.TrySetFont("fontawesome-webfont.ttf");
                    _textFont = _iconButton.Font.ToTypeface();
                    _iconButton.IconSize = _iconButton.IconSize == 0 ? (float)_iconButton.FontSize : _iconButton.IconSize;
                    var computedString = BuildRawTextString();

                    _iconSpan = BuildSpannableString(computedString);
                    if (_iconButton.TextAlignement == Xamarin.Forms.TextAlignment.Center)
                    {
                        _nativeBtn.Gravity = Android.Views.GravityFlags.Center;

                    }
                    else if (_iconButton.TextAlignement == Xamarin.Forms.TextAlignment.End)
                    {
                        _nativeBtn.Gravity = Android.Views.GravityFlags.Right;
                    }
                    else if (_iconButton.TextAlignement == Xamarin.Forms.TextAlignment.Start)
                    {
                        _nativeBtn.Gravity = Android.Views.GravityFlags.Left;
                    }
                    _nativeBtn.TransformationMethod = null;
                    _nativeBtn.SetPadding(0, 0, 0, 0);
                    _nativeBtn.AfterTextChanged += nativeBtn_AfterTextChanged;
                }
            }


        }

        /// <summary>
        /// Since they are several over write of the Test property during layout we have to set this field as long as it is not definitly set
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void nativeBtn_AfterTextChanged(object sender, AfterTextChangedEventArgs e)
        {

            ISpannable spannable = e.Editable;
            var indice = spannable.ToString().IndexOf(_iconButton.Icon);
            var spans = spannable.GetSpans(indice, indice + _iconButton.Icon.Length, Java.Lang.Class.FromType(typeof(TypefaceSpan))).ToList();
            if (spans.Count == 0)
                _nativeBtn.SetText(_iconSpan, TextView.BufferType.Spannable);

        }

      
        
        /// <summary>
        /// Build the content string by concating icon and text according to control options
        /// </summary>
        /// <returns></returns>
        private string BuildRawTextString()
        {
            string computedText = string.Empty;
            if (!string.IsNullOrEmpty(_iconButton.Icon) && !string.IsNullOrEmpty(_iconButton.Text))
            {
                string iconSeparator = _iconButton.ShowIconSeparator ? " | " : " ";

                switch (_iconButton.Orientation)
                {
                    case ImageOrientation.ImageToLeft:

                        computedText = _iconButton.Icon + iconSeparator + _iconButton.Text;
                        _textStartIndex = computedText.IndexOf(iconSeparator);
                        _textStopIndex = computedText.Length;

                        break;
                    case ImageOrientation.ImageToRight:
                        computedText = _iconButton.Text + iconSeparator + _iconButton.Icon;
                        _textStartIndex = 0;
                        _textStopIndex = computedText.IndexOf(iconSeparator) + iconSeparator.Length;
                        break;
                    case ImageOrientation.ImageOnTop:
                        computedText = _iconButton.Icon + System.Environment.NewLine + _iconButton.Text;
                        _textStartIndex = computedText.IndexOf(_iconButton.Text);
                        _textStopIndex = computedText.Length - 1;
                        break;
                    case ImageOrientation.ImageOnBottom:
                        computedText = _iconButton.Text + System.Environment.NewLine + _iconButton.Icon;
                        _textStartIndex = 0;
                        _textStopIndex = computedText.IndexOf(System.Environment.NewLine) - 1;
                        break;
                }
            }
            else if (!string.IsNullOrEmpty(_iconButton.Text) && string.IsNullOrEmpty(_iconButton.Icon))
            {
                computedText = _iconButton.Text;
            }
            else if (string.IsNullOrEmpty(_iconButton.Text) && !string.IsNullOrEmpty(_iconButton.Icon))
            {
                computedText = _iconButton.Icon;
            }
            return computedText;
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
            if (!string.IsNullOrEmpty(_iconButton.Icon))
            {
                //set icon
                span.SetSpan(new CustomTypefaceSpan("fontawesome", _iconFont, _helper.GetSpanColor(_iconButton.IconColor, Control.TextColors)),
                    computedString.IndexOf(_iconButton.Icon),
                    computedString.IndexOf(_iconButton.Icon) + _iconButton.Icon.Length,
                    SpanTypes.ExclusiveExclusive);
                //set icon size
                span.SetSpan(new AbsoluteSizeSpan((int)_iconButton.IconSize, true),
                     computedString.IndexOf(_iconButton.Icon),
                     computedString.IndexOf(_iconButton.Icon) + _iconButton.Icon.Length,
                     SpanTypes.ExclusiveExclusive);


            }
            //if there is text
            if (!string.IsNullOrEmpty(_iconButton.Text))
            {
                span.SetSpan(new CustomTypefaceSpan("", _textFont, _helper.GetSpanColor(_iconButton.TextColor, Control.TextColors)),
                     _textStartIndex,
                     _textStopIndex,
                     SpanTypes.ExclusiveExclusive);
                span.SetSpan(new AbsoluteSizeSpan((int)_iconButton.FontSize, true),
                    _textStartIndex,
                     _textStopIndex,
                    SpanTypes.ExclusiveExclusive);


            }

            return span;

        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            _nativeBtn.AfterTextChanged -= nativeBtn_AfterTextChanged;
            base.Dispose(disposing);
        }
    }
}