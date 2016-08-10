// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="BindableObjectExtensions.cs" company="XLabs Team">
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

namespace XLabs.Forms
{
	/// <summary>
	/// Class BindableObjectExtensions.
	/// </summary>
	public static class BindableObjectExtensions
	{
		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="bindableObject">The bindable object.</param>
		/// <param name="property">The property.</param>
		/// <returns>T.</returns>
		public static T GetValue<T>(this BindableObject bindableObject, BindableProperty property)
		{
			return (T)bindableObject.GetValue(property);
		}
	}
}
