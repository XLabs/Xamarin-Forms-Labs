// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="IFontManager.cs" company="XLabs Team">
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

using System.Collections.Generic;
using Xamarin.Forms;

namespace XLabs.Forms.Services
{
	/// <summary>
	/// Interface IFontManager
	/// </summary>
	public interface IFontManager
	{
		/// <summary>
		/// Gets all available system fonts.
		/// </summary>
		IEnumerable<string> AvailableFonts { get; }

		/// <summary>
		/// Gets height for the font.
		/// </summary>
		/// <param name="font">Font whose height is calculated.</param>
		/// <returns>Height of the font in inches.</returns>
		double GetHeight(Font font);

		/// <summary>
		/// Finds the closest font to the desired height.
		/// </summary>
		/// <param name="name">Name of the font.</param>
		/// <param name="desiredHeight">Desired height in inches.</param>
		/// <returns></returns>
		Font FindClosest(string name, double desiredHeight);
	}
}
