namespace XLabs.Ioc.Unity
{
    using System;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Dependency container using Unity container.
    /// </summary>
    public class UnityDependencyContainer : IDependencyContainer
    {
        private readonly UnityContainer container;
        private readonly IResolver resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityDependencyContainer"/> class with default UnityContainer.
        /// </summary>
        public UnityDependencyContainer() : this(new UnityContainer())
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityDependencyContainer"/> class.
        /// </summary>
        /// <param name="container">Unity container.</param>
        public UnityDependencyContainer(UnityContainer container)
        {
            this.container = container;
            this.resolver = new UnityResolver(this.container);
        }

        #region IDependencyContainer Members

        /// <summary>
        /// Gets the resolver from the container
        /// </summary>
        /// <returns>An instance of <see cref="IResolver"/></returns>
        public IResolver GetResolver()
        {
            return this.resolver;
        }

        /// <summary>
        /// Registers an instance of T to be stored in the container.
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        /// <param name="instance">Instance of type T.</param>
        /// <returns>An instance of <see cref="IDependencyContainer"/></returns>
        public IDependencyContainer Register<T>(T instance) where T : class
        {
            this.container.RegisterInstance<T>(instance);
            return this;
        }

        /// <summary>
        /// Registers a type to instantiate for type T.
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        /// <typeparam name="TImpl">Type to register for instantiation.</typeparam>
        /// <returns>An instance of <see cref="IDependencyContainer"/></returns>
        public IDependencyContainer Register<T, TImpl>()
            where T : class
            where TImpl : class, T
        {
            this.container.RegisterType<T, TImpl>();
            return this;
        }

        /// <summary>
        /// Registers a type to instantiate for type T as singleton.
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        /// <typeparam name="TImpl">Type to register for instantiation.</typeparam>
        /// <returns>An instance of <see cref="IDependencyContainer"/></returns>
        public IDependencyContainer RegisterSingle<T, TImpl>()
            where T : class
            where TImpl : class, T
        {
            this.container.RegisterType<T, TImpl>(new ContainerControlledLifetimeManager());
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
            return this.Register(typeof(T), type);
        }

        /// <summary>
        /// Tries to register a type
        /// </summary>
        /// <param name="type">Type to register.</param>
        /// <param name="impl">Type that implements registered type.</param>
        /// <returns>An instance of <see cref="IDependencyContainer"/></returns>
        public IDependencyContainer Register(Type type, Type impl)
        {
            this.container.RegisterType(type, impl);
            return this;
        }

        public IDependencyContainer Register<T>(Func<IResolver, T> func) where T : class
        {
            throw new NotImplementedException("Unity container does not support registering funcs for resolving.");
        }

        #endregion
    }
}
