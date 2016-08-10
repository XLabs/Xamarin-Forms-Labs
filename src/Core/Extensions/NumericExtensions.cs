// ***********************************************************************
// Assembly         : XLabs.Core
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="NumericExtensions.cs" company="XLabs Team">
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

namespace XLabs
{
	/// <summary>
	/// Class NumericExtensions.
	/// </summary>
	public static class NumericExtensions
	{
		/// <summary>
		/// Clamps the specified self.
		/// </summary>
		/// <param name="self">The self.</param>
		/// <param name="min">The minimum.</param>
		/// <param name="max">The maximum.</param>
		/// <returns>System.Double.</returns>
		public static double Clamp(this double self, double min, double max)
		{
			return Math.Min(max, Math.Max(self, min));
		}

		/// <summary>
		/// Clamps the specified self.
		/// </summary>
		/// <param name="self">The self.</param>
		/// <param name="min">The minimum.</param>
		/// <param name="max">The maximum.</param>
		/// <returns>System.Int32.</returns>
		public static int Clamp(this int self, int min, int max)
		{
			return Math.Min(max, Math.Max(self, min));
		}
	}
}

