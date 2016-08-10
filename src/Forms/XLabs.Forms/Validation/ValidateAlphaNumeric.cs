// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ValidateAlphaNumeric.cs" company="XLabs Team">
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

using System.Text.RegularExpressions;

namespace XLabs.Forms.Validation
{
	/// <summary>
	/// Class ValidateAlphaNumeric.
	/// </summary>
	internal class ValidateAlphaNumeric : ValidatorPredicate
	{
		#region Static Fields

		/// <summary>
		/// The alpha numeric
		/// </summary>
		private static readonly Regex AlphaNumeric = new Regex(@"^[\p{L}\p{N}]*$");

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ValidateAlphaNumeric"/> class.
		/// </summary>
		public ValidateAlphaNumeric() : base(Validators.AlphaOnly, PredicatePriority.Low, IsAlphaNumeric) { }

		#endregion

		#region Methods

		/// <summary>
		/// Determines whether [is alpha numeric] [the specified rule].
		/// </summary>
		/// <param name="rule">The rule.</param>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if [is alpha numeric] [the specified rule]; otherwise, <c>false</c>.</returns>
		private static bool IsAlphaNumeric(Rule rule, string value)
		{
			return string.IsNullOrEmpty(value) || AlphaNumeric.IsMatch(value);
		}

		#endregion
	}
}