// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ExtendedEntryCellRenderer.cs" company="XLabs Team">
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
using Xamarin.Forms.Platform.iOS;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedEntryCell), typeof(ExtendedEntryCellRenderer))]
namespace XLabs.Forms.Controls
{
    /// <summary>
    /// Class ExtendedEntryCellRenderer.
    /// </summary>
    public class ExtendedEntryCellRenderer : EntryCellRenderer
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
            var entryCell = (ExtendedEntryCell)item;
            var cell = base.GetCell (item, reusableCell, tv);
            
            if (cell != null)
            {
                var textField = (UITextField)cell.ContentView.Subviews [0]; 
                textField.SecureTextEntry = entryCell.IsPassword;
            }

            return cell;
        }
    } 
}

