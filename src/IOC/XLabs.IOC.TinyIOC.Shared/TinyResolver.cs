// ***********************************************************************
// Assembly         : XLabs.Ioc.TinyIOC
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="TinyResolver.cs" company="XLabs Team">
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
using TinyIoC;

namespace XLabs.Ioc.TinyIOC
{
	/// <summary>
	/// The tiny resolver.
	/// </summary>
	public class TinyResolver : IResolver
	{
		private readonly TinyIoCContainer container;

		/// <summary>
		/// Initializes a new instance of the <see cref="TinyResolver"/> class.
		/// </summary>
		/// <param name="container">
		/// The container.
		/// </param>
		public TinyResolver(TinyIoCContainer container)
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
				return this.container.Resolve<T>();
			}
			catch (TinyIoCResolutionException ex)
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
				return this.container.Resolve(type);
			}
			catch (TinyIoCResolutionException ex)
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
			return this.container.ResolveAll<T>();
		}

		/// <summary>
		/// Resolve a dependency by type.
		/// </summary>
		/// <param name="type">Type of object.</param>
		/// <returns>All instances of type if found as <see cref="object"/>, otherwise null.</returns>
		public IEnumerable<object> ResolveAll(Type type)
		{
			return this.container.ResolveAll(type);
		}

		/// <summary>
		/// Determines whether the specified type is registered.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns><c>true</c> if the specified type is registered; otherwise, <c>false</c>.</returns>
		public bool IsRegistered(Type type)
		{
			return this.container.CanResolve(type);
		}

		/// <summary>
		/// Determines whether this instance is registered.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns><c>true</c> if this instance is registered; otherwise, <c>false</c>.</returns>
		public bool IsRegistered<T>() where T : class
		{
			return this.container.CanResolve<T>();
		}
		#endregion
	}
}
