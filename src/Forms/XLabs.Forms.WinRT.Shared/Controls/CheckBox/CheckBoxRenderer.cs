// ***********************************************************************
// Assembly         : XLabs.Forms.WinUniversal
// Author           : XLabs Team
// Created          : 12-27-2015
//
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="CheckBoxRenderer.cs" company="XLabs Team">
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

using System.ComponentModel;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using XLabs.Forms.Controls;
using NativeCheckBox = Windows.UI.Xaml.Controls.CheckBox;

#if WINDOWS_APP || WINDOWS_PHONE_APP
using Xamarin.Forms.Platform.WinRT;
#elif WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
#endif

[assembly: ExportRenderer(typeof(XLabs.Forms.Controls.CheckBox), typeof(CheckBoxRenderer))]

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Class CheckBoxRenderer.
    /// </summary>
    public class CheckBoxRenderer : ViewRenderer<CheckBox, NativeCheckBox>
    {
        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<CheckBox> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.CheckedChanged -= CheckedChanged;
            }

            if (Control == null && e.NewElement != null)
            {
                var checkBox = new NativeCheckBox();
                checkBox.Checked += (s, args) => Element.Checked = true;
                checkBox.Unchecked += (s, args) => Element.Checked = false;

                SetNativeControl(checkBox);
            }

            if (e.NewElement == null || this.Control == null) return;

            Control.Content = e.NewElement.Text;
            Control.IsChecked = e.NewElement.Checked;
            Control.Foreground = e.NewElement.TextColor.ToBrush();

            UpdateFont();

            e.NewElement.CheckedChanged += CheckedChanged;
        }

        /// <summary>
        /// Handles the <see cref="E:ElementPropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Checked":
                    Control.IsChecked = Element.Checked;
                    break;
                case "TextColor":
                    Control.Foreground = Element.TextColor.ToBrush();
                    break;
                case "FontName":
                case "FontSize":
                    UpdateFont();
                    break;
                case "CheckedText":
                case "UncheckedText":
                    Control.Content = Element.Text;
                    break;
                default:
                    base.OnElementPropertyChanged(sender, e);
                    break;
            }
        }

        /// <summary>
        /// Checkeds the changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The event arguments.</param>
        private void CheckedChanged(object sender, EventArgs<bool> eventArgs)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Control.Content = Element.Text;
                Control.IsChecked = eventArgs.Value;
            });
        }

        /// <summary>
        /// Updates the font.
        /// </summary>
        private void UpdateFont()
        {
            if (!string.IsNullOrEmpty(Element.FontName))
            {
                Control.FontFamily = new FontFamily(Element.FontName);
            }

            Control.FontSize = (Element.FontSize > 0) ? (float)Element.FontSize : 12.0f;
        }
    }
}
