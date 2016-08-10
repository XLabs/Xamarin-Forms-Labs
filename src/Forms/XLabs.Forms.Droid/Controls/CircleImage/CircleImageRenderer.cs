// ***********************************************************************
// Assembly         : XLabs.Forms.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="CircleImageRenderer.cs" company="XLabs Team">
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
using Android.Graphics;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(CircleImage), typeof(CircleImageRenderer))]
namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Class CircleImageRenderer.
    /// </summary>
    public class CircleImageRenderer : ImageRenderer
    {
        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null && (int)Android.OS.Build.VERSION.SdkInt < 18)
            {
                SetLayerType(LayerType.Software, null);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:ElementPropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == Image.IsLoadingProperty.PropertyName && !this.Element.IsLoading
                && this.Control.Drawable != null && this.Element.Aspect != Aspect.AspectFit)
            {
                using (var sourceBitmap = Bitmap.CreateBitmap(this.Control.Drawable.IntrinsicWidth, this.Control.Drawable.IntrinsicHeight, Bitmap.Config.Argb8888))
                using (var canvas = new Canvas(sourceBitmap))   
                {
                    this.Control.Drawable.SetBounds(0, 0, canvas.Width, canvas.Height);
                    this.Control.Drawable.Draw(canvas);
                    this.ReshapeImage(sourceBitmap);
                }
            }
        }

        /// <summary>
        /// Draw one child of this View Group.
        /// </summary>
        /// <param name="canvas">The canvas on which to draw the child</param>
        /// <param name="child">Who to draw</param>
        /// <param name="drawingTime">The time at which draw is occurring</param>
        /// <returns>To be added.</returns>
        /// <since version="Added in API level 1" />
        /// <remarks><para tool="javadoc-to-mdoc">Draw one child of this View Group. This method is responsible for getting
        /// the canvas in the right state. This includes clipping, translating so
        /// that the child's scrolled origin is at 0, 0, and applying any animation
        /// transformations.</para>
        /// <para tool="javadoc-to-mdoc">
        ///   <format type="text/html">
        ///     <a href="http://developer.android.com/reference/android/view/ViewGroup.html#drawChild(android.graphics.Canvas, android.view.View, long)" target="_blank">[Android Documentation]</a>
        ///   </format>
        /// </para></remarks>
        protected override bool DrawChild(Canvas canvas, global::Android.Views.View child, long drawingTime)
        {
            if (this.Element.Aspect != Aspect.AspectFit)
            {
                return base.DrawChild(canvas, child, drawingTime);
            }

            using (var path = new Path())
            {
                path.AddCircle(Width / 2, Height / 2, (Math.Min(Width, Height) - 10) / 2, Path.Direction.Ccw);
                canvas.Save();
                canvas.ClipPath(path);
            }

            return base.DrawChild(canvas, child, drawingTime);
        }

        /// <summary>
        /// Reshapes the image.
        /// </summary>
        /// <param name="sourceBitmap">The source bitmap.</param>
        private void ReshapeImage(Bitmap sourceBitmap)
        {
            if (sourceBitmap == null) return;

            using (var sourceRect = GetScaledRect(sourceBitmap.Height, sourceBitmap.Width))
            using (var rect = this.GetTargetRect(sourceBitmap.Height, sourceBitmap.Width))
            using (var output = Bitmap.CreateBitmap(rect.Width(), rect.Height(), Bitmap.Config.Argb8888))
            using (var canvas = new Canvas(output))
            using (var paint = new Paint())
            using (var rectF = new RectF(rect))
            {
                var roundRx = rect.Width() / 2;
                var roundRy = rect.Height() / 2;

                paint.AntiAlias = true;
                canvas.DrawARGB(0, 0, 0, 0);
                paint.Color = Android.Graphics.Color.ParseColor("#ff424242");
                canvas.DrawRoundRect(rectF, roundRx, roundRy, paint);

                paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));
                canvas.DrawBitmap(sourceBitmap, sourceRect, rect, paint);

                //this.DrawBorder(canvas, rect.Width(), rect.Height());

                using (var drawable = this.Control.Drawable) // don't remove, this is making sure memory isn't leaked
                {
                    this.Control.SetImageBitmap(output); 
                }

                // Forces the internal method of InvalidateMeasure to be called.
                this.Element.WidthRequest = this.Element.WidthRequest;
            }
        }

        /// <summary>
        /// Gets the scaled rect.
        /// </summary>
        /// <param name="sourceHeight">Height of the source.</param>
        /// <param name="sourceWidth">Width of the source.</param>
        /// <returns>Rect.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private Rect GetScaledRect(int sourceHeight, int sourceWidth)
        {
            int height = 0;
            int width = 0;
            int top = 0;
            int left = 0;

            switch (this.Element.Aspect)
            {
                case Aspect.AspectFill:
                    height = sourceHeight;
                    width = sourceWidth;
                    height = MakeSquare(height, ref width);
                    left = (int)((sourceWidth - width) / 2);
                    top = (int)((sourceHeight - height) / 2);
                    break;
                case Aspect.Fill:
                    height = sourceHeight;
                    width = sourceWidth;
                    break;
                case Aspect.AspectFit:
                    height = sourceHeight;
                    width = sourceWidth;
                    height = MakeSquare(height, ref width);
                    left = (int)((sourceWidth - width) / 2);
                    top = (int)((sourceHeight - height) / 2);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return new Rect(left, top, width + left, height + top);
        }

        /// <summary>
        /// Makes the square.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <param name="width">The width.</param>
        /// <returns>System.Int32.</returns>
        private static int MakeSquare(int height, ref int width)
        {
            if (height < width)
            {
                width = height;
            }
            else
            {
                height = width;
            }
            return height;
        }

        /// <summary>
        /// Gets the target rect.
        /// </summary>
        /// <param name="sourceHeight">Height of the source.</param>
        /// <param name="sourceWidth">Width of the source.</param>
        /// <returns>Rect.</returns>
        private Rect GetTargetRect(int sourceHeight, int sourceWidth)
        {
            var height = this.Element.HeightRequest > 0
                ? (int)Math.Round(this.Element.HeightRequest, 0)
                : sourceHeight; 
            var width = this.Element.WidthRequest > 0
                ? (int)Math.Round(this.Element.WidthRequest, 0)
                : sourceWidth; 

            // Make Square
            height = MakeSquare(height, ref width);

            return new Rect(0, 0, width, height);
        }
    }
}
