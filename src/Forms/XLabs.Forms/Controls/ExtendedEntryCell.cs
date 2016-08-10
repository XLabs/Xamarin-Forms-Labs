// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ExtendedEntryCell.cs" company="XLabs Team">
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

using System.Runtime.CompilerServices;
using Xamarin.Forms;

[assembly: 
    InternalsVisibleTo("XLabs.Forms.Droid"),
    InternalsVisibleTo("XLabs.Forms.iOS"),
    InternalsVisibleTo("XLabs.Forms.WP8")]

namespace XLabs.Forms.Controls
{
    /// <summary>
    /// An extended entry cell control that allows set IsPassword
    /// </summary>
    public class ExtendedEntryCell : EntryCell
    {

        /// <summary>
        /// The IsPassword property
        /// </summary>
        public static readonly BindableProperty IsPasswordProperty =
            BindableProperty.Create<ExtendedEntryCell,bool> ( p => p.IsPassword, false);

        /// <summary>
        /// Gets or sets IsPassword 
        /// </summary>
        public bool IsPassword 
        {
            get { return (bool)GetValue (IsPasswordProperty); }
            set { SetValue (IsPasswordProperty,value); }
        }
    }
}

