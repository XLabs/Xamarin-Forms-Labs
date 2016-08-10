// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="AccessoryViewCellRenderer.cs" company="XLabs Team">
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

using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(AccessoryViewCell), typeof(AccessoryViewCellRenderer))]

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Class AccessoryViewCellRenderer.
    /// </summary>
    public class AccessoryViewCellRenderer : ExtendedTextCellRenderer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccessoryViewCellRenderer"/> class.
        /// </summary>
        public AccessoryViewCellRenderer ()
        {
        }

        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="reusableCell">The reusable table view cell.</param>
        /// <param name="tv">The table view.</param>
        /// <returns>MonoTouch.UIKit.UITableViewCell.</returns>
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UIKit.UITableView tv)
        {
            var viewCell = item as AccessoryViewCell;
            var nativeCell = base.GetCell(item, reusableCell, tv);

            if (viewCell != null)
            {
                var frame = new CGRect (0, 0, (float)viewCell.AccessoryView.WidthRequest, (float)viewCell.AccessoryView.HeightRequest);
                var nativeView = RendererFactory.GetRenderer (viewCell.AccessoryView).NativeView;
                nativeView.Frame = frame;
                nativeView.Bounds = frame;
                nativeCell.AccessoryView = nativeView;
            }

            return nativeCell;
        }
    }
}

