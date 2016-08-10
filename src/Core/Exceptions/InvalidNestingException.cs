// ***********************************************************************
// Assembly         : XLabs.Core
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="InvalidNestingException.cs" company="XLabs Team">
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
using System.Linq;

namespace XLabs.Exceptions
{
	/// <summary>
	/// Thrown when an element or attached property is not withing the 
	/// proper container
	/// </summary>
	public class InvalidNestingException : Exception
	{
		/// <summary>
		/// Hide any possible default constructor
		/// Redundant I know, but it costs nothing
		/// and communicates the design intent to
		/// other developers.
		/// </summary>
		private InvalidNestingException() { }

		/// <summary>
		/// Constructs the exception and passes a meaningful
		/// message to the base Exception
		/// </summary>
		/// <param name="nestedType">The inner type</param>
		/// <param name="expectedContainer">The container type that was expected</param>
		/// <param name="history">All parents considered in the search</param>
		public InvalidNestingException(Type nestedType, Type expectedContainer, List<string> history)
			: base(string.Format("{0} must be contained within a {1} (or a subclass).\nSearch History:{2}",nestedType.Name,expectedContainer.Name,history.Aggregate((s1, s2) => s1 + "," + s2)))
		{
			NestedType = nestedType;
			ExpectedContainer = expectedContainer;
			NestedName = nestedType.Name;
			ExpectedContainerName = expectedContainer.Name;
			SearchPath = history;
		}
		/// <summary>
		/// All parents considered in the search
		/// </summary>
		public IEnumerable<string> SearchPath { get; set; }
		/// <summary>
		/// The actual type of inner object
		/// </summary>
		public Type NestedType { get; set; }
		/// <summary>
		/// The expected type of the container
		/// </summary>
		public Type ExpectedContainer { get; set; }
		/// <summary>
		/// The name of the inner object
		/// </summary>
		public string NestedName { get; set; }
		/// <summary>
		/// The name of the expected container
		/// </summary>
		public string ExpectedContainerName { get; set; }
	}
}
