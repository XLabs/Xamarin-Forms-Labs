// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="GestureResult.cs" company="XLabs Team">
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
using System.Collections.Generic;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace XLabs.Forms.Behaviors
{
	/// <summary>
	/// Geture result sent along with the Paramater for 
	/// in a Gesture Exectue call.
	/// Also the EventArgs type for OnGesture
	/// </summary>
	public class GestureResult
	{
		/// <summary>
		/// The gesture type
		/// </summary>
		public GestureType GestureType { get; internal set; }
		/// <summary>
		/// The direction (if any) of the direction
		/// </summary>
		public Directionality Direction { get;  internal set; }
		/// <summary>
		/// The point, relative to the start view where the 
		/// gesture started
		/// </summary>
		public Point Origin { get; internal set; }
		/// <summary>
		/// The point, relative to the start view where the second finger of the
		/// gesture is located (valid for GestureType.Pinch)
		/// </summary>
		public Point Origin2 { get; internal set; }
		/// <summary>
		/// The view that the gesture started in
		/// </summary>
		public View StartView { get; internal set; }
		/// <summary>
		/// The Vector Length of the gesture (if appropiate)
		/// </summary>
		public Double Length { get; set; }
		/// <summary>
		/// The Vertical distance the gesture travelled
		/// </summary>
		internal Double VerticalDistance { get; set; }
		/// <summary>
		/// The horizontal distance the gesture travelled
		/// </summary>
		internal Double HorizontalDistance { get; set; }

		/// <summary>Gets or sets the view stack.</summary>
		/// <value>A list of all view elements containing the origin point.</value>
		/// Element created at 07/11/2014,11:54 PM by Charles
		internal List<View> ViewStack { get; set; }

	}
}
