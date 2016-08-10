// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ExtendedViewCellRenderer.cs" company="XLabs Team">
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
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedViewCell), typeof(ExtendedViewCellRenderer))]

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Class ExtendedViewCellRenderer.
    /// </summary>
    public class ExtendedViewCellRenderer : ViewCellRenderer
    {
        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="reusableCell">The reusable TableView cell.</param>
        /// <param name="tv">The TableView.</param>
        /// <returns>UITableViewCell.</returns>
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var extendedCell = (ExtendedViewCell)item;
            var cell = base.GetCell(item, reusableCell,tv);
            if (cell != null) 
            {
                cell.BackgroundColor = extendedCell.BackgroundColor.ToUIColor();
                cell.SeparatorInset = new UIEdgeInsets(
                    (nfloat)extendedCell.SeparatorPadding.Top, 
                    (nfloat)extendedCell.SeparatorPadding.Left,
                    (nfloat)extendedCell.SeparatorPadding.Bottom, 
                    (nfloat)extendedCell.SeparatorPadding.Right);

                if (extendedCell.ShowDisclousure) 
                {
                    cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                    if (!string.IsNullOrEmpty (extendedCell.DisclousureImage)) 
                    {
                        var detailDisclosureButton = UIButton.FromType (UIButtonType.Custom);
                        detailDisclosureButton.SetImage (UIImage.FromBundle (extendedCell.DisclousureImage), UIControlState.Normal);
                        detailDisclosureButton.SetImage (UIImage.FromBundle (extendedCell.DisclousureImage), UIControlState.Selected);

                        detailDisclosureButton.Frame = new CGRect (0f, 0f, 30f, 30f);
                        detailDisclosureButton.TouchUpInside += (sender, e) => 
                        {
                                try 
                                {
                                    var index = tv.IndexPathForCell (cell);
                                    tv.SelectRow (index, true, UITableViewScrollPosition.None);
                                    tv.Source.RowSelected (tv, index);
                                } 
                                catch (Foundation.You_Should_Not_Call_base_In_This_Method) 
                                {
                                    Console.Write("XLabs Weird stuff : You_Should_Not_Call_base_In_This_Method happend");
                                }
                        };
                        cell.AccessoryView = detailDisclosureButton;
                    }
                }
            }

            if (!extendedCell.ShowSeparator)
            {
                tv.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            }   

            tv.SeparatorColor = extendedCell.SeparatorColor.ToUIColor();

            return cell;
        }
    }
}

