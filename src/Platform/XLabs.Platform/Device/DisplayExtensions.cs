// ***********************************************************************
// Assembly         : XLabs.Platform
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="DisplayExtensions.cs" company="XLabs Team">
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

namespace XLabs.Platform.Device
{
	/// <summary>
	/// Class DisplayExtensions.
	/// </summary>
	public static class DisplayExtensions
	{
		/// <summary>
		/// Screens the size inches.
		/// </summary>
		/// <param name="screen">The screen.</param>
		/// <returns>System.Double.</returns>
		public static double ScreenSizeInches(this IDisplay screen)
		{
			return Math.Sqrt(Math.Pow(screen.ScreenWidthInches(), 2) + Math.Pow(screen.ScreenHeightInches(), 2));
		}

		/// <summary>
		/// Screens the width inches.
		/// </summary>
		/// <param name="screen">The screen.</param>
		/// <returns>System.Double.</returns>
		public static double ScreenWidthInches(this IDisplay screen)
		{
			return screen.Width / screen.Xdpi;
		}

		/// <summary>
		/// Screens the height inches.
		/// </summary>
		/// <param name="screen">The screen.</param>
		/// <returns>System.Double.</returns>
		public static double ScreenHeightInches(this IDisplay screen)
		{
			return screen.Height / screen.Ydpi;
		}
	}
}

