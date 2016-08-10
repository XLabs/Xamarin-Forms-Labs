// ***********************************************************************
// Assembly         : XLabs.Platform.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="IntentFilterExtensions.cs" company="XLabs Team">
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

using Android.App;
using Android.Content;

namespace XLabs.Platform
{
	/// <summary>
	/// Intent filter extensions.
	/// </summary>
	public static class IntentFilterExtensions
	{
		/// <summary>
		/// Gets a single result for the intent filter using <see cref="Application.Context"/>
		/// </summary>
		/// <param name="intentFilter">Intent filter</param>
		/// <returns>An intent result, null if not successful</returns>
		public static Intent RegisterReceiver(this IntentFilter intentFilter)
		{
			return Application.Context.RegisterReceiver(null, intentFilter);
		}
	}
}