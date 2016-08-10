// ***********************************************************************
// Assembly         : XLabs.Platform.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="MediaPickerController.cs" company="XLabs Team">
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
using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace XLabs.Platform.Services.Media
{
	/// <summary>
	/// Class MediaPickerController. This class cannot be inherited.
	/// </summary>
	public sealed class MediaPickerController : UIImagePickerController
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MediaPickerController"/> class.
		/// </summary>
		/// <param name="mpDelegate">The mp delegate.</param>
		internal MediaPickerController(MediaPickerDelegate mpDelegate)
		{
			base.Delegate = mpDelegate;
		}

		/// <summary>
		/// Gets or sets the delegate.
		/// </summary>
		/// <value>The delegate.</value>
		/// <exception cref="NotSupportedException"></exception>
		public override NSObject Delegate
		{
			get
			{
				return base.Delegate;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Gets the result asynchronous.
		/// </summary>
		/// <returns>Task&lt;MediaFile&gt;.</returns>
		public Task<MediaFile> GetResultAsync()
		{
			return ((MediaPickerDelegate)Delegate).Task;
		}
	}
}