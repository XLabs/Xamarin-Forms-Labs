// ***********************************************************************
// Assembly         : XLabs.Ioc.Ninject
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="NinjectResolver.cs" company="XLabs Team">
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
using System.Diagnostics.CodeAnalysis;
using Ninject;

namespace XLabs.Ioc.Ninject
{
	/// <summary>
	/// The Ninject resolver.
	/// </summary>
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
	public class NinjectResolver : IResolver
	{
		private readonly IKernel container;

		/// <summary>
		/// Initializes a new instance of the <see cref="NinjectResolver"/> class.
		/// </summary>
		/// <param name="container">
		/// The kernel.
		/// </param>
		public NinjectResolver(IKernel container)
		{
			this.container = container;
		}

		#region IResolver Members

		/// <summary>
		/// Resolve a dependency.
		/// </summary>
		/// <typeparam name="T">Type of instance to get.</typeparam>
		/// <returns>An instance of {T} if successful, otherwise null.</returns>
		public T Resolve<T>() where T : class
		{
			try
			{
				return this.container.Get<T>();
			}
			catch (ActivationException ex)
			{
				if (ex.InnerException != null)
				{
					throw ex.InnerException;
				}

				return null;
			}
		}
			

		/// <summary>
		/// Resolve a dependency by type.
		/// </summary>
		/// <param name="type">Type of object.</param>
		/// <returns>An instance to type if found as <see cref="object"/>, otherwise null.</returns>
		public object Resolve(Type type)
		{
			try
			{
				return this.container.Get(type);
			}
			catch (ActivationException ex)
			{
				if (ex.InnerException != null)
				{
					throw ex.InnerException;
				}

				return null;
			}
		}

		/// <summary>
		/// Resolve a dependency.
		/// </summary>
		/// <typeparam name="T">Type of instance to get.</typeparam>
		/// <returns>All instances of {T} if successful, otherwise null.</returns>
		public IEnumerable<T> ResolveAll<T>() where T : class
		{
			return this.container.GetAll<T>();
		}

		/// <summary>
		/// Resolve a dependency by type.
		/// </summary>
		/// <param name="type">Type of object.</param>
		/// <returns>All instances of type if found as <see cref="object"/>, otherwise null.</returns>
		public IEnumerable<object> ResolveAll(Type type)
		{
			return this.container.GetAll(type);
		}

		/// <summary>
		/// Determines whether the specified type is registered.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns><c>true</c> if the specified type is registered; otherwise, <c>false</c>.</returns>
		public bool IsRegistered(Type type)
		{
			return this.Resolve(type) != null;
		}

		/// <summary>
		/// Determines whether this instance is registered.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns><c>true</c> if this instance is registered; otherwise, <c>false</c>.</returns>
		public bool IsRegistered<T>() where T : class
		{
			return this.Resolve<T>() != null;
		}
		#endregion
	}
}
