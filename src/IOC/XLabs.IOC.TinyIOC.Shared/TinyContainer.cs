// ***********************************************************************
// Assembly         : XLabs.Ioc.TinyIOC.WP8
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="TinyContainer.cs" company="XLabs Team">
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
using TinyIoC;

namespace XLabs.Ioc.TinyIOC
{
    /// <summary>
    /// The tiny container wrapper
    /// Allows registering a tinyioc container with the IDependencyContainer interface
    /// </summary>
    public class TinyContainer : IDependencyContainer
    {
        /// <summary>
        /// The container.
        /// </summary>
        private readonly TinyIoCContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyContainer"/> class.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        public TinyContainer(TinyIoCContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Gets the resolver from the container
        /// </summary>
        /// <returns>An instance of <see cref="IResolver"/></returns>
        public IResolver GetResolver()
        {
            return new TinyResolver(this.container);
        }

        /// <summary>
        /// Registers an instance of T to be stored in the container.
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        /// <param name="instance">Instance of type T.</param>
        /// <returns>An instance of <see cref="IDependencyContainer"/></returns>
        public IDependencyContainer Register<T>(T instance) where T : class
        {
            this.container.Register<T>(instance);
            return this;
        }

        /// <summary>
        /// Registers a type to instantiate for type T.
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        /// <typeparam name="TImpl">Type to register for instantiation.</typeparam>
        /// <returns>An instance of <see cref="IDependencyContainer"/></returns>
        public IDependencyContainer Register<T, TImpl>() where T : class where TImpl : class, T
        {
            this.container.Register<T, TImpl>();
            return this;
        }

        /// <summary>
        /// Registers a type to instantiate for type T as singleton.
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        /// <typeparam name="TImpl">Type to register for instantiation.</typeparam>
        /// <returns>An instance of <see cref="IDependencyContainer"/></returns>
        public IDependencyContainer RegisterSingle<T, TImpl>() where T : class where TImpl : class, T
        {
            this.container.Register<T, TImpl>().AsSingleton();
            return this;
        }

        /// <summary>
        /// Tries to register a type
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        /// <param name="type">Type of implementation</param>
        /// <returns>An instance of <see cref="IDependencyContainer"/></returns>
        public IDependencyContainer Register<T>(Type type) where T : class
        {
            this.container.Register(type);
            return this;
        }

        /// <summary>
        /// Tries to register a type
        /// </summary>
        /// <param name="type">Type to register.</param>
        /// <param name="impl">Type that implements registered type.</param>
        /// <returns>An instance of <see cref="IDependencyContainer"/></returns>
        public IDependencyContainer Register(Type type, Type impl)
        {
            this.container.Register(type, impl);
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
            Func<TinyIoCContainer,NamedParameterOverloads,T> f;

            f = (c, t) => func(new TinyResolver(c));

            this.container.Register<T>(f);
            return this;
        }
    }
}
