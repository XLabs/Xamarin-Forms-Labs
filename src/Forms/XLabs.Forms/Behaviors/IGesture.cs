// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="IGesture.cs" company="XLabs Team">
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
namespace XLabs.Forms.Behaviors
{
	/// <summary>
	/// Interface implmenented to consume gestures
	/// analagous to ICommand
	/// </summary>
	public interface IGesture
	{
		/// <summary>
		/// Execute the gesture
		/// </summary>
		/// <param name="result">The <see cref="GestureResult"/></param>
		/// <param name="param">the user supplied paramater</param>
		void ExecuteGesture(GestureResult result, object param);
		/// <summary>
		/// Checks to see if the gesture should execute
		/// </summary>
		/// <param name="result">The <see cref="GestureResult"/></param>
		/// <param name="param">The user supplied parameter</param>
		/// <returns>True to execute the gesture, False otherwise</returns>
		bool CanExecuteGesture(GestureResult result, object param);
	}
}
