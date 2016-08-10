// ***********************************************************************
// Assembly         : XLabs.Platform
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="MediaFileNoteFoundException.cs" company="XLabs Team">
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
	/// Class MediaFileNotFoundException.
	/// </summary>
	public class MediaFileNotFoundException
		: Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MediaFileNotFoundException" /> class.
		/// </summary>
		/// <param name="path">The path.</param>
		public MediaFileNotFoundException(string path)
			: base("Unable to locate media file at " + path)
		{
			Path = path;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MediaFileNotFoundException" /> class.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="innerException">The inner exception.</param>
		public MediaFileNotFoundException(string path, Exception innerException)
			: base("Unable to locate media file at " + path, innerException)
		{
			Path = path;
		}

		/// <summary>
		/// Gets the path.
		/// </summary>
		/// <value>The path.</value>
		public string Path
		{
			get;
			private set;
		}
	}
}
