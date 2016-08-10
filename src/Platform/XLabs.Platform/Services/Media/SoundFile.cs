// ***********************************************************************
// Assembly         : XLabs.Platform
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="SoundFile.cs" company="XLabs Team">
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

namespace XLabs.Platform.Services.Media
{
	/// <summary>
	/// Class SoundFile.
	/// </summary>
	public class SoundFile
	{
		/// <summary>
		/// Gets or sets the filename.
		/// </summary>
		/// <value>The filename.</value>
		public string Filename { get; set; }

		/// <summary>
		/// Gets or sets the duration.
		/// </summary>
		/// <value>The duration.</value>
		public TimeSpan Duration { get; set; }
	}
}