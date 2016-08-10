// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ValidateEmailAddress.cs" company="XLabs Team">
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
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace XLabs.Forms.Validation
{
	/// <summary>
	/// Class ValidateEmailAddress.
	/// </summary>
	internal class ValidateEmailAddress : ValidatorPredicate
	{
		#region Static Fields

		/// <summary>
		/// The email address
		/// </summary>
		private static readonly Regex EmailAddress =
			new Regex(
				@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))"
				+ @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ValidateEmailAddress"/> class.
		/// </summary>
		public ValidateEmailAddress() : base(Validators.Email, PredicatePriority.Low, IsEmailAddress) { }

		#endregion

		#region Methods

		/// <summary>
		/// Determines whether [is email address] [the specified rule].
		/// </summary>
		/// <param name="rule">The rule.</param>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if [is email address] [the specified rule]; otherwise, <c>false</c>.</returns>
		private static bool IsEmailAddress(Rule rule, string value)
		{
			try
			{
				return string.IsNullOrEmpty(value) || EmailAddress.IsMatch(value);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				throw;
			}
		}

		#endregion
	}
}