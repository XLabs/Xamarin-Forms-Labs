// ***********************************************************************
// Assembly         : XLabs.Platform.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ViewExtensions.cs" company="XLabs Team">
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

using System.IO;
using System.Threading.Tasks;
using Android.Graphics;

namespace XLabs.Platform
{
    /// <summary>
    /// Class ViewExtensions.
    /// </summary>
    public static class ViewExtensions
    {
        /// <summary>
        /// To the bitmap.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns>Android.Graphics.Bitmap.</returns>
        public static Android.Graphics.Bitmap ToBitmap(this Android.Views.View view)
        {
            var bitmap = Bitmap.CreateBitmap(view.Width, view.Height, Bitmap.Config.Argb8888);
            using (var c = new Canvas(bitmap))
            {
                view.Draw(c);
            }

            return bitmap;
        }

        /// <summary>
        /// Streams to PNG.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="stream">The stream.</param>
        /// <returns>Task.</returns>
        public static async Task StreamToPng(this Android.Views.View view, Stream stream)
        {
            await view.ToBitmap().CompressAsync(Bitmap.CompressFormat.Png, 100, stream);
        }

        /// <summary>
        /// Determines whether the specified point is hit.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="point">The point.</param>
        /// <returns><c>true</c> if the specified point is hit; otherwise, <c>false</c>.</returns>
        public static bool IsHit(this Android.Views.View view, PointF point)
        {
            var r = new Rect();
            view.GetHitRect(r);

            var touch = new Rect((int)point.X, (int)point.Y, (int)point.X, (int)point.Y);

            return r.Intersect(touch);
        }
    }
}