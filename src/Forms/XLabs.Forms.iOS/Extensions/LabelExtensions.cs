// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="LabelExtensions.cs" company="XLabs Team">
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

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XLabs.Platform.Extensions;

namespace XLabs.Forms.Extensions
{
	/// <summary>
	/// Class LabelExtensions.
	/// </summary>
	public static class LabelExtensions
	{
		/// <summary>
		/// Adjusts the height.
		/// </summary>
		/// <param name="label">The label.</param>
		public static void AdjustHeight(this Label label)
		{
			label.HeightRequest = label.Text.StringHeight(label.Font.ToUIFont(), (float)label.Width);
		}
	}
}

