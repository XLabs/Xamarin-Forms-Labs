using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.WP8.Controls.ExtendedLabel;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(ExtendedLabel), typeof(ExtendedLabelRenderer))]

namespace Xamarin.Forms.Labs.WP8.Controls.ExtendedLabel
{
    using System.Windows;
    using System.Windows.Media;
    using Xamarin.Forms.Labs.Controls;

    /// <summary>
    /// Extended Label Rendered for Windows Phone Platform
    /// </summary>
    public class ExtendedLabelRenderer : LabelRenderer
    {

        /// <summary>
        /// The on element changed callback.
        /// </summary>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            var view = (ExtendedLabel)Element;

            UpdateUi(view, this.Control);
        }

        /// <summary>
        /// Updates the UI.
        /// </summary>
        /// <param name="view">
        /// The view.
        /// </param>
        /// <param name="control">
        /// The control.
        /// </param>
        private static void UpdateUi(ExtendedLabel view, TextBlock control)
        {

            if (!string.IsNullOrEmpty(view.FontName))
            {
         
                control.FontFamily = new System.Windows.Media.FontFamily(view.FontName);
            
            }
            if (!string.IsNullOrEmpty(view.FontNameWP))
            {
                control.FontFamily = new System.Windows.Media.FontFamily(view.FontNameWP);
            }

            if (view.FontSize > 0)
                control.FontSize = view.FontSize;


            if (view.IsUnderline)
            {
                control.TextDecorations = TextDecorations.Underline;
            }
        }
    }
}
