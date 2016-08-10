// ***********************************************************************
// Assembly         : XLabs.Platform
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="SoundFinishedEventArgs.cs" company="XLabs Team">
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
	/// Class SoundFinishedEventArgs.
	/// </summary>
	public class SoundFinishedEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SoundFinishedEventArgs"/> class.
		/// </summary>
		/// <param name="f">The f.</param>
		public SoundFinishedEventArgs(SoundFile f)
		{
			File = f;
		}

		/// <summary>
		/// Gets or sets the file.
		/// </summary>
		/// <value>The file.</value>
		public SoundFile File { get; set; }
	}
}