// ***********************************************************************
// Assembly         : XLabs.Ioc.Ninject
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="NinjectContainer.cs" company="XLabs Team">
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
using System.Diagnostics.CodeAnalysis;
using Ninject;

namespace XLabs.Ioc.Ninject
{
    /// <summary>
    /// The Ninject container.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class NinjectContainer : IDependencyContainer
    {
        private readonly IKernel kernel;
        private readonly IResolver resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectContainer"/> class.
        /// </summary>
        /// <param name="kernel">
        /// The kernel.
        /// </param>
        public NinjectContainer(IKernel kernel)
        {
            this.kernel = kernel;
            this.resolver = new NinjectResolver(kernel);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectContainer"/> class with <see cref="StandardKernel"/>.
        /// </summary>
        public NinjectContainer() : this(new StandardKernel())
        {
        }

        /// <summary>
        /// Gets the resolver from the container.
        /// </summary>
        /// <returns>An instance of <see cref="IResolver"/></returns>
        public IResolver GetResolver()
        {
            return this.resolver;
        }

        /// <summary>
        /// Registers an instance of T to be stored in the container.
        /// </summary>
        /// <typeparam name="T">Type of instance.</typeparam>
        /// <param name="instance">Instance of type T.</param>
        /// <returns>An instance of <see cref="IDependencyContainer"/></returns>
        public IDependencyContainer Register<T>(T instance) where T : class
        {
            this.kernel.Bind<T>().ToConstant<T>(instance);
            return this;
        }

        /// <summary>
        /// Registers a type to instantiate for type T.
        /// </summary>
        /// <typeparam name="T">Type of instance.</typeparam>
        /// <typeparam name="TImpl">Type to register for instantiation.</typeparam>
        /// <returns>An instance of <see cref="IDependencyContainer"/></returns>
        public IDependencyContainer Register<T, TImpl>()
            where T : class
            where TImpl : class, T
        {
            if (typeof (T) == typeof (TImpl))
            {
                this.kernel.Bind<T>().ToSelf();
            }
            else
            {
                this.kernel.Bind<T>().To<TImpl>();
            }

            return this;
        }

        /// <summary>
        /// Registers a type to instantiate for type T as singleton.
        /// </summary>
        /// <typeparam name="T">Type of instance.</typeparam>
        /// <typeparam name="TImpl">Type to register for instantiation.</typeparam>
        /// <returns>An instance of <see cref="IDependencyContainer"/></returns>
        public IDependencyContainer RegisterSingle<T, TImpl>()
            where T : class
            where TImpl : class, T
        {
            this.kernel.Bind<T>().To<TImpl>().InSingletonScope();
            return this;
        }

        /// <summary>
        /// Tries to register a type.
        /// </summary>
        /// <typeparam name="T">Type of instance.</typeparam>
        /// <param name="type">Type of implementation.</param>
        /// <returns>An instance of <see cref="IDependencyContainer"/></returns>
        public IDependencyContainer Register<T>(Type type) where T : class
        {
            this.kernel.Bind<T>().To(type);
            return this;
        }

        /// <summary>
        /// Tries to register a type.
        /// </summary>
        /// <param name="type">Type to register.</param>
        /// <param name="impl">Type that implements registered type.</param>
        /// <returns>An instance of <see cref="IDependencyContainer"/></returns>
        public IDependencyContainer Register(Type type, Type impl)
        {
            this.kernel.Bind(type).To(impl);
            return this;
        }

        /// <summary>
        /// Registers a function which returns an instance of type T.
        /// </summary>
        /// <typeparam name="T">Type of instance.</typeparam>
        /// <param name="func">Function which returns an instance of T.</param>
        /// <returns>An instance of <see cref="IDependencyContainer"/></returns>
        public IDependencyContainer Register<T>(Func<IResolver, T> func) where T : class
        {
            this.kernel.Bind<T>().ToMethod<T>(t => func(this.resolver));
            return this;
        }
    }
}
