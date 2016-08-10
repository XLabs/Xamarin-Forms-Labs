// ***********************************************************************
// Assembly         : XLabs.Forms
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="ValidationSets.cs" company="XLabs Team">
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

using System.Collections.ObjectModel;

namespace XLabs.Forms.Validation
{
	/// <summary>
	///     A Validation Set succeds or fails entirely
	///     If it succeeds then all Valid properties from
	///     the actions are applied.  If it fails
	///     then all InValid properties are applied
	/// </summary>
	/// Element created at 07/11/2014,6:11 AM by Charles
	public class ValidationSets : ObservableCollection<RuleSet>
	{
	}
}