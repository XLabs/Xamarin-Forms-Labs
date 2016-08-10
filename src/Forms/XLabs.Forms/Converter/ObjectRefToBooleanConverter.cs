// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ObjectRefToBooleanConverter.cs" company="XLabs Team">
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
using System.Globalization;
using Xamarin.Forms;

namespace XLabs.Forms.Converter
{
	/// <summary>
	/// Converts an objectreference to a boolean
	/// </summary>
	/// Element created at 05/11/2014,9:08 AM by Charles
	public class ObjectRefToBooleanConverter : IValueConverter
	{
		/// <summary>
		/// Converts an objectreferene to a boolean/&gt;.
		/// </summary>
		/// <param name="value">An objectreference</param>
		/// <param name="targetType">boolean</param>
		/// <param name="parameter">not used</param>
		/// <param name="culture">not used</param>
		/// <returns>True if the <see cref="System.Object" /> is not null, false otherwise</returns>
		/// Element created at 05/11/2014,9:09 AM by Charles
		/// <remarks>To be added.</remarks>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var reverse = parameter != null && parameter.ToString().ToLower() == "invert";

			return reverse ? value==null : value != null;
		}

		/// <summary>
		/// Not Implmented, this is a one way converter
		/// </summary>
		/// <param name="value">To be added.</param>
		/// <param name="targetType">To be added.</param>
		/// <param name="parameter">To be added.</param>
		/// <param name="culture">To be added.</param>
		/// <returns>To be added.</returns>
		/// Element created at 05/11/2014,9:11 AM by Charles
		/// <exception cref="System.NotImplementedException"></exception>
		/// <remarks>To be added.</remarks>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			//Not used
			throw new NotImplementedException();
		}
	}
}
