// ***********************************************************************
// Assembly         : XLabs.Platform.Droid
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="UriExtensions.cs" company="XLabs Team">
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

namespace XLabs.Platform
{
	/// <summary>
	/// Class UriExtensions.
	/// </summary>
	public static class UriExtensions
	{
		/// <summary>
		/// To the android URI.
		/// </summary>
		/// <param name="uri">The URI.</param>
		/// <returns>Android.Net.Uri.</returns>
		public static Android.Net.Uri ToAndroidUri(this Uri uri)
		{
			return Android.Net.Uri.Parse(uri.AbsoluteUri);
		}

		/// <summary>
		/// To the system URI.
		/// </summary>
		/// <param name="uri">The URI.</param>
		/// <returns>Uri.</returns>
		public static Uri ToSystemUri(this Android.Net.Uri uri)
		{
			return new Uri(uri.ToString());
		}
	}
}