// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="CheckBoxCellRenderer.cs" company="XLabs Team">
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

using UIKit;
using Xamarin.Forms;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(CheckboxCell), typeof(CheckBoxCellRenderer))]

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Class CheckBoxCellRenderer.
    /// </summary>
    public class CheckBoxCellRenderer : ExtendedTextCellRenderer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckBoxCellRenderer"/> class.
        /// </summary>
        public CheckBoxCellRenderer ()
        {
        }

        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="reusableCell">The reusable cell.</param>
        /// <param name="tv">The table view.</param>
        /// <returns>UITableViewCell.</returns>
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var viewCell = item as CheckboxCell;
            var nativeCell = base.GetCell(item, reusableCell, tv);
            nativeCell.SelectionStyle = UITableViewCellSelectionStyle.None;

            if (viewCell == null) return nativeCell;

            nativeCell.Accessory = viewCell.Checked ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;

            viewCell.CheckedChanged += (s, e) => tv.ReloadData();

            return nativeCell;
        }

    }
}

