// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ValidateDateTime.cs" company="XLabs Team">
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
using System.Text.RegularExpressions;

namespace XLabs.Forms.Validation
{
	/// <summary>
	/// Class ValidateDateTime.
	/// </summary>
	internal class ValidateDateTime : ValidatorPredicate
	{
		#region Static Fields

		/// <summary>
		/// The long date
		/// </summary>
		private static readonly Regex LongDate = new Regex(@"^\d{8}$");

		/// <summary>
		/// The short date
		/// </summary>
		private static readonly Regex ShortDate = new Regex(@"^\d{6}$");

		#endregion

		#region Constructors and Destructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ValidateDateTime"/> class.
		/// </summary>
		public ValidateDateTime() : base(Validators.DateTime, PredicatePriority.Low, IsDateTime) { }

		#endregion

		#region Methods

		/// <summary>
		/// Determines whether [is date time] [the specified rule].
		/// </summary>
		/// <param name="rule">The rule.</param>
		/// <param name="value">The value.</param>
		/// <returns><c>true</c> if [is date time] [the specified rule]; otherwise, <c>false</c>.</returns>
		private static bool IsDateTime(Rule rule, string value)
		{
			if (string.IsNullOrEmpty(value)) return true;

			value = value.Trim();
			if (ShortDate.Match(value).Success)
			{
				value = value.Substring(0, 2) + "/" + value.Substring(2, 2) + "/"
						+ value.Substring(4, 2);
			}
			if (LongDate.Match(value).Success)
			{
				value = value.Substring(0, 2) + "/" + value.Substring(2, 2) + "/"
						+ value.Substring(4, 4);
			}
			DateTime d;
			return DateTime.TryParse(value, out d);
		}

		#endregion
	}
}