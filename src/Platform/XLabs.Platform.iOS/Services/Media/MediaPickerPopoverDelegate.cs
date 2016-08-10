// ***********************************************************************
// Assembly         : XLabs.Platform.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="MediaPickerPopoverDelegate.cs" company="XLabs Team">
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

using UIKit;

namespace XLabs.Platform.Services.Media
{
	/// <summary>
	/// Class MediaPickerPopoverDelegate.
	/// </summary>
	internal class MediaPickerPopoverDelegate : UIPopoverControllerDelegate
	{
		/// <summary>
		/// The _picker
		/// </summary>
		private readonly UIImagePickerController _picker;

		/// <summary>
		/// The _picker delegate
		/// </summary>
		private readonly MediaPickerDelegate _pickerDelegate;

		/// <summary>
		/// Initializes a new instance of the <see cref="MediaPickerPopoverDelegate"/> class.
		/// </summary>
		/// <param name="pickerDelegate">The picker delegate.</param>
		/// <param name="picker">The picker.</param>
		internal MediaPickerPopoverDelegate(MediaPickerDelegate pickerDelegate, UIImagePickerController picker)
		{
			_pickerDelegate = pickerDelegate;
			_picker = picker;
		}

		/// <summary>
		/// Shoulds the dismiss.
		/// </summary>
		/// <param name="popoverController">The popover controller.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public override bool ShouldDismiss(UIPopoverController popoverController)
		{
			return true;
		}

		/// <summary>
		/// Dids the dismiss.
		/// </summary>
		/// <param name="popoverController">The popover controller.</param>
		public override void DidDismiss(UIPopoverController popoverController)
		{
			_pickerDelegate.Canceled(_picker);
		}
	}
}