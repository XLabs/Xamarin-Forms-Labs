// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="Validators.cs" company="XLabs Team">
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

namespace XLabs.Forms.Validation
{
	/// <summary>The set of available validators</summary>
	/// Element created at 07/11/2014,2:56 AM by Charles
	[Flags]
	public enum Validators : ulong
	{
		/// <summary>No validators</summary>
		/// Element created at 07/11/2014,2:55 AM by Charles
		None = 0x0000000000000000,

		/// <summary>A value must be present</summary>
		/// Element created at 07/11/2014,2:56 AM by Charles
		Required = 0x0000000000000001,

		/// <summary>The string value must be a valid email address</summary>
		/// Element created at 07/11/2014,2:56 AM by Charles
		Email = 0x0000000000000002,

		/// <summary>The minimum numeric value</summary>
		/// Element created at 07/11/2014,2:57 AM by Charles
		GreaterThan = 0x0000000000000004,

		/// <summary>The maximum numeric value</summary>
		/// Element created at 07/11/2014,2:57 AM by Charles
		LessThan = 0x0000000000000008,

		/// <summary>A regex pattern that must be matched</summary>
		/// Element created at 07/11/2014,2:58 AM by Charles
		Pattern = 0x0000000000000010,

		/// <summary>The numeric value is between <see cref="Rule.Minimum" /> and <see cref="Rule.Maximum" />/></summary>
		/// Element created at 07/11/2014,10:41 AM by Charles
		Between = 0x0000000000000020,

		/// <summary>Calls a user supplied predicate to validate the value</summary>
		/// Element created at 07/11/2014,10:49 AM by Charles
		Predicate = 0x0000000000000040,

		/// <summary>Verifies the value is a datetime</summary>
		/// Element created at 07/11/2014,11:07 AM by Charles
		DateTime = 0x0000000000000080,

		/// <summary>Verifies the value is numeric</summary>
		/// Element created at 07/11/2014,11:17 AM by Charles
		Numeric = 0x0000000000000100,

		/// <summary>Verifies the value is an integer</summary>
		/// Element created at 07/11/2014,11:21 AM by Charles
		Integer = 0x0000000000000200,

		/// <summary>Verifies the minimum string length of the property</summary>
		/// Element created at 07/11/2014,3:52 PM by Charles
		MinLength = 0x0000000000000400,

		/// <summary>Verifies the maximum string length of the property</summary>
		/// Element created at 07/11/2014,3:52 PM by Charles
		MaxLength = 0x0000000000000800,

		/// <summary>Allows letters only (Unicode support)</summary>
		/// Element created at 07/11/2014,3:56 PM by Charles
		AlphaOnly = 0x0000000000001000,

		/// <summary>Allows letters and numbers (Unicode support)</summary>
		/// Element created at 07/11/2014,3:57 PM by Charles
		AlphaNumeric = 0x0000000000002000,

		/// <summary>Allows only numbers</summary>
		/// Element created at 09/11/2014,9:50 PM by Charles
		NumericOnly =  0x0000000000004000,

		/// <summary>A user created Validator</summary>
		/// Element created at 08/11/2014,2:44 PM by Charles
		UserSupplied = 0x8000000000000000
	}
}