// ***********************************************************************
// Assembly         : XLabs.Forms.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="NoCaretField.cs" company="XLabs Team">
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

using CoreGraphics;
using UIKit;

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class NoCaretField.
	/// </summary>
	public class NoCaretField : UITextField
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NoCaretField"/> class.
		/// </summary>
		public NoCaretField() : base(default(CGRect))
		{
		}

		/// <summary>
		/// Gets the caret rect for position.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <returns>RectangleF.</returns>
		public override CGRect GetCaretRectForPosition(UITextPosition position)
		{
			return default(CGRect);
		}
	}
}

