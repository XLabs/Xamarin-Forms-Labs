// ***********************************************************************
// Assembly         : XLabs.Platform.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ViewGroupExtensions.cs" company="XLabs Team">
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
using Android.Graphics;
using Android.Views;

namespace XLabs.Platform
{
    /// <summary>
    /// Class ViewGroupExtensions.
    /// </summary>
    public static class ViewGroupExtensions
    {
        /// <summary>
        /// Gets the touched view.
        /// </summary>
        /// <param name="viewGroup">The view group.</param>
        /// <param name="point">The point.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>Tuple&lt;View, PointF&gt;.</returns>
        public static Tuple<View, PointF> GetTouchedView(this ViewGroup viewGroup, PointF point, PointF offset = null)
        {
            offset = offset ?? new PointF();

            for (var n = 0; n < viewGroup.ChildCount; n++)
            {
                var view = viewGroup.GetChildAt(n);

                if (view.IsHit(point))
                {
                    var vg = view as ViewGroup;
                    if (vg != null)
                    {
                        offset = new PointF(offset.X + vg.Left, offset.Y + vg.Top);
                        var p = new PointF(point.X - vg.Left, point.Y - vg.Top);
                        return GetTouchedView(vg, p, offset);
                    }
                    else if (string.IsNullOrWhiteSpace(view.ContentDescription))
                    {
                        return new Tuple<View, PointF>(viewGroup, offset);
                    }

                    return new Tuple<View, PointF>(view, offset);
                }
            }

            return null;
        }
    }
}