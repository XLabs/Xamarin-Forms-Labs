// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ExtendedTableView.cs" company="XLabs Team">
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
using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class ExtendedTableView.
	/// </summary>
	public class ExtendedTableView : TableView
	{
		/// <summary>
		/// Occurs when [data changed].
		/// </summary>
		public event EventHandler<EventArgs> DataChanged;

		/// <summary>
		/// Called when [data changed].
		/// </summary>
		public void OnDataChanged()
		{
			var handler = this.DataChanged;
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}
	}
}