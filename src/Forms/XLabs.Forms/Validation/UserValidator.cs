// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="UserValidator.cs" company="XLabs Team">
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
	/// <summary>
	/// Class UserValidator.
	/// </summary>
	internal class UserValidator : ValidatorPredicate
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ValidatorPredicate" /> class.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="priority">The priority.</param>
		/// <param name="eval">The eval.</param>
		public UserValidator(Validators id, PredicatePriority priority, Func<Rule, string, bool> eval) : base(id, priority, eval) {
		}

		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>The name of the user.</value>
		public string UserName { get; set; }
	}
}