// ***********************************************************************
// Assembly         : XLabs.Forms.WinUniversal
// Author           : XLabs Team
// Created          : 01-01-2016
//
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ImageButtonRenderer.cs" company="XLabs Team">
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

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using XLabs.Enums;
using XLabs.Forms.Controls;
using Button = Xamarin.Forms.Button;
using Image = Windows.UI.Xaml.Controls.Image;
using Orientation = Windows.UI.Xaml.Controls.Orientation;
using TextAlignment = Windows.UI.Xaml.TextAlignment;
using Thickness = Windows.UI.Xaml.Thickness;

#if WINDOWS_APP || WINDOWS_PHONE_APP
using Xamarin.Forms.Platform.WinRT;
#elif WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
#endif


[assembly: ExportRenderer(typeof(ImageButton), typeof(ImageButtonRenderer))]

namespace XLabs.Forms.Controls
{
    /// <summary>
    ///     Draws a button on the Windows Phone platform with the image shown in the right
    ///     position with the right size.
    /// </summary>
    public partial class ImageButtonRenderer : ButtonRenderer
    {
        /// <summary>
        ///     The image displayed in the button.
        /// </summary>
        private Image _currentImage;

        /// <summary>
        ///     Handles the initial drawing of the button.
        /// </summary>
        /// <param name="e">Information on the <see cref="ImageButton" />.</param>
        protected override async void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            await AssignContent();
        }

        /// <summary>
        ///     Called when the underlying model's properties are changed.
        /// </summary>
        /// <param name="sender">Model sending the change event.</param>
        /// <param name="e">Event arguments.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName != ImageButton.SourceProperty.PropertyName &&
                e.PropertyName != ImageButton.DisabledSourceProperty.PropertyName &&
                e.PropertyName != VisualElement.IsEnabledProperty.PropertyName)
            {
                return;
            }

            var sourceButton = this.Element as ImageButton;
            if (sourceButton == null)
            {
                return;
            }

            Device.BeginInvokeOnMainThread(async () => await AssignContent());
        }

        private Task<Image> GetCurrentImage()
        {
            var sourceButton = this.Element as ImageButton;
            if (sourceButton == null) return null;

            return GetImageAsync(
                (!sourceButton.IsEnabled && sourceButton.DisabledSource != null) ? sourceButton.DisabledSource : sourceButton.Source,
                GetHeight(sourceButton.ImageHeightRequest),
                GetWidth(sourceButton.ImageWidthRequest),
                null);
        }

        private async Task AssignContent()
        {
            var sourceButton = this.Element as ImageButton;
            var targetButton = this.Control;
            if (sourceButton != null && targetButton != null && sourceButton.Source != null)
            {
                var stackPanel = new StackPanel
                {
                    //Background = sourceButton.BackgroundColor.ToBrush(),
                    Orientation =
                        (sourceButton.Orientation == ImageOrientation.ImageOnTop
                         || sourceButton.Orientation == ImageOrientation.ImageOnBottom)
                            ? Orientation.Vertical
                            : Orientation.Horizontal,

                };

                this._currentImage = await GetCurrentImage();
                SetImageMargin(this._currentImage, sourceButton.Orientation);

                var label = new TextBlock
                {
                    TextAlignment = GetTextAlignment(sourceButton.Orientation),
                    FontSize = 16,
                    VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center,
                    Text = sourceButton.Text
                };

                if (sourceButton.Orientation == ImageOrientation.ImageToLeft)
                {
                    targetButton.HorizontalContentAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
                }
                else if (sourceButton.Orientation == ImageOrientation.ImageToRight)
                {
                    targetButton.HorizontalContentAlignment = Windows.UI.Xaml.HorizontalAlignment.Right;
                }

                if (sourceButton.Orientation == ImageOrientation.ImageOnTop
                    || sourceButton.Orientation == ImageOrientation.ImageToLeft)
                {
                    this._currentImage.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
                    stackPanel.Children.Add(this._currentImage);
                    stackPanel.Children.Add(label);
                }
                else
                {
                    stackPanel.Children.Add(label);
                    stackPanel.Children.Add(this._currentImage);
                }

                targetButton.Content = stackPanel;
            }
        }
        /// <summary>
        ///     Returns the alignment of the label on the button depending on the orientation.
        /// </summary>
        /// <param name="imageOrientation">The orientation to use.</param>
        /// <returns>The alignment to use for the text.</returns>
        private static TextAlignment GetTextAlignment(ImageOrientation imageOrientation)
        {
            TextAlignment returnValue;
            switch (imageOrientation)
            {
                case ImageOrientation.ImageToLeft:
                    returnValue = TextAlignment.Left;
                    break;
                case ImageOrientation.ImageToRight:
                    returnValue = TextAlignment.Right;
                    break;
                default:
                    returnValue = TextAlignment.Center;
                    break;
            }

            return returnValue;
        }

        /// <summary>
        /// Returns a <see cref="Xamarin.Forms.Image" /> from the <see cref="ImageSource" /> provided.
        /// </summary>
        /// <param name="source">The <see cref="ImageSource" /> to load the image from.</param>
        /// <param name="height">The height for the image (divides by 2 for the Windows Phone platform).</param>
        /// <param name="width">The width for the image (divides by 2 for the Windows Phone platform).</param>
        /// <param name="currentImage">The current image.</param>
        /// <returns>A properly sized image.</returns>
        private static async Task<Image> GetImageAsync(ImageSource source, int height, int width, Image currentImage)
        {
            var image = currentImage ?? new Image();
            var handler = GetHandler(source);

            var imageSource = await handler.LoadImageAsync(source);

            image.Source = imageSource;
            image.Height = Convert.ToDouble(height / 2);
            image.Width = Convert.ToDouble(width / 2);
            return image;
        }

        /// <summary>
        ///     Sets a margin of 10 between the image and the text.
        /// </summary>
        /// <param name="image">The image to add a margin to.</param>
        /// <param name="orientation">The orientation of the image on the button.</param>
        private static void SetImageMargin(Image image, ImageOrientation orientation)
        {
            const int DefaultMargin = 10;
            var left = 0;
            var top = 0;
            var right = 0;
            var bottom = 0;

            switch (orientation)
            {
                case ImageOrientation.ImageToLeft:
                    right = DefaultMargin;
                    break;
                case ImageOrientation.ImageOnTop:
                    bottom = DefaultMargin;
                    break;
                case ImageOrientation.ImageToRight:
                    left = DefaultMargin;
                    break;
                case ImageOrientation.ImageOnBottom:
                    top = DefaultMargin;
                    break;
            }

            image.Margin = new Thickness(left, top, right, bottom);
        }
    }
}