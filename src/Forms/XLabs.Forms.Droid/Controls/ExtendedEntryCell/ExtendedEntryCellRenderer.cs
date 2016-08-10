// ***********************************************************************
// Assembly         : XLabs.Forms.Droid
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

using Android.Content;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
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
        /// Gets the cell core.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="convertView">The convert view.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="context">The context.</param>
        /// <returns>Android.Views.View.</returns>
        protected override Android.Views.View GetCellCore (Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {
            var cell = base.GetCellCore (item, convertView, parent, context);
            if (cell != null) {
            
                var textField = ((EntryCellView) cell).EditText as TextView;
                
                if (textField != null && textField.TransformationMethod != PasswordTransformationMethod.Instance) 
                {
                    textField.TransformationMethod = PasswordTransformationMethod.Instance;
                }
            }
            return cell;
        }
    }
}

