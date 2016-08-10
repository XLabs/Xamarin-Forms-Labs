// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="InvalidBindableException.cs" company="XLabs Team">
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
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace XLabs.Forms.Exceptions
{
	/// <summary>
	/// Thrown when an invalid bindable object has been passed to a callback
	/// </summary>
	public class InvalidBindableException : Exception
	{

		/// <summary>
		/// Hide any possible default constructor
		/// Redundant I know, but it costs nothing
		/// and communicates the design intent to
		/// other developers.
		/// </summary>
		private InvalidBindableException() { }

		/// <summary>
		/// Constructs the exception and passes a meaningful
		/// message to the base Exception
		/// </summary>
		/// <param name="bindable">The bindable object that was passed</param>
		/// <param name="expected">The expected type</param>
		/// <param name="name">The calling methods name, uses [CallerMemberName]</param>
		public InvalidBindableException(BindableObject bindable, Type expected,[CallerMemberName]string name=null) 
			: base(string.Format("Invalid bindable passed to {0} expected a {1} received a {2}", name, expected.Name, bindable.GetType().Name))
		{
		}

		/// <summary>
		/// The bindable object that was passed
		/// </summary>
		public BindableObject IncorrectBindableObject { get; set; }
		/// <summary>
		/// The expected type of the bindable object
		/// </summary>
		public Type ExpectedType { get; set; }
	}
}
