// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="GestureInterest.cs" company="XLabs Team">
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

using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace XLabs.Forms.Behaviors
{
	/// <summary>
	/// Attaches an interest in a gesture to a View
	/// <see cref="Gestures"/> and <see cref="GesturesContentView"/>
	/// </summary>
	public class GestureInterest : BindableObject
	{
		/// <summary>
		/// The property definition for <see cref="Notification"/>
		/// </summary>
		public static BindableProperty NotifcationProperty = BindableProperty.Create<GestureInterest, GestureNotification>(x => x.Notification, GestureNotification.None);
		/// <summary>
		/// The property defintion for <see cref="GestureType"/>
		/// </summary>
		public static BindableProperty GestureTypeProperty =BindableProperty.Create<GestureInterest, GestureType>(x => x.GestureType, GestureType.Unknown);
		/// <summary>
		/// The property definitionf for <see cref="Direction"/>
		/// </summary>
		public static BindableProperty DirectionProperty = BindableProperty.Create<GestureInterest, Directionality>(x => x.Direction, Directionality.None);

		/// <summary>
		/// The property definition for <see cref="GestureCommand"/>
		/// </summary>
		public static BindableProperty GestureCommandProperty =BindableProperty.Create<GestureInterest, IGesture>(x => x.GestureCommand,default(IGesture));
		/// <summary>
		/// The property definition for <see cref="GestureParameter"/>
		/// </summary>
		public static BindableProperty GestureParameterProperty =BindableProperty.Create<GestureInterest, object>(x => x.GestureParameter, default(object));

		/// <summary>
		/// The notification to use with this gesture
		/// </summary>
		public GestureNotification Notification
		{
			get { return (GestureNotification)GetValue(NotifcationProperty); }
			set { SetValue(NotifcationProperty,value);}
		}
		/// <summary>
		/// The Gesture type you are interested in <see cref="GestureType"/>
		/// </summary>
		public GestureType GestureType
		{
			get { return (GestureType)GetValue(GestureTypeProperty); }
			set { SetValue(GestureTypeProperty,value);}
		}

		/// <summary>
		/// The Direction (if appropiate) <see cref="Directionality"/>
		/// </summary>
		public Directionality Direction
		{
			get { return (Directionality)GetValue(DirectionProperty); }
			set { SetValue(DirectionProperty,value);}
		}

		/// <summary>
		/// The implementation of <see cref="IGesture"/>
		/// </summary>
		public IGesture GestureCommand
		{
			get { return (IGesture)GetValue(GestureCommandProperty); }
			set { SetValue(GestureCommandProperty,value);}
		}

		/// <summary>
		/// An optional paramater passed to <see cref="IGesture.ExecuteGesture"/>
		/// and <see cref="IGesture.CanExecuteGesture"/>
		/// </summary>
		public object GestureParameter
		{
			get { return GetValue(GestureParameterProperty); }
			set {SetValue(GestureParameterProperty,value);}
		}
	}
}
