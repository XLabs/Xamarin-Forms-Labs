using System;
using SimpleInjector;

namespace XLabs.Ioc.SimpleInjectorContainer
{
    /// <summary>
    /// The simple injector container.
    /// </summary>
    public class SimpleInjectorContainer : IDependencyContainer
    {
        private readonly Container container;
        private readonly IResolver resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleInjectorContainer"/> class.
        /// </summary>
        public SimpleInjectorContainer()
            : this(new Container())
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleInjectorContainer"/> class.
        /// </summary>
        /// <param name="container">
        /// The container.
        /// </param>
        public SimpleInjectorContainer(Container container)
        {
            this.container = container;
            this.resolver = new SimpleInjectorResolver(this.container);
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
            this.container.RegisterSingle<T>(instance);
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
            this.container.Register<T, TImpl>();
            return this;
        }

        public IDependencyContainer RegisterSingle<T, TImpl>() where T : class where TImpl : class, T
        {
            this.container.RegisterSingle<T, TImpl>();
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
            this.container.Register(typeof(T), type);
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
            this.container.Register<T>(() => func(this.resolver));
            return this;
        }
    }
}
